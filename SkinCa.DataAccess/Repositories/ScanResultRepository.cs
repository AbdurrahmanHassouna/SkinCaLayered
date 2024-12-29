using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class ScanResultRepository:IScanResultRepository
{
    private readonly AppDbContext _context;

    public ScanResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScanResult?> GetByIdAsync(int id)
    {
        return await _context.ScanResults.FindAsync(id);
    }
    public async Task<List<ScanResult>> GetAllAsync()
    {
        return await _context.ScanResults.ToListAsync();
    }
    public async Task<bool> CreateAsync(ScanResult scanResult)
    {
        await _context.ScanResults.AddAsync(scanResult);
        return await _context.SaveChangesAsync() > 0;
       
    }
    public async Task<bool?> DeleteAsync(int id)
    {
        var scanResult = await _context.ScanResults.FindAsync(id);
        if (scanResult == null) return null;
        _context.ScanResults.Remove(scanResult);
        return await _context.SaveChangesAsync() > 0;

    }
}