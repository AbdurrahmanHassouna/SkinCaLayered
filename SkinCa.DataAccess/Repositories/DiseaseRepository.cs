using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class DiseaseRepository:IDiseaseRepository
{
    private readonly AppDbContext _context;

    public DiseaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Disease?> GetById(int id)
    {
        return await _context.Diseases.FindAsync(id);
    }
    public async Task<List<Disease>> GetAllAsyncAsync()
    {
        return await _context.Diseases.ToListAsync();
    }

    public async Task<bool?> EditAsync(Disease disease)
    {
        var existingDisease =await _context.Diseases.FindAsync(disease.Id);
        if(existingDisease==null) return null;
        existingDisease.Title=disease.Title;
        existingDisease.Causes=disease.Causes;
        existingDisease.Image = disease.Image;
        existingDisease.Prevention =disease.Prevention;
        existingDisease.Specialty = disease.Specialty;
        existingDisease.Symptoms = disease.Symptoms;
        existingDisease.Types = disease.Types;
        existingDisease.DiagnosticMethods = disease.DiagnosticMethods;
        
        _context.Diseases.Update(existingDisease);
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Disease>> SearchAsync(string searchString)
    {
        return await _context.Diseases.Where(d => d.Title.Contains(searchString)).ToListAsync();
    }

    public async Task<bool> CreateAsync(Disease disease)
    {
        await _context.Diseases.AddAsync(disease);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var disease = await _context.Diseases.FindAsync(id);
        if (disease == null) return null;
        _context.Diseases.Remove(disease);
        return await _context.SaveChangesAsync() > 0;
    }
}