using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SkinCa.Business.DTOs.Message;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess;

namespace SkinCa.Presentation.Hubs;
[Authorize]
public class ChatHub:Hub
{
    private IChatService _chatService;
    private IMessageService _messageService;
    public ChatHub(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(userId))
        {
            var chats = await _chatService.GetChatsByUserIdAsync(userId);
            foreach (var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chat.Id}");
            }
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(userId))
        {
            var chats = await _chatService.GetChatsByUserIdAsync(userId);
            foreach (var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chat.Id}");
            }
        }
    }
    
    public async Task SendMessage(string  receiverId, string message)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            var chat = await _chatService.GetChatByUsersIdAsync(userId, receiverId);
            //broadcast to the chat
            await Clients.Group($"{chat.Id}").SendAsync("ReceiveMessage", message);
        }
    }
}