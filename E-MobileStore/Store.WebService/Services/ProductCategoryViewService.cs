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
    public class ProductCategoryViewService : IProductCategoryViewService
    {
        private readonly ILogger<ProductCategoryViewService> _logger;
        private readonly IProductWebService _productWebService;
        private readonly IBannerWebService _bannerWebService;

        public ProductCategoryViewService(ILogger<ProductCategoryViewService> logger, IProductWebService productWebService, IBannerWebService bannerWebService)
        {
            _logger = logger;
            _productWebService = productWebService;
            _bannerWebService = bannerWebService;
        }
        public async Task<ProductCategoryVM> GetDataProductCategoryAsync(string categoryUrl, string? sortBy = "date_desc", int pageSize = 6)
        {
            _logger.LogInformation("This is category page");
            var banner = await _bannerWebService.GetBannerByCate(1, 100, categoryUrl);
            var listProduct = await _productWebService.GetProductListByCateUrl(categoryUrl, 1, pageSize, sortBy);
            int totalProduct = await _productWebService.TotalProductByCate(categoryUrl);
            return new ProductCategoryVM
            {
                Products = listProduct,
                Banners = banner,
                TotalProduct = totalProduct,
            };
        }
    }
}
