using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScanResultController : ControllerBase
    {
        private readonly IScanResultService _scanResultService;
        private readonly ILogger<ScanResultController> _logger;

        public ScanResultController(IScanResultService scanResultService, ILogger<ScanResultController> logger)
        {
            _scanResultService = scanResultService;
            _logger = logger;
        }

        // GET: api/scanresult/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ScanResultResponseDto>> GetScanResultById(int id)
        {
            var result = await _scanResultService.GetByIdAsync(id);
            return Ok(result);
        }

        // GET: api/scanresult/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ScanResultResponseDto>>> GetScanResultsByUser(int userId)
        {
            var results = await _scanResultService.GetAllByUserIdAsync(userId);
            return Ok(results);
        }

        // POST: api/scanresult
        [HttpPost]
        public async Task<IActionResult> CreateScanResult([FromBody] ScanResultRequestDto scanResultRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _scanResultService.CreateAsync(scanResultRequestDto);
            return CreatedAtAction(nameof(GetScanResultById), new { id = 0 }, null);
        }

        // DELETE: api/scanresult/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScanResult(int id)
        {
            await _scanResultService.DeleteAsync(id);
            return NoContent();
        }
    }
}