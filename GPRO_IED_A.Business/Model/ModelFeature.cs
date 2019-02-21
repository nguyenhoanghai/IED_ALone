 using System.Collections.Generic; 
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data; 

namespace GPRO_IED_A.Business.Model
{
    public class ModelFeature:SFeature
    {
        public List<ModelPermission> Permissions { get; set; }
        public List<ModelModule> Modules { get; set; }
    }
}
