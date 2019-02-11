using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace GPRO_IED_A.Business.Model
{
    public class FileUploadModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int? Size { get; set; }
        public string FileType { get; set; }
        public int CompanyId { get; set; }
    }
}
