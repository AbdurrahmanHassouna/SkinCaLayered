using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SkinCa.Business.DTOs.User;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.DataAccess;

namespace SkinCa.Business.Services;

public class UserService : IUserService
{
    private UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ProfileResponseDto?> GetProfileAsync(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user== null) return null;

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
    public async Task<ProfileResponseDto?> UpdateProfileAsync(ProfileRequestDto newProfile, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user== null) return null;
        user.Email = newProfile.Email;
        user.FirstName = newProfile.FirstName;
        user.LastName = newProfile.LastName;
        user.Address = newProfile.Address;
        user.Latitude = newProfile.Latitude;
        user.Longitude = newProfile.Longitude;
        user.BirthDate = newProfile.BirthDate;
        user.PhoneNumber  = newProfile.PhoneNumber;
        user.Governorate =(short)newProfile.Governorate;
        using MemoryStream memoryStream = new MemoryStream();
        if (newProfile.ProfilePicture != null)
        {
            await newProfile.ProfilePicture.CopyToAsync(memoryStream);
        }
        user.ProfilePicture = memoryStream.ToArray();
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
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
        return null;
    }

    public async Task<ProfileResponseDto?> UpdateProfilePictureAsync(IFormFile Image, ClaimsPrincipal claimsPrincipal)
    {
       
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null) return null;

        
        using MemoryStream memoryStream = new MemoryStream();
        await Image.CopyToAsync(memoryStream); 
        user.ProfilePicture = memoryStream.ToArray(); 

        
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            
            ProfileResponseDto profile = new ProfileResponseDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Age = CalculateAge(user.BirthDate),
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Governorate = (Governorate)user.Governorate
            };

            return profile;
        }

        return null;
    }
}