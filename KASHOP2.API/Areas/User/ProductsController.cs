using KASHOP2.API.Resources;
using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP2.API.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IReviewService _reviewService;

        public ProductsController(IProductService productService, 
            IStringLocalizer<SharedResource> localizer,
            IReviewService reviewService)
        {
            _productService = productService;
            _localizer = localizer;
            _reviewService = reviewService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string lang = "en", [FromQuery] int page = 1,
            [FromQuery] int limit = 3, [FromQuery] string? search = null, [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            var response = await _productService.GetAllForUser(lang, page, limit, search, categoryId, minPrice, maxPrice);

            return Ok(new { message = _localizer["Success"].Value, response });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] int id,[FromQuery] string lang = "en")
        {
            var response = await _productService.GetProductDetailsForUser(id, lang);

            return Ok(new { message = _localizer["Success"].Value, response });
        }
        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> AddReview([FromRoute] int productId, [FromBody] CreateReviewRequest request)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _reviewService.AddReviewAsync(user, productId, request);
            if(!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
