using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SkinCa.DataAccess
{
    public class BookMark:Entity<int>
    {
        public int DiseaseId {  get; set; }
        public Disease Disease { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}