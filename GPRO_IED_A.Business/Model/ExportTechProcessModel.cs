using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
    public class ExportTechProcessModel : TechProcessVersionModel
    {        
        public List<TechProcessVerDetailGroupModel> ListTechProcessGroup { get; set; }
    }
}
