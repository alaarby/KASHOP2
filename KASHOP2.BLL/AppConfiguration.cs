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
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();

            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
