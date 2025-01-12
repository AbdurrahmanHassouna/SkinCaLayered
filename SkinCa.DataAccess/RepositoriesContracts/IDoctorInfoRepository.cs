namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDoctorInfoRepository
{
    Task<List<DoctorInfo>> GetAllAsync();
    Task<DoctorInfo?> GetDoctorInfoAsync(string doctorId);
    Task<List<DoctorInfo>> GetWorkingDoctorsAsync();
    Task<bool> CreateAsync(DoctorInfo doctor);
    Task<bool?>  DeleteAsync(int id);
    Task<bool?> UpdateAsync(DoctorInfo doctor);
    bool IsWorkingNow(DoctorWorkingDay doctorWorkingDay);
}