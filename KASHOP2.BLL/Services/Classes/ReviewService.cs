using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request)
        {
            var hasDeliveredOrder = await _orderRepository.HasUserDeliveredOrderForProductAsync(userId, productId);
            if (!hasDeliveredOrder)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "you can only comment on product you have recieved"
                };
            }
            var alreqdyReview = await _reviewRepository.HasUserReviewedProduct(userId, productId);
            if (alreqdyReview)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "can't add review"
                };
            }

            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;
            
            await _reviewRepository.AddReviewAsync(review);

            return new BaseResponse
            {
                Success = true,
                Message = "review added succesfully"
            };
        }
    }
}
