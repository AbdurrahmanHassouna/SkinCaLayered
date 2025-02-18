using SkinCa.Business.DTOs;
using SkinCa.DataAccess;

namespace SkinCa.Business.ServicesContracts;

public interface IScanResultService
{
    Task<ScanResultResponseDto?> GetByIdAsync(int id);
    Task<List<ScanResultResponseDto>> GetAllByUserIdAsync(int userId);
    Task CreateAsync(ScanResultRequestDto scanResultRequestDto);
    Task DeleteAsync(int id);
}