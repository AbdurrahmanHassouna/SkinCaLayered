using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.DataAccess;


namespace SkinCa.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string ResetPasswordTokenPurpose = "ResetPassword";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly JWT _jwt;
        private readonly IdentityOptions _options;
        private AppDbContext _context;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt
            , IEmailService emailService, IOptions<IdentityOptions> options, AppDbContext context)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailService = emailService;
            _options = options?.Value ?? new();
            _context = context;
        }
        
        public async Task<AuthenticationResponse> RegisterAsync(RegistrationRequestDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthenticationResponse
                {
                    Message = "Email is already registered"
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                BirthDate =model.BirthDate,
                Address = model.Address?.Trim(),
                Governorate = (short)model.Governorate,
                Latitude=model.Latitude,
                Longitude=model.Longitude
            };
            if (model.ProfilePicture != null)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }
            var result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (!result.Succeeded)
            {
                return new AuthenticationResponse { IsAuthenticated = false,Message= "Unable to Register the User"};
            }
            
            await _userManager.AddToRoleAsync(user,"User");
            
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthenticationResponse
            {
                UserName= user.FirstName+" "+user.LastName,
                ProfilePicture = user.ProfilePicture,
                Message = "Email is registered successfully",
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }
        public async Task<AuthenticationResponse> GetTokenAsync(LoginRequestDto requestDto)
        {
            var user = await _userManager.FindByEmailAsync(requestDto.Email);

            if (user is null)
            {
                return new AuthenticationResponse()
                {
                    Message = "Email is not registered",
                    IsAuthenticated=false
                };
            }
            else if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new AuthenticationResponse()
                {
                    Message = "Email is not confirmed",
                    IsAuthenticated=false
                };
            }
            else if (!await _userManager.CheckPasswordAsync(user, requestDto.Password))
            {
                return new AuthenticationResponse()
                {
                    Message = "Password or username is incorrect",
                    IsAuthenticated=false
                };
            }
            
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthenticationResponse()
            {
                UserName = user.FirstName,
                ProfilePicture = user.ProfilePicture,
                Message = "Successful Login",
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }
        public async Task<bool?> RequestEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendConfirmationEmail(email, token);
            return true;
        } 
        public async Task<bool?> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        public async Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            return await _userManager.GetUserAsync(userPrincipal);
        }
        public async Task<bool?> ChangePasswordAsync(ClaimsPrincipal userPrincipal, string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);

            if (user == null) return null;
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            
            return result.Succeeded;
        }

        public async Task<bool?> DeleteAccountAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null) return null;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool?> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            await _emailService.SendForgotPasswordEmail(user.Email,token);
            
            return true;
        }
        public async Task<bool?> VerifyResetPasswordToken(string token,string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var result = await _userManager.VerifyUserTokenAsync(user,
                 _options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token);
            return result;
        }
        public async Task<bool?> ResetPassword(string token, string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            
            foreach (var sRole in roles)
                roleClaims.Add(new Claim("roles", sRole));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            }.Union(roleClaims).Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }
    }
}
