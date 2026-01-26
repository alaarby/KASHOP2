using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KASHOP2.API.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpPost]
        public async Task<IActionResult> Payment(CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _checkoutService.ProcessPaymentAsync(request, userId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("success")]
        public async Task<IActionResult> Success([FromQuery] string sessionId)
        {
            var response = await _checkoutService.HandleSuccessAsync(sessionId); 
            return Ok();
        }
    }
}
