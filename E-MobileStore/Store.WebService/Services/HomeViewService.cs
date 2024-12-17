using Microsoft.Extensions.Logging;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services
{
    public class HomeViewService : IHomeViewService
    {
        private readonly ILogger<HomeViewService> _logger;
        private readonly ICategoryWebService _categoryWebService;
        private readonly IBannerWebService _bannerWebService;
        private readonly INewsWebService _newsWebService;
        private readonly IFlashSaleWebService _flashSaleWebService;
        private readonly IStoreWebService _storeWebService;

        public HomeViewService(ILogger<HomeViewService> logger, ICategoryWebService categoryWebService, IBannerWebService bannerWebService, INewsWebService newsWebService, IFlashSaleWebService flashSaleWebService, IStoreWebService storeWebService)
        {
            _logger = logger;
            _categoryWebService = categoryWebService;
            _bannerWebService = bannerWebService;
            _newsWebService = newsWebService;
            _flashSaleWebService = flashSaleWebService;
            _storeWebService = storeWebService;
        }

        public async Task<HomeVM> GetHomeDataAsync()
        {
            _logger.LogInformation("This is home page");
            var catelist = await _categoryWebService.GetAllCategory(1, 10);
            var bannerHome = await _bannerWebService.GetBannerByCate(1, 100, "home");
            var tekZone = await _newsWebService.GetAllNews(1, 6);
            var flashSale = await _flashSaleWebService.GetFlashSale(1, 2);
            var storeList = await _storeWebService.GetStoreList(1, 10);

            return new HomeVM
            {
                ChosseCate = catelist,
                ProductByCate = catelist,
                HomeSlider = bannerHome,
                TekZone = tekZone,
                FlashSale = flashSale,
                Stores = storeList,
            };
        }
    }
}
