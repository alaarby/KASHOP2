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
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddReviewAsync(Review request)
        {
            await _context.Reviews.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserReviewedProduct(string userId, int productId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }
    }
}
