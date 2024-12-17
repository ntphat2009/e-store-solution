using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.ViewModels
{
    public class StoreVM
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string City { get; set; }
        public string District { get; set; }
        public string Adress { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Policy { get; set; }
    }
}
