using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class UsingTechReportModel
    {
        public int TotalProduct { get; set; }
        public int TotalPhase { get; set; }
        public int TotalSubmitPhase { get; set; }
        public int TotalApprovePhase { get; set; }
        public int TotalViewPhase { get; set; }
        public int TotalDownloadPhase { get; set; }
        public int TotalNewPhase { get; set; }
        public List<UsingTechReportDetailModel> Details { get; set; }
        public UsingTechReportModel()
        {
            Details = new List<UsingTechReportDetailModel>();
        }
    }

    public class UsingTechReportDetailModel
    {
        public int WorkshopId { get; set; }
        public string Name { get; set; }
        public int TotalProduct { get; set; }
        public int TotalPhase { get; set; }
        public int TotalSubmitPhase { get; set; }
        public int TotalApprovePhase { get; set; }
        public int TotalNewPhase { get; set; }
        public int TotalViewPhase { get; set; }
        public int TotalDownloadPhase { get; set; } 

    }

    public class ReportTechDetailModel
    {
        public int WorkshopId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public string WorkshopName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
        public int LogId { get; set; }
        public int PhaseId_Sample { get; set; }
        public string PhaseName_Sample { get; set; }
    }

}
