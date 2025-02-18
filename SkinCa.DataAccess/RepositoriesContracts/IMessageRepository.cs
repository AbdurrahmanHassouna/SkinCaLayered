namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IMessageRepository
{
    Task CreateAsync(Message massage);
    Task EditAsync(Message massage);
    Task DeleteAsync(int messageId);
}