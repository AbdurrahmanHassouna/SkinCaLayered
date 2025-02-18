namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IBookmarkRepository
{
    Task<List<Bookmark>> GetAllByUserIdAsync(string userId);
    Task CreateAsync(Bookmark bookmark);
    Task DeleteAsync(int id);
    
}