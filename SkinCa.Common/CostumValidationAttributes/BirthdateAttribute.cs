using System.ComponentModel.DataAnnotations;

namespace SkinCa.Common.CostumValidationAttributes;

public class BirthdateAttribute:ValidationAttribute
{
    private readonly int _minimumAge;

    public BirthdateAttribute(int minimumAge = 18)
    {
        _minimumAge = minimumAge;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime birthdate)
        {
            var age = DateTime.Now.Year - birthdate.Year;

            
            if (birthdate > DateTime.Now.AddYears(-_minimumAge))
            {
                return new ValidationResult($"Age must be at least {_minimumAge} years old.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }
}