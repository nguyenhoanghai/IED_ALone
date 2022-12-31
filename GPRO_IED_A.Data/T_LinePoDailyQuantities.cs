//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GPRO_IED_A.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_LinePoDailyQuantities
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int LabourDevisionId { get; set; }
        public int LabourDevision_VerId { get; set; }
        public int LinePo_DetailId { get; set; }
        public int ComandType { get; set; }
        public int Quantities { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int PhaseId { get; set; }
        public int EmployeeId { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
        public virtual SUser SUser { get; set; }
        public virtual T_CA_Phase T_CA_Phase { get; set; }
        public virtual T_LabourDevision_Ver T_LabourDevision_Ver { get; set; }
        public virtual T_LabourDivision T_LabourDivision { get; set; }
        public virtual T_LinePositionDetail T_LinePositionDetail { get; set; }
    }
}