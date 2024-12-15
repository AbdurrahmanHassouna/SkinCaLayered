using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace SkinCa.DataAccess
{
    public class Chat :Entity<int>
    {
        public virtual ICollection<ApplicationUserChat> ApplicationUserChats { get; set; }
        [MaxLength(2),JsonIgnore]
        public  List<ApplicationUser> Users { get; set; }
        public  List<Message> Messages { get; set; }
        
    }
}
