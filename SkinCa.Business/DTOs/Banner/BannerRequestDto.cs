using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SkinCa.Common.CostumValidationAttributes;

namespace SkinCa.Business.DTOs
{
    public class BannerRequestDto
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Image([".jpeg",".png"])]
        public IFormFile File { get; set; }
    }
}