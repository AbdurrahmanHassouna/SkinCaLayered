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
            await _context.Chats.AddAsync(chat);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while creating the chat.");
        }

        public async Task DeleteAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
                throw new NotFoundException($"Chat was not found.");

            _context.Chats.Remove(chat);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting the chat.");
        }

        public async Task<List<Chat>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }

        public async Task<Chat> GetByIdAsync(int chatId)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null)
            {
                throw new NotFoundException($"Chat was not found.");
            }
            return chat;
        }

        public async Task DeleteMessagesAsync(int chatId)
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                throw new NotFoundException($"Chat was not found.");

            _context.Messages.RemoveRange(chat.Messages);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting messages.");
        }
    }
}