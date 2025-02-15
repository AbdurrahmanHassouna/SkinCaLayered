using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ChatRepository> _logger;

    public ChatRepository(AppDbContext context, ILogger<ChatRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Chat chat)
    {
        try
        {
            await _context.Chats.AddAsync(chat);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error creating chat ");
            throw new RepositoryException("Error occurred while creating chat", e);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null) throw new NotFoundException($"Chat with id: {id} not found");
            
            _context.Chats.Remove(chat);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error deleting chat");
            throw new RepositoryException("Error occurred while deleting chat", e);
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
            _logger.LogError(e, $"Error retrieving chats{nameof(GetAllByUserIdAsync)}");
            throw new RepositoryException("Error occurred while retrieving chats", e);
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
            _logger.LogError(e, $"Error retrieving chat at {nameof(GetByIdAsync)}");
            throw new RepositoryException("Error occurred while retrieving chat", e);
        }
    }

    public async Task<bool> DeleteMessagesAsync(int chatId)
    {
        try
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null) throw new NotFoundException($"Chat with id: {chatId} not found");

            _context.Messages.RemoveRange(chat.Messages);
            return await _context.SaveChangesAsync() > 0;
        }
        catch(DbUpdateException e)
        {
            _logger.LogError(e, "Database error");
            throw new RepositoryException("Error occurred while deleting messages", e);
        }
    }
}