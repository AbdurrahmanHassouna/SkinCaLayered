using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Business.DTOs.Chat;
using SkinCa.Business.DTOs.Message;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChatService> _logger;
        private readonly IApplicationUserRepository _userChatRepository;
        public ChatService(IChatRepository chatRepository, UserManager<ApplicationUser> userManager,
            ILogger<ChatService> logger,
             IApplicationUserRepository userChat)
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
            _logger = logger;
            _userChatRepository = userChat;
        }

        public async Task<ChatResponseDto> CreateChatAsync(ClaimsPrincipal claimsPrincipal, ChatRequestDto dto)
        {
            var sender = await _userManager.GetUserAsync(claimsPrincipal);
            var receiver = await _userManager.FindByIdAsync(dto.UserId);
            
            if (sender == null || receiver == null) throw new NotFoundException("User not found");
            var chat = new Chat
            {
                ApplicationUserChats = new List<ApplicationUserChat>()
                {
                    new ApplicationUserChat
                    {
                        IsDeleted = false,
                        User = sender
                    },
                    new ApplicationUserChat
                    {
                        IsDeleted = false,
                        User = receiver
                    }
                }
            };
            
            await _chatRepository.CreateAsync(chat);
           
            var createdChat = await _chatRepository.GetByIdAsync(chat.Id);
            
            return MapChatToDto(createdChat);
        }

        public async Task DeleteChatAsync(ClaimsPrincipal claimsPrincipal,int chatId)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            
            var applicationUserChat =  chat.ApplicationUserChats.FirstOrDefault(u => u.UserId== user.Id);
            if (applicationUserChat == null)
            {
                throw new ServiceException("unauthorized");
            }
            applicationUserChat.IsDeleted = true;
            if (chat.ApplicationUserChats.Any(a => a.IsDeleted == false))
            {
                await _chatRepository.DeleteAsync(chatId);
            }
            else
            {
                await _userChatRepository.UpdateAsync(applicationUserChat);
            }
        }

        public async Task<ChatResponseDto> GetChatByIdAsync(int chatId)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            
            return MapChatToDto(chat);
        }

        public async Task<List<ChatResponseDto>> GetChatsByUserIdAsync(string userId)
        {
            var chats = await _chatRepository.GetAllByUserIdAsync(userId);
            return chats.Select(MapChatToDto).ToList();
        }

        public async Task<ChatResponseDto> GetChatByUsersIdAsync(string senderId, string receiverId)
        {
            var chat = await _chatRepository.GetChatByUsersIdAsync(senderId, receiverId);
            return MapChatToDto(chat);
        }

        public async Task DeleteChatMessagesAsync(int chatId)
        {
            await _chatRepository.DeleteMessagesAsync(chatId);
        }

        // --- Helper mapping method ---
        private ChatResponseDto MapChatToDto(Chat chat)
        {
            return new ChatResponseDto()
            {
                Id = chat.Id,
                Participants = chat.Users?.Select(u => new ChatUserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName
                }).ToList() ?? new List<ChatUserDto>(),
                Messages = chat.Messages?.Select(m => MapMessageToDto(m)).ToList() ?? new List<MessageResponseDto>()
            };
        }

        private MessageResponseDto MapMessageToDto(Message message)
        {
            return new MessageResponseDto()
            {
                Id = message.Id,
                Content = message.Content,
                ImageURL = message.ImageURL,
                ChatId = message.ChatId,
                Status = message.Status,
            };
        }
    }
}