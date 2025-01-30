using GeoCoordinatePortable;
using Microsoft.AspNetCore.Identity;
using SkinCa.Business.DTOs;
using SkinCa.Business.DTOs.DoctorInfo;
using SkinCa.Business.DTOs.WorkingDay;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.DataAccess;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Business.Services;

public class DoctorInfoService: IDoctorInfoService
{
    private IDoctorInfoRepository _doctorInfoRepository;
    private UserManager<ApplicationUser> _userManager;
    DoctorInfoService(IDoctorInfoRepository doctorInfoRepository, UserManager<ApplicationUser> userManager)
    {
        _doctorInfoRepository= doctorInfoRepository;
        _userManager = userManager;
    }
    public async Task<IList<DoctorSummaryDto>> GetDoctorsInfoAsync()
    {
        var doctors = await _doctorInfoRepository.GetAllAsync();
        return doctors.Select(d => new DoctorSummaryDto
        {
            Id = d.UserId,
            Name = d.User.UserName,
            Image = d.User.ProfilePicture,
            Experience = d.Experience,
            Specialization = d.Specialization,
            Governorate = (Governorate)d.User.Governorate,
            IsWroking = d.WorkingDays.Any(_doctorInfoRepository.IsWorkingNow)
        }).ToList();
    }

    public async Task<DoctorInfoResponseDto?> GetDoctorsInfoAsync(string id)
    {
        var doctor =await _doctorInfoRepository.GetDoctorInfoAsync(id);
        if(doctor == null) return null;
        return new DoctorInfoResponseDto
        {
            UserId = id,
            Address = doctor.User.Address,
            BirthDate = doctor.User.BirthDate,
            ClinicFees = doctor.ClinicFees,
            Services = doctor.Services.Split(","),
            ClinicSchedule = doctor.WorkingDays.Select(d => new WorkingDayDto()
                {
                    Day = d.Day,
                    OpenAt = d.OpenAt,
                    CloseAt = d.CloseAt
                })
                .ToList(),
            ProfilePicture = doctor.User.ProfilePicture,
            Description = doctor.Description,
            Latitude = doctor.User.Latitude,
            Longitude = doctor.User.Longitude,
            Rating = 0,
            Email = doctor.User.Email,
            FirstName = doctor.User.FirstName,
            LastName = doctor.User.LastName,
            Experience = doctor.Experience,
            IsWorking = doctor.WorkingDays.Any(_doctorInfoRepository.IsWorkingNow),
            PhoneNumber = doctor.User.PhoneNumber,
            Specialization = doctor.Specialization,
        };
    }

    public async Task<IList<DoctorSummaryDto>> GetNearbyDoctorsInfoAsync(double latitude, double longitude)
    {
        var doctors = await _doctorInfoRepository.GetAllAsync();
        var userLocation = new GeoCoordinate(latitude, longitude);
        return doctors.Where(d =>
        {
            if (d.User.Latitude == null || d.User.Longitude == null)
                return false;

            var doctorLocation = new GeoCoordinate(d.User.Latitude.Value, d.User.Longitude.Value);
            return userLocation.GetDistanceTo(doctorLocation) <= 10000; // meter
        }).Select(d => new DoctorSummaryDto
        {
            Id = d.UserId,
            Name = d.User.UserName,
            Image = d.User.ProfilePicture,
            Experience = d.Experience,
            Specialization = d.Specialization,
            Governorate = (Governorate)d.User.Governorate,
            IsWroking = d.WorkingDays.Any(_doctorInfoRepository.IsWorkingNow)
        }).ToList();
    }

    public async Task<bool> CreateDoctorInfoAsync(DoctorInfoRequestDto doctorInfoDto)
    {
        var model = (RegistrationRequestDto)doctorInfoDto;
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
        {
            return false;
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
            return false;
        }

        var doctorInfo = new DoctorInfo()
        {
            UserId = user.Id,
            ClinicFees = doctorInfoDto.ClinicFees,
            Description = doctorInfoDto.Description,
            WorkingDays = doctorInfoDto.WorkingDays.Select(w => new DoctorWorkingDay()
            {
                Day = w.Day,
                CloseAt = w.CloseAt,
                OpenAt = w.OpenAt
            }).ToList(),
            Experience = doctorInfoDto.Experience,
            Specialization = doctorInfoDto.Specialization,
            Services = string.Join(",", doctorInfoDto.Services)
        };
         return await _doctorInfoRepository.CreateAsync(doctorInfo);
    }

    public async Task<bool?> UpdateDoctorInfoAsync(string userId,DoctorInfoRequestDto doctorInfoRequestDto)
    {
        var doctorInfo = await _doctorInfoRepository.GetDoctorInfoAsync(userId);
        if (doctorInfo == null) return null;
        doctorInfo.ClinicFees = doctorInfoRequestDto.ClinicFees;
        doctorInfo.Description = doctorInfoRequestDto.Description;
        doctorInfo.WorkingDays = doctorInfoRequestDto.WorkingDays.Select(w=> new DoctorWorkingDay()
        {
            Day = w.Day,
            OpenAt = w.OpenAt,
            CloseAt = w.CloseAt
        }).ToList();
        doctorInfo.Experience = doctorInfoRequestDto.Experience;
        doctorInfo.Specialization = doctorInfoRequestDto.Specialization;
        doctorInfo.Services = string.Join(",", doctorInfoRequestDto.Services);
        var succeeded = await _doctorInfoRepository.UpdateAsync(doctorInfo);
        if(succeeded != true) return succeeded;
        var model = (RegistrationRequestDto)doctorInfoRequestDto;
        doctorInfo.User.UserName = model.Email.Trim();
        doctorInfo.User.Email = model.Email.Trim();
        doctorInfo.User.PhoneNumber = model.PhoneNumber.Trim();
        doctorInfo.User.FirstName = model.FirstName.Trim();
        doctorInfo.User.LastName = model.LastName.Trim();
        doctorInfo.User.BirthDate = model.BirthDate;
        doctorInfo.User.Address = model.Address?.Trim();
        doctorInfo.User.Governorate = (short)model.Governorate;
        doctorInfo.User.Latitude = model.Latitude;
        doctorInfo.User.Longitude = model.Longitude;
        var result = await _userManager.UpdateAsync(doctorInfo.User);
        return result.Succeeded;
    }

    public async Task<bool?> DeleteDoctorInfoAsync(string userId)
    {
        return await _doctorInfoRepository.DeleteAsync(userId);
    }
}