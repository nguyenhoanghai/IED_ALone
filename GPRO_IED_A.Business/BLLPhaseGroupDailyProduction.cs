using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using Hugate.Framework;

namespace GPRO_IED_A.Business
{
    public class BLLPhaseGroupDailyProduction
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLPhaseGroupDailyProduction _Instance;
        public static BLLPhaseGroupDailyProduction Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhaseGroupDailyProduction();

                return _Instance;
            }
        }
        private BLLPhaseGroupDailyProduction() { }
        #endregion

        public ResponseBase Insert(T_PhaseGroupDailyProduction model)
        {
            ResponseBase result = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {

                    db.T_PhaseGroupDailyProduction.Add(model);
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

        public PagedList<PhaseGroupDailyProductionModel> GetList(string date, int commoId, int phaseGroupId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    var _objs = db.T_PhaseGroupDailyProduction
                        .Where(x => !x.IsDeleted && x.Date == date && x.ComAnaId == commoId && x.PhaseGroupId == phaseGroupId)
                        .Select(x => new PhaseGroupDailyProductionModel()
                        {
                            Id = x.Id,
                            Date = x.Date,
                            CreatedDate = x.CreatedDate,
                            UserName = x.SUser.UserName,
                            Quantities = x.Quantities,
                            ComandType = x.ComandType
                        }).OrderBy(sorting).ToList();

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<PhaseGroupDailyProductionModel>(_objs, pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
