namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IBannerRepository
{
    Task<Banner?> GetBannerByIdAsync(int bannerId);
    Task<List<Banner>> GetAllAsync();
    Task<bool> AddAsync(Banner banner);
    Task<bool?> UpdateAsync(Banner banner);
    Task<bool?> DeleteAsync(int id);
}