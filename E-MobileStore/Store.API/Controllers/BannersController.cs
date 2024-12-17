using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using Store.ApiService.Services;
using Store.ApiService.Services.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.DTOs;
using System.Drawing.Printing;
using System.Net;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IRedisService _redisService;
        private BaseApiResponse _response;
        public BannersController(IBannerService bannerService, IRedisService redisService)
        {
            _bannerService = bannerService;
            _redisService = redisService;
            _response = new BaseApiResponse();
        }
        [HttpGet]
        [Route("GetBannerByCate")]
        public async Task<IActionResult> GetBannerByCate(int page, int pageSize, string categoryUrl, bool clearCache)
        {
            try
            {
                var key = $"banner:{categoryUrl}";
                if (clearCache)
                {
                    await _redisService.RemoveCacheAsync(key);
                }
                var cacheMember = await _redisService.GetCacheAsync(key);
                IEnumerable<Banner>? listBanner;
                if (String.IsNullOrEmpty(cacheMember))
                {
                    listBanner = await _bannerService.GetBannerByCateAsync(page, pageSize, categoryUrl);
                    await _redisService.SetCacheAsync(key, listBanner, TimeSpan.FromHours(1));
                }
                else
                {
                    listBanner = JsonConvert.DeserializeObject<IEnumerable<Banner>>(cacheMember);
                }
                _response.Result = listBanner;
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
        [Route("GetAllBanner")]
        public async Task<IActionResult> GetAllBanner(int page, int pageSize, bool clearCache = false)
        {
            try
            {
                var key = $"banner:{page}&{pageSize}";
                if (clearCache)
                {
                    await _redisService.RemoveCacheAsync(key);
                }
                var cacheMember = await _redisService.GetCacheAsync(key);
                IEnumerable<Banner> listBanner;
                if (String.IsNullOrEmpty(cacheMember))
                {
                    listBanner = await _bannerService.GetAllBanner(page, pageSize);
                    await _redisService.SetCacheAsync(key, listBanner, TimeSpan.FromHours(1));
                }
                else
                {
                    listBanner = JsonConvert.DeserializeObject<IEnumerable<Banner>>(cacheMember);
                }
                _response.Result = listBanner;
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
        [Route("GetBannerDetail")]
        public async Task<IActionResult> GetBannerDetail(int bannerId, bool clearCache = false)
        {
            try
            {
                var key = $"banner:{bannerId}";
                if (clearCache)
                {
                    await _redisService.RemoveCacheAsync(key);
                }
                var cacheMember = await _redisService.GetCacheAsync(key);
                Banner banner;
                if (String.IsNullOrEmpty(cacheMember))
                {
                    banner = await _bannerService.GetBannerDetail(bannerId);
                    await _redisService.SetCacheAsync(key, banner, TimeSpan.FromHours(1));
                }
                else
                {
                    banner = JsonConvert.DeserializeObject<Banner>(cacheMember);
                }
                _response.Result = banner;
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
        [Route("InsertOrUpdateBanner")]
        public async Task<IActionResult> InsertOrUpdateBanner(BannerDTO banner)
        {
            try
            {
                _bannerService.InsertOrUpdateBanner(banner);
                await _redisService.RemovePatternAsync("banner*");
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
        [HttpPut]
        [Route("DeleteBanner")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {
            try
            {
                _bannerService.DeletedBanner(bannerId);
                await _redisService.RemovePatternAsync("banner*");
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
