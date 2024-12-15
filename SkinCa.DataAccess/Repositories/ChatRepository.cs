using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class ChatRepository: IChatRepository    
{
    public Task CreateAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public Task<List<Chat>> GetAllByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat> GetByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DelesteMessagesAsync(int chatId)
    {
        throw new NotImplementedException();
    }
}