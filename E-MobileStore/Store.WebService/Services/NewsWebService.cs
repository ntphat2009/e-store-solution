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
    public class NewsWebService : INewsWebService
    {
        private readonly INewsApi _newsApi;
        private readonly HttpClient _client;
        public NewsWebService(INewsApi newsApi)
        {
            _newsApi = newsApi;
            _client = new HttpClient();
        }
        public async Task<List<NewsVM>> GetAllNews(int page, int pageSize)
        {
            try
            {
                var tekZones = new List<NewsVM>();
                var uri = _newsApi.GetNews(page, pageSize);
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<NewsResponse>(content);
                    if (apiResponse != null && apiResponse.result.Count > 0)
                    {
                        foreach (var tekZone in apiResponse.result)
                        {
                            tekZones.Add(new NewsVM
                            {
                                Id = tekZone.Id,
                                CategoryId = tekZone.CategoryId,
                                Title = tekZone.Title,
                                Description = tekZone.Description,
                                CreatedBy = tekZone.CreatedBy,
                                CreatedDate = tekZone.CreatedDate,
                                UpdatedBy = tekZone.UpdatedBy,
                                UpdatedDate = tekZone.UpdatedDate,
                                ImageURL = tekZone.ImageURL,
                                ShortDesc = tekZone.ShortDesc,
                                IsActive = tekZone.IsActive,
                                IsDeleted = tekZone.IsDeleted,
                            });
                        }
                    }
                }
                return tekZones;
            }
            catch (Exception ex)
            {
                return new List<NewsVM>();
            }
        }
    }
}
