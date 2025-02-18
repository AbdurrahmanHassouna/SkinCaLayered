using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ChatRepository> _logger;

        public ChatRepository(AppDbContext context, ILogger<ChatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateAsync(Chat chat)
        {
            try
            {
                await _context.Chats.AddAsync(chat);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while creating the chat.");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while creating the chat.");
                throw new RepositoryException("Error occurred while creating the chat.", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var chat = await _context.Chats.FindAsync(id);
                if (chat == null)
                    throw new NotFoundException($"Chat with id: {id} not found.");

                _context.Chats.Remove(chat);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while deleting the chat.");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while deleting the chat with id: {ChatId}", id);
                throw new RepositoryException("Error occurred while deleting the chat.", e);
            }
        }

        public async Task<List<Chat>> GetAllByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Chats
                    .Where(c => c.Users.Any(u => u.Id == userId))
                    .ToListAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error retrieving chats in {Method} for user with id: {UserId}", nameof(GetAllByUserIdAsync), userId);
                throw new RepositoryException("Error occurred while retrieving chats.", e);
            }
        }

        public async Task<Chat?> GetByIdAsync(int chatId)
        {
            try
            {
                return await _context.Chats
                    .Include(c => c.Users)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == chatId);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error retrieving chat in {Method} for chat id: {ChatId}", nameof(GetByIdAsync), chatId);
                throw new RepositoryException("Error occurred while retrieving chat.", e);
            }
        }

        public async Task DeleteMessagesAsync(int chatId)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null)
                    throw new NotFoundException($"Chat with id: {chatId} not found.");

                _context.Messages.RemoveRange(chat.Messages);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while deleting messages.");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error occurred while deleting messages for chat with id: {ChatId}", chatId);
                throw new RepositoryException("Error occurred while deleting messages.", e);
            }
        }
    }
}
