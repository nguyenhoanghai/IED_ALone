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
    
    public partial class T_UsingTechLog
    {
        public T_UsingTechLog()
        {
            this.T_UsingTech_Detail = new HashSet<T_UsingTech_Detail>();
        }
    
        public int Id { get; set; }
        public int WorkShopId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> PhaseId { get; set; }
        public Nullable<int> PhaseGroupId { get; set; }
        public Nullable<int> TKCId { get; set; }
        public Nullable<int> QTCNId { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public bool IsView { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual SUser SUser { get; set; }
        public virtual T_CA_Phase T_CA_Phase { get; set; }
        public virtual T_LabourDevision_Ver T_LabourDevision_Ver { get; set; }
        public virtual T_PhaseGroup T_PhaseGroup { get; set; }
        public virtual T_Product T_Product { get; set; }
        public virtual T_TechProcessVersion T_TechProcessVersion { get; set; }
        public virtual ICollection<T_UsingTech_Detail> T_UsingTech_Detail { get; set; }
        public virtual T_WorkShop T_WorkShop { get; set; }
    }
}
