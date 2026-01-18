using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseService _diseaseService;
        private readonly ILogger<DiseaseController> _logger;

        public DiseaseController(IDiseaseService diseaseService, ILogger<DiseaseController> logger)
        {
            _diseaseService = diseaseService;
            _logger = logger;
        }

        // GET: api/disease
        [HttpGet]
        public async Task<ActionResult<List<DiseaseResponseDto>>> GetAllDiseases()
        {
            var diseases = await _diseaseService.GetAllAsync();
            return Ok(diseases);
        }

        // GET: api/disease/search?searchString=...
        [HttpGet("search")]
        public async Task<ActionResult<List<DiseaseResponseDto>>> SearchDiseases([FromQuery] string searchString)
        {
            var diseases = await _diseaseService.SearchAsync(searchString);
            return Ok(diseases);
        }

        // POST: api/disease
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDisease([FromBody] DiseaseRequestDto diseaseRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _diseaseService.CreateAsync(diseaseRequestDto);
            return CreatedAtAction(nameof(GetAllDiseases), null);
        }

        // PUT: api/disease/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDisease(int id, [FromBody] DiseaseRequestDto diseaseRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _diseaseService.EditAsync(id, diseaseRequestDto);
            return Ok();
        }

        // DELETE: api/disease/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            await _diseaseService.DeleteAsync(id);
            return NoContent();
        }
    }
}