using KASHOP2.API.Resources;
using KASHOP2.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KASHOP2.API.Areas.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAll();

            return Ok(new { message= _localizer["Success"].Value, response}); 
        }
    }
}
