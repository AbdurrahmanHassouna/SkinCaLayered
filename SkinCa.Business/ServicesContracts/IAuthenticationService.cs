using System.Security.Claims;
using Microsoft.Identity.Client;
using SkinCa.Business.DTOs;
using SkinCa.DataAccess;

namespace SkinCa.Business.ServicesContracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> RegisterAsync(RegistrationRequestDto registrationDto);
        Task<AuthenticationResponse> GetTokenAsync(LoginRequestDto loginDto);
        Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal userPrincipal);
        Task<bool?> RequestEmailConfirmationAsync(string email);
        Task<bool?> ConfirmEmailAsync(string email, string token);
        Task<bool?> VerifyResetPasswordToken(string token, string email);
        Task<bool?> ForgotPasswordAsync(string email);
        Task<bool?> ResetPassword(string token, string email, string newPassword);
        Task<bool?> ChangePasswordAsync(ClaimsPrincipal userPrincipal,string oldPassword, string newPassword);
        Task<bool?> DeleteAccountAsync(ClaimsPrincipal userPrincipal);

    }
}
