using Azure.Core;
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

        public async Task<CategoryResponse> Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            var createdCategory = await _categoryRepository.CreateAsync(category);
            return createdCategory.Adapt<CategoryResponse>();   
        }
        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();  
            var response = categories.Adapt<List<CategoryResponse>>();  
            return response;
        }
        public async Task<List<CategoryUserResponse>> GetAllForUser(string lang = "en")
        {
            var categories = await _categoryRepository.GetAll();
            var response = categories.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<CategoryUserResponse>>();
            return response;
        }
        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }
                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Deleted Succesfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message}
                };
            }
        }
        public async Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }
                if(request.Translations != null)
                {
                    foreach(var translation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == translation.Language);
                        if(existing != null)
                        {
                            existing.Name = translation.Name;
                        }
                        else
                        {
                            return new BaseResponse()
                            {
                                Success = true,
                                Message = $"Language {translation.Language} not supported"
                            };
                        }
                    }
                }
                category.Translations = request.Translations.Adapt<List<CategoryTranslation>>();
                await _categoryRepository.UpdateAsync(category);

                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Updated"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<BaseResponse> ToggleStatusAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }
                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateAsync(category);

                return new BaseResponse()
                {
                    Success = false,
                    Message = $"Category status changed to {category.Status}"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Can't Toggle Status",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
