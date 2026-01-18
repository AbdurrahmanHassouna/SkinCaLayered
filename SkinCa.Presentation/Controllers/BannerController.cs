using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;

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
    public async Task<ActionResult<List<BannerResponseDto>>> GetAllBanners()
    {
        var banners = await _bannerService.GetAllAsync();
        return Ok(banners);
    }

    // POST: api/Banner
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBanner([FromForm] BannerRequestDto bannerRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _bannerService.CreateAsync(bannerRequestDto);
        return Created();
    }

    // PUT: api/Banner/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBanner(int id, [FromForm] BannerRequestDto bannerRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _bannerService.EditAsync(id, bannerRequestDto);
        return NoContent();
    }

    // DELETE: api/Banner/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        await _bannerService.DeleteAsync(id);
        return NoContent();
    }
}