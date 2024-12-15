using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IScanResultRepository
{
    Task<ScanResult> GetScanResultAsync(int id);
    Task<List<ScanResult>> GetScanResultsAsync();
    Task AddScanResultAsync(ScanResult scanResult);
    Task DeleteScanResultAsync(int id);
}