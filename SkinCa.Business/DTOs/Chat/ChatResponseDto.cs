using SkinCa.Business.DTOs.Message;

namespace SkinCa.Business.DTOs.Chat;

public class ChatResponseDto
{
    public int Id { get; set; }
    public List<ChatUserDto> Participants { get; set; } = new();
    public List<MessageResponseDto> Messages { get; set; } = new();
}