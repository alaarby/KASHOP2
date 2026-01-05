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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public Category Create(Category request)
        {
            _context.Categories.Add(request);
            _context.SaveChanges();
            return request;
        } 
        public List<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Translations).ToList();

        }
    }
}
