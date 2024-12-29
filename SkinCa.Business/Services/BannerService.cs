using System.Net.Mime;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
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

    public async Task<BannerResponseDto?> EditAsync(int id, BannerRequestDto bannerRequestDto)
    {
        var banner = new Banner()
        {
            Id = id,
            Description = bannerRequestDto.Description,
            Title = bannerRequestDto.Title
        };
        using var memoryStream = new MemoryStream();
        await bannerRequestDto.File.CopyToAsync(memoryStream);
        banner.Image = memoryStream.ToArray();
        var result = await _bannerRepository.UpdateAsync(banner);
        if (result == true)
            return new BannerResponseDto()
            {
                Description = bannerRequestDto.Description, Id = bannerRequestDto.Id, Title = bannerRequestDto.Title,
                Image = banner.Image
            };
        if (result == false) throw new Exception("Couldn't update banner");
        return null;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var result = await _bannerRepository.DeleteAsync(id); 
        
        if (result == false) throw new Exception("Couldn't Delete banner");
        return result;
    }


    public async Task<bool> CreateAsync(BannerRequestDto bannerRequestDto)
    {
        var banner = new Banner()
        {
            Description = bannerRequestDto.Description,
            Title = bannerRequestDto.Title,
            
        };
        using var memoryStream = new MemoryStream();
        await bannerRequestDto.File.CopyToAsync(memoryStream);
        banner.Image = memoryStream.ToArray();
        
       return await _bannerRepository.AddAsync(banner);
    }
}