﻿using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class EquipmentGroupModel 
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Icon { get; set; }
        public string Note { get; set; }
    }
}
