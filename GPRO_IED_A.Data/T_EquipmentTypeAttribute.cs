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
    
    public partial class T_EquipmentTypeAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EquipmentTypeId { get; set; }
        public int OrderIndex { get; set; }
        public bool IsUseForTime { get; set; }
        public Nullable<int> EquipTypeAtrrDefault_Id { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual T_EquipmentType T_EquipmentType { get; set; }
        public virtual T_EquipTypeAttr_Default T_EquipTypeAttr_Default { get; set; }
    }
}
