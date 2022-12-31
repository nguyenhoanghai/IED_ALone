using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class UsingTechLogModel : T_UsingTechLog
    {
        public string userName { get; set; }
        public int Count { get; set; }
        public List<UsingTechDetail> Details { get; set; }
        public UsingTechLogModel()
        {
            Details = new List<UsingTechDetail>();
        }
    }

    public class UsingTechDetail:T_UsingTech_Detail
    {

    }
}
