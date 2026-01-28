using Azure;
using KASHOP2.API.Resources;
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
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OrdersController(IOrderService orderService,
            IStringLocalizer<SharedResource> localizer)
        {
            _orderService = orderService;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatusEnum orderStatus = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetOrdersAsync(orderStatus);
            return Ok(new { message = _localizer["Success"].Value, orders });
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrders([FromRoute] int orderId)
        {
            var orders = await _orderService.GetOrdersByIdAsync(orderId);
            return Ok(new { message = _localizer["Success"].Value, orders });
        }
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId, 
            [FromBody] UpdateOrderStatusRequest newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus.Status);    
            if(!result.Success) return BadRequest(result);
            return Ok();
        }
    }
}
