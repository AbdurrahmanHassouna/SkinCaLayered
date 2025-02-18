namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IScanResultRepository
{
    Task<ScanResult> GetByIdAsync(int id);
    Task<List<ScanResult>> GetAllAsync();
    Task CreateAsync(ScanResult scanResult);
    Task DeleteAsync(int id);
}