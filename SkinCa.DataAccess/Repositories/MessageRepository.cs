using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class MessageRepository:IMessageRepository
{
    public Task SendMessage(int chat, Message massage)
    {
        throw new NotImplementedException();
    }

    public Task<List<Message>> GetMessages(int chatId)
    {
        throw new NotImplementedException();
    }

    public Task<Message> EditMessage(Message massage)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMessage(int messageId)
    {
        throw new NotImplementedException();
    }
}