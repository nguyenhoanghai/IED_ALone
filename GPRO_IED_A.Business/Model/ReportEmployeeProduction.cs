using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class ReportEmployeeProduction
    {
        public string ProName { get; set; }
        public string WorkShopName { get; set; }
        public string LineName { get; set; }
        public List<EmployeePhaseProduction> Phases { get; set; }
        public ReportEmployeeProduction()
        {
            Phases = new List<EmployeePhaseProduction>();
        }
    }
}
