using System;
using System.Collections.Generic; 

namespace GPRO_IED_A.Business
{
    public class APILinePositionModel
    {
        public int labourDeId { get; set; }
        public int labourDeVerId { get; set; }
        public int Id { get; set; } 
        public int Index { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ActionUserId { get; set; }
        public string Date { get; set; }
        public List<APILinePositionDetailModel> Details { get; set; }
        public APILinePositionModel()
        {
            Details = new List<APILinePositionDetailModel>();
        }
    }

    public class APILinePositionDetailModel
    {
        public int Id { get; set; }
        public int LinePoId { get; set; }
        public int PhaseId { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public double DevisionPercent { get; set; }
        public int Total { get; set; }
        public int TotalInDay { get; set; }
        public int Quantities { get; set; }
        public double DM { get; set; }
        public double TimeByPercent { get; set; }
    }
}
