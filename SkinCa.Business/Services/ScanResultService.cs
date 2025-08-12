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

    public async Task<ScanResultResponseDto?> GetByIdAsync(int id)
    {
        var scanResult = await _scanResultRepository.GetByIdAsync(id);

        return new ScanResultResponseDto
        {
            GotCancer = scanResult.GotCancer,
            Data = scanResult.Data,
            Confidence = scanResult.Confidence
        };
    }

    public async Task<List<ScanResultResponseDto>> GetAllByUserIdAsync(int userId)
    {
        var scanResults = await _scanResultRepository.GetAllAsync();
        var userScanResults = scanResults.Where(r => r.UserId == userId.ToString()).ToList();

        return userScanResults.Select(scanResult => new ScanResultResponseDto
        {
            GotCancer = scanResult.GotCancer,
            Data = scanResult.Data,
            Confidence = scanResult.Confidence
        }).ToList();
    }

    public async Task CreateAsync(ScanResultRequestDto scanResultRequestDto)
    {
        var scanResult = new ScanResult
        {
            GotCancer = scanResultRequestDto.GotCancer,
            Data = scanResultRequestDto.Data,
            Confidence = scanResultRequestDto.Confidence
        };
        await _scanResultRepository.CreateAsync(scanResult);
    }

    public async Task DeleteAsync(int id)
    {
        await _scanResultRepository.DeleteAsync(id);
    }
}