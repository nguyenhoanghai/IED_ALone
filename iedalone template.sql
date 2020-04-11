
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/08/2020 02:17:54
-- Generated from EDMX file: E:\Hai_GPRO\Git IED_Alone\GPRO_IED_A.Data\IEDModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [IED_Alone];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__SCompany__Parent__3B75D760]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SCompany] DROP CONSTRAINT [FK__SCompany__Parent__3B75D760];
GO
IF OBJECT_ID(N'[dbo].[FK__SFeature__Module__3D5E1FD2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SFeature] DROP CONSTRAINT [FK__SFeature__Module__3D5E1FD2];
GO
IF OBJECT_ID(N'[dbo].[FK__SMenu__MenuCateg__03F0984C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SMenu] DROP CONSTRAINT [FK__SMenu__MenuCateg__03F0984C];
GO
IF OBJECT_ID(N'[dbo].[FK__SPermissi__Featu__3C69FB99]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SPermission] DROP CONSTRAINT [FK__SPermissi__Featu__3C69FB99];
GO
IF OBJECT_ID(N'[dbo].[FK__SRolePerm__Featu__403A8C7D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SRolePermission] DROP CONSTRAINT [FK__SRolePerm__Featu__403A8C7D];
GO
IF OBJECT_ID(N'[dbo].[FK__SRolePerm__Modul__3F466844]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SRolePermission] DROP CONSTRAINT [FK__SRolePerm__Modul__3F466844];
GO
IF OBJECT_ID(N'[dbo].[FK__SRolePerm__Pemis__412EB0B6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SRolePermission] DROP CONSTRAINT [FK__SRolePerm__Pemis__412EB0B6];
GO
IF OBJECT_ID(N'[dbo].[FK__SRolePerm__RoleI__3E52440B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SRolePermission] DROP CONSTRAINT [FK__SRolePerm__RoleI__3E52440B];
GO
IF OBJECT_ID(N'[dbo].[FK__SUser__CompanyId__3A81B327]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SUser] DROP CONSTRAINT [FK__SUser__CompanyId__3A81B327];
GO
IF OBJECT_ID(N'[dbo].[FK_SMenu_SModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SMenu] DROP CONSTRAINT [FK_SMenu_SModule];
GO
IF OBJECT_ID(N'[dbo].[FK_SMenuCategory_SModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SCategory] DROP CONSTRAINT [FK_SMenuCategory_SModule];
GO
IF OBJECT_ID(N'[dbo].[FK_SRoLe_SCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SRoLe] DROP CONSTRAINT [FK_SRoLe_SCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_SUserRole_SRoLe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SUserRole] DROP CONSTRAINT [FK_SUserRole_SRoLe];
GO
IF OBJECT_ID(N'[dbo].[FK_SUserRole_SUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SUserRole] DROP CONSTRAINT [FK_SUserRole_SUser];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_ManiVer_De_T_CA_Phase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase_Mani] DROP CONSTRAINT [FK_T_CA_Phase_ManiVer_De_T_CA_Phase];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_ManiVer_De_T_ManipulationLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase_Mani] DROP CONSTRAINT [FK_T_CA_Phase_ManiVer_De_T_ManipulationLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_SWorkerLevel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase] DROP CONSTRAINT [FK_T_CA_Phase_SWorkerLevel];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_T_CommodityAnalysis]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase] DROP CONSTRAINT [FK_T_CA_Phase_T_CommodityAnalysis];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_T_Equipment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase] DROP CONSTRAINT [FK_T_CA_Phase_T_Equipment];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_T_PhaseGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase] DROP CONSTRAINT [FK_T_CA_Phase_T_PhaseGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_TimePrepare_T_CA_Phase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase_TimePrepare] DROP CONSTRAINT [FK_T_CA_Phase_TimePrepare_T_CA_Phase];
GO
IF OBJECT_ID(N'[dbo].[FK_T_CA_Phase_TimePrepare_T_TimePrepare]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_CA_Phase_TimePrepare] DROP CONSTRAINT [FK_T_CA_Phase_TimePrepare_T_TimePrepare];
GO
IF OBJECT_ID(N'[dbo].[FK_T_Equipment_T_EquipmentGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_Equipment] DROP CONSTRAINT [FK_T_Equipment_T_EquipmentGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_T_Equipment_T_EquipmentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_Equipment] DROP CONSTRAINT [FK_T_Equipment_T_EquipmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipmentAttribute_T_Equipment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipmentAttribute] DROP CONSTRAINT [FK_T_EquipmentAttribute_T_Equipment];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipmentAttribute_T_EquipmentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipmentAttribute] DROP CONSTRAINT [FK_T_EquipmentAttribute_T_EquipmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipmentType_T_EquipType_Default]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipmentType] DROP CONSTRAINT [FK_T_EquipmentType_T_EquipType_Default];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipmentTypeAttribute_T_EquipmentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipmentTypeAttribute] DROP CONSTRAINT [FK_T_EquipmentTypeAttribute_T_EquipmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipmentTypeAttribute_T_EquipTypeAttr_Default]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipmentTypeAttribute] DROP CONSTRAINT [FK_T_EquipmentTypeAttribute_T_EquipTypeAttr_Default];
GO
IF OBJECT_ID(N'[dbo].[FK_T_EquipTypeAttr_Default_T_EquipType_Default]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_EquipTypeAttr_Default] DROP CONSTRAINT [FK_T_EquipTypeAttr_Default_T_EquipType_Default];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LabourDivision_SUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LabourDivision] DROP CONSTRAINT [FK_T_LabourDivision_SUser];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LabourDivision_SUser1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LabourDivision] DROP CONSTRAINT [FK_T_LabourDivision_SUser1];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LabourDivision_T_CommoAna]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LabourDivision] DROP CONSTRAINT [FK_T_LabourDivision_T_CommoAna];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LabourDivision_T_Line]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LabourDivision] DROP CONSTRAINT [FK_T_LabourDivision_T_Line];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LabourDivision_T_TechProcessVersion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LabourDivision] DROP CONSTRAINT [FK_T_LabourDivision_T_TechProcessVersion];
GO
IF OBJECT_ID(N'[dbo].[FK_T_Line_T_WorkShop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_Line] DROP CONSTRAINT [FK_T_Line_T_WorkShop];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LinePosition_HR_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LinePosition] DROP CONSTRAINT [FK_T_LinePosition_HR_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LinePosition_T_LabourDivision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LinePosition] DROP CONSTRAINT [FK_T_LinePosition_T_LabourDivision];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LinePositionDetail_T_LinePosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LinePositionDetail] DROP CONSTRAINT [FK_T_LinePositionDetail_T_LinePosition];
GO
IF OBJECT_ID(N'[dbo].[FK_T_LinePositionDetail_T_TechProcessVersionDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_LinePositionDetail] DROP CONSTRAINT [FK_T_LinePositionDetail_T_TechProcessVersionDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationEquipment_T_Equipment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationEquipment] DROP CONSTRAINT [FK_T_ManipulationEquipment_T_Equipment];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationEquipment_T_ManipulationLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationEquipment] DROP CONSTRAINT [FK_T_ManipulationEquipment_T_ManipulationLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationFile_T_File]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationFile] DROP CONSTRAINT [FK_T_ManipulationFile_T_File];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationFile_T_ManipulationLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationFile] DROP CONSTRAINT [FK_T_ManipulationFile_T_ManipulationLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_ApplyPressureLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_ApplyPressureLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_EquipmentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_EquipmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_EquipmentType1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_EquipmentType1];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_ManipulationTypeLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_ManipulationTypeLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_ManipulationTypeLibrary1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_ManipulationTypeLibrary1];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_NatureCutsLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_NatureCutsLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_ManipulationLibrary_T_StopPrecisionLibrary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_ManipulationLibrary] DROP CONSTRAINT [FK_T_ManipulationLibrary_T_StopPrecisionLibrary];
GO
IF OBJECT_ID(N'[dbo].[FK_T_TechProcessVersion_T_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_TechProcessVersion] DROP CONSTRAINT [FK_T_TechProcessVersion_T_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_T_TechProcessVersionDetail_T_CA_Phase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_TechProcessVersionDetail] DROP CONSTRAINT [FK_T_TechProcessVersionDetail_T_CA_Phase];
GO
IF OBJECT_ID(N'[dbo].[FK_T_TechProcessVersionDetail_T_TechProcessVersion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_TechProcessVersionDetail] DROP CONSTRAINT [FK_T_TechProcessVersionDetail_T_TechProcessVersion];
GO
IF OBJECT_ID(N'[dbo].[FK_T_TimePrepare_T_TimeTypePrepare]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[T_TimePrepare] DROP CONSTRAINT [FK_T_TimePrepare_T_TimeTypePrepare];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_1_SCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SCompanyModule] DROP CONSTRAINT [FK_Table_1_SCompany];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[HR_Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HR_Employee];
GO
IF OBJECT_ID(N'[dbo].[SCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCategory];
GO
IF OBJECT_ID(N'[dbo].[SCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCompany];
GO
IF OBJECT_ID(N'[dbo].[SCompanyModule]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCompanyModule];
GO
IF OBJECT_ID(N'[dbo].[SConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SConfig];
GO
IF OBJECT_ID(N'[dbo].[SFeature]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SFeature];
GO
IF OBJECT_ID(N'[dbo].[SLevelCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SLevelCompany];
GO
IF OBJECT_ID(N'[dbo].[SMenu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SMenu];
GO
IF OBJECT_ID(N'[dbo].[SModule]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SModule];
GO
IF OBJECT_ID(N'[dbo].[SPermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SPermission];
GO
IF OBJECT_ID(N'[dbo].[SRoLe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SRoLe];
GO
IF OBJECT_ID(N'[dbo].[SRolePermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SRolePermission];
GO
IF OBJECT_ID(N'[dbo].[SUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SUser];
GO
IF OBJECT_ID(N'[dbo].[SUserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SUserRole];
GO
IF OBJECT_ID(N'[dbo].[SWorkerLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SWorkerLevel];
GO
IF OBJECT_ID(N'[dbo].[T_ApplyPressureLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_ApplyPressureLibrary];
GO
IF OBJECT_ID(N'[dbo].[T_CA_Phase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CA_Phase];
GO
IF OBJECT_ID(N'[dbo].[T_CA_Phase_Mani]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CA_Phase_Mani];
GO
IF OBJECT_ID(N'[dbo].[T_CA_Phase_TimePrepare]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CA_Phase_TimePrepare];
GO
IF OBJECT_ID(N'[dbo].[T_CommodityAnalysis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CommodityAnalysis];
GO
IF OBJECT_ID(N'[dbo].[T_CompanySkill]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CompanySkill];
GO
IF OBJECT_ID(N'[dbo].[T_CompanySkillDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_CompanySkillDetail];
GO
IF OBJECT_ID(N'[dbo].[T_Employee_CompanySkill]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Employee_CompanySkill];
GO
IF OBJECT_ID(N'[dbo].[T_Employee_PhaseGroupSkill]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Employee_PhaseGroupSkill];
GO
IF OBJECT_ID(N'[dbo].[T_EmployeePhase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EmployeePhase];
GO
IF OBJECT_ID(N'[dbo].[T_Equipment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Equipment];
GO
IF OBJECT_ID(N'[dbo].[T_EquipmentAttribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipmentAttribute];
GO
IF OBJECT_ID(N'[dbo].[T_EquipmentGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipmentGroup];
GO
IF OBJECT_ID(N'[dbo].[T_EquipmentType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipmentType];
GO
IF OBJECT_ID(N'[dbo].[T_EquipmentTypeAttribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipmentTypeAttribute];
GO
IF OBJECT_ID(N'[dbo].[T_EquipType_Default]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipType_Default];
GO
IF OBJECT_ID(N'[dbo].[T_EquipTypeAttr_Default]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_EquipTypeAttr_Default];
GO
IF OBJECT_ID(N'[dbo].[T_File]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_File];
GO
IF OBJECT_ID(N'[dbo].[T_IEDConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_IEDConfig];
GO
IF OBJECT_ID(N'[dbo].[T_LabourDivision]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_LabourDivision];
GO
IF OBJECT_ID(N'[dbo].[T_Line]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Line];
GO
IF OBJECT_ID(N'[dbo].[T_LinePosition]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_LinePosition];
GO
IF OBJECT_ID(N'[dbo].[T_LinePositionDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_LinePositionDetail];
GO
IF OBJECT_ID(N'[dbo].[T_ManipulationEquipment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_ManipulationEquipment];
GO
IF OBJECT_ID(N'[dbo].[T_ManipulationFile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_ManipulationFile];
GO
IF OBJECT_ID(N'[dbo].[T_ManipulationLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_ManipulationLibrary];
GO
IF OBJECT_ID(N'[dbo].[T_ManipulationTypeLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_ManipulationTypeLibrary];
GO
IF OBJECT_ID(N'[dbo].[T_NatureCutsLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_NatureCutsLibrary];
GO
IF OBJECT_ID(N'[dbo].[T_PhaseGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_PhaseGroup];
GO
IF OBJECT_ID(N'[dbo].[T_Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Product];
GO
IF OBJECT_ID(N'[dbo].[T_StopPrecisionLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_StopPrecisionLibrary];
GO
IF OBJECT_ID(N'[dbo].[T_TechProcessVersion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_TechProcessVersion];
GO
IF OBJECT_ID(N'[dbo].[T_TechProcessVersionDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_TechProcessVersionDetail];
GO
IF OBJECT_ID(N'[dbo].[T_TimePrepare]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_TimePrepare];
GO
IF OBJECT_ID(N'[dbo].[T_TimeTypePrepare]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_TimeTypePrepare];
GO
IF OBJECT_ID(N'[dbo].[T_WorkShop]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_WorkShop];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'HR_Employee'
CREATE TABLE [dbo].[HR_Employee] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(500)  NOT NULL,
    [FirstName] nvarchar(550)  NOT NULL,
    [LastName] nvarchar(550)  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Gender] bit  NOT NULL,
    [CompanyId] int  NULL,
    [Image] varchar(550)  NULL,
    [Email] varchar(250)  NULL,
    [Mobile] varchar(50)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SCategories'
CREATE TABLE [dbo].[SCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Position] nvarchar(200)  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [Description] nvarchar(255)  NULL,
    [Icon] nvarchar(255)  NULL,
    [IsViewIcon] bit  NOT NULL,
    [Link] nvarchar(255)  NULL,
    [ModuleId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CompanyId] int  NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SCompanies'
CREATE TABLE [dbo].[SCompanies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelCompanyId] int  NOT NULL,
    [CompanyName] nvarchar(250)  NOT NULL,
    [Address] nvarchar(250)  NULL,
    [Telephone] nvarchar(20)  NULL,
    [TaxCode] nvarchar(20)  NULL,
    [ParentId] int  NULL,
    [ParentNode] nvarchar(200)  NULL,
    [Logo] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [IsOwner] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SCompanyModules'
CREATE TABLE [dbo].[SCompanyModules] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ModuleName] nvarchar(250)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SConfigs'
CREATE TABLE [dbo].[SConfigs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ConpanyId] int  NOT NULL,
    [TableName] nvarchar(50)  NULL,
    [ObjectId] int  NULL,
    [IsHidden] bit  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SFeatures'
CREATE TABLE [dbo].[SFeatures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FeatureName] nvarchar(100)  NOT NULL,
    [SystemName] nvarchar(100)  NOT NULL,
    [ModuleId] int  NOT NULL,
    [Description] nvarchar(200)  NULL,
    [IsSystem] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SLevelCompanies'
CREATE TABLE [dbo].[SLevelCompanies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelName] nvarchar(200)  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SMenus'
CREATE TABLE [dbo].[SMenus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MenuCategoryId] int  NOT NULL,
    [MenuName] nvarchar(200)  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [Link] nvarchar(255)  NULL,
    [IsShow] bit  NOT NULL,
    [IsViewIcon] bit  NOT NULL,
    [Icon] nvarchar(255)  NULL,
    [Description] nvarchar(255)  NULL,
    [IsDeleted] bit  NOT NULL,
    [ModuleId] int  NOT NULL,
    [CompanyId] int  NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SModules'
CREATE TABLE [dbo].[SModules] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemName] nvarchar(100)  NOT NULL,
    [ModuleName] nvarchar(200)  NOT NULL,
    [IsSystem] bit  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [Description] nvarchar(100)  NULL,
    [ModuleUrl] nvarchar(255)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL,
    [IsShow] bit  NOT NULL
);
GO

-- Creating table 'SPermissions'
CREATE TABLE [dbo].[SPermissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FeatureId] int  NOT NULL,
    [PermissionTypeId] int  NOT NULL,
    [SystemName] nvarchar(100)  NOT NULL,
    [PermissionName] nvarchar(100)  NOT NULL,
    [Description] nvarchar(100)  NULL,
    [Url] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SRoLes'
CREATE TABLE [dbo].[SRoLes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [RoleName] nvarchar(200)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsSystem] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SRolePermissions'
CREATE TABLE [dbo].[SRolePermissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] int  NOT NULL,
    [ModuleId] int  NOT NULL,
    [FeatureId] int  NOT NULL,
    [PermissionId] int  NOT NULL,
    [Description] nvarchar(100)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SUsers'
CREATE TABLE [dbo].[SUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [IsOwner] bit  NOT NULL,
    [UserName] nvarchar(200)  NOT NULL,
    [PassWord] nvarchar(250)  NOT NULL,
    [IsLock] bit  NOT NULL,
    [IsRequireChangePW] bit  NOT NULL,
    [IsForgotPassword] bit  NOT NULL,
    [NoteForgotPassword] nvarchar(250)  NULL,
    [Email] nvarchar(250)  NULL,
    [ImagePath] nvarchar(200)  NULL,
    [LockedTime] datetime  NULL,
    [LastName] nvarchar(100)  NULL,
    [FisrtName] nvarchar(200)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SUserRoles'
CREATE TABLE [dbo].[SUserRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Decription] nvarchar(200)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'SWorkerLevels'
CREATE TABLE [dbo].[SWorkerLevels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Coefficient] float  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CompanyId] int  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_ApplyPressureLibrary'
CREATE TABLE [dbo].[T_ApplyPressureLibrary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Level] nvarchar(250)  NOT NULL,
    [Value] float  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CA_Phase'
CREATE TABLE [dbo].[T_CA_Phase] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Index] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] varchar(50)  NULL,
    [Description] nvarchar(max)  NULL,
    [EquipmentId] int  NULL,
    [WorkerLevelId] int  NOT NULL,
    [PhaseGroupId] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [TotalTMU] float  NOT NULL,
    [ApplyPressuresId] int  NULL,
    [PercentWasteEquipment] float  NOT NULL,
    [PercentWasteManipulation] float  NOT NULL,
    [PercentWasteSpecial] float  NOT NULL,
    [PercentWasteMaterial] float  NOT NULL,
    [Node] varchar(150)  NOT NULL,
    [Video] varchar(500)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CA_Phase_Mani'
CREATE TABLE [dbo].[T_CA_Phase_Mani] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CA_PhaseId] int  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [ManipulationId] int  NULL,
    [ManipulationCode] nchar(30)  NULL,
    [ManipulationName] nvarchar(500)  NULL,
    [TMUEquipment] float  NULL,
    [TMUManipulation] float  NULL,
    [Loop] float  NOT NULL,
    [TotalTMU] float  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CA_Phase_TimePrepare'
CREATE TABLE [dbo].[T_CA_Phase_TimePrepare] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Commo_Ana_PhaseId] int  NOT NULL,
    [TimePrepareId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CommodityAnalysis'
CREATE TABLE [dbo].[T_CommodityAnalysis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [ObjectType] int  NOT NULL,
    [ObjectId] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [Node] varchar(250)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [CompanyId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CompanySkill'
CREATE TABLE [dbo].[T_CompanySkill] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Coefficient] float  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [CompanyId] int  NOT NULL,
    [SkillLevel] float  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_CompanySkillDetail'
CREATE TABLE [dbo].[T_CompanySkillDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanySkillId] int  NOT NULL,
    [PhaseGroupId] int  NOT NULL,
    [PassLevel] int  NOT NULL,
    [Performance] float  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_Employee_CompanySkill'
CREATE TABLE [dbo].[T_Employee_CompanySkill] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [CompanySkillId] int  NOT NULL,
    [Coefficient] float  NOT NULL,
    [CompanyId] int  NOT NULL,
    [IsCalculatedByFormula] bit  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_Employee_PhaseGroupSkill'
CREATE TABLE [dbo].[T_Employee_PhaseGroupSkill] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [PhaseGroupId] int  NOT NULL,
    [Level] float  NOT NULL,
    [Performance] float  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_EmployeePhase'
CREATE TABLE [dbo].[T_EmployeePhase] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [CommodityId] int  NOT NULL,
    [LineId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [ListEmployeePhase] nvarchar(200)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL,
    [Estatus] bit  NULL
);
GO

-- Creating table 'T_Equipment'
CREATE TABLE [dbo].[T_Equipment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(500)  NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Expend] float  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [EquipmentTypeId] int  NOT NULL,
    [EquipmentGroupId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL,
    [ObjectType] int  NULL
);
GO

-- Creating table 'T_EquipmentAttribute'
CREATE TABLE [dbo].[T_EquipmentAttribute] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EquipmentId] int  NOT NULL,
    [EquipmentTypeId] int  NOT NULL,
    [Column1] nvarchar(200)  NULL,
    [Column2] nvarchar(200)  NULL,
    [Column3] nvarchar(200)  NULL,
    [Column4] nvarchar(200)  NULL,
    [Column5] nvarchar(200)  NULL,
    [Column6] nvarchar(200)  NULL,
    [Column7] nvarchar(200)  NULL,
    [Column8] nvarchar(200)  NULL,
    [Column9] nvarchar(200)  NULL,
    [Column10] nvarchar(200)  NULL,
    [Column11] nvarchar(200)  NULL,
    [Column12] nvarchar(200)  NULL,
    [Column13] nvarchar(200)  NULL,
    [Column14] nvarchar(200)  NULL,
    [Column15] nvarchar(200)  NULL,
    [Column16] nvarchar(200)  NULL,
    [Column17] nvarchar(200)  NULL,
    [Column18] nvarchar(200)  NULL,
    [Column19] nvarchar(200)  NULL,
    [Column20] nvarchar(200)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_EquipmentGroup'
CREATE TABLE [dbo].[T_EquipmentGroup] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(max)  NOT NULL,
    [GroupCode] nvarchar(500)  NOT NULL,
    [Icon] nvarchar(500)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_EquipmentType'
CREATE TABLE [dbo].[T_EquipmentType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Code] varchar(250)  NULL,
    [Description] nvarchar(max)  NULL,
    [EquipTypeDefaultId] int  NULL,
    [ObjectType] int  NULL,
    [CompanyId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_EquipmentTypeAttribute'
CREATE TABLE [dbo].[T_EquipmentTypeAttribute] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [EquipmentTypeId] int  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [IsUseForTime] bit  NOT NULL,
    [EquipTypeAtrrDefault_Id] int  NULL,
    [IsDefault] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_EquipType_Default'
CREATE TABLE [dbo].[T_EquipType_Default] (
    [Id] int  NOT NULL,
    [Name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'T_EquipTypeAttr_Default'
CREATE TABLE [dbo].[T_EquipTypeAttr_Default] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Code] nvarchar(250)  NOT NULL,
    [EquipType_DefaultId] int  NOT NULL
);
GO

-- Creating table 'T_File'
CREATE TABLE [dbo].[T_File] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(250)  NULL,
    [Path] nvarchar(max)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [Size] int  NULL,
    [FileType] nchar(10)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_IEDConfig'
CREATE TABLE [dbo].[T_IEDConfig] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Value] nvarchar(500)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_LabourDivision'
CREATE TABLE [dbo].[T_LabourDivision] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TechProVer_Id] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [LineId] int  NOT NULL,
    [TotalPosition] int  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_Line'
CREATE TABLE [dbo].[T_Line] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(50)  NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Description] nvarchar(200)  NULL,
    [CountOfLabours] int  NOT NULL,
    [WorkShopId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_LinePosition'
CREATE TABLE [dbo].[T_LinePosition] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LabourDivisionId] int  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [EmployeeId] int  NULL,
    [IsHasBTP] bit  NOT NULL,
    [IsHasExitLine] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_LinePositionDetail'
CREATE TABLE [dbo].[T_LinePositionDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Line_PositionId] int  NOT NULL,
    [TechProVerDe_Id] int  NOT NULL,
    [OrderIndex] int  NOT NULL,
    [DevisionPercent] float  NOT NULL,
    [NumberOfLabor] float  NOT NULL,
    [Note] nvarchar(500)  NULL,
    [IsPass] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_ManipulationEquipment'
CREATE TABLE [dbo].[T_ManipulationEquipment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ManipulationId] int  NOT NULL,
    [EquipmentId] int  NOT NULL,
    [UserTMU] float  NOT NULL,
    [MachineTMU] float  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_ManipulationFile'
CREATE TABLE [dbo].[T_ManipulationFile] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ManipulationId] int  NOT NULL,
    [FileId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_ManipulationLibrary'
CREATE TABLE [dbo].[T_ManipulationLibrary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ManipulationTypeId] int  NOT NULL,
    [Code] nvarchar(500)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [StandardTMU] float  NOT NULL,
    [UserTMU] float  NOT NULL,
    [EquipmentTypeId] int  NULL,
    [StopPrecisionId] int  NULL,
    [ApplyPressureId] int  NULL,
    [NatureCutsId] int  NULL,
    [Distance] float  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_ManipulationTypeLibrary'
CREATE TABLE [dbo].[T_ManipulationTypeLibrary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(500)  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ParentId] int  NOT NULL,
    [IsUseMachine] bit  NOT NULL,
    [Node] varchar(250)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_NatureCutsLibrary'
CREATE TABLE [dbo].[T_NatureCutsLibrary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Percent] float  NOT NULL,
    [Factor] float  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL
);
GO

-- Creating table 'T_PhaseGroup'
CREATE TABLE [dbo].[T_PhaseGroup] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [MaxLevel] float  NOT NULL,
    [MinLevel] float  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_Product'
CREATE TABLE [dbo].[T_Product] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(500)  NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Img] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CompanyId] int  NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_StopPrecisionLibrary'
CREATE TABLE [dbo].[T_StopPrecisionLibrary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [StopPrecision] nvarchar(250)  NOT NULL,
    [MTM_2] varchar(250)  NOT NULL,
    [TMUNumber] float  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_TechProcessVersion'
CREATE TABLE [dbo].[T_TechProcessVersion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [TimeCompletePerCommo] float  NOT NULL,
    [NumberOfWorkers] int  NOT NULL,
    [WorkingTimePerDay] float  NOT NULL,
    [PacedProduction] float  NOT NULL,
    [ProOfGroupPerHour] float  NOT NULL,
    [ProOfGroupPerDay] float  NOT NULL,
    [ProOfPersonPerDay] float  NOT NULL,
    [PricePerSecond] float  NOT NULL,
    [Allowance] float  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_TechProcessVersionDetail'
CREATE TABLE [dbo].[T_TechProcessVersionDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TechProcessVersionId] int  NOT NULL,
    [CA_PhaseId] int  NOT NULL,
    [StandardTMU] float  NOT NULL,
    [Percent] float  NOT NULL,
    [TimeByPercent] float  NOT NULL,
    [Worker] float  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [DeletedUser] int  NULL,
    [UpdatedUser] int  NULL,
    [CreatedDate] datetime  NULL,
    [DeletedDate] datetime  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'T_TimePrepare'
CREATE TABLE [dbo].[T_TimePrepare] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] nvarchar(250)  NULL,
    [TMUNumber] float  NOT NULL,
    [TimeTypePrepareId] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_TimeTypePrepare'
CREATE TABLE [dbo].[T_TimeTypePrepare] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] nvarchar(250)  NULL,
    [IsPublic] bit  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- Creating table 'T_WorkShop'
CREATE TABLE [dbo].[T_WorkShop] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Code] varchar(250)  NULL,
    [Description] nvarchar(max)  NULL,
    [CompanyId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedUser] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedUser] int  NULL,
    [UpdatedDate] datetime  NULL,
    [DeletedUser] int  NULL,
    [DeletedDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'HR_Employee'
ALTER TABLE [dbo].[HR_Employee]
ADD CONSTRAINT [PK_HR_Employee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SCategories'
ALTER TABLE [dbo].[SCategories]
ADD CONSTRAINT [PK_SCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SCompanies'
ALTER TABLE [dbo].[SCompanies]
ADD CONSTRAINT [PK_SCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SCompanyModules'
ALTER TABLE [dbo].[SCompanyModules]
ADD CONSTRAINT [PK_SCompanyModules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SConfigs'
ALTER TABLE [dbo].[SConfigs]
ADD CONSTRAINT [PK_SConfigs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SFeatures'
ALTER TABLE [dbo].[SFeatures]
ADD CONSTRAINT [PK_SFeatures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SLevelCompanies'
ALTER TABLE [dbo].[SLevelCompanies]
ADD CONSTRAINT [PK_SLevelCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SMenus'
ALTER TABLE [dbo].[SMenus]
ADD CONSTRAINT [PK_SMenus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SModules'
ALTER TABLE [dbo].[SModules]
ADD CONSTRAINT [PK_SModules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SPermissions'
ALTER TABLE [dbo].[SPermissions]
ADD CONSTRAINT [PK_SPermissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SRoLes'
ALTER TABLE [dbo].[SRoLes]
ADD CONSTRAINT [PK_SRoLes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SRolePermissions'
ALTER TABLE [dbo].[SRolePermissions]
ADD CONSTRAINT [PK_SRolePermissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SUsers'
ALTER TABLE [dbo].[SUsers]
ADD CONSTRAINT [PK_SUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SUserRoles'
ALTER TABLE [dbo].[SUserRoles]
ADD CONSTRAINT [PK_SUserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SWorkerLevels'
ALTER TABLE [dbo].[SWorkerLevels]
ADD CONSTRAINT [PK_SWorkerLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_ApplyPressureLibrary'
ALTER TABLE [dbo].[T_ApplyPressureLibrary]
ADD CONSTRAINT [PK_T_ApplyPressureLibrary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CA_Phase'
ALTER TABLE [dbo].[T_CA_Phase]
ADD CONSTRAINT [PK_T_CA_Phase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CA_Phase_Mani'
ALTER TABLE [dbo].[T_CA_Phase_Mani]
ADD CONSTRAINT [PK_T_CA_Phase_Mani]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CA_Phase_TimePrepare'
ALTER TABLE [dbo].[T_CA_Phase_TimePrepare]
ADD CONSTRAINT [PK_T_CA_Phase_TimePrepare]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CommodityAnalysis'
ALTER TABLE [dbo].[T_CommodityAnalysis]
ADD CONSTRAINT [PK_T_CommodityAnalysis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CompanySkill'
ALTER TABLE [dbo].[T_CompanySkill]
ADD CONSTRAINT [PK_T_CompanySkill]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_CompanySkillDetail'
ALTER TABLE [dbo].[T_CompanySkillDetail]
ADD CONSTRAINT [PK_T_CompanySkillDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Employee_CompanySkill'
ALTER TABLE [dbo].[T_Employee_CompanySkill]
ADD CONSTRAINT [PK_T_Employee_CompanySkill]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Employee_PhaseGroupSkill'
ALTER TABLE [dbo].[T_Employee_PhaseGroupSkill]
ADD CONSTRAINT [PK_T_Employee_PhaseGroupSkill]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EmployeePhase'
ALTER TABLE [dbo].[T_EmployeePhase]
ADD CONSTRAINT [PK_T_EmployeePhase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Equipment'
ALTER TABLE [dbo].[T_Equipment]
ADD CONSTRAINT [PK_T_Equipment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipmentAttribute'
ALTER TABLE [dbo].[T_EquipmentAttribute]
ADD CONSTRAINT [PK_T_EquipmentAttribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipmentGroup'
ALTER TABLE [dbo].[T_EquipmentGroup]
ADD CONSTRAINT [PK_T_EquipmentGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipmentType'
ALTER TABLE [dbo].[T_EquipmentType]
ADD CONSTRAINT [PK_T_EquipmentType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipmentTypeAttribute'
ALTER TABLE [dbo].[T_EquipmentTypeAttribute]
ADD CONSTRAINT [PK_T_EquipmentTypeAttribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipType_Default'
ALTER TABLE [dbo].[T_EquipType_Default]
ADD CONSTRAINT [PK_T_EquipType_Default]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_EquipTypeAttr_Default'
ALTER TABLE [dbo].[T_EquipTypeAttr_Default]
ADD CONSTRAINT [PK_T_EquipTypeAttr_Default]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_File'
ALTER TABLE [dbo].[T_File]
ADD CONSTRAINT [PK_T_File]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_IEDConfig'
ALTER TABLE [dbo].[T_IEDConfig]
ADD CONSTRAINT [PK_T_IEDConfig]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [PK_T_LabourDivision]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Line'
ALTER TABLE [dbo].[T_Line]
ADD CONSTRAINT [PK_T_Line]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_LinePosition'
ALTER TABLE [dbo].[T_LinePosition]
ADD CONSTRAINT [PK_T_LinePosition]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_LinePositionDetail'
ALTER TABLE [dbo].[T_LinePositionDetail]
ADD CONSTRAINT [PK_T_LinePositionDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_ManipulationEquipment'
ALTER TABLE [dbo].[T_ManipulationEquipment]
ADD CONSTRAINT [PK_T_ManipulationEquipment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_ManipulationFile'
ALTER TABLE [dbo].[T_ManipulationFile]
ADD CONSTRAINT [PK_T_ManipulationFile]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [PK_T_ManipulationLibrary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_ManipulationTypeLibrary'
ALTER TABLE [dbo].[T_ManipulationTypeLibrary]
ADD CONSTRAINT [PK_T_ManipulationTypeLibrary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_NatureCutsLibrary'
ALTER TABLE [dbo].[T_NatureCutsLibrary]
ADD CONSTRAINT [PK_T_NatureCutsLibrary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_PhaseGroup'
ALTER TABLE [dbo].[T_PhaseGroup]
ADD CONSTRAINT [PK_T_PhaseGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Product'
ALTER TABLE [dbo].[T_Product]
ADD CONSTRAINT [PK_T_Product]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_StopPrecisionLibrary'
ALTER TABLE [dbo].[T_StopPrecisionLibrary]
ADD CONSTRAINT [PK_T_StopPrecisionLibrary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_TechProcessVersion'
ALTER TABLE [dbo].[T_TechProcessVersion]
ADD CONSTRAINT [PK_T_TechProcessVersion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_TechProcessVersionDetail'
ALTER TABLE [dbo].[T_TechProcessVersionDetail]
ADD CONSTRAINT [PK_T_TechProcessVersionDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_TimePrepare'
ALTER TABLE [dbo].[T_TimePrepare]
ADD CONSTRAINT [PK_T_TimePrepare]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_TimeTypePrepare'
ALTER TABLE [dbo].[T_TimeTypePrepare]
ADD CONSTRAINT [PK_T_TimeTypePrepare]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_WorkShop'
ALTER TABLE [dbo].[T_WorkShop]
ADD CONSTRAINT [PK_T_WorkShop]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EmployeeId] in table 'T_LinePosition'
ALTER TABLE [dbo].[T_LinePosition]
ADD CONSTRAINT [FK_T_LinePosition_HR_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[HR_Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LinePosition_HR_Employee'
CREATE INDEX [IX_FK_T_LinePosition_HR_Employee]
ON [dbo].[T_LinePosition]
    ([EmployeeId]);
GO

-- Creating foreign key on [MenuCategoryId] in table 'SMenus'
ALTER TABLE [dbo].[SMenus]
ADD CONSTRAINT [FK__SMenu__MenuCateg__03F0984C]
    FOREIGN KEY ([MenuCategoryId])
    REFERENCES [dbo].[SCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SMenu__MenuCateg__03F0984C'
CREATE INDEX [IX_FK__SMenu__MenuCateg__03F0984C]
ON [dbo].[SMenus]
    ([MenuCategoryId]);
GO

-- Creating foreign key on [ModuleId] in table 'SCategories'
ALTER TABLE [dbo].[SCategories]
ADD CONSTRAINT [FK_SMenuCategory_SModule]
    FOREIGN KEY ([ModuleId])
    REFERENCES [dbo].[SModules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SMenuCategory_SModule'
CREATE INDEX [IX_FK_SMenuCategory_SModule]
ON [dbo].[SCategories]
    ([ModuleId]);
GO

-- Creating foreign key on [ParentId] in table 'SCompanies'
ALTER TABLE [dbo].[SCompanies]
ADD CONSTRAINT [FK__SCompany__Parent__3B75D760]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[SCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SCompany__Parent__3B75D760'
CREATE INDEX [IX_FK__SCompany__Parent__3B75D760]
ON [dbo].[SCompanies]
    ([ParentId]);
GO

-- Creating foreign key on [CompanyId] in table 'SUsers'
ALTER TABLE [dbo].[SUsers]
ADD CONSTRAINT [FK__SUser__CompanyId__3A81B327]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[SCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SUser__CompanyId__3A81B327'
CREATE INDEX [IX_FK__SUser__CompanyId__3A81B327]
ON [dbo].[SUsers]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'SRoLes'
ALTER TABLE [dbo].[SRoLes]
ADD CONSTRAINT [FK_SRoLe_SCompany]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[SCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SRoLe_SCompany'
CREATE INDEX [IX_FK_SRoLe_SCompany]
ON [dbo].[SRoLes]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'SCompanyModules'
ALTER TABLE [dbo].[SCompanyModules]
ADD CONSTRAINT [FK_Table_1_SCompany]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[SCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_1_SCompany'
CREATE INDEX [IX_FK_Table_1_SCompany]
ON [dbo].[SCompanyModules]
    ([CompanyId]);
GO

-- Creating foreign key on [ModuleId] in table 'SFeatures'
ALTER TABLE [dbo].[SFeatures]
ADD CONSTRAINT [FK__SFeature__Module__3D5E1FD2]
    FOREIGN KEY ([ModuleId])
    REFERENCES [dbo].[SModules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SFeature__Module__3D5E1FD2'
CREATE INDEX [IX_FK__SFeature__Module__3D5E1FD2]
ON [dbo].[SFeatures]
    ([ModuleId]);
GO

-- Creating foreign key on [FeatureId] in table 'SPermissions'
ALTER TABLE [dbo].[SPermissions]
ADD CONSTRAINT [FK__SPermissi__Featu__3C69FB99]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[SFeatures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SPermissi__Featu__3C69FB99'
CREATE INDEX [IX_FK__SPermissi__Featu__3C69FB99]
ON [dbo].[SPermissions]
    ([FeatureId]);
GO

-- Creating foreign key on [FeatureId] in table 'SRolePermissions'
ALTER TABLE [dbo].[SRolePermissions]
ADD CONSTRAINT [FK__SRolePerm__Featu__403A8C7D]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[SFeatures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SRolePerm__Featu__403A8C7D'
CREATE INDEX [IX_FK__SRolePerm__Featu__403A8C7D]
ON [dbo].[SRolePermissions]
    ([FeatureId]);
GO

-- Creating foreign key on [ModuleId] in table 'SMenus'
ALTER TABLE [dbo].[SMenus]
ADD CONSTRAINT [FK_SMenu_SModule]
    FOREIGN KEY ([ModuleId])
    REFERENCES [dbo].[SModules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SMenu_SModule'
CREATE INDEX [IX_FK_SMenu_SModule]
ON [dbo].[SMenus]
    ([ModuleId]);
GO

-- Creating foreign key on [ModuleId] in table 'SRolePermissions'
ALTER TABLE [dbo].[SRolePermissions]
ADD CONSTRAINT [FK__SRolePerm__Modul__3F466844]
    FOREIGN KEY ([ModuleId])
    REFERENCES [dbo].[SModules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SRolePerm__Modul__3F466844'
CREATE INDEX [IX_FK__SRolePerm__Modul__3F466844]
ON [dbo].[SRolePermissions]
    ([ModuleId]);
GO

-- Creating foreign key on [PermissionId] in table 'SRolePermissions'
ALTER TABLE [dbo].[SRolePermissions]
ADD CONSTRAINT [FK__SRolePerm__Pemis__412EB0B6]
    FOREIGN KEY ([PermissionId])
    REFERENCES [dbo].[SPermissions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SRolePerm__Pemis__412EB0B6'
CREATE INDEX [IX_FK__SRolePerm__Pemis__412EB0B6]
ON [dbo].[SRolePermissions]
    ([PermissionId]);
GO

-- Creating foreign key on [RoleId] in table 'SRolePermissions'
ALTER TABLE [dbo].[SRolePermissions]
ADD CONSTRAINT [FK__SRolePerm__RoleI__3E52440B]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[SRoLes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SRolePerm__RoleI__3E52440B'
CREATE INDEX [IX_FK__SRolePerm__RoleI__3E52440B]
ON [dbo].[SRolePermissions]
    ([RoleId]);
GO

-- Creating foreign key on [RoleId] in table 'SUserRoles'
ALTER TABLE [dbo].[SUserRoles]
ADD CONSTRAINT [FK_SUserRole_SRoLe]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[SRoLes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SUserRole_SRoLe'
CREATE INDEX [IX_FK_SUserRole_SRoLe]
ON [dbo].[SUserRoles]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'SUserRoles'
ALTER TABLE [dbo].[SUserRoles]
ADD CONSTRAINT [FK_SUserRole_SUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[SUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SUserRole_SUser'
CREATE INDEX [IX_FK_SUserRole_SUser]
ON [dbo].[SUserRoles]
    ([UserId]);
GO

-- Creating foreign key on [CreatedUser] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [FK_T_LabourDivision_SUser]
    FOREIGN KEY ([CreatedUser])
    REFERENCES [dbo].[SUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LabourDivision_SUser'
CREATE INDEX [IX_FK_T_LabourDivision_SUser]
ON [dbo].[T_LabourDivision]
    ([CreatedUser]);
GO

-- Creating foreign key on [UpdatedUser] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [FK_T_LabourDivision_SUser1]
    FOREIGN KEY ([UpdatedUser])
    REFERENCES [dbo].[SUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LabourDivision_SUser1'
CREATE INDEX [IX_FK_T_LabourDivision_SUser1]
ON [dbo].[T_LabourDivision]
    ([UpdatedUser]);
GO

-- Creating foreign key on [WorkerLevelId] in table 'T_CA_Phase'
ALTER TABLE [dbo].[T_CA_Phase]
ADD CONSTRAINT [FK_T_CA_Phase_SWorkerLevel]
    FOREIGN KEY ([WorkerLevelId])
    REFERENCES [dbo].[SWorkerLevels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_SWorkerLevel'
CREATE INDEX [IX_FK_T_CA_Phase_SWorkerLevel]
ON [dbo].[T_CA_Phase]
    ([WorkerLevelId]);
GO

-- Creating foreign key on [ApplyPressureId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_ApplyPressureLibrary]
    FOREIGN KEY ([ApplyPressureId])
    REFERENCES [dbo].[T_ApplyPressureLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_ApplyPressureLibrary'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_ApplyPressureLibrary]
ON [dbo].[T_ManipulationLibrary]
    ([ApplyPressureId]);
GO

-- Creating foreign key on [CA_PhaseId] in table 'T_CA_Phase_Mani'
ALTER TABLE [dbo].[T_CA_Phase_Mani]
ADD CONSTRAINT [FK_T_CA_Phase_ManiVer_De_T_CA_Phase]
    FOREIGN KEY ([CA_PhaseId])
    REFERENCES [dbo].[T_CA_Phase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_ManiVer_De_T_CA_Phase'
CREATE INDEX [IX_FK_T_CA_Phase_ManiVer_De_T_CA_Phase]
ON [dbo].[T_CA_Phase_Mani]
    ([CA_PhaseId]);
GO

-- Creating foreign key on [ParentId] in table 'T_CA_Phase'
ALTER TABLE [dbo].[T_CA_Phase]
ADD CONSTRAINT [FK_T_CA_Phase_T_CommodityAnalysis]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[T_CommodityAnalysis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_T_CommodityAnalysis'
CREATE INDEX [IX_FK_T_CA_Phase_T_CommodityAnalysis]
ON [dbo].[T_CA_Phase]
    ([ParentId]);
GO

-- Creating foreign key on [EquipmentId] in table 'T_CA_Phase'
ALTER TABLE [dbo].[T_CA_Phase]
ADD CONSTRAINT [FK_T_CA_Phase_T_Equipment]
    FOREIGN KEY ([EquipmentId])
    REFERENCES [dbo].[T_Equipment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_T_Equipment'
CREATE INDEX [IX_FK_T_CA_Phase_T_Equipment]
ON [dbo].[T_CA_Phase]
    ([EquipmentId]);
GO

-- Creating foreign key on [PhaseGroupId] in table 'T_CA_Phase'
ALTER TABLE [dbo].[T_CA_Phase]
ADD CONSTRAINT [FK_T_CA_Phase_T_PhaseGroup]
    FOREIGN KEY ([PhaseGroupId])
    REFERENCES [dbo].[T_PhaseGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_T_PhaseGroup'
CREATE INDEX [IX_FK_T_CA_Phase_T_PhaseGroup]
ON [dbo].[T_CA_Phase]
    ([PhaseGroupId]);
GO

-- Creating foreign key on [Commo_Ana_PhaseId] in table 'T_CA_Phase_TimePrepare'
ALTER TABLE [dbo].[T_CA_Phase_TimePrepare]
ADD CONSTRAINT [FK_T_CA_Phase_TimePrepare_T_CA_Phase]
    FOREIGN KEY ([Commo_Ana_PhaseId])
    REFERENCES [dbo].[T_CA_Phase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_TimePrepare_T_CA_Phase'
CREATE INDEX [IX_FK_T_CA_Phase_TimePrepare_T_CA_Phase]
ON [dbo].[T_CA_Phase_TimePrepare]
    ([Commo_Ana_PhaseId]);
GO

-- Creating foreign key on [CA_PhaseId] in table 'T_TechProcessVersionDetail'
ALTER TABLE [dbo].[T_TechProcessVersionDetail]
ADD CONSTRAINT [FK_T_TechProcessVersionDetail_T_CA_Phase]
    FOREIGN KEY ([CA_PhaseId])
    REFERENCES [dbo].[T_CA_Phase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_TechProcessVersionDetail_T_CA_Phase'
CREATE INDEX [IX_FK_T_TechProcessVersionDetail_T_CA_Phase]
ON [dbo].[T_TechProcessVersionDetail]
    ([CA_PhaseId]);
GO

-- Creating foreign key on [ManipulationId] in table 'T_CA_Phase_Mani'
ALTER TABLE [dbo].[T_CA_Phase_Mani]
ADD CONSTRAINT [FK_T_CA_Phase_ManiVer_De_T_ManipulationLibrary]
    FOREIGN KEY ([ManipulationId])
    REFERENCES [dbo].[T_ManipulationLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_ManiVer_De_T_ManipulationLibrary'
CREATE INDEX [IX_FK_T_CA_Phase_ManiVer_De_T_ManipulationLibrary]
ON [dbo].[T_CA_Phase_Mani]
    ([ManipulationId]);
GO

-- Creating foreign key on [TimePrepareId] in table 'T_CA_Phase_TimePrepare'
ALTER TABLE [dbo].[T_CA_Phase_TimePrepare]
ADD CONSTRAINT [FK_T_CA_Phase_TimePrepare_T_TimePrepare]
    FOREIGN KEY ([TimePrepareId])
    REFERENCES [dbo].[T_TimePrepare]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_CA_Phase_TimePrepare_T_TimePrepare'
CREATE INDEX [IX_FK_T_CA_Phase_TimePrepare_T_TimePrepare]
ON [dbo].[T_CA_Phase_TimePrepare]
    ([TimePrepareId]);
GO

-- Creating foreign key on [ParentId] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [FK_T_LabourDivision_T_CommoAna]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[T_CommodityAnalysis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LabourDivision_T_CommoAna'
CREATE INDEX [IX_FK_T_LabourDivision_T_CommoAna]
ON [dbo].[T_LabourDivision]
    ([ParentId]);
GO

-- Creating foreign key on [EquipmentGroupId] in table 'T_Equipment'
ALTER TABLE [dbo].[T_Equipment]
ADD CONSTRAINT [FK_T_Equipment_T_EquipmentGroup]
    FOREIGN KEY ([EquipmentGroupId])
    REFERENCES [dbo].[T_EquipmentGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_Equipment_T_EquipmentGroup'
CREATE INDEX [IX_FK_T_Equipment_T_EquipmentGroup]
ON [dbo].[T_Equipment]
    ([EquipmentGroupId]);
GO

-- Creating foreign key on [EquipmentTypeId] in table 'T_Equipment'
ALTER TABLE [dbo].[T_Equipment]
ADD CONSTRAINT [FK_T_Equipment_T_EquipmentType]
    FOREIGN KEY ([EquipmentTypeId])
    REFERENCES [dbo].[T_EquipmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_Equipment_T_EquipmentType'
CREATE INDEX [IX_FK_T_Equipment_T_EquipmentType]
ON [dbo].[T_Equipment]
    ([EquipmentTypeId]);
GO

-- Creating foreign key on [EquipmentId] in table 'T_EquipmentAttribute'
ALTER TABLE [dbo].[T_EquipmentAttribute]
ADD CONSTRAINT [FK_T_EquipmentAttribute_T_Equipment]
    FOREIGN KEY ([EquipmentId])
    REFERENCES [dbo].[T_Equipment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipmentAttribute_T_Equipment'
CREATE INDEX [IX_FK_T_EquipmentAttribute_T_Equipment]
ON [dbo].[T_EquipmentAttribute]
    ([EquipmentId]);
GO

-- Creating foreign key on [EquipmentId] in table 'T_ManipulationEquipment'
ALTER TABLE [dbo].[T_ManipulationEquipment]
ADD CONSTRAINT [FK_T_ManipulationEquipment_T_Equipment]
    FOREIGN KEY ([EquipmentId])
    REFERENCES [dbo].[T_Equipment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationEquipment_T_Equipment'
CREATE INDEX [IX_FK_T_ManipulationEquipment_T_Equipment]
ON [dbo].[T_ManipulationEquipment]
    ([EquipmentId]);
GO

-- Creating foreign key on [EquipmentTypeId] in table 'T_EquipmentAttribute'
ALTER TABLE [dbo].[T_EquipmentAttribute]
ADD CONSTRAINT [FK_T_EquipmentAttribute_T_EquipmentType]
    FOREIGN KEY ([EquipmentTypeId])
    REFERENCES [dbo].[T_EquipmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipmentAttribute_T_EquipmentType'
CREATE INDEX [IX_FK_T_EquipmentAttribute_T_EquipmentType]
ON [dbo].[T_EquipmentAttribute]
    ([EquipmentTypeId]);
GO

-- Creating foreign key on [EquipTypeDefaultId] in table 'T_EquipmentType'
ALTER TABLE [dbo].[T_EquipmentType]
ADD CONSTRAINT [FK_T_EquipmentType_T_EquipType_Default]
    FOREIGN KEY ([EquipTypeDefaultId])
    REFERENCES [dbo].[T_EquipType_Default]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipmentType_T_EquipType_Default'
CREATE INDEX [IX_FK_T_EquipmentType_T_EquipType_Default]
ON [dbo].[T_EquipmentType]
    ([EquipTypeDefaultId]);
GO

-- Creating foreign key on [EquipmentTypeId] in table 'T_EquipmentTypeAttribute'
ALTER TABLE [dbo].[T_EquipmentTypeAttribute]
ADD CONSTRAINT [FK_T_EquipmentTypeAttribute_T_EquipmentType]
    FOREIGN KEY ([EquipmentTypeId])
    REFERENCES [dbo].[T_EquipmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipmentTypeAttribute_T_EquipmentType'
CREATE INDEX [IX_FK_T_EquipmentTypeAttribute_T_EquipmentType]
ON [dbo].[T_EquipmentTypeAttribute]
    ([EquipmentTypeId]);
GO

-- Creating foreign key on [EquipmentTypeId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_EquipmentType]
    FOREIGN KEY ([EquipmentTypeId])
    REFERENCES [dbo].[T_EquipmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_EquipmentType'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_EquipmentType]
ON [dbo].[T_ManipulationLibrary]
    ([EquipmentTypeId]);
GO

-- Creating foreign key on [EquipmentTypeId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_EquipmentType1]
    FOREIGN KEY ([EquipmentTypeId])
    REFERENCES [dbo].[T_EquipmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_EquipmentType1'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_EquipmentType1]
ON [dbo].[T_ManipulationLibrary]
    ([EquipmentTypeId]);
GO

-- Creating foreign key on [EquipTypeAtrrDefault_Id] in table 'T_EquipmentTypeAttribute'
ALTER TABLE [dbo].[T_EquipmentTypeAttribute]
ADD CONSTRAINT [FK_T_EquipmentTypeAttribute_T_EquipTypeAttr_Default]
    FOREIGN KEY ([EquipTypeAtrrDefault_Id])
    REFERENCES [dbo].[T_EquipTypeAttr_Default]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipmentTypeAttribute_T_EquipTypeAttr_Default'
CREATE INDEX [IX_FK_T_EquipmentTypeAttribute_T_EquipTypeAttr_Default]
ON [dbo].[T_EquipmentTypeAttribute]
    ([EquipTypeAtrrDefault_Id]);
GO

-- Creating foreign key on [EquipType_DefaultId] in table 'T_EquipTypeAttr_Default'
ALTER TABLE [dbo].[T_EquipTypeAttr_Default]
ADD CONSTRAINT [FK_T_EquipTypeAttr_Default_T_EquipType_Default]
    FOREIGN KEY ([EquipType_DefaultId])
    REFERENCES [dbo].[T_EquipType_Default]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_EquipTypeAttr_Default_T_EquipType_Default'
CREATE INDEX [IX_FK_T_EquipTypeAttr_Default_T_EquipType_Default]
ON [dbo].[T_EquipTypeAttr_Default]
    ([EquipType_DefaultId]);
GO

-- Creating foreign key on [FileId] in table 'T_ManipulationFile'
ALTER TABLE [dbo].[T_ManipulationFile]
ADD CONSTRAINT [FK_T_ManipulationFile_T_File]
    FOREIGN KEY ([FileId])
    REFERENCES [dbo].[T_File]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationFile_T_File'
CREATE INDEX [IX_FK_T_ManipulationFile_T_File]
ON [dbo].[T_ManipulationFile]
    ([FileId]);
GO

-- Creating foreign key on [LineId] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [FK_T_LabourDivision_T_Line]
    FOREIGN KEY ([LineId])
    REFERENCES [dbo].[T_Line]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LabourDivision_T_Line'
CREATE INDEX [IX_FK_T_LabourDivision_T_Line]
ON [dbo].[T_LabourDivision]
    ([LineId]);
GO

-- Creating foreign key on [TechProVer_Id] in table 'T_LabourDivision'
ALTER TABLE [dbo].[T_LabourDivision]
ADD CONSTRAINT [FK_T_LabourDivision_T_TechProcessVersion]
    FOREIGN KEY ([TechProVer_Id])
    REFERENCES [dbo].[T_TechProcessVersion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LabourDivision_T_TechProcessVersion'
CREATE INDEX [IX_FK_T_LabourDivision_T_TechProcessVersion]
ON [dbo].[T_LabourDivision]
    ([TechProVer_Id]);
GO

-- Creating foreign key on [LabourDivisionId] in table 'T_LinePosition'
ALTER TABLE [dbo].[T_LinePosition]
ADD CONSTRAINT [FK_T_LinePosition_T_LabourDivision]
    FOREIGN KEY ([LabourDivisionId])
    REFERENCES [dbo].[T_LabourDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LinePosition_T_LabourDivision'
CREATE INDEX [IX_FK_T_LinePosition_T_LabourDivision]
ON [dbo].[T_LinePosition]
    ([LabourDivisionId]);
GO

-- Creating foreign key on [WorkShopId] in table 'T_Line'
ALTER TABLE [dbo].[T_Line]
ADD CONSTRAINT [FK_T_Line_T_WorkShop]
    FOREIGN KEY ([WorkShopId])
    REFERENCES [dbo].[T_WorkShop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_Line_T_WorkShop'
CREATE INDEX [IX_FK_T_Line_T_WorkShop]
ON [dbo].[T_Line]
    ([WorkShopId]);
GO

-- Creating foreign key on [Line_PositionId] in table 'T_LinePositionDetail'
ALTER TABLE [dbo].[T_LinePositionDetail]
ADD CONSTRAINT [FK_T_LinePositionDetail_T_LinePosition]
    FOREIGN KEY ([Line_PositionId])
    REFERENCES [dbo].[T_LinePosition]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LinePositionDetail_T_LinePosition'
CREATE INDEX [IX_FK_T_LinePositionDetail_T_LinePosition]
ON [dbo].[T_LinePositionDetail]
    ([Line_PositionId]);
GO

-- Creating foreign key on [TechProVerDe_Id] in table 'T_LinePositionDetail'
ALTER TABLE [dbo].[T_LinePositionDetail]
ADD CONSTRAINT [FK_T_LinePositionDetail_T_TechProcessVersionDetail]
    FOREIGN KEY ([TechProVerDe_Id])
    REFERENCES [dbo].[T_TechProcessVersionDetail]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_LinePositionDetail_T_TechProcessVersionDetail'
CREATE INDEX [IX_FK_T_LinePositionDetail_T_TechProcessVersionDetail]
ON [dbo].[T_LinePositionDetail]
    ([TechProVerDe_Id]);
GO

-- Creating foreign key on [ManipulationId] in table 'T_ManipulationEquipment'
ALTER TABLE [dbo].[T_ManipulationEquipment]
ADD CONSTRAINT [FK_T_ManipulationEquipment_T_ManipulationLibrary]
    FOREIGN KEY ([ManipulationId])
    REFERENCES [dbo].[T_ManipulationLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationEquipment_T_ManipulationLibrary'
CREATE INDEX [IX_FK_T_ManipulationEquipment_T_ManipulationLibrary]
ON [dbo].[T_ManipulationEquipment]
    ([ManipulationId]);
GO

-- Creating foreign key on [ManipulationId] in table 'T_ManipulationFile'
ALTER TABLE [dbo].[T_ManipulationFile]
ADD CONSTRAINT [FK_T_ManipulationFile_T_ManipulationLibrary]
    FOREIGN KEY ([ManipulationId])
    REFERENCES [dbo].[T_ManipulationLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationFile_T_ManipulationLibrary'
CREATE INDEX [IX_FK_T_ManipulationFile_T_ManipulationLibrary]
ON [dbo].[T_ManipulationFile]
    ([ManipulationId]);
GO

-- Creating foreign key on [ManipulationTypeId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_ManipulationTypeLibrary]
    FOREIGN KEY ([ManipulationTypeId])
    REFERENCES [dbo].[T_ManipulationTypeLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_ManipulationTypeLibrary'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_ManipulationTypeLibrary]
ON [dbo].[T_ManipulationLibrary]
    ([ManipulationTypeId]);
GO

-- Creating foreign key on [ManipulationTypeId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_ManipulationTypeLibrary1]
    FOREIGN KEY ([ManipulationTypeId])
    REFERENCES [dbo].[T_ManipulationTypeLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_ManipulationTypeLibrary1'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_ManipulationTypeLibrary1]
ON [dbo].[T_ManipulationLibrary]
    ([ManipulationTypeId]);
GO

-- Creating foreign key on [NatureCutsId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_NatureCutsLibrary]
    FOREIGN KEY ([NatureCutsId])
    REFERENCES [dbo].[T_NatureCutsLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_NatureCutsLibrary'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_NatureCutsLibrary]
ON [dbo].[T_ManipulationLibrary]
    ([NatureCutsId]);
GO

-- Creating foreign key on [StopPrecisionId] in table 'T_ManipulationLibrary'
ALTER TABLE [dbo].[T_ManipulationLibrary]
ADD CONSTRAINT [FK_T_ManipulationLibrary_T_StopPrecisionLibrary]
    FOREIGN KEY ([StopPrecisionId])
    REFERENCES [dbo].[T_StopPrecisionLibrary]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_ManipulationLibrary_T_StopPrecisionLibrary'
CREATE INDEX [IX_FK_T_ManipulationLibrary_T_StopPrecisionLibrary]
ON [dbo].[T_ManipulationLibrary]
    ([StopPrecisionId]);
GO

-- Creating foreign key on [ProductId] in table 'T_TechProcessVersion'
ALTER TABLE [dbo].[T_TechProcessVersion]
ADD CONSTRAINT [FK_T_TechProcessVersion_T_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[T_Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_TechProcessVersion_T_Product'
CREATE INDEX [IX_FK_T_TechProcessVersion_T_Product]
ON [dbo].[T_TechProcessVersion]
    ([ProductId]);
GO

-- Creating foreign key on [TechProcessVersionId] in table 'T_TechProcessVersionDetail'
ALTER TABLE [dbo].[T_TechProcessVersionDetail]
ADD CONSTRAINT [FK_T_TechProcessVersionDetail_T_TechProcessVersion]
    FOREIGN KEY ([TechProcessVersionId])
    REFERENCES [dbo].[T_TechProcessVersion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_TechProcessVersionDetail_T_TechProcessVersion'
CREATE INDEX [IX_FK_T_TechProcessVersionDetail_T_TechProcessVersion]
ON [dbo].[T_TechProcessVersionDetail]
    ([TechProcessVersionId]);
GO

-- Creating foreign key on [TimeTypePrepareId] in table 'T_TimePrepare'
ALTER TABLE [dbo].[T_TimePrepare]
ADD CONSTRAINT [FK_T_TimePrepare_T_TimeTypePrepare]
    FOREIGN KEY ([TimeTypePrepareId])
    REFERENCES [dbo].[T_TimeTypePrepare]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_T_TimePrepare_T_TimeTypePrepare'
CREATE INDEX [IX_FK_T_TimePrepare_T_TimeTypePrepare]
ON [dbo].[T_TimePrepare]
    ([TimeTypePrepareId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------