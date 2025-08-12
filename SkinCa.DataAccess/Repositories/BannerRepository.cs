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
        return await _context.Banners.ToListAsync();
    }

    public async Task AddAsync(Banner banner)
    {
        await _context.Banners.AddAsync(banner);
        if (await _context.SaveChangesAsync() == 0)
            throw new RepositoryException("No changes were saved to the database while adding the banner");
    }

    public async Task UpdateAsync(Banner banner)
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

    public async Task DeleteAsync(int id)
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null)
            throw new NotFoundException($"Banner with id: {id} was not found");

        _context.Banners.Remove(banner);
        if (await _context.SaveChangesAsync() == 0)
            throw new RepositoryException("No changes were saved to the database while deleting the banner");
    }
}