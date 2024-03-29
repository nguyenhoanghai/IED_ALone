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
    
    public partial class T_Equipment
    {
        public T_Equipment()
        {
            this.T_EquipmentAttribute = new HashSet<T_EquipmentAttribute>();
            this.T_ManipulationEquipment = new HashSet<T_ManipulationEquipment>();
            this.T_CA_Phase = new HashSet<T_CA_Phase>();
            this.T_PhaseGroup_Phase = new HashSet<T_PhaseGroup_Phase>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public double Expend { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public int EquipmentGroupId { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ObjectType { get; set; }
    
        public virtual T_EquipmentType T_EquipmentType { get; set; }
        public virtual ICollection<T_EquipmentAttribute> T_EquipmentAttribute { get; set; }
        public virtual ICollection<T_ManipulationEquipment> T_ManipulationEquipment { get; set; }
        public virtual T_EquipmentGroup T_EquipmentGroup { get; set; }
        public virtual ICollection<T_CA_Phase> T_CA_Phase { get; set; }
        public virtual ICollection<T_PhaseGroup_Phase> T_PhaseGroup_Phase { get; set; }
    }
}
