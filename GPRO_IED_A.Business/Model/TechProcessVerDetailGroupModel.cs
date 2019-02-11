 using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
    public class TechProcessVerDetailGroupModel
    {
        public int PhaseGroupId { get; set; }
        public string PhaseGroupName { get; set; }
        public List<TechProcessVerDetailModel> ListTechProcessVerDetail{ get; set; }         
    }
}
