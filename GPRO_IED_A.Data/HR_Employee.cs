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
    
    public partial class HR_Employee
    {
        public HR_Employee()
        {
            this.T_LinePosition = new HashSet<T_LinePosition>();
            this.T_LinePoDailyQuantities = new HashSet<T_LinePoDailyQuantities>();
            this.SUsers = new HashSet<SUser>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public System.DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> WorkshopId { get; set; }
        public Nullable<int> LineId { get; set; }
    
        public virtual T_Line T_Line { get; set; }
        public virtual T_WorkShop T_WorkShop { get; set; }
        public virtual ICollection<T_LinePosition> T_LinePosition { get; set; }
        public virtual ICollection<T_LinePoDailyQuantities> T_LinePoDailyQuantities { get; set; }
        public virtual ICollection<SUser> SUsers { get; set; }
    }
}
