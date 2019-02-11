using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;

namespace GPRO_IED_A.Business.Model
{
   public class ManipulationEquipmentModel  
    {
        public int Id { get; set; }
        public int ManipulationId { get; set; }
        public int EquipmentId { get; set; }
        public double UserTMU { get; set; }
        public double MachineTMU { get; set; }
        public string Note { get; set; }
        public string  EquipmentName { get; set; }
    }
}
