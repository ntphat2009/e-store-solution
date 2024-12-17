using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class ProductCategoryVM
    {
        public List<BannerVM>? Banners { get; set; }
        public List<ProductVM>? Products { get; set; }
        public int TotalProduct { get; set; }
    }
}
