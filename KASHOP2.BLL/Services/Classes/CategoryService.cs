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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryResponse Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            var createdCategory = _categoryRepository.Create(category);
            return createdCategory.Adapt<CategoryResponse>();   
        }

        public List<CategoryResponse> GetAll()
        {
            var categories = _categoryRepository.GetAll();  
            var response = categories.Adapt<List<CategoryResponse>>();  
            return response;
        }
    }
}
