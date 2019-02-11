using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
   public class LinePositionsAndTechProcessActiveVersionModel
    {
        public TechProcessVersionModel TechProcess { get; set; }
        public List<LinePositionModel> LinePositions { get; set; }
        public LinePositionsAndTechProcessActiveVersionModel()
        {
            LinePositions = new List<LinePositionModel>();
        }
    }
}
