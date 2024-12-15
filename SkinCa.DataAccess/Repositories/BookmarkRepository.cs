using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class BookmarkRepository:IBookmarkRepository
{
    public Task<List<BookMark>> GetAllBookMarksByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<BookMark> CreateAsync(BookMark bookMark)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(BookMark bookMark)
    {
        throw new NotImplementedException();
    }
}