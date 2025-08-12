using System.ComponentModel.DataAnnotations;

namespace SkinCa.DataAccess
{
    public class DoctorInfo
    {
        [Key]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Experience {  get; set; }
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal ClinicFees {  get; set; }
        public string Services {  get; set; }
        public string Specialization { get; set; }
        public virtual ICollection<DoctorWorkingDay> WorkingDays { get; set; }
        DateTime Created{ get; set; }
        DateTime LastModified{ get; set; }
    }
}
