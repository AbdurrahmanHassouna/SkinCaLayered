using Microsoft.AspNetCore.Identity;
using SkinCa.Business.DTOs.DoctorInfo;

namespace SkinCa.Business.ServicesContracts;

public interface IDoctorInfoService
{
    Task<IList<DoctorSummaryDto>> GetDoctorsInfoAsync();
    Task<DoctorInfoResponseDto?> GetDoctorsInfoAsync(string id);
    Task<IList<DoctorSummaryDto>> GetNearbyDoctorsInfoAsync(double latitude, double longitude);
    Task<IdentityResult> CreateDoctorInfoAsync(DoctorInfoRequestDto doctorInfoDto); 
    Task<IdentityResult> UpdateDoctorInfoAsync(string userId ,DoctorInfoRequestDto doctorInfoRequestDto);
    Task DeleteDoctorInfoAsync(string id);
}