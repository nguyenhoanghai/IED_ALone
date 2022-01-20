using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;

namespace GPRO_IED_A.Business.Model
{
    public class ProductGroupModel :T_ProductGroup
    {  
        public int CustomerId { get; set; }
        public bool IsPrivate { get; set; }
        public int ActionUser { get; set; }
    }
}
