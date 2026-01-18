using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.UtilityExtensions;

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
        public async Task<ActionResult<ScanResultDto>> GetScanResultById(int id)
        {
            var result = await _scanResultService.GetByIdAsync(id);
            return Ok(result);
        }

        // GET: api/scanresult/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ScanResultDto>>> GetScanResultsByUser(string userId)
        {
            var results = await _scanResultService.GetAllByUserIdAsync(userId);
            return Ok(results);
        }
         // POST: api/scanresult
        [HttpPost]
        public async Task<ActionResult> CreateScanResult([FromForm] ScanResultRequestDto scanResultRequest)
        {
            if (scanResultRequest.Image.Length == 0)
                return BadRequest(new { Message = "Uploaded file is empty." });
            const long maxBytes = 10_000_000;
            if (scanResultRequest.Image.Length > maxBytes)
                return BadRequest(new { Message = $"Image too large. Max allowed size is {maxBytes / 1_000_000} MB." });

            byte[] bytes;
            try
            {
                bytes = await scanResultRequest.Image.ToBytesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read uploaded image into memory.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Unable to read the uploaded file." });
            }

            // Run mock AI prediction (synchronous deterministic simulation)
            var (gotCancer, confidence) = Predict(bytes);

            // Build DTO for persistence and save
            var scanDto = new ScanResultDto
            {
                GotCancer = gotCancer,
                Data = bytes,
                Confidence = confidence
            };

            try
            {
                // persist to DB (your existing service)
                await _scanResultService.CreateAsync(scanDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save scan result.");
                // Still return detection result to user but indicate persistence failed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Detection completed but saving the result failed.",
                    Detection = new { gotCancer, confidence }
                });
            }

            // Return the AI result to the user
            // 200 OK with JSON: { gotCancer: bool, confidence: short }
            return Ok(new
            {
                GotCancer = gotCancer,
                Confidence = confidence
            });
        }

        //mock AI
        private (bool, short) Predict(byte[] bytes)
        {
            var random = new Random();
            var guess = random.NextSingle();
            return (guess > 0.5, 90);
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