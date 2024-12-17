using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using Store.ApiService.Services;
using Store.ApiService.Services.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.DTOs;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Net;
using System.Threading;
using static Store.Common.Utility.ProductsUtility;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private BaseApiResponse _response;
        private readonly IRedisService _redisService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService, IRedisService redisService)
        {
            _logger = logger;
            _productService = productService;
            _response = new BaseApiResponse();
            _redisService = redisService;
        }
        [HttpGet]
        [Route("GetSaleProducts")]
        public async Task<IActionResult> GetSaleProductsAsync(int flashSaleId, bool clearCache = false)
        {
            try
            {
                var key = $"sale:{flashSaleId}";
                if (clearCache)
                {
                    await _redisService.RemovePatternAsync(key);
                }
                var cacheMember = await _redisService.GetCacheAsync(key);
                IEnumerable<FlashSaleProduct>? products;
                if (String.IsNullOrEmpty(cacheMember))
                {
                    products = await _productService.GetSaleProducts(flashSaleId);
                    await _redisService.SetCacheAsync(key, products, TimeSpan.FromHours(1));
                }
                else
                {
                    products = JsonConvert.DeserializeObject<IEnumerable<FlashSaleProduct>>(cacheMember);
                }
                _response.Success(products);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.NotFound;
                var message = _response.ErrorMessages = new List<string> { ex.Message };

                _response.Failed(statuscode, message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("GetProductByUrl")]
        public async Task<IActionResult> GetProductByUrl(string productUrl, bool clearCache = false)
        {
            try
            {
                var key = $"product={productUrl}";
                if (clearCache)
                {
                    await _redisService.RemovePatternAsync(key);
                }
                var cacheMember = await _redisService.GetCacheAsync(key);
                Product? product;
                if (String.IsNullOrEmpty(cacheMember))
                {
                    product = await _productService.GetProductByUrlAsync(productUrl);
                    await _redisService.SetCacheAsync(key, product, TimeSpan.FromHours(1));
                }
                else
                {
                    product = JsonConvert.DeserializeObject<Product>(cacheMember);
                }
                _response.Success(product);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.NotFound;
                var message = _response.ErrorMessages = new List<string> { ex.Message };
                _response.Failed(statuscode, message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Route("InsertOrUpdateProduct")]
        public async Task<IActionResult> AddProduct(ProductDTO product)
        {
            try
            {
                _response.Message = await _productService.AddOrUpdateProduct(product);
                await _redisService.RemovePatternAsync("productCate:*");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error to add or update:{ex.Message}");
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var message = _response.ErrorMessages = new List<string> { ex.Message };
                _response.Message = ex.Message;
                _response.Failed(statuscode, message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }   
        [HttpPut]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string productUrl)
        {
            try
            {
                _productService.DeleteProduct(productUrl);
                await _redisService.RemovePatternAsync("productCate:*");
                _response.IsSuccess = true;
                _response.Message = "200";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.NotFound;
                var message = _response.ErrorMessages = new List<string> { ex.Message };
                _response.Failed(statuscode, message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("GetProductListByCateUrl")]
        public async Task<IActionResult> GetProductListByCate(string cateUrl, int page, int pageSize, string? sortBy, bool clearCache = false)
        {
            var key = $"productCate:{cateUrl}&sortBy:{sortBy}&pageSize:{pageSize}";
            if (clearCache)
            {
                await _redisService.RemovePatternAsync(key);
            }
            var cacheMember = await _redisService.GetCacheAsync(key);
            IEnumerable<Product>? products;
            if (String.IsNullOrEmpty(cacheMember))
            {
                products = await _productService.GetProductListByCateUrlAsync(cateUrl, page, pageSize, sortBy);
                await _redisService.SetCacheAsync(key, products, TimeSpan.FromHours(1));
            }
            else
            {
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(cacheMember);
            }
            _response.Result = products;

            return Ok(_response);
        }
        [HttpGet]
        [Route("GetProductList")]
        public async Task<IActionResult> GetProductList(int page, int pageSize, string? sortBy, bool clearCache = false)
        {
            var key = $"productCate:page={page}";
            if (clearCache)
            {
                await _redisService.RemovePatternAsync(key);
            }
            var cacheMember = await _redisService.GetCacheAsync(key);
            IEnumerable<Product>? products;
            if (String.IsNullOrEmpty(cacheMember))
            {
                products = await _productService.GetProductListAsync(page, pageSize, sortBy);
                await _redisService.SetCacheAsync(key, products, TimeSpan.FromHours(1));
            }
            else
            {
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(cacheMember);
            }
            _response.Result = products;
            return Ok(_response);
        }
        [HttpGet]
        [Route("TotalProductByCate")]
        public async Task<IActionResult> TotalProductByCateAsync(string cateUrl)
        {
            var key = $"productCate:totalProductCate{cateUrl}";
            var cacheMember = await _redisService.GetCacheAsync(key);
            int total;
            if (String.IsNullOrEmpty(cacheMember))
            {
                total = await _productService.TotalProductByCateAsync(cateUrl);
                await _redisService.SetCacheAsync(key, total, TimeSpan.FromHours(1));
            }
            else
            {
                total = int.Parse(cacheMember);
            }
            _response.Result = total;
            return Ok(_response);
        }
        [HttpGet]
        [Route("TotalProduct")]
        public async Task<IActionResult> TotalProductAsync()
        {
            var key = $"productCate:totalProduct";
            var cacheMember = await _redisService.GetCacheAsync(key);
            int total;
            if (String.IsNullOrEmpty(cacheMember))
            {
                total = await _productService.TotalProductAsync();
                await _redisService.SetCacheAsync(key, total, TimeSpan.FromHours(1));
            }
            else
            {
                total = int.Parse(cacheMember);
            }
            _response.Result = total;
            return Ok(_response);
        }
        [HttpGet]
        [Route("GetProductSearch")]
        public async Task<IActionResult> GetProductSearch(string search, int page, int pageSize)
        {
            var products = await _productService.GetProductSearchAsync(search, page, pageSize);
            if (products == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            _response.Result = products;
            return Ok(_response);
        }
    }
}
