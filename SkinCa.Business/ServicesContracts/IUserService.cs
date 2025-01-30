using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SkinCa.Business.DTOs.User;

namespace SkinCa.Business.ServicesContracts;

public interface IUserService
{
    Task<ProfileResponseDto?> GetProfileAsync(ClaimsPrincipal user);
    Task<ProfileResponseDto?> UpdateProfileAsync(ProfileRequestDto newProfile,ClaimsPrincipal claimsPrincipal);
    Task<ProfileResponseDto> UpdateProfilePictureAsync(IFormFile Image ,ClaimsPrincipal claimsPrincipal);
}