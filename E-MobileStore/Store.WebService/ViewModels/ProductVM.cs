using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class ProductVM
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public string CategoryUrl { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
