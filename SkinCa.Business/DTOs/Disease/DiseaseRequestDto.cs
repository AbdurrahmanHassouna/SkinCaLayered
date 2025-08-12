using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using SkinCa.Common.CostumValidationAttributes;

namespace SkinCa.Business.DTOs
{
    public class DiseaseRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "An image is required.")]
        [Image(["png","jpeg"])]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Specialty must be between 3 and 50 characters.")]
        public string Specialty { get; set; }

        [Required(ErrorMessage = "Symptoms are required.")]
        public string Symptoms { get; set; }

        [StringLength(200, ErrorMessage = "Types must not exceed 200 characters.")]
        public string? Types { get; set; }

        [StringLength(200, ErrorMessage = "Causes must not exceed 200 characters.")]
        public string? Causes { get; set; }

        [StringLength(500, ErrorMessage = "Diagnostic methods must not exceed 500 characters.")]
        public string? DiagnosticMethods { get; set; }

        [StringLength(500, ErrorMessage = "Prevention must not exceed 500 characters.")]
        public string? Prevention { get; set; }
    }
}