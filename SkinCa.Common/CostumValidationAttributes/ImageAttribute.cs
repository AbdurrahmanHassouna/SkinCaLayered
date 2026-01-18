using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace SkinCa.Common.CostumValidationAttributes;

public class ImageAttribute:ValidationAttribute
{
    private readonly string[] validTypes;

    public ImageAttribute(string[] validTypes)
    {
        this.validTypes = validTypes;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            string ext = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!Array.Exists(validTypes, val => val.Equals(ext)))
            {
                return new ValidationResult($"Invalid type, Allowed types are {string.Join(',',validTypes)}."); 
            }
            else if (file.Length == 0)
            {
                return new ValidationResult("File is empty.");
            }
            else if (file.Length > 10_000_000)
            {
                return new ValidationResult($"Invalid file size, Allowed size is >= 10 MB.");  
            }
            else  return ValidationResult.Success;
        }
        return new ValidationResult("File is required.");
    }
}