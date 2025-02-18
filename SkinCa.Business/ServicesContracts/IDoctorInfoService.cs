using Microsoft.AspNetCore.Identity;
using SkinCa.Business.DTOs.DoctorInfo;
using SkinCa.Common;

namespace SkinCa.Business.ServicesContracts;

public interface IDoctorInfoService
{
    Task<IList<DoctorSummaryDto>> GetDoctorsInfoAsync();
    Task<DoctorInfoResponseDto?> GetDoctorsInfoAsync(string id);
    Task<IList<DoctorSummaryDto>> GetNearbyDoctorsInfoAsync(double latitude, double longitude);
    Task<OperationResult<IEnumerable<IdentityError>>> CreateDoctorInfoAsync(DoctorInfoRequestDto doctorInfoDto); 
    Task<OperationResult<IEnumerable<IdentityError>>> UpdateDoctorInfoAsync(string userId ,DoctorInfoRequestDto doctorInfoRequestDto);
    Task DeleteDoctorInfoAsync(string id);
}