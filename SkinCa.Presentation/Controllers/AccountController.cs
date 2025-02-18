
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkinCa.Business.DTOs;
using SkinCa.Business.DTOs.User;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;

namespace SkinCaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AccountController> _logger;
        
        private readonly IUserService _userService;
        public AccountController(IAuthenticationService authService ,ILogger<AccountController> logger,IUserService userService)
        {
            _authService = authService;
            _logger=logger;
            _userService = userService;
        }

        [HttpPost("register")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync([FromForm] RegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return NoContent();
        }
       
        [HttpPost("get-token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ActionResult<AuthenticationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthenticationResponse>> GetToken([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.GetTokenAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("forgot-password")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassowrd([EmailAddress]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.ForgotPasswordAsync(email);
            return Ok(new {status= true, message = "Token has been sent to your email" });
        }
        [HttpPost("verify-reset-password-code")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyResetPasswordCode([Required]string email,[Required]string token)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.VerifyResetPasswordToken(token,email);
            return result.Succeeded?Ok(OperationResult):BadRequest(new { status = result, message = "Invalid token" });
        }
        [HttpPost("reset-password")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(string email,string token,string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = ModelState.IsValid, error = ModelState["errors"]?.RawValue??"no errors" });
            }
            var result = await _authService.ResetPasswordAsync(token, email, password);
            if (result == null)
            {
                NotFound(new { message = $"User with {email} not found", status = false });
            }
            return result!.Value?Ok(new { status = result,message="Valid token"})
                :BadRequest(new { status = result, message = "Invalid token" });
        }
        [HttpGet("send-email-confirmation/{email}")]
        public async Task<IActionResult> SendEmailConfirmationCode(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.GetUserWithEmailAsync(email);
            await _authService.RequestEmailConfirmationAsync(email);

            return Ok(new { status = true, message = "Email sent successfully" });

        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email,string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ConfirmEmailAsync(email, token);
            if (result == null)
            {
                return BadRequest("Email not registered");
            }
            return result!.Value?
                Ok(new { status = result,message="valid token"})
                :BadRequest(new { status = result, message = "Invalid Token" });
        }
        [HttpGet("profile"), Authorize]
        public async Task<IActionResult> Profile()
        {
            ProfileResponseDto? profile =await  _userService.GetProfileAsync(User);
            if (profile == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new { message = "Couldn't retrieve the profile" });
            }
            return Ok(profile);
        }
        [HttpPut("update-profile"), Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileRequestDto model)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
               
            }

            var editedProfile = await _userService.UpdateProfileAsync(model,User);
            if (editedProfile == null)
            {
                return NotFound(new { status = false, message = "Couldn't Found the profile contact support" });
            }

            return Ok(editedProfile);
        }
        [HttpPut("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword,string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ChangePasswordAsync(User,oldPassword,newPassword); 
            if (result == null) return NotFound(new { message = "Something wrong contact support !!"});
            return result.Succeeded?Ok(new { status = result.Succeeded,message="Password is changed successfully"})
                :BadRequest(new { status = result.Succeeded, message = "Wrong password" });
        }
 /*       [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse)) };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Principal != null)
            {
               
            }

            return BadRequest();
        }
*/
        [HttpPut("profile-picture"), Authorize]
        public async Task<IActionResult> UploadProfilePicture( IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var profile = await _userService.UpdateProfilePictureAsync(file, User);
            if (profile == null) return StatusCode(StatusCodes.Status500InternalServerError,new { message = "Error! contact support." });
            return Ok(profile);
        }
        [HttpDelete("delete-account"), Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await _authService.DeleteAccountAsync(User);
            if(result == null) return StatusCode(StatusCodes.Status500InternalServerError,new { message = "Error! contact support." });
            return NoContent();
        }

    }
}

