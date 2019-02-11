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
    
    public partial class T_ManipulationTypeLibrary
    {
        public T_ManipulationTypeLibrary()
        {
            this.T_ManipulationLibrary = new HashSet<T_ManipulationLibrary>();
            this.T_ManipulationLibrary1 = new HashSet<T_ManipulationLibrary>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public bool IsUseMachine { get; set; }
        public string Node { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual ICollection<T_ManipulationLibrary> T_ManipulationLibrary { get; set; }
        public virtual ICollection<T_ManipulationLibrary> T_ManipulationLibrary1 { get; set; }
    }
}