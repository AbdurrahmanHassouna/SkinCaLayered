using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
        Task<ApplicationUser?> GetUserWithEmailAsync(string email);
        Task<bool?> RequestEmailConfirmationAsync(string email);
        Task<bool?> ConfirmEmailAsync(string email, string token);
        Task<bool?> VerifyResetPasswordToken(string token, string email);
        Task<bool?> ForgotPasswordAsync(string email);
        Task<bool?> ResetPasswordAsync(string token, string email, string newPassword);
        Task<IdentityResult?> ChangePasswordAsync(ClaimsPrincipal userPrincipal,string oldPassword, string newPassword);
        Task<bool?> DeleteAccountAsync(ClaimsPrincipal userPrincipal);

    }
}
