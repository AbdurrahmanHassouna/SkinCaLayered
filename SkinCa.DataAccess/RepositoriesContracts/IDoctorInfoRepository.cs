namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDoctorInfoRepository
{
    Task<List<DoctorInfo>> GetAllAsync();
    Task<DoctorInfo> GetDoctorInfoAsync(string doctorId);
    Task<List<DoctorInfo>> GetWorkingDoctorsAsync();
    Task CreateAsync(DoctorInfo doctor);
    Task DeleteAsync(string id);
    Task UpdateAsync(DoctorInfo doctor);
    bool IsWorkingNow(DoctorWorkingDay doctorWorkingDay);
}