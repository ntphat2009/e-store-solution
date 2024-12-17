using Newtonsoft.Json;
using Store.Domain.Entities;
using Store.WebService.APIs;
using Store.WebService.APIs.Interfaces;
using Store.WebService.DTO;
using Store.WebService.Response;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace Store.WebService.Services
{
    public class ProductWebService : IProductWebService
    {
        private readonly HttpClient _httpClient;
        private readonly IProductApi _productApi;
        private readonly IProductImageApi _productImageApi;

        public ProductWebService(IProductApi productApi, IProductImageApi productImageApi)
        {
            _httpClient = new HttpClient();
            _productApi = productApi;
            _productImageApi = productImageApi;
        }
        public async Task<ProductVM> GetProductDetail(string productUrl)
        {
            try
            {
                var uri = _productApi.GetProductByUrl(productUrl);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<ProductDetailResponse>(content);
                    if (productresponse != null && productresponse.result != null)
                    {
                        var product = productresponse.result;
                        return new ProductVM()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Price = product.Price,
                            ShortDesc = product.ShortDesc,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            ProductUrl = product.ProductUrl,
                            CategoryUrl = product.Category?.CategoryUrl,
                            CategoryName = product.Category?.Name,
                            Quantity = product.Quantity,
                            ProductImages = product.ProductImages,
                            PriceSale = product.PriceSale,
                            ProductAttributes = product.ProductAttributes,
                            IsDeleted = product.IsDeleted,
                            IsActive = product.IsActive,
                            CreatedBy = product.CreatedBy,
                            CreatedDate = product.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            UpdatedBy = product.UpdatedBy,
                            UpdatedDate = product.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                        };
                    }
                }
                return new ProductVM();
            }
            catch
            {
                return new ProductVM();
            }
        }

        public async Task<int> TotalProductByCate(string categoryUrl)
        {
            try
            {
                var uri = _productApi.TotalProductByCateAsync(categoryUrl);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<TotalProductResponse>(content);
                    return productresponse.result;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public async Task<int> TotalProduct()
        {
            try
            {
                var uri = _productApi.TotalProductAsync();
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<TotalProductResponse>(content);
                    return productresponse.result;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public async Task<List<ProductVM>> GetProductListByCateUrl(string categoryUrl, int page, int pageSize, string? sortBy)
        {

            var products = new List<ProductVM>();
            var uri = _productApi.GetProductListByCateUrl(categoryUrl, page, pageSize, sortBy);
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productresponse = JsonConvert.DeserializeObject<ProductResponse>(content);
                if (productresponse != null && productresponse.result.Count > 0)
                {
                    foreach (var product in productresponse.result)
                    {
                        products.Add(new ProductVM
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Price = product.Price,
                            ShortDesc = product.ShortDesc,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            CategoryName = product.Category?.Name,
                            ProductUrl = product.ProductUrl,
                            CategoryUrl = product.Category?.CategoryUrl,
                            Quantity = product.Quantity,
                            ProductImages = product.ProductImages,
                            PriceSale = product.PriceSale,
                            IsDeleted = product.IsDeleted,
                            IsActive = product.IsActive,
                            CreatedBy = product.CreatedBy,
                            CreatedDate = product.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            UpdatedBy = product.UpdatedBy,
                            UpdatedDate = product.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                        });
                    }
                }
            }
            return products;

        }
        public async Task<List<ProductVM>> GetProductSearch(string search, int page, int pageSize)
        {
            try
            {
                var products = new List<ProductVM>();
                var uri = _productApi.GetProductSearch(search, page, pageSize);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<ProductResponse>(content);
                    if (productresponse != null && productresponse.result.Count > 0)
                    {
                        foreach (var product in productresponse.result)
                        {
                            products.Add(new ProductVM
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Price = product.Price,
                                ShortDesc = product.ShortDesc,
                                Description = product.Description,
                                CategoryId = product.CategoryId,
                                CategoryName = product.Category?.Name,
                                CategoryUrl = product.Category.CategoryUrl,
                                ProductUrl = product.ProductUrl,
                                Quantity = product.Quantity,
                                ProductImages = product.ProductImages,
                                PriceSale = product.PriceSale,
                                IsDeleted = product.IsDeleted,
                                IsActive = product.IsActive,
                                CreatedBy = product.CreatedBy,
                                CreatedDate = product.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                UpdatedBy = product.UpdatedBy,
                                UpdatedDate = product.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            });
                        }
                    }
                }
                return products;
            }
            catch (Exception ex)
            {
                return new List<ProductVM>();
            }
        }

        public async Task<List<ProductVM>> GetProductBySaleId(int flashsaleId)
        {
            try
            {
                var products = new List<ProductVM>();
                var uri = _productApi.GetProductBySaleId(flashsaleId);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<ProductSaleResponse>(content);
                    if (productresponse != null && productresponse.result.Count > 0)
                    {
                        foreach (var product in productresponse.result)
                        {
                            products.Add(new ProductVM
                            {
                                ProductId = product.ProductId,
                                ProductName = product.Product.Name,
                                Price = product.Product.Price,
                                ShortDesc = product.Product.ShortDesc,
                                Description = product.Product.Description,
                                ProductUrl = product.Product.ProductUrl,
                                CategoryUrl = product.Product.Category?.CategoryUrl,
                                CategoryName = product.Product.Category?.Name,
                                Quantity = product.Product.Quantity,
                                ProductImages = product.Product.ProductImages,
                                PriceSale = product.PriceSale,
                                IsDeleted = product.IsDeleted,
                                IsActive = product.IsActive,
                                CreatedBy = product.Product.CreatedBy,
                                CreatedDate = product.Product.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                UpdatedBy = product.Product.UpdatedBy,
                                UpdatedDate = product.Product.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            });
                        }
                    }
                }
                return products;
            }
            catch
            {
                return new List<ProductVM>();
            }
        }

        public async Task<List<ProductVM>> GetProductList(int page, int pageSize, string? sortBy)
        {
            try
            {
                var products = new List<ProductVM>();
                var uri = _productApi.GetProductList(page, pageSize, sortBy);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productresponse = JsonConvert.DeserializeObject<ProductResponse>(content);
                    if (productresponse != null && productresponse.result.Count > 0)
                    {
                        foreach (var product in productresponse.result)
                        {
                            products.Add(new ProductVM
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Price = product.Price,
                                ShortDesc = product.ShortDesc,
                                Description = product.Description,
                                CategoryId = product.CategoryId,
                                CategoryName = product.Category?.Name,
                                ProductUrl = product.ProductUrl,
                                CategoryUrl = product.Category?.CategoryUrl,
                                Quantity = product.Quantity,
                                ProductImages = product.ProductImages,
                                PriceSale = product.PriceSale,
                                IsDeleted = product.IsDeleted,
                                IsActive = product.IsActive,
                                CreatedBy = product.CreatedBy,
                                CreatedDate = product.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                UpdatedBy = product.UpdatedBy,
                                UpdatedDate = product.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                            });
                        }
                    }
                }
                return products;
            }
            catch
            {
                return new List<ProductVM>();
            }
        }
        public async Task<string> InserOrUpdateProduct(ProductDTO productDTO, List<ProductImageDTO> Images)
        {
            try
            {
                //add product
                var uri = _productApi.InserOrUpdateProduct();
                var jsonContent = JsonConvert.SerializeObject(productDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                var result = "";
                if (response.IsSuccessStatusCode && content != null)
                {
                    result = content.message.ToString();
                }
                else
                {
                    result = content.message;
                }
                //add product Image
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        var uriProductImage = _productImageApi.AddOrUpdateProductImage(result);
                        var jsonContentProductImage = JsonConvert.SerializeObject(Images[i]);
                        var httpContentProductImage = new StringContent(jsonContentProductImage, Encoding.UTF8, "application/json");
                        var responseProductImage = await _httpClient.PostAsync(uriProductImage, httpContentProductImage);
                        var responseApiProductImage = await responseProductImage.Content.ReadAsStringAsync();
                        var contentProductImage = JsonConvert.DeserializeObject<AuthenResponse>(responseApiProductImage);
                        if (responseProductImage.IsSuccessStatusCode && contentProductImage != null)
                        {
                        }
                        else
                        {
                            result += "error to add image" + contentProductImage.message;
                        }
                    }
                    result = string.Empty;
                    result = "200";
                }

                return result;

            }
            catch
            {
                return "404";
            }
        }

        public async Task<string> DeleteProduct(string productUrl)
        {
            try
            {
                //add product
                var uri = _productApi.DeletedProduct(productUrl);
                var jsonContent = JsonConvert.SerializeObject(productUrl);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                var result = "";
                if (response.IsSuccessStatusCode && content != null)
                {
                    result = content.message.ToString();
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
