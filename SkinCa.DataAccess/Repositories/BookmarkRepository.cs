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

    public async Task<List<Bookmark>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Bookmarks.Where(b => b.UserId == userId).Include(b=>b.Disease).ToListAsync();
    }

    public async Task<bool?> CreateAsync(Bookmark bookmark)
    {
        await _context.Bookmarks.AddAsync(bookmark);
        
       
        
        return await _context.SaveChangesAsync() >0;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var bookmark = await _context.Bookmarks.FindAsync(id);
        if (bookmark == null) return null;
        _context.Bookmarks.Remove(bookmark);
        return await _context.SaveChangesAsync() > 0;

    }
}