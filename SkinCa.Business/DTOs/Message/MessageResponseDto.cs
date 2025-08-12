using SkinCa.DataAccess;

namespace SkinCa.Business.DTOs.Message;

public class MessageResponseDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public byte[]? Image { get; set; }
    public int ChatId { get; set; }
    public string SenderId { get; set; }
    public MStatus Status { get; set; }
}