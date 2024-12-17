using Newtonsoft.Json;
using Store.Domain.Entities;
using Store.WebService.APIs;
using Store.WebService.APIs.Interfaces;
using Store.WebService.DTO;
using Store.WebService.Response;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Store.WebService.Services
{
    public class CategoryWebService : ICategoryWebService
    {
        private readonly HttpClient _httpClient;
        private readonly ICategoryApi _categoryApi;
        public CategoryWebService(ICategoryApi categoryApi)
        {
            _httpClient = new HttpClient();
            _categoryApi = categoryApi;
        }

        public async Task<string> DeleteCategory(string categoryUrl)
        {
            try
            {
                //add category
                var uri = _categoryApi.DeleteCategory(categoryUrl);
                var jsonContent = JsonConvert.SerializeObject(categoryUrl);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                var result = "";
                if (response.IsSuccessStatusCode && content != null)
                {
                    result = content.statusCode.ToString();
                }
                else
                {
                    result = content.message;
                }
                return result;
            }
            catch
            {
                return "404";
            }
        }

        public async Task<List<CategoryVM>> GetAllCategory(int page, int pageSize)
        {
            try
            {
                var categories = new List<CategoryVM>();
                var uri = _categoryApi.GetAllCategory(page, pageSize);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<CategoryResponse>(content);
                    if (apiResponse != null && apiResponse.result.Count > 0)
                    {
                        foreach (var category in apiResponse.result)
                        {
                            categories.Add(new CategoryVM
                            {
                                Id = category.Id,
                                Name = category.Name,
                                ImageURL = category.ImageURL,
                                IsActive = category.IsActive,
                                CategoryUrl = category.CategoryUrl,
                                CreatedBy = category.CreatedBy,
                                Position = category.Position,
                                Description = category.Description,
                                IsDeleted = category.IsDeleted,
                                CreatedDate = category.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                UpdatedBy = category.UpdatedBy,
                                UpdatedDate = category.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                Products = category.Products
                            });
                        }
                    }
                }
                return categories;
            }
            catch (Exception ex)
            {
                return new List<CategoryVM>();
            }
        }

        public async Task<CategoryVM> GetCategoryByURL(string categoryUrl)
        {
            try
            {
                var uri = _categoryApi.GetCategoryByUrl(categoryUrl);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var categoryresponse = JsonConvert.DeserializeObject<CategoryDetailResponse>(content);
                    if (categoryresponse != null && categoryresponse.result != null)
                    {
                        var category = categoryresponse.result;
                        return new CategoryVM()
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Description = category.Description,
                            ImageURL = category.ImageURL,
                            Position = category.Position,
                            IsDeleted = category.IsDeleted,
                            IsActive = category.IsActive,
                            CreatedBy = category.CreatedBy,
                            CreatedDate = category.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            UpdatedBy = category.UpdatedBy,
                            UpdatedDate = category.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                        };
                    }
                }
                return new CategoryVM();
            }
            catch
            {
                return new CategoryVM();
            }
        }

        public async Task<string> InsertOrUpdateCategory(CategoryDTO categoryDTO)
        {
                //add category
                var uri = _categoryApi.InsertOrUpdateCategory();
                var jsonContent = JsonConvert.SerializeObject(categoryDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                if (response.IsSuccessStatusCode && content != null)
                {
                    var result = content.statusCode.ToString();
                    return result;
                }
                else
                {
                    var result = content.message;
                    return result;
                }
        }
    }
}
