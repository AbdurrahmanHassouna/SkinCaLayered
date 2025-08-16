namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IChatRepository
{
    Task CreateAsync(Chat chat);
    Task  DeleteAsync(int id);
    Task<List<Chat>> GetAllByUserIdAsync(string userId);
    Task<Chat?> GetChatByUsersIdAsync(string senderId, string receiverId);
    Task<Chat> GetByIdAsync(int chatId);
    Task DeleteMessagesAsync(int chatId);
    
}