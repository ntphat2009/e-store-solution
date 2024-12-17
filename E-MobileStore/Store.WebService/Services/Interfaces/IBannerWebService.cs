using Store.WebService.DTO;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IBannerWebService
    {
        Task<List<BannerVM>> GetBannerByCate(int page, int pageSize, string categoryUrl);
        Task<List<BannerVM>> GetAllBanner(int page, int pageSize);
        Task<BannerVM> GetBannerDetail(int bannerId);
        Task<string> InsertOrUpdateBanner(BannerDTO bannerDTO);

        Task<string> DeleteBanner(int bannerId);
    }
}
