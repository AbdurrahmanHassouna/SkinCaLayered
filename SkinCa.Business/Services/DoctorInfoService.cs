using GeoCoordinatePortable;
using SkinCa.Business.DTOs.DoctorInfo;
using SkinCa.Business.DTOs.WorkingDay;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Business.Services;

public class DoctorInfoService: IDoctorInfoService
{
    private IDoctorInfoRepository _doctorInfoRepository;
    DoctorInfoService(IDoctorInfoRepository doctorInfoRepository)
    {
        _doctorInfoRepository= doctorInfoRepository;
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

    public Task<bool> CreateDoctorInfoAsync(DoctorInfoRequestDto doctorInfoDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateDoctorInfoAsync(string id ,DoctorInfoRequestDto doctorInfoRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteDoctorInfoAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}