namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDoctorInfoRepository
{
    Task<List<DoctorInfo>> GetAllAsync();
    Task<List<DoctorInfo>> GetWorkingDoctorsAsync();
    Task<bool> CraeteAsync(DoctorInfo doctor);
    Task<bool?>  DeleteAsync(int id);
    
}