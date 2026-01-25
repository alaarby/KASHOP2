using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }
        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _productRepository.FindByIdAsync(request.ProductId);
            if (product != null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Product not found"
                };
            }
            if(product.Quantity < request.Count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough stock"
                };
            }
            var cart = request.Adapt<Cart>();
            cart.UserId = userId;

            await _cartRepository.AddAsync(cart);

            return new BaseResponse
            {
                Success = true,
                Message = "Product added succesfully"
            };
        }

        public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            var items = cartItems.Select(c => new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,
                Count = c.Count,
                Price = c.Product.Price
            }).ToList();

            return new CartSummaryResponse
            {
                Items = items
            }; 
        }
    }
}
