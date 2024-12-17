using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IProductDetailViewService
    {
        Task<ProductDetailVM> GetDetailProductDataAsync(string productUrl, string categoryUrl, string? sortBy);
    }
}
