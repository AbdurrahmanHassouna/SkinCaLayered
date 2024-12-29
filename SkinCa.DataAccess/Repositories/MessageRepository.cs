using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class MessageRepository:IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool?> EditAsync(Message message)
    {
        var existingMessage = await _context.Messages.FindAsync(message.Id);
        if (existingMessage == null) return null;
        
        existingMessage.Content = message.Content;
        existingMessage.Image=message.Image;
        existingMessage.Status=message.Status;
        
        _context.Messages.Update(existingMessage);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return null;
        _context.Messages.Remove(message);
        return await _context.SaveChangesAsync() > 0;
    }
}