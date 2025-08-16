using System.ComponentModel.DataAnnotations;
using SkinCa.Common.CostumValidationAttributes;

namespace SkinCa.Business.DTOs.Message;

public class MessageRequestDto
{
    [MaxLength(1000)]
    public string? Content { get; set; }
    
    public string? ImageURL { get; set; }
    public int ChatId { get; set; }
}