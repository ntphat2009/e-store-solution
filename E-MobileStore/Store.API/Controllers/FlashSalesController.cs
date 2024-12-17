using Microsoft.AspNetCore.Mvc;
using Store.ApiService.Services.Interfaces;
using Store.Infrastructure.DTOs;
using System.Net;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashSalesController : ControllerBase
    {
        private readonly IFlashSaleService _flashSale;
        private BaseApiResponse _response;
        public FlashSalesController(IFlashSaleService flashSale)
        {
            _flashSale = flashSale;
            _response = new BaseApiResponse();
        }
        [HttpPost]
        [Route("InsertOrUpdateFlashSale")]
        public IActionResult InsertOrUpdateFlashSale(FlashSaleDTO flashSale)
        {
            try
            {
                _flashSale.AddOrUpdateFlashSale(flashSale);
                if (flashSale.Id > 0)
                {
                    _response.Message = "Update success";
                    _response.Result = flashSale;
                    return Ok(_response);
                }
                else
                {
                    _response.Message = "Insert success";
                    _response.Result = flashSale;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                if (flashSale.Id > 0)
                {
                    _response.Message = "Update Failed";
                }
                else
                {
                    _response.Message = "Insert Failed";
                }
                _response.IsSuccess = false;
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
        [HttpPost]
        [Route("InsertListFlashSaleProduct")]
        public IActionResult AddListFlashSaleProduct(List<FlashSaleProductDTO> flashSaleProductDTO, int flashSaleId)
        {
            try
            {
                _flashSale.AddListFlashSaleProduct(flashSaleProductDTO, flashSaleId);
                _response.Message = "Insert success";
                _response.Result = flashSaleProductDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                _response.Message = "Insert Failed";
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
        [HttpPut]
        [Route("UpdateFlashSaleProduct")]
        public IActionResult UpdateFlashSaleProduct(FlashSaleProductDTO flashSaleProductDTO)
        {
            try
            {
                _flashSale.UpdateFlashSaleProduct(flashSaleProductDTO);
                _response.Message = "Update success";
                _response.Result = flashSaleProductDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                _response.Message = "Update Failed";
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
        [HttpGet]
        [Route("GetListFlashSale")]
        public async Task<IActionResult> GetFlashSaleAsync(int page, int pageSize)
        {
            try
            {
                var listFlashSale = await _flashSale.GetAllFlashSale(page, pageSize);
                _response.Message = "get success";
                _response.Result = listFlashSale;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                _response.Message = "get Failed";
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
        [HttpPost]
        [Route("DeleteFlashSale")]
        public IActionResult DeleteFlashSale(int flashSaleId)
        {
            try
            {
                _flashSale.DeletedFlashSale(flashSaleId);
                _response.Message = "Deleted success";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                _response.Message = "Deleted Failed";
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
        [HttpPost]
        [Route("DeleteFlashSaleProduct")]
        public IActionResult DeleteFlashSaleProduct(FlashSaleProductDTO flashSaleProductDTO)
        {
            try
            {
                _flashSale.DeletedFlashSaleProduct(flashSaleProductDTO);
                _response.Message = "Deleted success";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                var statusCode = _response.StatusCode = HttpStatusCode.NotFound;
                var errorMessages = _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                _response.Message = "Deleted Failed";
                _response.Failed(statusCode, errorMessages);
                return NotFound(_response);
            }
        }
    }
}
