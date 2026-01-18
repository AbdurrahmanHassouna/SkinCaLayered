using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [Authorize(Roles = "User")]
        public async Task<ChatResponseDto> CreateChatAsync(string userId, ChatRequestDto dto)
        {
            var sender = await _userManager.FindByIdAsync(userId);
            var receiver = await _userManager.FindByIdAsync(dto.DoctorId);
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

        public async Task DeleteChatAsync(string userId, int chatId)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            var user = await _userManager.FindByIdAsync(userId);

            if (chat == null)
            {
                _logger.LogWarning("DeleteChatAsync: chat {ChatId} not found", chatId);
                throw new NotFoundException("Chat not found");
            }

            if (user == null)
            {
                _logger.LogWarning("DeleteChatAsync: user from claims not found");
                throw new NotFoundException("User not found");
            }

            var userChat = chat.ApplicationUserChats.FirstOrDefault(u => u.UserId == user.Id);
            if (userChat == null)
            {
                _logger.LogWarning("DeleteChatAsync: user {UserId} is not part of chat {ChatId}", user.Id, chatId);
                throw new ServiceException("Unauthorized to delete this chat");
            }


            userChat.IsDeleted = true;
            await _userChatRepository.UpdateAsync(userChat);
            _logger.LogInformation("DeleteChatAsync: user {UserId} marked chat {ChatId} as deleted", user.Id, chatId);


            chat = await _chatRepository.GetByIdAsync(chatId);

            if (chat.ApplicationUserChats.All(a => a.IsDeleted))
            {
                _logger.LogInformation("DeleteChatAsync: all participants deleted chat {ChatId}; deleting chat",
                    chatId);
                await _chatRepository.DeleteAsync(chatId);
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
            if (chat == null)
            {
                return await CreateChatAsync(senderId, new ChatRequestDto() { DoctorId = receiverId });
            }
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
                SenderId = message.SenderId,
            };
        }
    }
}