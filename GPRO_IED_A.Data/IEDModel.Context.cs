﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IEDEntities : DbContext
    {
        public IEDEntities()
            : base("name=IEDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<HR_Employee> HR_Employee { get; set; }
        public DbSet<SCategory> SCategories { get; set; }
        public DbSet<SCompany> SCompanies { get; set; }
        public DbSet<SCompanyModule> SCompanyModules { get; set; }
        public DbSet<SConfig> SConfigs { get; set; }
        public DbSet<SFeature> SFeatures { get; set; }
        public DbSet<SLevelCompany> SLevelCompanies { get; set; }
        public DbSet<SMenu> SMenus { get; set; }
        public DbSet<SModule> SModules { get; set; }
        public DbSet<SPermission> SPermissions { get; set; }
        public DbSet<SRoLe> SRoLes { get; set; }
        public DbSet<SRolePermission> SRolePermissions { get; set; }
        public DbSet<SUserRole> SUserRoles { get; set; }
        public DbSet<SWorkerLevel> SWorkerLevels { get; set; }
        public DbSet<T_ApplyPressureLibrary> T_ApplyPressureLibrary { get; set; }
        public DbSet<T_CA_Phase_Mani> T_CA_Phase_Mani { get; set; }
        public DbSet<T_CA_Phase_TimePrepare> T_CA_Phase_TimePrepare { get; set; }
        public DbSet<T_CommodityAnalysis> T_CommodityAnalysis { get; set; }
        public DbSet<T_CompanySkill> T_CompanySkill { get; set; }
        public DbSet<T_CompanySkillDetail> T_CompanySkillDetail { get; set; }
        public DbSet<T_Employee_CompanySkill> T_Employee_CompanySkill { get; set; }
        public DbSet<T_Employee_PhaseGroupSkill> T_Employee_PhaseGroupSkill { get; set; }
        public DbSet<T_EmployeePhase> T_EmployeePhase { get; set; }
        public DbSet<T_Equipment> T_Equipment { get; set; }
        public DbSet<T_EquipmentAttribute> T_EquipmentAttribute { get; set; }
        public DbSet<T_EquipmentGroup> T_EquipmentGroup { get; set; }
        public DbSet<T_EquipmentTypeAttribute> T_EquipmentTypeAttribute { get; set; }
        public DbSet<T_EquipType_Default> T_EquipType_Default { get; set; }
        public DbSet<T_EquipTypeAttr_Default> T_EquipTypeAttr_Default { get; set; }
        public DbSet<T_File> T_File { get; set; }
        public DbSet<T_IEDConfig> T_IEDConfig { get; set; }
        public DbSet<T_LabourDivision> T_LabourDivision { get; set; }
        public DbSet<T_Line> T_Line { get; set; }
        public DbSet<T_LinePosition> T_LinePosition { get; set; }
        public DbSet<T_LinePositionDetail> T_LinePositionDetail { get; set; }
        public DbSet<T_ManipulationEquipment> T_ManipulationEquipment { get; set; }
        public DbSet<T_ManipulationFile> T_ManipulationFile { get; set; }
        public DbSet<T_ManipulationLibrary> T_ManipulationLibrary { get; set; }
        public DbSet<T_ManipulationTypeLibrary> T_ManipulationTypeLibrary { get; set; }
        public DbSet<T_NatureCutsLibrary> T_NatureCutsLibrary { get; set; }
        public DbSet<T_Product> T_Product { get; set; }
        public DbSet<T_StopPrecisionLibrary> T_StopPrecisionLibrary { get; set; }
        public DbSet<T_TechProcessVersion> T_TechProcessVersion { get; set; }
        public DbSet<T_TechProcessVersionDetail> T_TechProcessVersionDetail { get; set; }
        public DbSet<T_TimePrepare> T_TimePrepare { get; set; }
        public DbSet<T_TimeTypePrepare> T_TimeTypePrepare { get; set; }
        public DbSet<T_WorkShop> T_WorkShop { get; set; }
        public DbSet<T_EquipmentType> T_EquipmentType { get; set; }
        public DbSet<T_CA_Phase> T_CA_Phase { get; set; }
        public DbSet<T_Customer> T_Customer { get; set; }
        public DbSet<SUser> SUsers { get; set; }
        public DbSet<T_PhaseGroup> T_PhaseGroup { get; set; }
    }
}
