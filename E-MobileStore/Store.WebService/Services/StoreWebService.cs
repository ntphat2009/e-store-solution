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
    public class StoreWebService : IStoreWebService
    {
        private readonly IStoreApi _storeApi;
        private readonly HttpClient _client;

        public StoreWebService(IStoreApi storeApi)
        {
            _storeApi = storeApi;
            _client = new HttpClient();
        }

        public async Task<List<StoreVM>> GetStoreList(int page, int pageSize)
        {
            try
            {
                var stores = new List<StoreVM>();
                var uri = _storeApi.GetStoreList(page, pageSize);
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseApi = JsonConvert.DeserializeObject<StoreResponse>(content);
                    if (responseApi != null && responseApi.result.Count > 0)
                    {
                        foreach (var store in responseApi.result)
                        {
                            stores.Add(new StoreVM()
                            {
                                Adress = store.Adress,
                                District = store.District,
                                City = store.City,
                                ImageUrl = store.ImageUrl,
                                Id = store.Id,
                                Description = store.Description,
                                Policy = store.Policy,
                                CreatedBy = store.CreatedBy,
                                UpdatedBy = store.UpdatedBy,
                                CreatedDate = store.CreatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                UpdatedDate = store.UpdatedDate.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                                IsDeleted = store.IsDeleted,
                                IsActive = store.IsActive,
                            });
                        }
                    }
                }
                return stores;
            }
            catch (Exception ex)
            {
                return new List<StoreVM>();
            }
        }
    }
}
