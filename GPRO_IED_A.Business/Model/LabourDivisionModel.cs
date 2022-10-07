using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
   public class LabourDivisionModel  
    {
        public int LabourDivisionVerId { get; set; }
        public int Id { get; set; }
        public int TechProVer_Id { get; set; }
        public int ParentId { get; set; }
        public int LineId { get; set; }
        public int  TotalPosition { get; set; }
        public List<LinePositionModel> Positions { get; set; }
        public TechProcessVersionModel TechProcess { get; set; }
        public int WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineName { get; set; } 
        public int CommoId { get; set; }
        public string CommoName { get; set; }
        public string LastEditer { get; set; }
        public DateTime LastEditTime { get; set; }
        public int ActionUser { get; set; }
        public LabourDivisionModel()
        {
            Positions = new List<LinePositionModel>(); 
        }
        public bool IsTechVersionChange { get; set; }
    }
}
