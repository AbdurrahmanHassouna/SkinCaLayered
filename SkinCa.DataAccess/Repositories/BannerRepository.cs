using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class BannerRepository:IBannerRepository
{
    private AppDbContext _context;
    private ILogger<BannerRepository> _logger;
    public BannerRepository(AppDbContext context, ILogger<BannerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Banner> GetBannerByIdAsync(int bannerId)
    {
        try
        {
            var banner = await _context.Banners.FirstOrDefaultAsync(banner => banner.Id == bannerId);
            if (banner == null) throw new NotFoundException($"Banner with id: {banner.Id} was not found");
            return banner;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(message: e.Message);
            throw new RepositoryException("Error occurred while retrieving the banner", e);
        }
       
    }
    public async Task<List<Banner>> GetAllAsync()
    {
        try
        {
            return await _context.Banners.ToListAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(message: e.Message);
            throw new RepositoryException("Error occurred while retrieving the banners", e);
        }
       
    }

    public async Task<bool> AddAsync(Banner banner)
    {
        try
        {
             await _context.Banners.AddAsync(banner);
             return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
           _logger.LogError(message: e.Message);
           throw new RepositoryException("Error occurred while adding the banner", e);
        }
       
    }

    public async Task<bool> UpdateAsync(Banner banner)
    {
        try
        {
            var existingBanner = await _context.Banners.FindAsync(banner.Id);
            if (existingBanner == null) throw new NotFoundException($"Banner with id: {banner.Id} was not found");
            _context.Banners.Update(banner);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while updating the banner", e);
        }
        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) throw new NotFoundException($"Banner with id: {banner.Id} was not found");
            _context.Banners.Remove(banner);
            return await _context.SaveChangesAsync() > 0;
        }
       
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            throw new RepositoryException("Error occurred while deleting the banner", e);
        }
        
    }
}