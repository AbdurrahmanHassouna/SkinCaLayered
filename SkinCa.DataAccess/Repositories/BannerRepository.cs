using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class BannerRepository:IBannerRepository
{
    private AppDbContext _context;
    public BannerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Banner?> GetBannerAsync(int bannerId)
    {
        return await _context.Banners.FindAsync(bannerId);
    }
    public async Task<List<Banner>> GetAllAsync()
    {
        return await _context.Banners.ToListAsync();
    }

    public async Task<bool> AddAsync(Banner banner)
    { 
        await _context.Banners.AddAsync(banner);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> UpdateAsync(Banner banner)
    { 
        var existingBanner = await _context.Banners.FindAsync(banner.Id);
        if (existingBanner == null) return null;
        _context.Banners.Update(banner);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null) return null;
        _context.Banners.Remove(banner);
        return await _context.SaveChangesAsync() > 0;

    }
}