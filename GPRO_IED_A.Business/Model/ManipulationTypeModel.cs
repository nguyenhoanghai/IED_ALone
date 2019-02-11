using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class ManipulationTypeModel
    {
         public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public bool IsUseMachine { get; set; }
        public string Node { get; set; }
        public int ActionUser { get; set; }
    }
}
