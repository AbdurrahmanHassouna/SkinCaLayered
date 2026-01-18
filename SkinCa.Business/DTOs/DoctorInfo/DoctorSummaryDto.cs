using SkinCa.Common;

namespace SkinCa.Business.DTOs.DoctorInfo;

public class DoctorSummaryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public int Experience {  get; set; }
    public string Specialization { get; set; }
    public Governorate Governorate { get; set; }
    public bool IsWorking { get; set; }
}