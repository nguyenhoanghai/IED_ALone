﻿using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class CompanyModel : SCompany
    {
        public int ActionUser { get; set; }
        public string LevelName { get; set; }
    }
}
