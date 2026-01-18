using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IScanResultService
{
    Task<ScanResultDto?> GetByIdAsync(int id);
    Task<List<ScanResultDto>> GetAllByUserIdAsync(string userId);
    Task CreateAsync(ScanResultDto scanResultDto);
    Task DeleteAsync(int id);
}