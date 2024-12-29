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
                BirthDate =(model.BirthDate != DateTime.MinValue)?model.BirthDate : null,
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
        public async Task<string?> GetUserIdAsync(string header)
        {
            string token;
            if (header.StartsWith("Bearer "))
            {
                token = header.Substring("Bearer ".Length);
            }
            else return null;
            var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidAudience = _jwt.Audience,
                ValidIssuer = _jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key))
            });
            return !result.IsValid ? null : result.Claims["userid"].ToString();
        }
        public async Task<bool> VerifyResetPasswordToken(string token, ApplicationUser user)
        {
            var result = await _userManager.VerifyUserTokenAsync(user,
                 _options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token);
            return result;
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
