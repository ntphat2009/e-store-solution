
using Newtonsoft.Json;
using Store.WebService.APIs.Interfaces;
using Store.WebService.DTO;
using Store.WebService.Response;
using Store.WebService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services
{
    public class ProductImageWebService : IProductImageWebService
    {
        private readonly IProductImageApi _imageApi;
        private readonly HttpClient _httpClient;

        public ProductImageWebService(IProductImageApi imageApi)
        {
            _imageApi = imageApi;
            _httpClient = new HttpClient();
        }
        public async Task<string> InserOrUpdateProduct(ProductImageDTO imageDTO, string productId)
        {
            try
            {
                var uri = _imageApi.AddOrUpdateProductImage(productId);
                var jsonContent = JsonConvert.SerializeObject(imageDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                var result = "";
                if (response.IsSuccessStatusCode && content != null)
                {
                    result = content.statusCode.ToString();
                }
                else
                {
                    result = content.message;
                }
                return result;
            }
            catch
            {
                return "404";
            }
        }
    }
}
