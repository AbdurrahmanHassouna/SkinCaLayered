using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IBookmarkRepository
{
    
    Task<List<BookMark>> GetAllBookMarksByUserIdAsync(string userId);
    Task<BookMark> CreateAsync(BookMark bookMark);
    Task DeleteAsync(BookMark bookMark);
    
    
}