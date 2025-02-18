namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IBannerRepository
{
    Task<Banner> GetBannerByIdAsync(int bannerId);
    Task<List<Banner>> GetAllAsync();
    Task AddAsync(Banner banner);
    Task UpdateAsync(Banner banner);
    Task DeleteAsync(int id);
}