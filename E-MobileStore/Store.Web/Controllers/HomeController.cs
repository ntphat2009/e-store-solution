using Microsoft.AspNetCore.Mvc;
using Store.Domain.Entities;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeViewService _homeWebService;

        private readonly IProductWebService _productWebService;

        public HomeController(ILogger<HomeController> logger, IProductWebService productWebService, IHomeViewService homeWebService)
        {
            _logger = logger;
            _homeWebService = homeWebService;
            _productWebService = productWebService;

        }
        [Route("/Home/ProductSearchResults")]
        public async Task<IActionResult> ProductSearchResults(string search)
        {
            var searchResult = await _productWebService.GetProductSearch(search, 1, 10);
            return Json(searchResult);
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var homeData = new HomeVM();
                homeData = await _homeWebService.GetHomeDataAsync();
                var jwt = TempData["jwt"];
                ViewBag.jwt = jwt;
                return PartialView("/Views/Home/Index.cshtml", homeData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetHomeData throw exception");
                return PartialView("PageNotFound");
            }
        }
        public IActionResult ProductByCate()
        {
            return PartialView();
        }
        public IActionResult ChosseCate()
        {
            return PartialView();
        }
        public IActionResult TekZone()
        {
            return PartialView();
        }
        public IActionResult FlashSale()
        {
            return PartialView();
        }
        //[Route("trang-chu/{flashsaleId?}")]
        //public async Task<JsonResult> FlashSaleItem(int flashsaleId)
        //{
        //    var productlist = await _productWebService.GetProductBySaleId(flashsaleId);
        //    var result = new ViewsModel.HomeVM
        //    {
        //        Products = productlist,
        //        Count = productlist.Count()
        //    };
        //    return Json(result);
        //}
        public IActionResult HomeSlider()
        {
            return PartialView();
        }
        public IActionResult ListBranch()
        {
            return PartialView();
        }
        public IActionResult Privacy()
        {
            return PartialView();
        }
        public IActionResult PageNotFound()
        {
            _logger.LogWarning("dont have page");
            return View();
        }
    }
}
