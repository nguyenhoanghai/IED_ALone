using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
    public class LinePositionModel  
    {
        public int Id { get; set; }
        public int LabourDivisionId { get; set; }
        public int OrderIndex { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public bool IsHasBTP { get; set; }
        public bool IsHasExitLine { get; set; }
        public List<LinePositionDetailModel> Details { get; set; } 
        public string EmployeeName { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int TechProVerId { get; set; }
        public LinePositionModel() {
            Details = new List<LinePositionDetailModel>();
        }
    }
}
