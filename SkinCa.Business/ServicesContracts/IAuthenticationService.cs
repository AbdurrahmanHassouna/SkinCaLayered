using SkinCa.Business.DTOs;
using SkinCa.DataAccess;

namespace SkinCa.Business.ServicesContracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> RegisterAsync(RegistrationRequestDto registrationDto);
        Task<AuthenticationResponse> GetTokenAsync(LoginRequestDto loginDto);
        Task<string?> GetUserIdAsync(string header);
        Task<bool> VerifyResetPasswordToken(string token, ApplicationUser user);
       
    }
}
