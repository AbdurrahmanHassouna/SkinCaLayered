namespace SkinCa.Business.DTOs.WorkingDay;

public class WorkingDayDto
{
    public TimeSpan OpenAt { get; set; }
    public TimeSpan CloseAt { get; set; }
    public DayOfWeek Day { get; set; }
}