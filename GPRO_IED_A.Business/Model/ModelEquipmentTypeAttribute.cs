using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;


namespace GPRO_IED_A.Business.Model
{
    public class ModelEquipmentTypeAttribute 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EquipmentTypeId { get; set; }
        public int OrderIndex { get; set; }
        public bool IsUseForTime { get; set; }
        public int? EquipTypeAtrrDefault_Id { get; set; }
        public bool IsDefault { get; set; }
        public string EquipmentTypeName { get; set; }
        public int ActionUser { get; set; }
    }
}
