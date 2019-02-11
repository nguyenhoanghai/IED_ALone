using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class ExportPhaseActionsModel
    {
        public double TimePrepare { get; set; }
        public double TotalTMU { get; set; }
        public List<Commo_Ana_Phase_ManiModel> Details { get; set; }
        public ExportPhaseActionsModel() {
            Details = new List<Commo_Ana_Phase_ManiModel>();
        }
    }
}
