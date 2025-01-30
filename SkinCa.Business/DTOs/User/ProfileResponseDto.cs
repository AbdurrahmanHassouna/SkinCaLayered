using SkinCa.Common;

namespace SkinCa.Business.DTOs.User;

public class ProfileResponseDto
{
   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int Age { get; set; }
    public Governorate Governorate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; }
    public byte[] ProfilePicture { get; set; }
}