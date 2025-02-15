using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IDiseaseService
{
    Task<List<DiseaseResponseDto>> GetAllAsync();
    Task<bool> EditAsync(int id, DiseaseRequestDto diseaseRequestDto);
    Task<bool> CreateAsync(DiseaseRequestDto diseaseRequestDto);
    Task<List<DiseaseResponseDto>> SearchAsync(string searchString);
    Task<bool> DeleteAsync(int id);
    
}