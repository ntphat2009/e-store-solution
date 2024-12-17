using Store.WebService.APIs.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.APIs
{
    public class ProductImageApi : IProductImageApi
    {
        private string baseUrl = "http://localhost:5163";
        public string AddOrUpdateProductImage(string productId)
        {
            return $"{baseUrl}/api/ProductImages/InsertOrUpdateProductImage?productId={productId}";
        }
    }
}
