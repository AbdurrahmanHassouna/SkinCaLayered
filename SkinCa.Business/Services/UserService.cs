using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SkinCa.Business.DTOs.User;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.Common.Exceptions;
using SkinCa.Common.UtilityExtensions;
using SkinCa.DataAccess;

namespace SkinCa.Business.Services;

public class UserService : IUserService
{
    private UserManager<ApplicationUser> _userManager;
    private ILogger<UserService> _logger;
    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<ProfileResponseDto> GetProfileAsync(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            _logger.LogError("Unable to retrieve user with {claims principal} at {Method}"
                ,claimsPrincipal,nameof(UpdateProfileAsync));
            throw new NotFoundException("Could not find user");
        }
        
        ProfileResponseDto profile = new ProfileResponseDto
        {
            Email       = user.Email,
            FirstName   = user.FirstName,
            LastName    = user.LastName,
            Address     = user.Address,
            Latitude    = user.Latitude,
            Longitude   = user.Longitude,
            Age   = CalculateAge(user.BirthDate),
            PhoneNumber = user.PhoneNumber,
            ProfilePicture = user.ProfilePicture,
            Governorate = (Governorate)user.Governorate
        };
        return profile;
    }
    private static int CalculateAge(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        
        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
    public async Task<IdentityResult> UpdateProfileAsync(ProfileRequestDto newProfile, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            _logger.LogError("Unable to retrieve user with {claims principal} at {Method}",claimsPrincipal,nameof(UpdateProfileAsync));
            throw new NotFoundException("Could not find user");
        }
        user.Email = newProfile.Email;
        user.FirstName = newProfile.FirstName;
        user.LastName = newProfile.LastName;
        user.Address = newProfile.Address;
        user.Latitude = newProfile.Latitude;
        user.Longitude = newProfile.Longitude;
        user.BirthDate = newProfile.BirthDate;
        user.PhoneNumber  = newProfile.PhoneNumber;
        user.Governorate =(short)newProfile.Governorate;
        if (newProfile.ProfilePicture != null)
        {
            user.ProfilePicture = await newProfile.ProfilePicture.ToBytesAsync();
        }

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdateProfilePictureAsync(IFormFile image, ClaimsPrincipal claimsPrincipal)
    {
       
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            _logger.LogError("Unable to retrieve user with {claims principal} at {Method}",claimsPrincipal,nameof(UpdateProfileAsync));
            throw new NotFoundException("Could not find user");
        }
        
        user.ProfilePicture =await image.ToBytesAsync();
        var result = await _userManager.UpdateAsync(user);
        
        return result;
    }
}