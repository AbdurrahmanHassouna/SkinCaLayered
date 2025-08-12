using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookmarkRepository> _logger;

        public BookmarkRepository(AppDbContext context, ILogger<BookmarkRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Bookmark>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Bookmarks
                .Where(b => b.UserId == userId)
                .Include(b => b.Disease)
                .ToListAsync();
        }
        public async Task CreateAsync(Bookmark bookmark)
        {
            await _context.Bookmarks.AddAsync(bookmark);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while adding the bookmark");
        }

        public async Task DeleteAsync(string userId,int id)
        {
            var bookmark = await _context.Bookmarks.FindAsync(id);
            if (bookmark == null || bookmark.UserId != userId)
                throw new NotFoundException($"Bookmark was not found");
            
            _context.Bookmarks.Remove(bookmark);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting the bookmark");
        }
    }
}