using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.Exceptions;
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
    public async Task<DiseaseResponseDto?> GetByIdAsync( int diseaseId)
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
            Specialty = diseaseRequestDto.Specialty,
            Symptoms = diseaseRequestDto.Symptoms,
            Types = diseaseRequestDto.Types,
            Causes = diseaseRequestDto.Causes,
            DiagnosticMethods = diseaseRequestDto.DiagnosticMethods,
            Prevention = diseaseRequestDto.Prevention
        };
        if (diseaseRequestDto.Image.Length > 2*1000) throw new ServiceException("large file size");
        
        disease.Image=await diseaseRequestDto.Image.ToBytesAsync();
        
        await _diseaseRepository.CreateAsync(disease);
    }

    public async Task EditAsync(int id,DiseaseRequestDto diseaseRequestDto)
    {
        var disease = await _diseaseRepository.GetByIdAsync(id);
        if (disease == null)
        {
            throw new ServiceException("Item not found.");
        }
        disease.Title = diseaseRequestDto.Title;
        disease.Specialty = diseaseRequestDto.Specialty;
        disease.Symptoms = diseaseRequestDto.Symptoms;
        disease.Types = diseaseRequestDto.Types;
        disease.Causes = diseaseRequestDto.Causes;
        disease.DiagnosticMethods = diseaseRequestDto.DiagnosticMethods;
        disease.Prevention = diseaseRequestDto.Prevention;
        if (diseaseRequestDto.Image.Length > 2*1000) throw new ServiceException("large file size");
        
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

