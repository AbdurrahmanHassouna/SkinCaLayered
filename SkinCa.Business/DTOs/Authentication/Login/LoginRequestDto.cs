using System.ComponentModel.DataAnnotations;

namespace SkinCa.Business.DTOs
{
    public class LoginRequestDto
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, lowercase letter, number, and special character.")]
        public string Password { get; set; }
    }
}
