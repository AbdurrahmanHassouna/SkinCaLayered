namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IChatRepository
{
    Task<bool> CreateAsync(Chat chat);
    Task<bool>  DeleteAsync(int id);
    Task<List<Chat>> GetAllByUserIdAsync(string userId);
    Task<Chat> GetByIdAsync(int chatId);
    Task<bool>  DeleteMessagesAsync(int chatId);
    
}