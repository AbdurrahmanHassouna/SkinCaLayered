using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class DiseaseRepository : IDiseaseRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<DiseaseRepository> _logger;

    public DiseaseRepository(AppDbContext context, ILogger<DiseaseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Disease?> GetByIdAsync(int id)
    {
        try
        {
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == id);
            if (disease == null) throw new NotFoundException($"Disease with id: {id} was not found");
            return disease;
        }
        catch (NotFoundException e)
        {
            throw;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while retrieving the disease", e);
        }
    }

    public async Task<List<Disease>> GetAllAsync()
    {
        try
        {
            return await _context.Diseases.ToListAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while retrieving diseases", e);
        }
    }

    public async Task<bool> EditAsync(Disease disease)
    {
        try
        {
            var existingDisease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == disease.Id);
            if (existingDisease == null)
                throw new NotFoundException($"Disease with id: {disease.Id} was not found");

            existingDisease.Title = disease.Title;
            existingDisease.Causes = disease.Causes;
            existingDisease.Image = disease.Image;
            existingDisease.Prevention = disease.Prevention;
            existingDisease.Specialty = disease.Specialty;
            existingDisease.Symptoms = disease.Symptoms;
            existingDisease.Types = disease.Types;
            existingDisease.DiagnosticMethods = disease.DiagnosticMethods;

            _context.Diseases.Update(existingDisease);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while updating the disease", e);
        }
    }

    public async Task<List<Disease>> SearchAsync(string searchString)
    {
        try
        {
            return await _context.Diseases
                .Where(d => d.Title.Contains(searchString))
                .ToListAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while searching diseases", e);
        }
    }

    public async Task<bool> CreateAsync(Disease disease)
    {
        try
        {
            await _context.Diseases.AddAsync(disease);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while adding the disease", e);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == id);
            if (disease == null)
                throw new NotFoundException($"Disease with id: {id} was not found");

            _context.Diseases.Remove(disease);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while deleting the disease", e);
        }
    }
}
