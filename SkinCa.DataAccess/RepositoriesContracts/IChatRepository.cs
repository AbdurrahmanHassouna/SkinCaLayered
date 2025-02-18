namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IChatRepository
{
    Task CreateAsync(Chat chat);
    Task  DeleteAsync(int id);
    Task<List<Chat>> GetAllByUserIdAsync(string userId);
    Task<Chat> GetByIdAsync(int chatId);
    Task DeleteMessagesAsync(int chatId);
    
}