using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class PhaseGroup_Phase_ManiModel
    {
        public int Id { get; set; }
        public int PhaseGroup_PhaseId { get; set; }
        public int OrderIndex { get; set; }
        public int? ManipulationId { get; set; }
        public string ManipulationCode { get; set; }
        public string ManipulationName { get; set; }
        public double? TMUEquipment { get; set; }
        public double? TMUManipulation { get; set; }
        public double Loop { get; set; }
        public double TotalTMU { get; set; } 
        public string EquipmentName { get; set; }      
    }
}
