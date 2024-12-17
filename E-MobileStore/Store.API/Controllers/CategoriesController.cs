using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using Store.ApiService.Services;
using Store.ApiService.Services.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.DTOs;
using System.Net;
using static Store.Common.Utility.ProductsUtility;
namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private BaseApiResponse _response;
        private readonly IRedisService _redisService;
        private readonly ICategoryService _category;
        public CategoriesController(ICategoryService category, IRedisService redisService)
        {
            _redisService = redisService;
            _category = category;
            _response = new BaseApiResponse();
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAllCategoriesAsync(int page, int pageSize, bool clearCache = false)
        {
            try
            {
                var key = $"productCate::page={page}";
                if (clearCache)
                {
                    await _redisService.RemoveCacheAsync(key);
                }
                string? catchMember = await _redisService.GetCacheAsync(key);
                IEnumerable<Category>? listCate;
                if (string.IsNullOrEmpty(catchMember))
                {
                    listCate = await _category.GetCategoriesAsync(page, pageSize);
                    if (listCate == null)
                    {
                        return NotFound();
                    }

                    await _redisService.SetCacheAsync(key, listCate, TimeSpan.FromHours(1));
                }
                else
                {
                    listCate = JsonConvert.DeserializeObject<IEnumerable<Category>>(catchMember);
                }
                _response.Result = listCate;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var errorMessenger = _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                _response.Failed(statuscode, errorMessenger);
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("GetCategoryByUrl")]
        public async Task<IActionResult> GetCategoryByIdAsync(string categoryUrl, bool clearCate = false)
        {
            try
            {
                var key = $"productCate::{categoryUrl}";
                if (clearCate)
                {
                    await _redisService.RemoveCacheAsync(key);
                }
                string? catchMember = await _redisService.GetCacheAsync(key);
                Category? cate;
                if (string.IsNullOrEmpty(catchMember))
                {
                    cate = await _category.GetByUrl(categoryUrl);
                    if (cate == null)
                    {
                        return NotFound();
                    }
                    await _redisService.SetCacheAsync(key, cate, TimeSpan.FromHours(1));
                }
                else
                {
                    cate = JsonConvert.DeserializeObject<Category>(catchMember);
                }
                _response.Result = cate;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var errorMessenger = _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                _response.Failed(statuscode, errorMessenger);
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Route("InsertOrUpdateCategory")]
        public async Task<IActionResult> InsertOrUpdateCategory(CategoryDTO category)
        {
            try
            {
                _category.AddOrUpdateCategory(category);
                await _redisService.RemovePatternAsync("productCate::*");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var errorMessenger = _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.Failed(statuscode, errorMessenger);
                return BadRequest(_response);
            }
        }
        [HttpPut]
        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(string categoryUrl)
        {
            try
            {
                _category.DeleteCategory(categoryUrl);
                await _redisService.RemovePatternAsync("productCate::*");
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "200";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var errorMessenger = _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                _response.Failed(statuscode, errorMessenger);
                return BadRequest(_response);
            }
        }
    }
}
