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
    
    public partial class SUser
    {
        public SUser()
        {
            this.SUserRoles = new HashSet<SUserRole>();
            this.T_LabourDivision = new HashSet<T_LabourDivision>();
            this.T_LabourDivision1 = new HashSet<T_LabourDivision>();
        }
    
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool IsOwner { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool IsLock { get; set; }
        public bool IsRequireChangePW { get; set; }
        public bool IsForgotPassword { get; set; }
        public string NoteForgotPassword { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.DateTime> LockedTime { get; set; }
        public string LastName { get; set; }
        public string FisrtName { get; set; }
        public string WorkshopIds { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual SCompany SCompany { get; set; }
        public virtual ICollection<SUserRole> SUserRoles { get; set; }
        public virtual ICollection<T_LabourDivision> T_LabourDivision { get; set; }
        public virtual ICollection<T_LabourDivision> T_LabourDivision1 { get; set; }
    }
}
