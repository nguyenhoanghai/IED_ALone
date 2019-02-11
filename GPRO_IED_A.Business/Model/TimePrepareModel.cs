using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class TimePrepareModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double TMUNumber { get; set; }
        public int TimeTypePrepareId { get; set; }
        public string Description { get; set; }
        public string WorkShopName { get; set; }
        public string TimeTypePrepareName { get; set; }
        public int ActionUser { get; set; }
    }
}
