using System.ComponentModel.DataAnnotations;
using SkinCa.Business.DTOs.WorkingDay;

namespace SkinCa.Business.DTOs.DoctorInfo;

public class DoctorInfoRequestDto : RegistrationRequestDto
{ 
    public int Experience { get; set; } 

    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid currency format.")]
    public decimal ClinicFees { get; set; } 
    [StringLength(1000)]
    public string Description { get; set; }
    [StringLength(10)]
    public string[] Services { get; set; }
    [StringLength(100)]
    public string Specialization { get; set; }
    [MaxLength(7)]
    public WorkingDayDto[] WorkingDays { get; set; }
}