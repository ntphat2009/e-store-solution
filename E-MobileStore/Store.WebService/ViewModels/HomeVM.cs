using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class HomeVM
    {
        public List<CategoryVM>? ChosseCate { get; set; }
        public List<CategoryVM>? ProductByCate { get; set; }
        public List<BannerVM>? HomeSlider { get; set; }
        public List<NewsVM>? TekZone { get; set; }
        public List<FlashSaleVM>? FlashSale { get; set; }
        public List<StoreVM>? Stores { get; set; }
        //public int Count { get; set; }
    }
}
