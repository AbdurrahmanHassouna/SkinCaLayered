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
            try
            {
                return await _context.Bookmarks
                    .Where(b => b.UserId == userId)
                    .Include(b => b.Disease)
                    .ToListAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while retrieving bookmarks for user with id: {UserId}", userId);
                throw new RepositoryException("Error occurred while retrieving the bookmarks", e);
            }
        }

        public async Task CreateAsync(Bookmark bookmark)
        {
            try
            {
                await _context.Bookmarks.AddAsync(bookmark);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while adding the bookmark");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while adding the bookmark");
                throw new RepositoryException("Error occurred while adding the bookmark", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var bookmark = await _context.Bookmarks.FindAsync(id);
                if (bookmark == null)
                    throw new NotFoundException($"Bookmark with id: {id} was not found");

                _context.Bookmarks.Remove(bookmark);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while deleting the bookmark");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while deleting the bookmark with id: {BookmarkId}", id);
                throw new RepositoryException("Error occurred while deleting the bookmark", e);
            }
        }
    }
}
