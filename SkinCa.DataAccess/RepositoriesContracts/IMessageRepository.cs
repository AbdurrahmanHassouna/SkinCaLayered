namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int id);
    Task CreateAsync(Message massage);
    Task EditAsync(Message massage);
    Task DeleteAsync(int messageId);
    
}