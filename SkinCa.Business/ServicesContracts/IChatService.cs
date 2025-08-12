using SkinCa.Business.DTOs.Chat;

namespace SkinCa.Business.ServicesContracts;

public interface IChatService
{
    Task<ChatResponseDto> CreateChatAsync(ChatRequestDto dto);
    Task DeleteChatAsync(int chatId);
    Task<ChatResponseDto> GetChatByIdAsync(int chatId);
    Task<List<ChatResponseDto>> GetChatsByUserIdAsync(string userId);
    Task DeleteChatMessagesAsync(int chatId);
}