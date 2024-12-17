using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class FlashSaleVM
    {
        public int Id { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime DateClose { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public ICollection<FlashSaleProduct> FlashSaleProducts { get; set; }

    }
}
