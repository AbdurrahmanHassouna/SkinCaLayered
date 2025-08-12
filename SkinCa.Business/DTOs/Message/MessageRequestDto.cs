using System.ComponentModel.DataAnnotations;
using SkinCa.Common.CostumValidationAttributes;

namespace SkinCa.Business.DTOs.Message;

public class MessageRequestDto
{
    [MaxLength(1000)]
    public string? Content { get; set; }
    [Image(["png","jpeg"])]
    public byte[]? Image { get; set; }
    public int ChatId { get; set; }
    public string SenderId { get; set; }
}