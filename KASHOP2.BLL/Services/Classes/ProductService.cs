using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Classes;
using KASHOP2.DAL.Repository.Interfaces;
using Mapster;
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
            var categories = await _productRepository.GetAll();
            var response = categories.Adapt<List<ProductResponse>>();
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
        public async Task<List<ProductUserResponse>> GetAllForUser(string lang = "en")
        {
            var products = await _productRepository.GetAll();
            var response = products.BuildAdapter()
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
