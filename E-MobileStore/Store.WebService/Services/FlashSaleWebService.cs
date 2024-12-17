using Newtonsoft.Json;
using Store.WebService.APIs.Interfaces;
using Store.WebService.Response;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services
{
    public class FlashSaleWebService : IFlashSaleWebService
    {
        private readonly IFlashSaleApi _flashSaleApi;
        private readonly HttpClient _client;
        public FlashSaleWebService(IFlashSaleApi flashSaleApi)
        {
            _flashSaleApi = flashSaleApi;
            _client = new HttpClient();
        }
        public async Task<List<FlashSaleVM>> GetFlashSale(int page, int pageSize)
        {
            try
            {
                var flashSales = new List<FlashSaleVM>();
                var uri = _flashSaleApi.GetFlashSale(page, pageSize);
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseApi = JsonConvert.DeserializeObject<FlashSaleResponse>(content);
                    if (responseApi != null && responseApi.result.Count > 0)
                    {
                        foreach (var flashSale in responseApi.result)
                        {
                            flashSales.Add(new FlashSaleVM
                            {
                                Id = flashSale.Id,
                                DateOpen = flashSale.DateOpen,
                                DateClose = flashSale.DateClose,
                                Description = flashSale.Description,
                                CreatedBy = flashSale.CreatedBy,
                                Title = flashSale.Title,
                                CreatedDate = flashSale.CreatedDate,
                                UpdatedBy = flashSale.UpdatedBy,
                                UpdatedDate = flashSale.UpdatedDate,
                                IsActive = flashSale.IsActive,
                                IsDeleted = flashSale.IsDeleted,
                                FlashSaleProducts = flashSale.FlashSaleProducts,
                            });
                        }
                    }
                }
                return flashSales;
            }
            catch (Exception ex)
            {
                return new List<FlashSaleVM>();
            }
        }
    }
}
