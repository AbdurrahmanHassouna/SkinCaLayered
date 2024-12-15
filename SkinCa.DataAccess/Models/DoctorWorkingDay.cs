using System.ComponentModel.DataAnnotations;

namespace SkinCa.DataAccess
{
    public class DoctorWorkingDay:Entity<int>
    {
        public int DoctorInfoId { get; set; }
        public DoctorInfo DoctorInfo { get; set; }
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
        public DayOfWeek Day { get; set; }
    }
}
