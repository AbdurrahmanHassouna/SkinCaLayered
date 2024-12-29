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
            if (Array.Exists(validTypes, val => val.Equals(ext)))
            {
                return ValidationResult.Success;
            }
            else return new ValidationResult($"Invalid type, Allowed types are {string.Join(',',validTypes)}.");   
        }
        return new ValidationResult("No file");
    }
}