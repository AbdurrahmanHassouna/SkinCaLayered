using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ILogger<BookmarkController> _logger;

        public BookmarkController(IBookmarkService bookmarkService, ILogger<BookmarkController> logger)
        {
            _bookmarkService = bookmarkService;
            _logger = logger;
        }

        // GET: api/bookmark
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<BookmarkResponseDto>>> GetAllBookmarks()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookmarks = await _bookmarkService.GetAllByUserIdAsync(userId);
            return Ok(bookmarks);
        }

        // POST: api/bookmark
        [HttpPost]
        public async Task<IActionResult> CreateBookmark([FromBody] BookmarkRequestDto bookmarkRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookmarkService.CreateAsync(bookmarkRequestDto);
            return CreatedAtAction(nameof(GetAllBookmarks), null);
        }

        // DELETE: api/bookmark/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookmark(int bookmarkId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("userId was not found ,despite being Authorized");
            }
            await _bookmarkService.DeleteAsync(userId,bookmarkId);
            return NoContent();
        }
    }
}
