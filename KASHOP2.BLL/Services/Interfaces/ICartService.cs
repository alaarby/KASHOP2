using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request);
        Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en");
        Task<BaseResponse> ClearCartAsync(string userId);
    }
}
