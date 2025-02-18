using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using SkinCa.Business.DTOs;
using SkinCa.DataAccess;

namespace SkinCa.Business.ServicesContracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterAsync(RegistrationRequestDto registrationDto);
        Task<AuthenticationResponse> GetTokenAsync(LoginRequestDto loginDto);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal userPrincipal);
        Task<ApplicationUser> GetUserWithEmailAsync(string email);
        Task RequestEmailConfirmationAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(string email, string token);
        Task<IdentityResult> VerifyResetPasswordToken(string token, string email);
        Task ForgotPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(string token, string email, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal userPrincipal,string oldPassword, string newPassword);
        Task<IdentityResult> DeleteAccountAsync(ClaimsPrincipal userPrincipal);

    }
}
