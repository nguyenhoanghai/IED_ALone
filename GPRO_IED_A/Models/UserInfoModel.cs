using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPRO_IED_A.Models
{
    public class UserInfoModel
    {
        public int UserId { get; set; }
        public string ImagePath { get; set; }
        public string LogoCompany { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
    }
}