using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IMessageRepository
{
    Task SendMessage(int chat, Message massage);
    Task<List<Message>> GetMessages(int chatId);
    Task<Message> EditMessage(Message massage);
    Task DeleteMessage(int messageId);
}