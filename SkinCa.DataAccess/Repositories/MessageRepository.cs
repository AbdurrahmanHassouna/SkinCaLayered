using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(AppDbContext context, ILogger<MessageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task EditAsync(Message message)
        {
            try
            {
                var existingMessage = await _context.Messages.FindAsync(message.Id);
                if (existingMessage == null)
                    throw new NotFoundException($"Message with ID {message.Id} not found");

                existingMessage.Content = message.Content;
                existingMessage.Image = message.Image;
                existingMessage.Status = message.Status;

                _context.Messages.Update(existingMessage);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while updating the message");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating message {MessageId}", message.Id);
                throw new RepositoryException("Error updating message", ex);
            }
        }

        public async Task CreateAsync(Message message)
        {
            try
            {
                await _context.Messages.AddAsync(message);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while creating the message");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating message");
                throw new RepositoryException("Error creating message", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);
                if (message == null)
                    throw new NotFoundException($"Message with ID {id} not found");

                _context.Messages.Remove(message);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while deleting the message");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting message {MessageId}", id);
                throw new RepositoryException("Error deleting message", ex);
            }
        }
    }
}
