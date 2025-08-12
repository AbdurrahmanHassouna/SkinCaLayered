using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs.DoctorInfo;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorInfoService _doctorInfoService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(IDoctorInfoService doctorInfoService, ILogger<DoctorController> logger)
        {
            _doctorInfoService = doctorInfoService;
            _logger = logger;
        }

        // GET: api/doctor
        [HttpGet]
        public async Task<ActionResult<IList<DoctorSummaryDto>>> GetDoctors()
        {
            var doctors = await _doctorInfoService.GetDoctorsInfoAsync();
            return Ok(doctors);
        }

        // GET: api/doctor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorInfoResponseDto>> GetDoctorById(string id)
        {
            var doctor = await _doctorInfoService.GetDoctorsInfoAsync(id);
            return Ok(doctor);
        }

        // GET: api/doctor/nearby?latitude=...&longitude=...
        [HttpGet("nearby")]
        public async Task<ActionResult<IList<DoctorSummaryDto>>> GetNearbyDoctors([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var doctors = await _doctorInfoService.GetNearbyDoctorsInfoAsync(latitude, longitude);
            return Ok(doctors);
        }

        // POST: api/doctor
        [HttpPost]
        public async Task<ActionResult<IdentityResult>> CreateDoctor([FromForm] DoctorInfoRequestDto doctorInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _doctorInfoService.CreateDoctorInfoAsync(doctorInfoDto);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // PUT: api/doctor/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<IdentityResult>> UpdateDoctor(string id, [FromForm] DoctorInfoRequestDto doctorInfoRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _doctorInfoService.UpdateDoctorInfoAsync(id, doctorInfoRequestDto);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/doctor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            await _doctorInfoService.DeleteDoctorInfoAsync(id);
            return NoContent();
        }
    }
}