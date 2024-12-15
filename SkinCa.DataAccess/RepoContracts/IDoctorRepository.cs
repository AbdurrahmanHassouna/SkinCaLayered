using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IDoctorRepository
{
    Task<List<DoctorInfo>> GetDoctorsAsync();
    Task<List<DoctorInfo>> GetNearbyDoctorsAsync();
    Task<List<DoctorInfo>> GetWorkingDoctorsAsync();
    Task<DoctorInfo> CraeteDoctorInfo(DoctorInfo doctor);
    Task DeleteDoctorAsync(DoctorInfo doctor);
    
}