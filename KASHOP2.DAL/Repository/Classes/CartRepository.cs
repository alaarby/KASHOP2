using KASHOP2.DAL.Data;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Classes
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cart?> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<List<Cart>> GetUserCartAsync(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product).ThenInclude(p => p.Translations)
                .ToListAsync();
        } 
        public async Task<Cart?> GetCartItemAsync(string userId, int productId)
        {
            return await _context.Carts.Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }
        public async Task<Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task ClearCartAsync(string userId)
        {
            var items = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
