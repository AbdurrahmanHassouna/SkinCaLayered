using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IChatRepository
{
    Task CreateAsync(Chat chat);
    Task DeleteAsync(Chat chat);
    Task<List<Chat>> GetAllByUserIdAsync(string userId);
    Task<Chat> GetByIdAsync(string userId);
    Task DelesteMessagesAsync(int chatId);
    
}