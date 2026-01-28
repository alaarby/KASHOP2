using KASHOP2.DAL.Data;
using KASHOP2.DAL.DTOs.Responses;
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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Query()
        {
            return _context.Products.Include(p => p.Translations).AsQueryable();
        }
        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DecreaseQuantityAsync(List<(int productId, int quantity)> items)
        {
            var productIds = items.Select(i => i.productId).ToList();

            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            foreach (var product in products)
            {
                var item = items.First(i => i.productId == product.Id);
                if (product.Quantity < item.quantity)
                {
                    return false;
                }
                product.Quantity = item.quantity;

            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> FindByIdAsync(int id)
        {
            return await _context.Products.Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.Include(c => c.Translations)
                .Include(c => c.User).ToListAsync();
        }
    }
}
