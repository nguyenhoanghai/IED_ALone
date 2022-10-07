using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
  public  class ExportPhaseGroupModel
    {
        public string Name { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string Node { get; set; }

        public List<ExportPhaseActionsModel> Phases { get; set; }

        public List<ModelSelectItem> Equipments { get; set; }
        public ExportPhaseGroupModel()
        {
            Phases = new List<ExportPhaseActionsModel>();
            Equipments = new List<ModelSelectItem>();
        }
    }
}
