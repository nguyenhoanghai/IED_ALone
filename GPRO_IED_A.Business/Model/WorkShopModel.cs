using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;

namespace GPRO_IED_A.Business.Model
{
    public class WorkShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FactoryName { get; set; }
        public int ActionUser { get; set; }
    }
}

