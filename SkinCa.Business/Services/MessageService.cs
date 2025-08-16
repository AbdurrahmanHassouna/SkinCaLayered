using Microsoft.Extensions.Logging;
using SkinCa.Business.DTOs.Message;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
      
        private readonly ILogger<MessageService> _logger;
        private IMessageService _messageServiceImplementation;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<MessageResponseDto> CreateMessageAsync(MessageRequestDto dto)
        {
            var message = new Message
            {
                Content = dto.Content,
                ImageURL = dto.ImageURL,
                ChatId = dto.ChatId,
                Status = MStatus.Sent
                
            };

            await _messageRepository.CreateAsync(message);
            

            return MapMessageToDto(message);
        }
        public async Task DeleteMessageAsync(int messageId)
        {
            await _messageRepository.DeleteAsync(messageId);
        }

        // --- Helper mapping method ---
        private MessageResponseDto MapMessageToDto(Message message)
        {
            return new MessageResponseDto
            {
                Id = message.Id,
                Content = message.Content,
                ImageURL = message.ImageURL,
                ChatId = message.ChatId,
                Status = message.Status
            };
        }
    }
}
