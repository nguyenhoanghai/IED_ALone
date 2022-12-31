using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business.Model
{
    public class EmployeeProductionModel
    {
        public int Id { get; set; }
        public string product { get; set; }
        public string workshop { get; set; }
        public string line { get; set; }
        public string employee { get; set; }
        public List<EmployeePhaseProduction> Phases { get; set; }
    }

    public class EmployeePhaseProduction {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public int Total { get; set; }
        public double Price { get; set; }
        public double Coefficient { get; set; }

    }

    public class EmployeePhaseProductionInDay
    {
        public List<EmployeePhaseProduction> Details { get; set; } 
        public string ProName { get; set; }
        public string WorkshopName { get; set; }
        public string LineName { get; set; } 

        public EmployeePhaseProductionInDay()
        {
            Details = new List<EmployeePhaseProduction>();
        }

    }
}
