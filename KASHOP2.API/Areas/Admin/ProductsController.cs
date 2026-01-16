using KASHOP2.API.Resources;
using KASHOP2.BLL.Services.Classes;
using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP2.API.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAll();

            return Ok(new { message = _localizer["Success"].Value, response });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductRequest product)
        {
            var response = await _productService.CreateProduct(product);
            return Ok(new { message = _localizer["Success"].Value, response });

        }
    }
}
