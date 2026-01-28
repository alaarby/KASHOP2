using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Classes;
using KASHOP2.DAL.Repository.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }
        public async Task<List<ProductResponse>> GetAll()
        {
            var products = _productRepository.GetAll();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }
        public async Task<BaseResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if(request.SubImages != null)
            {
                product.SubImages = new List<ProductImage>();
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.SubImages.Add(new ProductImage
                    {
                        ImageName = image.Name
                    });
                }
            }
            await _productRepository.AddAsync(product);

            return new BaseResponse()
            {
                Success = true,
                Message = "Product added succesfully"
            };
        }
        public async Task<List<ProductUserResponse>> GetAllForUser(string lang = "en", 
            int page = 1, 
            int limit = 3,
            string? search = null, 
            int? categoryId = null, 
            decimal? minPrice = null, 
            decimal? maxPrice = null)
        {
            var query = _productRepository.Query();
            if(search != null)
            {
                query = query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search)));
            }
            if(categoryId != null)
            {
                query = query.Where(p => p.Category.Id == categoryId);
            }
            if(minPrice != null)
            {
                query = query.Where(p => p.Price >=  minPrice);
            }
            if(maxPrice != null)
            {
                query = query.Where(p => p.Price <=  maxPrice);
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((page - 1) * limit).Take(limit);
            var list = query.ToList();

            var response = query.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<ProductUserResponse>>();
            return response;
        }
        public async Task<ProductUserDetails> GetProductDetailsForUser(int id, string lang = "en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var response = product.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<ProductUserDetails>();
            return response;
        }
    }
}
