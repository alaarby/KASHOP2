using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using KASHOP2.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;

namespace KASHOP2.BLL.Services.Classes
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public CheckoutService(ICartRepository cartRepository, 
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];

            var order = await _orderRepository.GetOrderBySessionIdAsync(userId);
            order.PaymnetId = session.PaymentIntentId;
            order.OrderStatus = OrderStatusEnum.Approved;

            await _orderRepository.UpdateAsync(order);

            var user = await _userManager.FindByIdAsync(userId);

            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            var orderItems = new List<OrderItem>();
            var productUpdated = new List<(int productId, int quantity)>();
            foreach(var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = item.Product.Price,
                    TotlaPrice = item.Product.Price * item.Count
                };
                orderItems.Add(orderItem);
                productUpdated.Add((item.ProductId, item.Count));
            }
            await _orderItemRepository.CreateRangeAsync(orderItems);
            await _productRepository.DecreaseQuantityAsync(productUpdated);
            await _cartRepository.ClearCartAsync(userId);
            await _emailSender.SendEmailAsync(user.Email, "Payment succefull", "");

            return new CheckoutResponse
            {
                Success = true,
                Message = "Payment completed succesfully"
            };
        }

        public async Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "no items in your cart"
                };
            }

            decimal totalAmount = 0;
            
            foreach(var item in cartItems)
            {
                if(item.Product.Quantity < item.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "not enough stock"
                    };
                }
                totalAmount += item.Product.Price * item.Count;
            }

            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totalAmount,
                PaymentStatus = PaymentStatusEnum.UnPaid
            };

            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }
            
            else if(request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {

                    },
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7421/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7421/checkout/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        {"UserId", userId }
                    }
                };
                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name
                            },
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count,
                    });
                }
                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;
                order.PaymentStatus = PaymentStatusEnum.Paid;
                await _orderRepository.CreateAsync(order);
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "payment session created",
                    Url = session.Url
                };
            }
            
            else
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "invalid payment method"
                };
            }

        }
    }
}
