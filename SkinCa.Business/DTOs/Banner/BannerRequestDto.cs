using Microsoft.AspNetCore.Http;
namespace SkinCa.Business.DTOs
{
    public class BannerRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}