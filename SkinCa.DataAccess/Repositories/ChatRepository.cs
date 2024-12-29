using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool>  CreateAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?>  DeleteAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null) return null;
            _context.Chats.Remove(chat);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Chat>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Chats.Where(c => c.Users.Any(a=>a.Id==userId)).ToListAsync();
        }

        public async Task<Chat?> GetByIdAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<bool?>  DeleteMessagesAsync(int chatId)
        {
            var chat = await _context.Chats.Include(c=>c.Messages).FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null) return null;
            _context.Messages.RemoveRange(chat.Messages);
            return await _context.SaveChangesAsync() > 0;
        }
    }