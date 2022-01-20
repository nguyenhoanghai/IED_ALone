using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business.Model
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public int? CompanyId { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public int ActionUser { get; set; }
        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }

    }

    public class EmployeeSmallModel
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
