using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IBannerRepository
{
    Task<Banner> GetBannerAsync(int id);
    Task<List<Banner>> GetBannersAsync();
    Task AddBannerAsync(Banner banner);
    Task UpdateBannerAsync(Banner banner);
    Task DeleteBannerAsync(int id);
}