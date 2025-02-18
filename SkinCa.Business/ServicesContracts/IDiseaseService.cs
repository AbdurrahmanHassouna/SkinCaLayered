using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IDiseaseService
{
    Task<List<DiseaseResponseDto>> GetAllAsync();
    Task<List<DiseaseResponseDto>> SearchAsync(string searchString);
    Task EditAsync(int id, DiseaseRequestDto diseaseRequestDto);
    Task CreateAsync(DiseaseRequestDto diseaseRequestDto);
    
    Task  DeleteAsync(int id);
    
}