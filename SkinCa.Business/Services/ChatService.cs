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
        private readonly AppDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatRepository chatRepository, AppDbContext context, ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<ChatResponseDto> CreateChatAsync(ChatRequestDto dto)
        {
            // Create a new chat and assign users based on the provided IDs.
            var chat = new Chat
            {
                Users = await _context.Users
                    .Where(u => dto.UserIds.Contains(u.Id))
                    .ToListAsync()
            };

            await _chatRepository.CreateAsync(chat);

            // Retrieve the created chat (with users and messages)
            var createdChat = await _chatRepository.GetByIdAsync(chat.Id);
            if (createdChat == null)
                throw new RepositoryException("Chat creation failed; chat not found after creation.");

            return MapChatToDto(createdChat);
        }

        public async Task DeleteChatAsync(int chatId)
        {
            await _chatRepository.DeleteAsync(chatId);
        }

        public async Task<ChatResponseDto> GetChatByIdAsync(int chatId)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat == null)
                throw new NotFoundException($"Chat with ID {chatId} not found");

            return MapChatToDto(chat);
        }

        public async Task<List<ChatResponseDto>> GetChatsByUserIdAsync(string userId)
        {
            var chats = await _chatRepository.GetAllByUserIdAsync(userId);
            return chats.Select(c => MapChatToDto(c)).ToList();
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
                Image = message.Image,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                Status = message.Status,
            };
        }
    }
}
