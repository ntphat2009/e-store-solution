using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class ProductDetailVM
    {
        public List<ProductVM>? SuggestProduct { get; set; }
        public ProductVM? Product { get; set; }
    }
}
