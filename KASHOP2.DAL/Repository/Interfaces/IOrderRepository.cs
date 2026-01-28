using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order request);
        Task<Order?> GetOrderBySessionIdAsync(string sessionId);
        Task<Order?> UpdateAsync(Order order);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status);
        Task<Order?> GetOrdersByIdAsync(int id);
    }
}
