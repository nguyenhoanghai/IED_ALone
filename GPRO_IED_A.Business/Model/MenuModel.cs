using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRO_IED_A.Business
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public int OrderIndex { get; set; }
        public string Link { get; set; }
        public bool IsShow { get; set; }
        public bool IsViewIcon { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
        public int? CompanyId { get; set; }
        public int MenuCategoryId { get; set; }
    }
}
