using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class SampleProAna
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Node { get; set; }
        public int ParentId { get; set; }
        public List<SampleProAna> Details { get; set; }
        public SampleProAna()
        {
            Details = new List<SampleProAna>();
        }
    }
}
