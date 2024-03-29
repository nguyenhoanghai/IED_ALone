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
    
    public partial class T_Product
    {
        public T_Product()
        {
            this.T_ProductFile = new HashSet<T_ProductFile>();
            this.T_TechProcessVersion = new HashSet<T_TechProcessVersion>();
            this.T_UsingTechLog = new HashSet<T_UsingTechLog>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ProductGroupId { get; set; }
    
        public virtual T_Customer T_Customer { get; set; }
        public virtual T_ProductGroup T_ProductGroup { get; set; }
        public virtual ICollection<T_ProductFile> T_ProductFile { get; set; }
        public virtual ICollection<T_TechProcessVersion> T_TechProcessVersion { get; set; }
        public virtual ICollection<T_UsingTechLog> T_UsingTechLog { get; set; }
    }
}
