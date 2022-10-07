using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using Hugate.Framework;
using GPRO_IED_A.Business.Enum;

namespace GPRO_IED_A.Business
{
    public class BLLLinePosition
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLLinePosition _Instance;
        public static BLLLinePosition Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLinePosition();

                return _Instance;
            }
        }
        private BLLLinePosition() { }
        #endregion

        public LabourDevisionVerModel GetLinePositionWithLastVersion(int laDevisionId, string date)
        {
            using (var db = new IEDEntities())
            {
                var lastVerObj = db.T_LabourDevision_Ver
                      .Where(x => x.LabourDivisionId == laDevisionId)
                      .OrderByDescending(x => x.CreatedDate)
                      .FirstOrDefault();
                if (lastVerObj != null)
                {
                    var model = new LabourDevisionVerModel()
                    {
                        Id = lastVerObj.Id,
                        LaDevisionId = lastVerObj.LabourDivisionId,
                        CreatedDate = lastVerObj.CreatedDate,
                        NumberOfWorkers = lastVerObj.T_LabourDivision.T_TechProcessVersion.NumberOfWorkers,
                        TimeCompletePerCommo = lastVerObj.T_LabourDivision.T_TechProcessVersion.TimeCompletePerCommo,
                        WorkingTimePerDay = lastVerObj.T_LabourDivision.T_TechProcessVersion.WorkingTimePerDay,
                        PacedProduction = lastVerObj.T_LabourDivision.T_TechProcessVersion.PacedProduction,
                        ProOfGroupPerDay = lastVerObj.T_LabourDivision.T_TechProcessVersion.ProOfGroupPerDay,
                        ProOfGroupPerHour = lastVerObj.T_LabourDivision.T_TechProcessVersion.ProOfGroupPerHour,
                        ProOfPersonPerDay = lastVerObj.T_LabourDivision.T_TechProcessVersion.ProOfPersonPerDay,
                        Note = lastVerObj.T_LabourDivision.T_TechProcessVersion.Note,
                    };
                    var positionObjs = db.T_LinePosition.Where(x => !x.IsDeleted && x.LabourDivisionVerId == lastVerObj.Id)
                                       .Select(x => new LinePositionDailyModel()
                                       {
                                           Id = x.Id,
                                           Index = x.OrderIndex,
                                           EmployId = x.EmployeeId ?? 0,
                                           EmployName = x.EmployeeId.HasValue ? x.HR_Employee.Name   : "",
                                           LabourDevision_VerId = x.LabourDivisionVerId,
                                           LabourDevisionId = x.T_LabourDevision_Ver.LabourDivisionId,
                                           Phases = x.T_LinePositionDetail.Select(c => new LinePositionDetail_DailyModel()
                                           {
                                               Id = c.Id,
                                               PhaseName = c.T_TechProcessVersionDetail.T_CA_Phase.Name,
                                               PhaseId = c.T_TechProcessVersionDetail.CA_PhaseId,
                                               Quantities = 0
                                           }).ToList()
                                       })
                                       .ToList();
                    var quantitiesInDays = db.T_LinePoDailyQuantities
                        .Where(x => x.LabourDevision_VerId == lastVerObj.Id && x.Date == date)
                                       .Select(x => new
                                       {
                                           Id = x.T_LinePositionDetail.Id,
                                           Quantities = x.Quantities,
                                           ComandType = x.ComandType
                                       }).ToList();
                    if (positionObjs.Count > 0 && quantitiesInDays.Count > 0)
                    {
                        foreach (var item in positionObjs)
                        {
                            if (item.Phases != null && item.Phases.Count > 0)
                            {
                                for (int i = 0; i < item.Phases.Count; i++)
                                {
                                    var _increase = quantitiesInDays.Where(x => x.Id == item.Phases[i].Id && x.ComandType == (int)eCommandType.Increase).Sum(x => x.Quantities);
                                    var _reduce = quantitiesInDays.Where(x => x.Id == item.Phases[i].Id && x.ComandType == (int)eCommandType.Reduce).Sum(x => x.Quantities);
                                    item.Phases[i].Quantities = _increase - _reduce;
                                }
                            }
                        }
                    }
                    model.Positions.AddRange(positionObjs);
                    return model;
                }
            }
            return new LabourDevisionVerModel();
        }
    }
}
