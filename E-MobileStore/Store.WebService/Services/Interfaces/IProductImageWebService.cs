using Store.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IProductImageWebService
    {
        Task<string> InserOrUpdateProduct(ProductImageDTO imageDTO, string productId);

    }
}
