using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkinCa.Business.DTOs;
using SkinCa.Business.DTOs.User;
using SkinCa.Business.ServicesContracts;

namespace SkinCa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(IAuthenticationService authService, ILogger<AccountController> logger, IUserService userService)
        {
            _authService = authService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<IdentityResult>> RegisterAsync([FromForm] RegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("get-token")]
        public async Task<ActionResult<AuthenticationResponse>> GetToken([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.GetTokenAsync(model);
            return result.IsAuthenticated ? Ok(result) : BadRequest(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([EmailAddress] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.ForgotPasswordAsync(email);
            return Ok(new { status = true, message = "Token has been sent to your email" });
        }

        [HttpPost("verify-reset-password-code")]
        public async Task<ActionResult<IdentityResult>> VerifyResetPasswordCode([EmailAddress] string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.VerifyResetPasswordToken(token, email);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<IdentityResult>> ResetPassword([EmailAddress]string email, string token, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ResetPasswordAsync(token, email, password);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("send-email-confirmation/{email}")]
        public async Task<IActionResult> SendEmailConfirmationCode([EmailAddress]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.RequestEmailConfirmationAsync(email);
            return Ok(new { status = true, message = "Email was sent successfully" });
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<IdentityResult>> ConfirmEmail([EmailAddress]string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ConfirmEmailAsync(email, token);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("profile"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ProfileResponseDto>> Profile()
        {
            var profile = await _userService.GetProfileAsync(User);
            return Ok(profile);
        }

        [HttpPut("update-profile"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IdentityResult>> UpdateProfile([FromForm] ProfileRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateProfileAsync(model, User);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut("change-password"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IdentityResult>> ChangePassword(string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ChangePasswordAsync(User, oldPassword, newPassword);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile-picture"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IdentityResult>> UploadProfilePicture(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateProfilePictureAsync(file, User);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-account"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IdentityResult>> DeleteAccount()
        {
            var result = await _authService.DeleteAccountAsync(User);
            return result.Succeeded ? NoContent() : BadRequest(result);
        }
    }
}