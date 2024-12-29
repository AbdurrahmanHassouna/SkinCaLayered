namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IBookmarkRepository
{
    
    Task<List<BookMark>> GetAllByUserIdAsync(string userId);
    Task<BookMark> CreateAsync(BookMark bookMark);
    Task<bool?>  DeleteAsync(int id);
    
    
}