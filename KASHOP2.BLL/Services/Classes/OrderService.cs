using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> GetOrdersAsync(OrderStatusEnum status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            var response =  orders.Adapt<List<OrderResponse>>();
            return response;
        }

        public async Task<OrderResponse?> GetOrdersByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrdersByIdAsync(orderId);
            var response = order.Adapt<OrderResponse>();
            return response;
        }

        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await _orderRepository.GetOrdersByIdAsync(orderId);

            order.OrderStatus = newStatus;

            if(newStatus == OrderStatusEnum.Delivered)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else if(newStatus == OrderStatusEnum.Cancelled)
            {
                if(order.OrderStatus == OrderStatusEnum.Shipped)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "can't cancelled this order"
                    };
                }
            }

            await _orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Success = true,
                Message = "order status updated"
            };
        }
    }
}
