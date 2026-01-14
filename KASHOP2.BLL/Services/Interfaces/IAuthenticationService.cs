using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request, HttpRequest httpRequest);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmail(string token, string UserId);
        Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordReauest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
    }
}
