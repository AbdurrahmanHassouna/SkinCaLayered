using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SkinCa.Business.DTOs.User;

namespace SkinCa.Business.ServicesContracts;

public interface IUserService
{
    Task<ProfileResponseDto> GetProfileAsync(ClaimsPrincipal user);
    Task<IdentityResult> UpdateProfileAsync(ProfileRequestDto newProfile,ClaimsPrincipal claimsPrincipal);
    Task<IdentityResult> UpdateProfilePictureAsync(IFormFile Image ,ClaimsPrincipal claimsPrincipal);
}