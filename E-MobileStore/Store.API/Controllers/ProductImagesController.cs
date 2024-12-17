using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.ApiService.Services;
using Store.ApiService.Services.Interfaces;
using Store.Infrastructure.DTOs;
using System.Net;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly BaseApiResponse _response;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
            _response = new BaseApiResponse();

        }
        [HttpPost]
        [Route("InsertOrUpdateProductImage")]
        public IActionResult InsertOrUpdateProductImage(ProductImageDTO image,Guid productId)
        {
            try
            {
                _productImageService.AddOrUpdateProductImage(image,productId);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statuscode = _response.StatusCode = HttpStatusCode.BadRequest;
                var message = _response.ErrorMessages = new List<string> { ex.Message };
                _response.Message = ex.Message;
                _response.Failed(statuscode, message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
    }
}
