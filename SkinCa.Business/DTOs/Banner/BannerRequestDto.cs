using Microsoft.AspNetCore.Http;
namespace SkinCa.Business.DTOs
{
    public class BannerRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}