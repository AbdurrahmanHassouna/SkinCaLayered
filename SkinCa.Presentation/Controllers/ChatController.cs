using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs.Chat;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // POST: api/chat
        [HttpPost,Authorize(Roles="User")]
        public async Task<IActionResult> CreateChat([FromBody] ChatRequestDto dto)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null) return Unauthorized();
            var chat = await _chatService.CreateChatAsync(userId, dto);
            return CreatedAtAction(nameof(GetChatById), new { chatId = chat.Id }, chat);
        }

        // DELETE: api/chat/{chatId}
        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            await _chatService.DeleteChatAsync(userId, chatId);
            return NoContent();
        }

        // GET: api/chat/{chatId}
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(int chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            return Ok(chat);
        }

        // GET: api/chat/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserChats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var chats = await _chatService.GetChatsByUserIdAsync(userId);
            return Ok(chats);
        }

        // GET: api/chat/with/{otherUserId}
        [HttpGet("with/{otherUserId}")]
        public async Task<IActionResult> GetChatByUsers(string otherUserId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            var chat = await _chatService.GetChatByUsersIdAsync(currentUserId, otherUserId);
            return Ok(chat);
        }

        // DELETE: api/chat/{chatId}/messages
        [HttpDelete("{chatId}/messages")]
        public async Task<IActionResult> DeleteChatMessages(int chatId)
        {
            await _chatService.DeleteChatMessagesAsync(chatId);
            return NoContent();
        }
    }
}