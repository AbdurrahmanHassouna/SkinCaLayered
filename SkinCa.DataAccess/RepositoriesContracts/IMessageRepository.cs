namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IMessageRepository
{
    Task<bool> CreateAsync(Message massage);
    Task<bool?> EditAsync(Message massage);
    Task<bool?>  DeleteAsync(int messageId);
}