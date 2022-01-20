using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;

namespace GPRO_IED_A.Business.Model
{
    public class ProductModel 
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; } 
        public int? CompanyId { get; set; }
        public int CustomerId { get; set; }
        public int? ProductGroupId { get; set; }
        public bool IsPrivate { get; set; }
        public int ActionUser { get; set; }
        public List<ModelSelectItem> Files { get; set; }
        public ProductModel()
        {
            Files = new List<ModelSelectItem>();
        }
    }
}
