using System.Security.Claims;
using SkinCa.Business.DTOs.Chat;

namespace SkinCa.Business.ServicesContracts;

public interface IChatService
{
    Task<ChatResponseDto> CreateChatAsync(string userId,ChatRequestDto dto);
    Task DeleteChatAsync(string userId,int chatId);
    Task<ChatResponseDto> GetChatByIdAsync(int chatId);
    Task<List<ChatResponseDto>> GetChatsByUserIdAsync(string userId);
    Task<ChatResponseDto> GetChatByUsersIdAsync(string senderId, string receiverId);
    Task DeleteChatMessagesAsync(int chatId);
}