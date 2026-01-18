using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;
using SkinCa.Common.UtilityExtensions;
using SkinCa.DataAccess;

namespace SkinCa.Business.Services;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _bannerRepository;

    public BannerService(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<List<BannerResponseDto>> GetAllAsync()
    {
        var banners = await _bannerRepository.GetAllAsync();
        var bannerDtos = banners.Select(n => new BannerResponseDto()
        {
            Id = n.Id,
            Title = n.Title,
            Description = n.Description,
            Image = n.Image,
        });
        return bannerDtos.ToList();
    }

    public async Task EditAsync(int id, BannerRequestDto bannerRequestDto)
    {
        var banner = new Banner()
        {
            Id = id,
            Description = bannerRequestDto.Description,
            Title = bannerRequestDto.Title
        };
        
        if (bannerRequestDto.File.Length > 2*1000) throw new ServiceException("large file size");
        banner.Image = await bannerRequestDto.File.ToBytesAsync();
        await _bannerRepository.UpdateAsync(banner);
    }

    public async Task DeleteAsync(int id)
    {
         await _bannerRepository.DeleteAsync(id);
    }

    public async Task CreateAsync(BannerRequestDto bannerRequestDto)
    {
        var banner = new Banner()
        {
            Description = bannerRequestDto.Description,
            Title = bannerRequestDto.Title,
        };
        if (bannerRequestDto.File.Length > 2*1000) throw new ServiceException("large file size");
        banner.Image = await bannerRequestDto.File.ToBytesAsync();

        await _bannerRepository.AddAsync(banner);
    }
}