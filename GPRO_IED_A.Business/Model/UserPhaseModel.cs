using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class UserPhaseModel
    {
        public string Type { get; set; }
        public string PhaseName { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PhaseGroupName { get; set; }
        public int ParentId { get; set; }
        public string Node { get; set; }
        public double TotalTMU { get; set; } 
        public string Status { get; set; }
    }
}
