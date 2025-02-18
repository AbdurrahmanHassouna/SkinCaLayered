using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<BannerRepository> _logger;

    public BannerRepository(AppDbContext context, ILogger<BannerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Banner> GetBannerByIdAsync(int bannerId)
    {
        try
        {
            var banner = await _context.Banners.FirstOrDefaultAsync(b => b.Id == bannerId);
            if (banner == null)
                throw new NotFoundException($"Banner with id: {bannerId} was not found");

            return banner;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error occurred while retrieving the banner with id: {BannerId}", bannerId);
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
            _logger.LogError(e, "Error occurred while retrieving banners");
            throw new RepositoryException("Error occurred while retrieving the banners", e);
        }
    }

    public async Task AddAsync(Banner banner)
    {
        try
        {
            await _context.Banners.AddAsync(banner);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while adding the banner");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error occurred while adding the banner");
            throw new RepositoryException("Error occurred while adding the banner", e);
        }
    }

    public async Task UpdateAsync(Banner banner)
    {
        try
        {
            var existingBanner = await _context.Banners.FindAsync(banner.Id);
            if (existingBanner == null)
                throw new NotFoundException($"Banner with id: {banner.Id} was not found");
            
            existingBanner.Title = banner.Title;
            existingBanner.Description = banner.Description;
            existingBanner.Image = banner.Image;

            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while updating the banner");

        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error occurred while updating the banner with id: {BannerId}", banner.Id);
            throw new RepositoryException("Error occurred while updating the banner", e);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                throw new NotFoundException($"Banner with id: {id} was not found");

            _context.Banners.Remove(banner);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting the banner");

           
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error occurred while deleting the banner with id: {BannerId}", id);
            throw new RepositoryException("Error occurred while deleting the banner", e);
        }
    }
}
