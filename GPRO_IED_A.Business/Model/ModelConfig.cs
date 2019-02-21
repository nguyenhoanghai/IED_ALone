using System;
using System.Collections.Generic;
using System.Linq; 
using GPRO_IED_A.Data;


namespace GPRO_IED_A.Business.Model
{
    public class ModelConfig //: SConfig
    {
        public int Id { get; set; }
        public int ConpanyId { get; set; }
        public string TableName { get; set; }
        public Nullable<int> ObjectId { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDefault { get; set; }
    }
}
