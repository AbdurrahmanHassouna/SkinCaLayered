using System.ComponentModel.DataAnnotations;

namespace SkinCa.DataAccess
{
    public class Message:Entity<int>
    {
        [Length(1,300)]
        public string? Content { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}