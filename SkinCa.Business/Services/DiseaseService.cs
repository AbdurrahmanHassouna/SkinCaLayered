using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.UtilityExtensions;
using SkinCa.DataAccess;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Business.Services;

public class DiseaseService : IDiseaseService
{
    private readonly IDiseaseRepository _diseaseRepository;

    public DiseaseService(IDiseaseRepository diseaseRepository)
    {
        _diseaseRepository = diseaseRepository;
    }
    public async Task<DiseaseResponseDto?> GetByIdAstbc( int diseaseId)
    {
        var disease = await _diseaseRepository.GetByIdAsync(diseaseId);
        if (disease == null) return null;
        var diseaseResponseDto = new DiseaseResponseDto()
        {
            Id = disease.Id,
            Title = disease.Title,
            UserId = disease.UserId,
            Image = disease.Image,
            Specialty = disease.Specialty,
            Symptoms = disease.Symptoms,
            Types = disease.Types,
            Causes = disease.Causes,
            DiagnosticMethods = disease.DiagnosticMethods,
            Prevention = disease.Prevention
        };
        return diseaseResponseDto;
    }

    public async Task<List<DiseaseResponseDto>> GetAllAsync()
    {
        var diseases = await _diseaseRepository.GetAllAsync();
        return diseases.Select(disease => new DiseaseResponseDto
        {
            Id = disease.Id,
            Title = disease.Title,
            UserId = disease.UserId,
            Image = disease.Image,
            Specialty = disease.Specialty,
            Symptoms = disease.Symptoms,
            Types = disease.Types,
            Causes = disease.Causes,
            DiagnosticMethods = disease.DiagnosticMethods,
            Prevention = disease.Prevention
        }).ToList();
    }

    public async Task CreateAsync(DiseaseRequestDto diseaseRequestDto)
    {
        var disease = new Disease
        {
            Title = diseaseRequestDto.Title,
            UserId = diseaseRequestDto.UserId,
            Specialty = diseaseRequestDto.Specialty,
            Symptoms = diseaseRequestDto.Symptoms,
            Types = diseaseRequestDto.Types,
            Causes = diseaseRequestDto.Causes,
            DiagnosticMethods = diseaseRequestDto.DiagnosticMethods,
            Prevention = diseaseRequestDto.Prevention
        };
        disease.Image=await diseaseRequestDto.Image.ToBytesAsync();
        
        await _diseaseRepository.CreateAsync(disease);
    }

    public async Task EditAsync(int id,DiseaseRequestDto diseaseRequestDto)
    {
        var disease = new Disease
        {
            Id = id,
            Title = diseaseRequestDto.Title,
            UserId = diseaseRequestDto.UserId,
            Specialty = diseaseRequestDto.Specialty,
            Symptoms = diseaseRequestDto.Symptoms,
            Types = diseaseRequestDto.Types,
            Causes = diseaseRequestDto.Causes,
            DiagnosticMethods = diseaseRequestDto.DiagnosticMethods,
            Prevention = diseaseRequestDto.Prevention
        };
        disease.Image=await diseaseRequestDto.Image.ToBytesAsync();
        
         await _diseaseRepository.EditAsync(disease);
    }

    public async Task<List<DiseaseResponseDto>> SearchAsync(string searchString)
    {
        var diseases = await _diseaseRepository.SearchAsync(searchString);
        return diseases.Select(disease => new DiseaseResponseDto
        {
            Id = disease.Id,
            Title = disease.Title,
            UserId = disease.UserId,
            Image = disease.Image,
            Specialty = disease.Specialty,
            Symptoms = disease.Symptoms,
            Types = disease.Types,
            Causes = disease.Causes,
            DiagnosticMethods = disease.DiagnosticMethods,
            Prevention = disease.Prevention
        }).ToList();
    }

    public async Task DeleteAsync(int id)
    {
        await _diseaseRepository.DeleteAsync(id);
    }
}

