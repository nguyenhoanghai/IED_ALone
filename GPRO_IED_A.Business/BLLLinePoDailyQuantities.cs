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
    public class BLLLinePoDailyQuantities
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLLinePoDailyQuantities _Instance;
        public static BLLLinePoDailyQuantities Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLinePoDailyQuantities();

                return _Instance;
            }
        }
        private BLLLinePoDailyQuantities() { }
        #endregion

        public ResponseBase Insert(APILinePositionModel model)
        {
            ResponseBase result = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    if (model.Details.Count > 0)
                    {
                        T_LinePoDailyQuantities obj = null;
                        foreach (var item in model.Details)
                        {
                            if (item.Quantities != 0)
                            {
                                obj = new T_LinePoDailyQuantities();
                                obj.ComandType = (int)eCommandType.Increase;
                                obj.CreatedUser = model.ActionUserId;
                                obj.CreatedDate = DateTime.Now;
                                obj.Date = model.Date;
                                obj.Quantities = item.Quantities;
                                obj.PhaseId = item.PhaseId;
                                obj.EmployeeId = model.EmployeeId;
                                obj.LinePo_DetailId = item.Id;
                                obj.LabourDevisionId = model.labourDeId;
                                obj.LabourDevision_VerId = model.labourDeVerId;
                                db.T_LinePoDailyQuantities.Add(obj);
                            }
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Insert(T_LinePoDailyQuantities model)
        {
            ResponseBase result = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    db.T_LinePoDailyQuantities.Add(model);
                    db.SaveChanges();
                    result.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public PagedList<EmployeePhaseProduction> Gets(int _labourId, int _employeeId, int startIndexRecord, int pageSize, string sorting)
        {
            PagedList<EmployeePhaseProduction> pagelistReturn = null;
            List<EmployeePhaseProduction> phases = new List<EmployeePhaseProduction>();
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";
                    var _objsIQuery = db.T_LinePoDailyQuantities
                        //  .Where(x => !x.IsDeleted &&  && x.LabourDevision_VerId == _labourId)
                        .Where(x => !x.IsDeleted && x.LabourDevision_VerId == _labourId);
                    if (_employeeId != 0)
                        _objsIQuery = _objsIQuery.Where(x => x.T_LinePositionDetail.T_LinePosition.EmployeeId.Value == _employeeId);

                    var _objs = _objsIQuery.Select(x => new
                    {
                        Id = x.PhaseId,
                        Name = x.T_CA_Phase.Name,
                        Quantity = x.Quantities,
                        cmType = x.ComandType,
                        Coeff = x.T_CA_Phase.SWorkerLevel.Coefficient,
                        Price = x.T_LabourDivision.T_TechProcessVersion.PricePerSecond,
                        EmployName = (x.T_LinePositionDetail.T_LinePosition.EmployeeId.HasValue ? x.T_LinePositionDetail.T_LinePosition.HR_Employee.Name : "")
                    })
                     .GroupBy(x => x.Id).ToList();
                    if (_objs.Count > 0)
                    {
                        EmployeePhaseProduction _newObj;
                        int _increase = 0, _reduce = 0;
                        foreach (var item in _objs)
                        {
                            _newObj = new EmployeePhaseProduction();
                            _newObj.Id = item.Key;
                            _newObj.EmployeeName = item.FirstOrDefault().Name;
                            _newObj.Name = item.FirstOrDefault().Name;
                            _newObj.Price = item.FirstOrDefault().Price;
                            _newObj.Coefficient = item.FirstOrDefault().Coeff;
                            _increase = item.Where(x => x.cmType == (int)eCommandType.Increase).Sum(x => x.Quantity);
                            _reduce = item.Where(x => x.cmType == (int)eCommandType.Reduce).Sum(x => x.Quantity);

                            _newObj.Total = _increase - _reduce;
                            phases.Add(_newObj);
                        }
                    }

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    pagelistReturn = new PagedList<EmployeePhaseProduction>(phases, pageNumber, pageSize);
                    return pagelistReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportEmployeeProduction Gets(int _labourId, int _employeeId)
        {
            ReportEmployeeProduction _report = new ReportEmployeeProduction();
            try
            {
                using (db = new IEDEntities())
                {
                    var _objsIQuery = db.T_LinePoDailyQuantities
                        //  .Where(x => !x.IsDeleted &&  && x.LabourDevision_VerId == _labourId)
                        .Where(x => !x.IsDeleted && x.LabourDevision_VerId == _labourId);
                    if (_employeeId != 0)
                        _objsIQuery = _objsIQuery.Where(x => x.T_LinePositionDetail.T_LinePosition.EmployeeId.Value == _employeeId);

                    var _objs = _objsIQuery.Select(x => new
                    {
                        Id = x.PhaseId,
                        Name = x.T_CA_Phase.Name,
                        Quantity = x.Quantities,
                        cmType = x.ComandType,
                        Coeff = x.T_CA_Phase.SWorkerLevel.Coefficient,
                        Price = x.T_LabourDivision.T_TechProcessVersion.PricePerSecond
                    })
                        .GroupBy(x => x.Id).ToList();
                    var lbour = db.T_LabourDevision_Ver.Where(x => x.Id == _labourId).Select(x => new
                    {
                        LineName = x.T_LabourDivision.T_Line.Name,
                        ParentId = x.T_LabourDivision.ParentId
                    }).FirstOrDefault();

                    var _commo = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == lbour.ParentId);
                    var _id = Convert.ToInt32(_commo.Node.Split(',')[1]);


                    var procommos = db.T_CommodityAnalysis
                        .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity && x.Id == _id)
                        .Select(x => new { Id = x.Id, Name = x.Name }).FirstOrDefault();

                    _report.ProName = procommos.Name;
                    _report.WorkShopName = _commo.Name;
                    _report.LineName = lbour.LineName;
                    if (_objs.Count > 0)
                    {
                        EmployeePhaseProduction _newObj;
                        int _increase = 0, _reduce = 0;
                        foreach (var item in _objs)
                        {
                            _newObj = new EmployeePhaseProduction();
                            _newObj.Id = item.Key;
                            _newObj.Name = item.FirstOrDefault().Name;
                            _newObj.Price = item.FirstOrDefault().Price;
                            _newObj.Coefficient = item.FirstOrDefault().Coeff;
                            _increase = item.Where(x => x.cmType == (int)eCommandType.Increase).Sum(x => x.Quantity);
                            _reduce = item.Where(x => x.cmType == (int)eCommandType.Reduce).Sum(x => x.Quantity);

                            _newObj.Total = _increase - _reduce;
                            _report.Phases.Add(_newObj);
                        }
                    }

                    return _report;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PagedList<EmployeePhaseProduction> GetDetails(int _labourVerId, int _employeeId, string date, int startIndexRecord, int pageSize, string sorting)
        {
            PagedList<EmployeePhaseProduction> pagelistReturn = null;
            List<EmployeePhaseProduction> phases = new List<EmployeePhaseProduction>();
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";
                    /*
                    var _objsIQuery = db.T_LinePoDailyQuantities
                        //  .Where(x => !x.IsDeleted &&  && x.LabourDevision_VerId == _labourId)
                        .Where(x => !x.IsDeleted && x.LabourDevision_VerId == _labourVerId);
                    if (_employeeId != 0)
                        _objsIQuery = _objsIQuery.Where(x => x.T_LinePositionDetail.T_LinePosition.EmployeeId.Value == _employeeId);

                    var _objs = _objsIQuery.Select(x => new
                    {
                        Id = x.PhaseId,
                        Name = x.T_CA_Phase.Name,
                        Quantity = x.Quantities,
                        cmType = x.ComandType,
                        Coeff = x.T_CA_Phase.SWorkerLevel.Coefficient,
                        Price = x.T_LabourDivision.T_TechProcessVersion.PricePerSecond,
                        EmployName = (x.T_LinePositionDetail.T_LinePosition.EmployeeId.HasValue ? x.T_LinePositionDetail.T_LinePosition.HR_Employee.Name : "")
                    })
                     .GroupBy(x => x.Id).ToList();
                    if (_objs.Count > 0)
                    {
                        EmployeePhaseProduction _newObj;
                        int _increase = 0, _reduce = 0;
                        foreach (var item in _objs)
                        {
                            _newObj = new EmployeePhaseProduction();
                            _newObj.Id = item.Key;
                            _newObj.EmployeeName = item.FirstOrDefault().Name;
                            _newObj.Name = item.FirstOrDefault().Name;
                            _newObj.Price = item.FirstOrDefault().Price;
                            _newObj.Coefficient = item.FirstOrDefault().Coeff;
                            _increase = item.Where(x => x.cmType == (int)eCommandType.Increase).Sum(x => x.Quantity);
                            _reduce = item.Where(x => x.cmType == (int)eCommandType.Reduce).Sum(x => x.Quantity);

                            _newObj.Total = _increase - _reduce;
                            phases.Add(_newObj);
                        }
                    }
                    */


                    var _objs = db.T_LinePoDailyQuantities.Where(x => !x.IsDeleted && x.Date == date && x.EmployeeId == _employeeId && x.LabourDevision_VerId == _labourVerId).
                        Select(x => new
                        {
                            Date = x.CreatedDate,
                            UName = x.SUser.UserName,
                            EName = x.HR_Employee.Name,
                            PName = x.T_CA_Phase.Name,
                            PCode = x.T_CA_Phase.Code,
                            Quantities = x.Quantities,
                        }).ToList();





                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    pagelistReturn = new PagedList<EmployeePhaseProduction>(_objs.Select(x => new EmployeePhaseProduction()
                    {
                        Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                        EmployeeName = x.EName,
                        Name = x.PName,
                        Total = x.Quantities
                    }).ToList(), pageNumber, pageSize);
                    return pagelistReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeePhaseProductionInDay GetDetails(int _labourVerId, int _employeeId, string date)
        {
            var objReturn = new EmployeePhaseProductionInDay();
            try
            {
                using (db = new IEDEntities())
                {
                    var _objs = db.T_LinePoDailyQuantities.Where(x => !x.IsDeleted && x.Date == date && x.EmployeeId == _employeeId && x.LabourDevision_VerId == _labourVerId).
                        Select(x => new
                        {
                            Date = x.CreatedDate,
                            UName = x.SUser.UserName,
                            EName = x.HR_Employee.Name,
                            PName = x.T_CA_Phase.Name,
                            PCode = x.T_CA_Phase.Code,
                            Quantities = x.Quantities,
                        }).ToList();

                    var obj = db.T_LabourDevision_Ver.FirstOrDefault(x => x.Id == _labourVerId);
                    var _commo = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.T_LabourDivision.ParentId);
                    var _id = Convert.ToInt32(_commo.Node.Split(',')[1]);

                    var procommos = db.T_CommodityAnalysis
                        .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity && x.Id == _id)
                        .Select(x => new { Id = x.Id, Name = x.Name }).FirstOrDefault();

                    objReturn.ProName = procommos.Name;
                    objReturn.WorkshopName = _commo.Name;
                    objReturn.LineName = obj.T_LabourDivision.T_Line.Name;

                    objReturn.Details.AddRange(_objs.Select(x => new EmployeePhaseProduction()
                    {
                        CreatedDate = x.Date,
                        Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                        EmployeeName = x.EName,
                        Name = x.PName,
                        Total = x.Quantities
                    }).OrderByDescending(x => x.CreatedDate).ToList());
                    return objReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
