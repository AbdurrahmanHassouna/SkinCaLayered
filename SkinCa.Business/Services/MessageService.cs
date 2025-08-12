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
                Image = dto.Image,
                ChatId = dto.ChatId,
                SenderId = dto.SenderId,
                Status = MStatus.Sent
                
            };

            await _messageRepository.CreateAsync(message);
            

            return MapMessageToDto(message);
        }

        public async Task<MessageResponseDto> EditMessageAsync(EditMessageRequestDto dto)
        {
            // Retrieve the existing message.
            var message = await _messageRepository.GetByIdAsync(dto.Id);

            // Update properties.
            message.Content = dto.Content;
            message.Image = dto.Image;

            await _messageRepository.EditAsync(message);

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
                Image = message.Image,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                Status = message.Status
            };
        }
    }
}
