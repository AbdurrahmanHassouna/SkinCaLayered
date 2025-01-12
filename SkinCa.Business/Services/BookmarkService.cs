using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess.RepositoriesContracts;
using SkinCa.DataAccess;

namespace SkinCa.Business.Services;

public class BookmarkService:IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;

    public BookmarkService(IBookmarkRepository bookmarkRepository)
    {
        _bookmarkRepository = bookmarkRepository;
    }
    
    public async Task<List<BookmarkResponseDto>> GetAllByUserIdAsync(string userId)
    {
        var bookmarks = await _bookmarkRepository.GetAllByUserIdAsync(userId);
        return bookmarks.Select(b=>new BookmarkResponseDto
            {
                Title = b.Disease.Title,
                DiseaseId = b.DiseaseId,
                Image = b.Disease.Image
            }
        ).ToList();
    }

    public async Task<bool?> CreateAsync(BookmarkRequestDto bookmark)
    {
        var newBookmark = new Bookmark()
        {
            DiseaseId = bookmark.DiseaseId,
            UserId = bookmark.UserId
        };
        return await _bookmarkRepository.CreateAsync(newBookmark);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _bookmarkRepository.DeleteAsync(id);
    }
}