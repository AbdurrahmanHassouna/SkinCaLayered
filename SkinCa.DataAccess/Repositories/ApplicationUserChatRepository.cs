using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class ApplicationUserChatRepository:IApplicationUserRepository
{
    private readonly AppDbContext _context;
    
    public ApplicationUserChatRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task UpdateAsync(ApplicationUserChat userChat)
    {
        _context.ApplicationUserChats.Update(userChat);
        if (await _context.SaveChangesAsync() == 0)
            throw new RepositoryException("No changes were saved to the database while updating the UserChat");
    }
}