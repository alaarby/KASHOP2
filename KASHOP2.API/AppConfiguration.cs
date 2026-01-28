using KASHOP2.API;
using KASHOP2.BLL.Services.Classes;
using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.Repository.Classes;
using KASHOP2.DAL.Repository.Interfaces;
using KASHOP2.DAL.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KASHOP2.BLL
{
    public static class AppConfiguration
    {
        public static void Config(IServiceCollection Services)
        {
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICheckoutService, CheckoutService>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            Services.AddScoped<IManageUserService, ManageUserService>();

            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<ITokenService, TokenService>();

            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();

            Services.AddExceptionHandler<GlobalExceptionHandler>();
            Services.AddProblemDetails();
        }
    }
}
