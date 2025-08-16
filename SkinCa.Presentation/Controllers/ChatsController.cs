using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs.Chat;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common.Exceptions;
using System.Security.Claims;

namespace SkinCa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // POST: api/chats
        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] ChatRequestDto dto)
        {
            var chat = await _chatService.CreateChatAsync(User, dto);
            return CreatedAtAction(nameof(GetChatById), new { chatId = chat.Id }, chat);
        }

        // DELETE: api/chats/{chatId}
        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            await _chatService.DeleteChatAsync(User, chatId);
            return NoContent();
        }

        // GET: api/chats/{chatId}
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(int chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            return Ok(chat);
        }

        // GET: api/chats/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserChats()
        {
            // Extract current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var chats = await _chatService.GetChatsByUserIdAsync(userId);
            return Ok(chats);
        }

        // GET: api/chats/with/{otherUserId}
        [HttpGet("with/{otherUserId}")]
        public async Task<IActionResult> GetChatByUsers(string otherUserId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            var chat = await _chatService.GetChatByUsersIdAsync(currentUserId, otherUserId);
            return Ok(chat);
        }

        // DELETE: api/chats/{chatId}/messages
        [HttpDelete("{chatId}/messages")]
        public async Task<IActionResult> DeleteChatMessages(int chatId)
        {
            await _chatService.DeleteChatMessagesAsync(chatId);
            return NoContent();
        }
    }
}