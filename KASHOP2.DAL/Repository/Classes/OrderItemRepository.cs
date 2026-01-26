using KASHOP2.DAL.Data;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Classes
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(List<OrderItem> request)
        {
            await _context.OrderItems.AddRangeAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
