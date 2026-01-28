using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.User.UserName);
            
            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());
            
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"");

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"");

            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Description).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"");

            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
                .Map(dest => dest.UserName, source => source.User.UserName);
        }
    }
}
