using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class ReportProductionBalance
    {
        public int LabourDivisionVerId { get; set; }
        public int Id { get; set; }
        public int TechProVer_Id { get; set; }
        public int ParentId { get; set; }
        public int LineId { get; set; }
        public int TotalPosition { get; set; }
        public List<ReportProductionBalancePosition> Positions { get; set; }
        public TechProcessVersionModel TechProcess { get; set; }
        public int WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineName { get; set; }
        public int CommoId { get; set; }
        public string CommoName { get; set; }
        public string LastEditer { get; set; }
        public DateTime LastEditTime { get; set; }
        public int ActionUser { get; set; }
        public bool IsActive { get; set; }
        public ReportProductionBalance()
        {
            Positions = new List<ReportProductionBalancePosition>();
        }
    }

    public class ReportProductionBalancePosition
    {
        public int LabourDivisionId { get; set; }
        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LineId { get; set; }

        public List<ReportProductionBalancePositionDetail> Phases { get; set; }
        public ReportProductionBalancePosition()
        {
            Phases = new List<ReportProductionBalancePositionDetail>();
        }
    }

    public class ReportProductionBalancePositionDetail
    {
        public int Id { get; set; }
        public int Line_PositionId { get; set; }
        public int TechProVerDe_Id { get; set; }
        public int OrderIndex { get; set; }
        public double DevisionPercent { get; set; }
        public double NumberOfLabor { get; set; }
       // public string Note { get; set; }
       // public bool IsPass { get; set; }
        public string PhaseCode { get; set; }
        public string PhaseName { get; set; }
        public double TotalLabor { get; set; }
        public double TotalTMU { get; set; }
      //  public double SkillRequired { get; set; }
      //  public int EquipmentId { get; set; }
      //  public string EquipmentCode { get; set; }
      //  public string EquipmentName { get; set; }
      //  public int PhaseGroupId { get; set; }
     //   public double DevisionPercent_Temp { get; set; }
        public int Index { get; set; }
        public int PhaseId { get; set; }

        public int DM { get; set; }
        public int ProductInDay { get; set; }
    }
}
