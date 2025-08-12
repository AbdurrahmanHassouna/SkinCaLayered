using SkinCa.Business.DTOs.Message;

namespace SkinCa.Business.ServicesContracts;


public interface IMessageService
{
    Task<MessageResponseDto> CreateMessageAsync(MessageRequestDto dto);
    Task<MessageResponseDto> EditMessageAsync(EditMessageRequestDto dto);
    Task DeleteMessageAsync(int messageId);
}