using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAll();
        Task<BaseResponse> CreateProduct(ProductRequest request);
        Task<List<ProductUserResponse>> GetAllForUser(string lang = "en");
        Task<List<ProductUserResponse>> GetProductDetailsForUser(int id, string lang = "en");
    }
}
