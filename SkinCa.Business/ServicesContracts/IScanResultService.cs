using SkinCa.DataAccess;

namespace SkinCa.Business.ServicesContracts;

public interface IScanResultService
{
    Task<ScanResult?> GetByIdAsync(int id);
    Task<List<ScanResult>> GetAllAsync();
    Task<bool>  CreateAsync(ScanResultRequestDto scanResultRequestDto);
    Task<bool?>  DeleteAsync(int id);
}