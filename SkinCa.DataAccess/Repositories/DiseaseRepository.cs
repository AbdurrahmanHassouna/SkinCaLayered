using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
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
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == id);
            if (disease == null)
                throw new NotFoundException($"Disease with id: {id} was not found");
            return disease;
        }

        public async Task<List<Disease>> GetAllAsync()
        {
            return await _context.Diseases.ToListAsync();
        }

        public async Task EditAsync(Disease disease)
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
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while updating the disease");
        }

        public async Task<List<Disease>> SearchAsync(string searchString)
        {
            return await _context.Diseases
                .Where(d => d.Title.Contains(searchString))
                .ToListAsync();
        }

        public async Task CreateAsync(Disease disease)
        {
            await _context.Diseases.AddAsync(disease);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while adding the disease");
        }

        public async Task DeleteAsync(int id)
        {
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == id);
            if (disease == null)
                throw new NotFoundException($"Disease with id: {id} was not found");

            _context.Diseases.Remove(disease);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting the disease");
        }
    }
}