using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Business.Services;

public class ScanResultService : IScanResultService
{
    private readonly IScanResultRepository _scanResultRepository;

    public ScanResultService(IScanResultRepository scanResultRepository)
    {
        _scanResultRepository = scanResultRepository;
    }

    public async Task<ScanResultDto?> GetByIdAsync(int id)
    {
        var scanResult = await _scanResultRepository.GetByIdAsync(id);

        return new ScanResultDto
        {
            GotCancer = scanResult.GotCancer,
            Data = scanResult.Data,
            Confidence = scanResult.Confidence
        };
    }

    public async Task<List<ScanResultDto>> GetAllByUserIdAsync(string userId)
    {
        var scanResults = await _scanResultRepository.GetAllAsync();
        var userScanResults = scanResults.Where(r => r.UserId == userId.ToString()).ToList();

        return userScanResults.Select(scanResult => new ScanResultDto
        {
            GotCancer = scanResult.GotCancer,
            Data = scanResult.Data,
            Confidence = scanResult.Confidence
        }).ToList();
    }

    public async Task CreateAsync(ScanResultDto scanResultDto)
    {
        var scanResult = new ScanResult
        {
            GotCancer = scanResultDto.GotCancer,
            Data = scanResultDto.Data,
            Confidence = scanResultDto.Confidence
        };
        await _scanResultRepository.CreateAsync(scanResult);
    }

    public async Task DeleteAsync(int id)
    {
        await _scanResultRepository.DeleteAsync(id);
    }
}