using Microsoft.AspNetCore.Mvc;
using Store.Domain.Entities;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;

namespace Store.Web.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailViewService _productDetailViewService;
        private readonly ILogger<ProductDetailController> _logger;

        public ProductDetailController(ILogger<ProductDetailController> logger, IProductDetailViewService productDetailViewService)
        {
            _productDetailViewService = productDetailViewService;
            _logger = logger;

        }
        [Route("{categoryUrl}/{productUrl}")]
        public async Task<IActionResult> Index(string productUrl, string categoryUrl, string? sortBy)
        {
            try
            {
                var productDetailData = new ProductDetailVM();
                productDetailData = await _productDetailViewService.GetDetailProductDataAsync(productUrl, categoryUrl, sortBy);
                ViewBag.productName = productDetailData.Product?.ProductName;
                return PartialView("/Views/ProductDetail/Index.cshtml", productDetailData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to get data");
                return BadRequest();
            }
        }
        public IActionResult IntroDetail()
        {
            return PartialView("/Views/ProductDetail/IntroDetail.cshtml");
        }
        public IActionResult Description()
        {
            return PartialView("/Views/ProductDetail/Description.cshtml");
        }
        public IActionResult SuggestProduct()
        {
            return PartialView("/Views/ProductDetail/SuggestProduct.cshtml");
        }   
    }
}
