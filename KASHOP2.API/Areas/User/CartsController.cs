using KASHOP2.API.Resources;
using KASHOP2.BLL.Services.Classes;
using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP2.API.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        CartsController(ICartService cartService, IStringLocalizer<SharedResource> localizer)
        {
            _cartService = cartService;
            _localizer = localizer;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cartService.AddToCartAsync(userId, request);

            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
