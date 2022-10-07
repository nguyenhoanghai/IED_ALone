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
                    var _objs = db.T_LinePoDailyQuantities
                        .Where(x => !x.IsDeleted && x.T_LinePositionDetail.T_LinePosition.EmployeeId.Value == _employeeId && x.LabourDevisionId == _labourId)
                        .Select(x => new { Id = x.PhaseId, Name = x.T_CA_Phase.Name, Quantity = x.Quantities, cmType = x.ComandType, Coeff = x.T_CA_Phase.SWorkerLevel.Coefficient, Price = x.T_LabourDivision.T_TechProcessVersion.PricePerSecond })
                        .GroupBy(x => x.Id).ToList();
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

        public  List<EmployeePhaseProduction> Gets(int _labourId, int _employeeId )
        { 
            List<EmployeePhaseProduction> phases = new List<EmployeePhaseProduction>();
            try
            {
                using (db = new IEDEntities())
                { 
                    var _objs = db.T_LinePoDailyQuantities
                        .Where(x => !x.IsDeleted && x.T_LinePositionDetail.T_LinePosition.EmployeeId.Value == _employeeId && x.LabourDevisionId == _labourId)
                        .Select(x => new { Id = x.PhaseId, Name = x.T_CA_Phase.Name, Quantity = x.Quantities, cmType = x.ComandType, Coeff = x.T_CA_Phase.SWorkerLevel.Coefficient, Price = x.T_LabourDivision.T_TechProcessVersion.PricePerSecond })
                        .GroupBy(x => x.Id).ToList();
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
                            phases.Add(_newObj);
                        }
                    }
                     
                    return phases;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
