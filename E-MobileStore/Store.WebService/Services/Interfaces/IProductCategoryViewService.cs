using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IProductCategoryViewService
    {
        Task<ProductCategoryVM> GetDataProductCategoryAsync(string categoryUrl, string? sortBy = "date_desc", int pageSize = 6);
    }
}
