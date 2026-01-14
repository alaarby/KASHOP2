using Azure.Core;
using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Email"
                    };
                }
                if(await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked, try again later"
                    };
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account locked due to multiple failed attempts"
                    };
                }
                else if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "please confirm your email"
                    };
                }
                else if (!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Password"
                    };
                }

                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login succesfully",
                    AccessToken = await GenerateAccessToken(user)
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "An unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, HttpRequest httpRequest)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, request.Password);

                if(!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "Error",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var escapeToken = Uri.EscapeDataString(token);
                var emailUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/identity/Account/ConfirmEmail?token={escapeToken}&userId={user.Id}";

                await _emailSender.SendEmailAsync(user.Email, "wellcome", $"<h1>Hello {user.UserName}</h1>" +
                    $"<a href='{emailUrl}' >confirm email</a>");

                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "An unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<bool> ConfirmEmail(string token, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return false; ;
            }
            return true;
        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(',', roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtOptions:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: userClaims,
                    expires: DateTime.UtcNow.AddDays(15),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordReauest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ForgotPasswordResponse()
                {
                    Success = false,
                    Message = "Email not Found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPasssword = code;
            user.PasswordResetCodeExpiery = DateTime.UtcNow.AddMinutes(5);
            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(user.Email, "reset password", $"<p>code is {code}</p>");

            return new ForgotPasswordResponse()
            {
                Success = true,
                Message = "Code sent to your email"
            };
        }
        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Email not Found"
                };
            }
            else if(user.CodeResetPasssword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "invalid code"
                };
            }
            else if(user.PasswordResetCodeExpiery < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Code Expired"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "Password reset succesfully"
            };
        }
    }
}
