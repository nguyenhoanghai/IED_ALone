using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Hugate.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLUsingTechLog
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLUsingTechLog _Instance;
        public static BLLUsingTechLog Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUsingTechLog();

                return _Instance;
            }
        }
        private BLLUsingTechLog() { }
        #endregion

        public ResponseBase Insert(T_UsingTechLog model)
        {
            ResponseBase result = new ResponseBase();
            result.IsSuccess = false; 
            try
            {
                using (db = new IEDEntities())
                {
                    T_UsingTech_Detail detail;
                    T_UsingTechLog obj = db.T_UsingTechLog
                        .FirstOrDefault(x =>
                        x.IsView == model.IsView &&
                                            x.UserId == model.UserId &&
                                            x.ProductId == model.ProductId &&
                                            x.PhaseId == model.PhaseId &&
                                            x.PhaseGroupId == model.PhaseGroupId &&
                                            x.QTCNId == model.QTCNId &&
                                            x.TKCId == model.TKCId);
                    if (obj != null)
                    {
                        detail = new T_UsingTech_Detail();
                        detail.UsingTechLogId = obj.Id;
                        detail.CreatedDate = model.CreatedDate;
                        db.T_UsingTech_Detail.Add(detail);
                    }
                    else
                    {
                        model.T_UsingTech_Detail = new List<T_UsingTech_Detail>();
                        detail = new T_UsingTech_Detail();
                        detail.CreatedDate = model.CreatedDate;
                        detail.T_UsingTechLog = model;
                        model.T_UsingTech_Detail.Add(detail);
                        db.T_UsingTechLog.Add(model);
                    }
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

        public UsingTechReportModel GetReport(   DateTime from, DateTime to)
        {
            UsingTechReportModel report = null;
            using (db = new IEDEntities())
            {
                var allProducts = db.T_CommodityAnalysis
                    .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity)
                    .Select(x => new { Id = x.Id, CreatedDate = x.CreatedDate, node = x.Node })
                    .ToList();

                var allWorkshops = db.T_CommodityAnalysis
                    .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isWorkShop)
                    .Select(x => new { Id = x.ObjectId, CreatedDate = x.CreatedDate, node = x.Node })
                    .ToList();

                var allPhases = db.T_CA_Phase
                    .Where(x => !x.IsDeleted && x.CreatedDate >= from && x.CreatedDate <= to) // && x.IsApprove)
                    .Select(x => new { Id = x.Id, CreatedDate = x.CreatedDate, node = x.Node, wkId = x.WorkShopId, ApproveDate = x.ApprovedDate , Status = x.Status })
                    .ToList();

                var allTechPhases = db.T_UsingTechLog
                                        .Where(x => x.PhaseId.HasValue && x.CreatedDate >= from && x.CreatedDate <= to)
                   .Select(x => new { wkId = x.WorkShopId, CreatedDate = x.CreatedDate, IsView = x.IsView })
                   .ToList();

                var wkShops = db.T_WorkShop
                                     .Where(x => !x.IsDeleted)
                .Select(x => new UsingTechReportDetailModel { WorkshopId = x.Id, Name = x.Name })
                .ToList();
                 
                report = new UsingTechReportModel(); 
                report.TotalViewPhase = allTechPhases.Where(x => x.IsView).Count();
                report.TotalDownloadPhase = allTechPhases.Where(x => !x.IsView).Count();


                if (wkShops.Count > 0)
                {
                    foreach (var ws in wkShops)
                    {
                        var _foundWS = allWorkshops.FirstOrDefault(x => x.Id == ws.WorkshopId);
                        if (_foundWS != null)
                        {
                            ws.TotalProduct = allWorkshops.Where(x => x.Id == ws.WorkshopId).Count();
                            ws.TotalPhase = allPhases.Where(x => x.wkId == ws.WorkshopId).Count();
                            ws.TotalSubmitPhase = allPhases.Where(x => x.wkId == ws.WorkshopId && x.Status == eStatus.Submit).Count();
                            ws.TotalApprovePhase = allPhases.Where(x => x.wkId == ws.WorkshopId && x.Status == eStatus.Approved).Count();
                            ws.TotalPhase = allPhases.Where(x => x.wkId == ws.WorkshopId).Count();
                            ws.TotalNewPhase = allPhases.Where(x => x.wkId == ws.WorkshopId && x.Status == eStatus.Approved && x.ApproveDate.HasValue && x.ApproveDate >= from && x.ApproveDate<=to).Count();
                            ws.TotalViewPhase = allTechPhases.Where(x => x.wkId == ws.WorkshopId && x.IsView).Count();
                            ws.TotalDownloadPhase = allTechPhases.Where(x => x.wkId == ws.WorkshopId && !x.IsView).Count();

                            report.TotalProduct += ws.TotalProduct;
                            report.TotalPhase += ws.TotalPhase;
                            report.TotalSubmitPhase += ws.TotalSubmitPhase;
                            report.TotalApprovePhase += ws.TotalApprovePhase;
                            report.TotalNewPhase += ws.TotalNewPhase;
                        }
                    }
                    report.Details = wkShops;
                }
            }
            return report;
        }
     
        public List<ReportTechDetailModel> GetReportDetail(int userId, int workshopId, bool isView, DateTime from, DateTime to)
        {
            using (db = new IEDEntities())
            {
                var iquery = db.T_UsingTech_Detail.Where(x => x.T_UsingTechLog.PhaseId.HasValue && x.T_UsingTechLog.IsView == isView && x.CreatedDate >= from && x.CreatedDate <= to);
                if (userId != 0)
                    iquery = iquery.Where(x => x.T_UsingTechLog.UserId == userId);
                if (workshopId != 0)
                    iquery = iquery.Where(x => x.T_UsingTechLog.WorkShopId == workshopId);

                return iquery
                    .Select(x => new ReportTechDetailModel
                    {
                        PhaseId = x.T_UsingTechLog.PhaseId ?? 0,
                        UserId = x.T_UsingTechLog.UserId,
                        UserName = (x.T_UsingTechLog.SUser.UserName + " (" + x.T_UsingTechLog.SUser.Name + ")"),
                        PhaseName = x.T_UsingTechLog.T_CA_Phase.Name,
                        CreatedDate = x.CreatedDate,
                        LogId = x.UsingTechLogId,
                        Note = x.T_UsingTechLog.Note,
                        WorkshopName = x.T_UsingTechLog.T_WorkShop.Name,
                        WorkshopId = x.T_UsingTechLog.WorkShopId
                    })
                    .OrderBy(x => x.UserId)
                    .ThenBy(x => x.WorkshopId)
                    .ThenBy(x => x.LogId)
                    .ThenBy(x => x.CreatedDate)
                   .ToList();
            }
        }
    }
}
