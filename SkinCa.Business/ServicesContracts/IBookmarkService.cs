using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IBookmarkService
{
    Task<List<BookmarkResponseDto>> GetAllByUserIdAsync(string userId);
    Task CreateAsync(BookmarkRequestDto bookMark);
    Task  DeleteAsync(int id);
}