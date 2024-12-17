using Microsoft.AspNetCore.Mvc;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;

namespace Store.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IProductCategoryViewService _productCategoryViewService;
        public CategoryController(ILogger<CategoryController> logger, IProductCategoryViewService productCategoryViewService)
        {
            _logger = logger;
            _productCategoryViewService = productCategoryViewService;

        }
        [Route("{categoryUrl}")]
        public async Task<IActionResult> Index(string categoryUrl, string? sortBy = "date_desc", int pageSize = 6)
        {
            try
            {
                var productCategoryDate = new ProductCategoryVM();
                productCategoryDate = await _productCategoryViewService.GetDataProductCategoryAsync(categoryUrl, sortBy, pageSize);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("/Views/Category/ProductList.cshtml", productCategoryDate.Products);
                }
                return View(productCategoryDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogErorr Exception throw");
                return BadRequest(ex);
            }

        } 
        public IActionResult Banner()
        {
            return PartialView("/Views/Category/BannerCate.cshtml");
        }
        public IActionResult ListProduct()
        {
            return PartialView("/Views/Category/ProductList.cshtml");
        }
    }
}
