using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly AppDbContext _context;

    public BookmarkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookMark>> GetAllByUserIdAsync(string userId)
    {
        return await _context.BookMarks.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task<BookMark> CreateAsync(BookMark bookMark)
    {
        await _context.BookMarks.AddAsync(bookMark);
        await _context.SaveChangesAsync();
        return bookMark;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var bookMark = await _context.BookMarks.FindAsync(id);
        if (bookMark == null) return null;
        _context.BookMarks.Remove(bookMark);
        return await _context.SaveChangesAsync() > 0;

    }
}