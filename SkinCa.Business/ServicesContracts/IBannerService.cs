using SkinCa.DataAccess;

using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IBannerService
{
    Task<List<BannerResponseDto>> GetAllAsync();
    Task<BannerResponseDto?> EditAsync(int id, BannerRequestDto bannerRequestDto);
    Task<bool?> DeleteAsync(int id);
    Task CreateAsync(BannerRequestDto bannerRequestDto);
}