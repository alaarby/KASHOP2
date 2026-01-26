using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> AddAsync(Product product);
        Task<Product?> FindByIdAsync(int id);
        Task<bool> DecreaseQuantityAsync(List<(int productId, int quantity)> items);
    }
}
