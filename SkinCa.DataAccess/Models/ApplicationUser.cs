using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SkinCa.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)] public string FirstName { get; set; }
        [MaxLength(100)] public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public short Governorate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        public byte[] ProfilePicture { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
        public List<Disease> Diseases { get; set; }
        public List<ScanResult> ModelResults { get; set; }
        public List<Banner> Banners { get; set; }
        public ICollection<ApplicationUserChat> ApplicationUserChats { get; set; }
        public List<Chat> Chats { get; set; }
        public DoctorInfo DoctorInfo { get; set; }
    }

   
}