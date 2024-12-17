using Microsoft.AspNetCore.Mvc;
using Store.WebService.DTO;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;
using static Store.Web.Utility.ProductsUtilities;
namespace Store.Web.Areas.Admin.Controllers

{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IProductWebService _productWebService;
        private readonly ICategoryWebService _categoryWebService;
        private readonly IWebHostEnvironment _env;


        public ProductController(IWebHostEnvironment env, IProductWebService productWebService, ICategoryWebService categoryWebService)
        {
            _productWebService = productWebService;
            _categoryWebService = categoryWebService;
            _env = env;

        }
        [Route("quan-li-san-pham/tim-kiem")]
        public async Task<IActionResult> ProductSearchResults(string search)
        {
            var searchResult = await _productWebService.GetProductSearch(search, 1, 10);
            return Json(searchResult);
        }
        [Route("quan-li-san-pham")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? sortBy = "date_desc")
        {
            var jwt = HttpContext.Session.GetString("jwtadmin");
            if (jwt == null)
            {
                return RedirectToRoute("login");
            }
            var productList = await _productWebService.GetProductList(page, pageSize, sortBy);
            ViewBag.TotalItem = await _productWebService.TotalProduct();
            ViewBag.CategoryList = await _categoryWebService.GetAllCategory(1, 100);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(productList);
            }
            return View(productList);
        }
        private async void UploadImage(IFormFile fileImage)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "uploads/images", fileImage.FileName);
                if (!System.IO.File.Exists(path))
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileImage.CopyToAsync(stream);
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        [HttpPost]
        [Route("quan-li-san-pham/them-san-pham")]
        public async Task<IActionResult> ProductAdd(ProductDTO productDTO, List<ProductImageDTO> Images, List<IFormFile> formFile)
        {

            for (int i = 0; i < formFile.Count; i++)
            {
                if (!string.IsNullOrEmpty(formFile[i].FileName))
                {
                    UploadImage(formFile[i]);
                    Images[i].ImageURL = formFile[i].FileName;
                }
            }
            var result = await _productWebService.InserOrUpdateProduct(productDTO, Images);
            if (result == "200")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorCode = result;
                return View();
            }
        }

        [Route("quan-li-san-pham/cap-nhat-san-pham")]
        public async Task<IActionResult> ProductEdit(string productUrl)
        {
            ViewBag.ProductInfo = await _productWebService.GetProductDetail(productUrl);
            ViewBag.CategoryListt = await _categoryWebService.GetAllCategory(1, 20);
            return View();
        }
        [HttpPost]
        [Route("quan-li-san-pham/cap-nhat-san-pham")]
        public async Task<IActionResult> ProductEdit(ProductDTO productDTO, List<ProductImageDTO> Images, List<IFormFile> formFile)
        {
            int newImageIndex = 0;
            if (Images.Any() && formFile.Any())
            {
                for (int i = 0; i < Images.Count; i++)
                {
                    if (string.IsNullOrEmpty(Images[i].ImageURL) && newImageIndex < formFile.Count)
                    {
                        UploadImage(formFile[newImageIndex]);
                        Images[i].ImageURL = formFile[newImageIndex].FileName;
                        newImageIndex++;
                    }
                }
            }
            var result = await _productWebService.InserOrUpdateProduct(productDTO, Images);
            if (result == "200")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorCode = result;
                return View();
            }
        }
        [Route("quan-li-san-pham/{productUrl}")]
        public async Task<IActionResult> ProductDetail(string productUrl)
        {
            var result = await _productWebService.GetProductDetail(productUrl);
            return View("~/Areas/Admin/Views/Product/ProductDetail.cshtml", result);
        }
        [HttpPut]
        [Route("quan-li-san-pham/xoa-san-pham")]
        public async Task<IActionResult> ProductDelete(string productUrl)
        {
            try
            {
                var result = await _productWebService.DeleteProduct(productUrl);
                ViewBag.DeletedMessage = result;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                return Json(new { error = ex });
            }
        }
        [HttpPut]
        [Route("quan-li-san-pham/xoa-nhieu-san-pham")]
        public async Task<IActionResult> ProductsDelete(List<string> productUrls)
        {
            try
            {
                if (productUrls != null)
                {
                    foreach (var productUrl in productUrls)
                    {
                        var result = await _productWebService.DeleteProduct(productUrl);
                        ViewBag.DeletedMessage = result;
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
        }
    }
}
