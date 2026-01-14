using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KASHOP2.API.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request, Request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("confirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await _authenticationService.ConfirmEmail(token, userId);
            return Ok(result);
        }
        [HttpPost("send-code")]
        public async Task<IActionResult> RequestPasswordReset(ForgotPasswordReauest request)
        {
            var result = await _authenticationService.RequestPasswordReset(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPassword(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
