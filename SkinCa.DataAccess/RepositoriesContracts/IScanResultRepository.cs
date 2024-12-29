namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IScanResultRepository
{
    Task<ScanResult?> GetByIdAsync(int id);
    Task<List<ScanResult>> GetAllAsync();
    Task<bool>  CreateAsync(ScanResult scanResult);
    Task<bool?>  DeleteAsync(int id);
}