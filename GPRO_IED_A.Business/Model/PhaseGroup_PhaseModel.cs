﻿using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business.Model
{
    public class PhaseGroup_PhaseModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? EquipmentId { get; set; }
        public int WorkerLevelId { get; set; }
        public int PhaseGroupId { get; set; } 
        public string PhaseGroupName { get; set; }
        public int TimePrepareId { get; set; }
        public string TimePrepareName { get; set; }
        public double TotalTMU { get; set; }
        public int? ApplyPressuresId { get; set; }
        public double PercentWasteEquipment { get; set; }
        public double PercentWasteManipulation { get; set; }
        public double PercentWasteSpecial { get; set; }
        public double PercentWasteMaterial { get; set; }
        public string Node { get; set; }
        public string Video { get; set; }
        public bool IsTimePrepareChange { get; set; }
        public bool IsAccessoryChange { get; set; }  
        public double ManiVerTMU { get; set; }
        public double TimePrepareTMU { get; set; }
        public string WorkerLevelName { get; set; }
        public bool HasChild { get; set; }
        public string EquipName { get; set; }
        public string EquipDes { get; set; }
        public int EquipTypeDefaultId { get; set; }
        public int ActionUser { get; set; }
        public bool IsLibrary { get; set; }
        public string Status { get; set; }
        public bool IsApprove { get; set; }
        public int? Approver { get; set; }
        public string ApproverName { get; set; }
        public DateTime? ApproveDate { get; set; }
        public List<PhaseGroup_Phase_ManiModel> actions { get; set; }
        public List<Commo_Ana_Phase_TimePrepareModel> timePrepares { get; set; }
        public PhaseGroup_PhaseModel()
        { 
            actions = new List<PhaseGroup_Phase_ManiModel>();
            timePrepares = new List<Commo_Ana_Phase_TimePrepareModel>();
        }
    }
}
