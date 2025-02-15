using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<MessageRepository> _logger;

    public MessageRepository(AppDbContext context, ILogger<MessageRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> EditAsync(Message message)
    {
        try
        {
            var existingMessage = await _context.Messages.FindAsync(message.Id);
            if (existingMessage == null) throw new NotFoundException($"Message with ID {message.Id} not found");

            existingMessage.Content = message.Content;
            existingMessage.Image = message.Image;
            existingMessage.Status = message.Status;

            _context.Messages.Update(existingMessage);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating message {MessageId}", message.Id);
            throw new RepositoryException("Error updating message", ex);
        }
    }

    public async Task<bool> CreateAsync(Message message)
    {
        try
        {
            await _context.Messages.AddAsync(message);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating message");
            throw new RepositoryException("Error creating message", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) throw new NotFoundException($"Message with ID {id} not found");

            _context.Messages.Remove(message);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting message {MessageId}", id);
            throw new RepositoryException("Error deleting message", ex);
        }
    }
}