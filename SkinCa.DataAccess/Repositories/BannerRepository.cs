using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class BannerRepository:IBannerRepository
{
    public Task<Banner> GetBannerAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Banner>> GetBannersAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddBannerAsync(Banner banner)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBannerAsync(Banner banner)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBannerAsync(int id)
    {
        throw new NotImplementedException();
    }
}