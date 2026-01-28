using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetOrdersAsync(OrderStatusEnum status);
        Task<OrderResponse?> GetOrdersByIdAsync(int orderId);
        Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus);
    }
}
