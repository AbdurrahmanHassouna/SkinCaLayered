namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IBookmarkRepository
{
    Task<List<Bookmark>> GetAllByUserIdAsync(string userId);
    Task<bool> CreateAsync(Bookmark bookmark);
    Task<bool>  DeleteAsync(int id);
    
}