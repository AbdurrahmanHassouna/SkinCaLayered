using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess;

namespace SkinCa.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string ResetPasswordTokenPurpose = "ResetPassword";
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly JWT _jwt;
        private readonly IdentityOptions _options;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, 
            IOptions<JWT> jwt,
            IEmailService emailService, 
            IOptions<IdentityOptions> options,
            ILogger<AuthenticationService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailService = emailService;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterAsync(RegistrationRequestDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser is not null)
            {
                _logger.LogWarning("Registration attempted with existing email: {Email}", model.Email);
                return IdentityResult.Failed(new IdentityError 
                { 
                    Code = "DuplicateEmail",
                    Description = "Email is already registered" 
                });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                BirthDate = model.BirthDate,
                Address = model.Address?.Trim(),
                Governorate = (short)model.Governorate,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            if (model.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }

            var result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role to user: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                await _userManager.DeleteAsync(user);
                return roleResult;
            }

            _logger.LogInformation("User registered successfully: {Email}", user.Email);
            return IdentityResult.Success;
        }

        public async Task<AuthenticationResponse> GetTokenAsync(LoginRequestDto requestDto)
        {
            var user = await _userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempted with non-existent email: {Email}", requestDto.Email);
                throw new NotFoundException($"User with email {requestDto.Email} not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user: {Email}, IsLockedOut: {IsLockedOut}, RequiresTwoFactor: {RequiresTwoFactor}", 
                    requestDto.Email, result.IsLockedOut, result.RequiresTwoFactor);
                
                var message = result.IsLockedOut 
                    ? "Account locked out" 
                    : result.RequiresTwoFactor 
                        ? "Requires two-factor authentication" 
                        : "Invalid login attempt";

                return new AuthenticationResponse()
                {
                    IsAuthenticated = false,
                    Message = message,
                };
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            _logger.LogInformation("User logged in successfully: {Email}", user.Email);

            return new AuthenticationResponse
            {
                IsAuthenticated = true,
                Message = "Successful Login",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task RequestEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation requested for non-existent user: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendConfirmationEmail(email, token);
            _logger.LogInformation("Email confirmation token generated and sent for user: {Email}", email);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation attempted for non-existent user: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email confirmed successfully for user: {Email}", email);
            }
            else
            {
                _logger.LogWarning("Email confirmation failed for user: {Email}, Errors: {Errors}", 
                    email, string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                _logger.LogError("User not found for ClaimsPrincipal:{Claims} \n at {Method}"
                    ,string.Join(", ", userPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}")),
                    nameof(GetUserAsync));
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task<ApplicationUser> GetUserWithEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }
            return user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal userPrincipal, string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                _logger.LogError("Password change attempted for non-existent user {ClaimsPricipal}\n at {Method}"
                    ,string.Join(", ", userPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}")),
                    nameof(ChangePasswordAsync));
                throw new NotFoundException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password changed successfully for user: {Email}", user.Email);
            }
            else
            {
                _logger.LogWarning("Password change failed for user: {Email}, Errors: {Errors}", 
                    user.Email, string.Join(", ", result.Errors.Select(e => e.Code+" : "+e.Description)));
            }

            return result;
        }

        public async Task<IdentityResult> DeleteAccountAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                _logger.LogError("User not found for ClaimsPrincipal:{Claims} \n at {Method}"
                    ,string.Join(", ", userPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}")),
                    nameof(DeleteAccountAsync));
                throw new NotFoundException("User not found");
                
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Account deleted successfully for user: {Email}", user.Email);
            }
            else
            {
                _logger.LogWarning("Account deletion failed for user: {Email}, Errors: {Errors}", 
                    user.Email, string.Join(", ", result.Errors.Select(e => e.Code+" : "+e.Description)));
            }

            return result;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent user: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendForgotPasswordEmail(user.Email, token);
            _logger.LogInformation("Password reset token generated and sent for user: {Email}", email);
        }

        public async Task<IdentityResult> VerifyResetPasswordToken(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset token verification attempted for non-existent user: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }

            var result = await _userManager.VerifyUserTokenAsync(user,
                _options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token);

            if (result)
            {
                _logger.LogInformation("Password reset token verified successfully for user: {Email}", email);
                return IdentityResult.Success;
            }
            
            _logger.LogWarning("Invalid password reset token for user: {Email}", email);
            return IdentityResult.Failed(new IdentityError 
            { 
                Code = "InvalidToken",
                Description = "Invalid password reset token" 
            });
        }

        public async Task<IdentityResult> ResetPasswordAsync(string token, string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent user: {Email}", email);
                throw new NotFoundException($"User with email {email} not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for user: {Email}", email);
            }
            else
            {
                _logger.LogWarning("Password reset failed for user: {Email}, Errors: {Errors}", 
                    email, string.Join(", ", result.Errors.Select(e => e.Code+" : "+e.Description)));
            }

            return result;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }
            .Union(roleClaims)
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
    }
}