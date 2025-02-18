using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;
using System.Net.Mime;

namespace SkinCa.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BannerController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public BannerController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    // GET: api/Banner
    [HttpGet]
    [ProducesResponseType(typeof(List<BannerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<BannerResponseDto>>> GetAllBanners()
    {
        try
        {
            var banners = await _bannerService.GetAllAsync();
            return Ok(banners);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // POST: api/Banner
    [HttpPost]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBanner([FromForm] BannerRequestDto bannerRequestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _bannerService.CreateAsync(bannerRequestDto);


            return Created();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT: api/Banner/5
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBanner(int id, [FromForm] BannerRequestDto bannerRequestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _bannerService.EditAsync(id, bannerRequestDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE: api/Banner/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        try
        {
            await _bannerService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}