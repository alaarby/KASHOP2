using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> AddAsync(Cart cart);
        Task<List<Cart>> GetUserCartAsync(string userId);
    }
}
