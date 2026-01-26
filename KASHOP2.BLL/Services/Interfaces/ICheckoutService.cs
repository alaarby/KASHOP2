using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId);
        Task<CheckoutResponse> HandleSuccessAsync(string sessionId);
    }
}
