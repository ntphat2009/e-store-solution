using Store.WebService.DTO;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IProductWebService
    {
        Task<int> TotalProductByCate(string categoryUrl);
        Task<string> DeleteProduct(string productUrl);
        Task<int> TotalProduct();
        Task<ProductVM> GetProductDetail(string productUrl);
        Task<List<ProductVM>> GetProductListByCateUrl(string categoryUrl, int page, int pageSize, string? sortBy);
        Task<List<ProductVM>> GetProductList(int page, int pageSize, string? sortBy);
        Task<List<ProductVM>> GetProductSearch(string search, int page, int pageSize);
        Task<List<ProductVM>> GetProductBySaleId(int flashsaleId);
        Task<string> InserOrUpdateProduct(ProductDTO productDTO, List<ProductImageDTO> Images);
    }
}
