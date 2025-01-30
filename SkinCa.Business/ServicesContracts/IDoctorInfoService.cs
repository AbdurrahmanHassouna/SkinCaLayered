using SkinCa.Business.DTOs.DoctorInfo;

namespace SkinCa.Business.ServicesContracts;

public interface IDoctorInfoService
{
    Task<IList<DoctorSummaryDto>> GetDoctorsInfoAsync();
    Task<DoctorInfoResponseDto?> GetDoctorsInfoAsync(string id);
    Task<IList<DoctorSummaryDto>> GetNearbyDoctorsInfoAsync(double latitude, double longitude);
    Task<bool> CreateDoctorInfoAsync(DoctorInfoRequestDto doctorInfoDto); 
    Task<bool?> UpdateDoctorInfoAsync(string userId ,DoctorInfoRequestDto doctorInfoRequestDto);
    Task<bool?> DeleteDoctorInfoAsync(string id);
}