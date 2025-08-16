using System.ComponentModel.DataAnnotations;

namespace SkinCa.DataAccess
{
    public class Message:Entity<int>
    {
        [MaxLength(400)]
        public string? Content { get; set; }
        public string?  ImageURL { get; set; }
        public int ChatId { get; set; }
        public MStatus Status { get; set; }
        public virtual Chat Chat { get; set; }
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}