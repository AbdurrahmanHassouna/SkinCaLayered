using SkinCa.Business.DTOs;

namespace SkinCa.Business.ServicesContracts;

public interface IBookmarkService
{
    Task<List<BookmarkResponseDto>> GetAllByUserIdAsync(string userId);
    Task<bool?> CreateAsync(BookmarkRequestDto bookMark);
    Task<bool?>  DeleteAsync(int id);
}