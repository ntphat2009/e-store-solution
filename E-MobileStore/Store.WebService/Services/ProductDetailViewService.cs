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
    public class ProductDetailViewService : IProductDetailViewService
    {
        private readonly ILogger<ProductDetailViewService> _logger;
        private readonly IProductWebService _productWebService;

        public ProductDetailViewService(ILogger<ProductDetailViewService> logger, IProductWebService productWebService)
        {
            _logger = logger;
            _productWebService = productWebService;
        }
        public async Task<ProductDetailVM> GetDetailProductDataAsync(string productUrl, string categoryUrl, string? sortBy)
        {
            var product = await _productWebService.GetProductDetail(productUrl);
            var suggestProduct = await _productWebService.GetProductListByCateUrl(categoryUrl, 1, 10, sortBy);
            return new ProductDetailVM
            {
                Product = product,
                SuggestProduct = suggestProduct
            };
        }
    }
}
