using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
   public class PagedListModel
    {
        public dynamic List { get; set; }
        public int TotalItemCount { get; set; } 
    }
}
