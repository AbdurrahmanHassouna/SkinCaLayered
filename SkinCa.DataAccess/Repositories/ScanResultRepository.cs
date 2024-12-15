using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class ScanResultRepository:IScanResultRepository
{
    public Task<ScanResult> GetScanResultAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ScanResult>> GetScanResultsAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddScanResultAsync(ScanResult scanResult)
    {
        throw new NotImplementedException();
    }

    public Task DeleteScanResultAsync(int id)
    {
        throw new NotImplementedException();
    }
}