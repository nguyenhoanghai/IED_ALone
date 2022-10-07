using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class LabourDevisionVerModel
    {
        public int Id { get; set; }
        public int LaDevisionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NumberOfWorkers { get; set; }
        public double TimeCompletePerCommo { get; set; }
        public double WorkingTimePerDay { get; set; }
        public double PacedProduction { get; set; }
        public double ProOfGroupPerDay { get; set; }
        public double ProOfGroupPerHour { get; set; }
        public double ProOfPersonPerDay { get; set; }
        public string Note { get; set; } 

        public List<LinePositionDailyModel> Positions { get; set; }
        public LabourDevisionVerModel()
        {
            Positions = new List<LinePositionDailyModel>();
        }
    }

   public class LinePositionDailyModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int EmployId { get; set; }
        public string EmployName { get; set; }
        public int LabourDevisionId { get; set; }
        public int LabourDevision_VerId { get; set; }
        public List<LinePositionDetail_DailyModel> Phases { get; set; }
        public LinePositionDailyModel()
        {
            Phases = new List<LinePositionDetail_DailyModel>();
        }
    }

    public class LinePositionDetail_DailyModel
    {
        public int Id { get; set; }
        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public int Quantities { get; set; }
    }
}
