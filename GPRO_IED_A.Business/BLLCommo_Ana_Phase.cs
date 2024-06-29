using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Hugate.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLCommo_Ana_Phase
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLCommo_Ana_Phase _Instance;
        public static BLLCommo_Ana_Phase Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCommo_Ana_Phase();

                return _Instance;
            }
        }
        private BLLCommo_Ana_Phase() { }
        #endregion

        bool checkPermis(T_CA_Phase obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public ResponseBase InsertOrUpdate(Commo_Ana_PhaseModel model, List<Commo_Ana_Phase_TimePrepareModel> timePreparesModel, bool isOwner, bool isMDGVersion)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    T_CA_Phase phase = null;
                    T_CA_Phase_TimePrepare timePrepare = null;
                    T_CA_Phase_Mani maniVerDetail;
                    DateTime now = DateTime.Now;
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.ParentId, false))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên Công Đoạn này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                    }
                    else
                    {
                        if (model.Id == 0)
                        {
                            #region create
                            //  var lastPhase = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == phaseModel.ParentId).OrderByDescending(x => x.Index).FirstOrDefault();
                            phase = new T_CA_Phase();
                            Parse.CopyObject(model, ref phase);
                            phase.Node = phase.Node + phase.ParentId + ",";
                            phase.CreatedDate = DateTime.Now;
                            phase.CreatedUser = model.ActionUser;

                            //TODO MDG
                            if (isMDGVersion)
                            {
                                phase.IsApprove = true;
                                phase.Approver = model.ActionUser;
                                phase.ApprovedDate = phase.CreatedDate;
                                phase.Status = eStatus.Approved;
                            }

                            //thêm workshop id
                            int _id = Convert.ToInt32(phase.Node.Split(',')[2]);
                            var _obj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == _id);
                            if (_obj != null)
                                phase.WorkShopId = _obj.ObjectId;

                            if (timePreparesModel != null && timePreparesModel.Count > 0)
                            {
                                // phase.TotalTMU = timePreparesModel.Sum(x => x.TMUNumber) / 27.8;
                                phase.T_CA_Phase_TimePrepare = new Collection<T_CA_Phase_TimePrepare>();
                                foreach (var item in timePreparesModel)
                                {
                                    timePrepare = new T_CA_Phase_TimePrepare();
                                    Parse.CopyObject(item, ref timePrepare);
                                    timePrepare.T_CA_Phase = phase;
                                    timePrepare.CreatedUser = phase.CreatedUser;
                                    timePrepare.CreatedDate = phase.CreatedDate;
                                    phase.T_CA_Phase_TimePrepare.Add(timePrepare);
                                }
                            }
                            if (model.actions != null && model.actions.Count > 0)
                            {
                                phase.T_CA_Phase_Mani = new Collection<T_CA_Phase_Mani>();

                                foreach (var item in model.actions)
                                {
                                    if (item.OrderIndex < model.actions.Count)
                                    {
                                        maniVerDetail = new T_CA_Phase_Mani();
                                        Parse.CopyObject(item, ref maniVerDetail);
                                        maniVerDetail.ManipulationCode = maniVerDetail.ManipulationCode.Trim();
                                        maniVerDetail.ManipulationName = maniVerDetail.ManipulationName.Trim();
                                        maniVerDetail.ManipulationId = maniVerDetail.ManipulationId == 0 ? null : maniVerDetail.ManipulationId;
                                        maniVerDetail.CreatedUser = phase.CreatedUser;
                                        maniVerDetail.CreatedDate = phase.CreatedDate;
                                        maniVerDetail.T_CA_Phase = phase;
                                        phase.T_CA_Phase_Mani.Add(maniVerDetail);
                                    }
                                }
                            }

                            db.T_CA_Phase.Add(phase);
                            db.SaveChanges();

                            //TODO MDG
                            if (isMDGVersion)
                            {
                                //version cũ
                                //TODO cđ cần duyệt mới dc sử dụng
                                //ktra xem co qtcn chua
                                int paId = (phase.Node.Substring(0, phase.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                                var qt = db.T_TechProcessVersion.FirstOrDefault(x => !x.IsDeleted && x.ParentId == paId);
                                if (qt != null)
                                {
                                    var allDetails = db.T_TechProcessVersionDetail.Where(x => !x.IsDeleted && x.TechProcessVersionId == qt.Id);

                                    var verDetail = new T_TechProcessVersionDetail();
                                    verDetail.TechProcessVersionId = qt.Id;
                                    verDetail.CA_PhaseId = phase.Id;
                                    verDetail.StandardTMU = phase.TotalTMU;
                                    verDetail.Percent = allDetails.First().Percent;
                                    verDetail.TimeByPercent = (phase.TotalTMU == 0 || verDetail.Percent == 0 ? 0 : Math.Round((phase.TotalTMU * 100) / verDetail.Percent, 3));
                                    verDetail.CreatedDate = phase.CreatedDate;
                                    verDetail.CreatedUser = phase.CreatedUser;


                                    qt.TimeCompletePerCommo = Math.Round((qt.TimeCompletePerCommo + verDetail.TimeByPercent), 3);
                                    qt.PacedProduction = (qt.NumberOfWorkers == 0 ? 0 : (Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3)));
                                    qt.ProOfGroupPerHour = Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3);
                                    qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                    qt.ProOfPersonPerDay = (qt.NumberOfWorkers == 0 ? 0 : (Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3)));
                                    foreach (var item in allDetails)
                                        item.Worker = (qt.PacedProduction == 0 ? 0 : (Math.Round(((item.TimeByPercent / qt.PacedProduction)), 3)));

                                    verDetail.Worker = (qt.PacedProduction == 0 ? 0 : (Math.Round(((verDetail.TimeByPercent / qt.PacedProduction)), 3)));
                                    db.T_TechProcessVersionDetail.Add(verDetail);
                                }
                            }

                            db.SaveChanges();
                            ReOrderPhase(db, phase.ParentId, phase.Index, phase.Id);
                            result.IsSuccess = true;
                            result.Data = phase.Id;
                            #endregion
                        }
                        else
                        {
                            #region update
                            phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (phase != null)
                            {
                                if (!checkPermis(phase, model.ActionUser, isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo công đoạn này nên bạn không cập nhật được thông tin cho công đoạn này." });
                                    return result;
                                }
                                //TODO MDG
                                if (phase.Status == eStatus.Submit && !isMDGVersion)
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Công đoạn này đang chờ duyệt nên bạn không cập nhật được thông tin cho công đoạn này." });
                                    return result;
                                }
                                //TODO MDG
                                if (phase.Status == eStatus.Approved && !isMDGVersion)
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Công đoạn này đã được duyệt nên bạn không cập nhật được thông tin cho công đoạn này." });
                                    return result;
                                }

                                /*
                                phase.Name = model.Name;
                                phase.Index = model.Index;
                                phase.WorkerLevelId = model.WorkerLevelId;
                                phase.Code = model.Code;
                                phase.Description = model.Description;
                                phase.EquipmentId = model.EquipmentId;
                                phase.TotalTMU = model.TotalTMU;
                                phase.ApplyPressuresId = model.ApplyPressuresId;
                                phase.PercentWasteEquipment = model.PercentWasteEquipment;
                                phase.PercentWasteManipulation = model.PercentWasteManipulation;
                                phase.PercentWasteMaterial = model.PercentWasteMaterial;
                                phase.PercentWasteSpecial = model.PercentWasteSpecial;
                                phase.Video = model.Video;
                                
                                phase.IsLibrary = model.IsLibrary;
                                phase.Status = model.Status;
                                */
                                phase.UpdatedDate = DateTime.Now;
                                phase.UpdatedUser = model.ActionUser;
                                string updateQuery = string.Format(@"UPDATE [dbo].[T_CA_Phase]
                                                                       SET [Index] = {0} 
                                                                          ,[Name] =N'{1}' 
                                                                          ,[Code] ='{2}'
                                                                          ,[Description] = N'{3}'
                                                                          ,[EquipmentId] ={4}  
                                                                          ,[WorkerLevelId] ={5}    
                                                                          ,[TotalTMU] ={6}  
                                                                          ,[ApplyPressuresId] ={7} 
                                                                          ,[PercentWasteEquipment] ={8} 
                                                                          ,[PercentWasteManipulation] ={9} 
                                                                          ,[PercentWasteSpecial] ={10}  
                                                                          ,[PercentWasteMaterial] ={11}  
                                                                          ,[Video] ='{12}' 
                                                                          ,[UpdatedUser] ={13}  
                                                                          ,[UpdatedDate] ='{14}' 
                                                                          ,[IsLibrary] ={15}  
                                                                          ,[Status] =N'{16}'
                                                                          ,[IsApprove] ={17} ", model.Index, model.Name, model.Code, model.Description, model.EquipmentId,
                                                                     model.WorkerLevelId, model.TotalTMU, model.ApplyPressuresId, model.PercentWasteEquipment,
                                                                     model.PercentWasteManipulation, model.PercentWasteMaterial, model.PercentWasteSpecial, model.Video,
                                                                     model.ActionUser, now, (model.IsLibrary ? 1 : 0), (isMDGVersion && !phase.IsApprove ? eStatus.Approved : model.Status),
                                                                     (isMDGVersion && !phase.IsApprove ? 1 : 0));

                                //TODO MDG
                                if (isMDGVersion && !phase.IsApprove)
                                {
                                    //phase.IsApprove = true;
                                    //phase.Approver = model.ActionUser;
                                    //phase.ApprovedDate = phase.CreatedDate;
                                    //phase.Status = eStatus.Approved;

                                    updateQuery += string.Format(@" ,[Approver] ={0}  ,[ApprovedDate] ='{1}' ", model.ActionUser, now);
                                }

                                updateQuery += string.Format(@" WHERE Id ={0}", model.Id);
                                db.Database.ExecuteSqlCommand(updateQuery);

                                #region time prepare
                                var oldTimes = db.T_CA_Phase_TimePrepare
                                    .Where(x => !x.IsDeleted && x.Commo_Ana_PhaseId == model.Id)
                                    .Select(x => new { Id = x.Id, TimePrepareId = x.TimePrepareId })
                                    .ToList();

                                if (oldTimes != null && oldTimes.Count() > 0 && (timePreparesModel == null || timePreparesModel.Count == 0))
                                {
                                    foreach (var item in oldTimes)
                                    {
                                        /*
                                        item.IsDeleted = true;
                                        item.DeletedUser = model.ActionUser;
                                        item.DeletedDate = DateTime.Now;
                                        */
                                        db.Database.ExecuteSqlCommand(string.Format(@"UPDATE [dbo].[T_CA_Phase_TimePrepare] SET  isdeleted =1, [DeletedUser] = {0} ,[DeletedDate] = '{1}' WHERE Id ={2}", model.ActionUser, now, item.Id));
                                    }
                                }
                                else if (oldTimes != null && oldTimes.Count() > 0 && timePreparesModel != null && timePreparesModel.Count > 0)
                                {
                                    foreach (var item in oldTimes)
                                    {
                                        var obj = timePreparesModel.FirstOrDefault(x => x.TimePrepareId == item.TimePrepareId);
                                        if (obj == null)
                                        {
                                            /*
                                            item.IsDeleted = true;
                                            item.DeletedUser = model.ActionUser;
                                            item.DeletedDate = DateTime.Now;
                                            */
                                            db.Database.ExecuteSqlCommand(string.Format(@"UPDATE [dbo].[T_CA_Phase_TimePrepare] SET  isdeleted =1, [DeletedUser] = {0} ,[DeletedDate] = '{1}' WHERE Id ={2}", model.ActionUser, now, item.Id));
                                        }
                                        else
                                            timePreparesModel.Remove(obj);
                                    }

                                    if (timePreparesModel.Count > 0)
                                        foreach (var item in timePreparesModel)
                                        {
                                            timePrepare = new T_CA_Phase_TimePrepare();
                                            timePrepare.Commo_Ana_PhaseId = phase.Id;
                                            timePrepare.TimePrepareId = item.TimePrepareId;
                                            timePrepare.CreatedUser = phase.UpdatedUser ?? 0;
                                            timePrepare.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                            db.T_CA_Phase_TimePrepare.Add(timePrepare);
                                        }
                                }
                                else if ((oldTimes == null || oldTimes.Count() == 0) && timePreparesModel != null && timePreparesModel.Count > 0)
                                {
                                    foreach (var item in timePreparesModel)
                                    {
                                        timePrepare = new T_CA_Phase_TimePrepare();
                                        timePrepare.Commo_Ana_PhaseId = phase.Id;
                                        timePrepare.TimePrepareId = item.TimePrepareId;
                                        timePrepare.CreatedUser = phase.UpdatedUser ?? 0;
                                        timePrepare.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                        db.T_CA_Phase_TimePrepare.Add(timePrepare);
                                    }
                                }
                                #endregion

                                #region actions
                                var oldDetails = db.T_CA_Phase_Mani
                                    .Where(x => !x.IsDeleted && x.CA_PhaseId == phase.Id)
                                    .Select(x => new
                                    {
                                        Id = x.Id,
                                        OrderIndex = x.OrderIndex,
                                        ManipulationCode = x.ManipulationCode,
                                        ManipulationName = x.ManipulationName,
                                        TMUEquipment = x.TMUEquipment,
                                        TMUManipulation = x.TMUManipulation,
                                        Loop = x.Loop,
                                        TotalTMU = x.TotalTMU,
                                        ManipulationId = x.ManipulationId
                                    }).ToList();
                                if (model.actions == null && model.actions.Count == 0 && oldDetails != null && oldDetails.Count() > 0)
                                {
                                    foreach (var item in oldDetails)
                                    {
                                        /*
                                        item.IsDeleted = true;
                                        item.DeletedUser = phase.UpdatedUser;
                                        item.DeletedDate = phase.UpdatedDate;
                                        */
                                        db.Database.ExecuteSqlCommand(string.Format(@"UPDATE [dbo].[T_CA_Phase_Mani] SET  isdeleted =1, [DeletedUser] = {0} ,[DeletedDate] = '{1}' WHERE Id ={2}", phase.UpdatedUser, phase.UpdatedDate, item.Id));
                                    }
                                }
                                else if (model.actions != null && model.actions.Count > 0 && oldDetails != null && oldDetails.Count() > 0)
                                {
                                    foreach (var item in oldDetails)
                                    {
                                        var obj = model.actions.FirstOrDefault(x => x.Id == item.Id);
                                        if (obj == null)
                                        {
                                            /*
                                            item.IsDeleted = true;
                                            item.DeletedUser = phase.UpdatedUser;
                                            item.DeletedDate = phase.UpdatedDate;
                                            */
                                            db.Database.ExecuteSqlCommand(string.Format(@"UPDATE [dbo].[T_CA_Phase_Mani] SET isdeleted =1,  [DeletedUser] = {0} ,[DeletedDate] = '{1}' WHERE Id ={2}", phase.UpdatedUser, phase.UpdatedDate, item.Id));
                                        }
                                        else
                                        {
                                            /*
                                            item.OrderIndex = obj.OrderIndex;
                                            item.ManipulationCode = obj.ManipulationCode.Trim();
                                            item.ManipulationName = obj.ManipulationName.Trim();
                                            item.TMUEquipment = obj.TMUEquipment;
                                            item.TMUManipulation = obj.TMUManipulation;
                                            item.Loop = obj.Loop;
                                            item.TotalTMU = obj.TotalTMU;
                                            item.ManipulationId = obj.ManipulationId == 0 ? null : obj.ManipulationId;
                                            item.UpdatedUser = phase.UpdatedUser;
                                            item.UpdatedDate = phase.UpdatedDate;
                                            */
                                            string _query = string.Format(@"UPDATE [dbo].[T_CA_Phase_Mani] SET  
                                                                                            [UpdatedUser] = {0} 
                                                                                            ,[UpdatedDate] = '{1}' 
                                                                                            ,[OrderIndex] = '{2}' 
                                                                                            ,[ManipulationCode] = '{3}' 
                                                                                            ,[ManipulationName] = N'{4}' 
                                                                                            ,[TMUEquipment] = '{5}' 
                                                                                            ,[TMUManipulation] = '{6}' 
                                                                                            ,[Loop] = '{7}' 
                                                                                            ,[TotalTMU] = '{8}'  
                                                                                            WHERE Id ={9}",
                                                                                            phase.UpdatedUser, phase.UpdatedDate, obj.OrderIndex, obj.ManipulationCode.Trim(),
                                                                                            obj.ManipulationName.Trim(), obj.TMUEquipment, obj.TMUManipulation, obj.Loop, obj.TotalTMU,
                                                                                            item.Id);
                                            db.Database.ExecuteSqlCommand(_query);
                                            model.actions.Remove(obj);
                                        }
                                    }
                                    if (model.actions.Count > 0)
                                        for (int i = 0; i < model.actions.Count; i++)
                                        {
                                            if (i < (model.actions.Count - 1))
                                            {
                                                maniVerDetail = new T_CA_Phase_Mani();
                                                Parse.CopyObject(model.actions[i], ref maniVerDetail);
                                                maniVerDetail.ManipulationId = model.actions[i].ManipulationId == 0 ? null : model.actions[i].ManipulationId;
                                                maniVerDetail.CA_PhaseId = phase.Id;
                                                maniVerDetail.CreatedUser = phase.UpdatedUser ?? 0;
                                                maniVerDetail.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                                db.T_CA_Phase_Mani.Add(maniVerDetail);
                                            }
                                        }
                                }
                                else if ((oldDetails == null || oldDetails.Count() == 0) && model.actions != null && model.actions.Count > 0)
                                {
                                    for (int i = 0; i < model.actions.Count; i++)
                                    {
                                        if (i < (model.actions.Count - 1))
                                        {
                                            maniVerDetail = new T_CA_Phase_Mani();
                                            Parse.CopyObject(model.actions[i], ref maniVerDetail);
                                            maniVerDetail.ManipulationId = model.actions[i].ManipulationId == 0 ? null : model.actions[i].ManipulationId;
                                            maniVerDetail.CA_PhaseId = phase.Id;
                                            maniVerDetail.CreatedUser = phase.UpdatedUser ?? 0;
                                            maniVerDetail.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                            db.T_CA_Phase_Mani.Add(maniVerDetail);
                                        }
                                    }
                                }
                                #endregion

                                //TODO MDG
                                if (isMDGVersion)
                                {
                                    //ktra xem co qtcn chua 
                                    int paId = (phase.Node.Substring(0, phase.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                                    var qt = db.T_TechProcessVersion.FirstOrDefault(x => !x.IsDeleted && x.ParentId == paId);
                                    if (qt != null)
                                    {
                                        var dt = db.T_TechProcessVersionDetail.FirstOrDefault(x => !x.IsDeleted && x.CA_PhaseId == phase.Id && x.TechProcessVersionId == qt.Id);
                                        if (dt == null)
                                        {
                                            var allDetails = db.T_TechProcessVersionDetail.Where(x => !x.IsDeleted && x.TechProcessVersionId == qt.Id).Select(x => new
                                            {
                                                Percent = x.Percent,
                                                Worker = x.Worker,
                                                TimeByPercent = x.TimeByPercent
                                            });
                                            var verDetail = new T_TechProcessVersionDetail();
                                            verDetail.TechProcessVersionId = qt.Id;
                                            verDetail.CA_PhaseId = phase.Id;
                                            verDetail.StandardTMU = phase.TotalTMU;
                                            verDetail.Percent = allDetails.First().Percent;
                                            verDetail.TimeByPercent = (phase.TotalTMU == 0 || verDetail.Percent == 0 ? 0 : Math.Round((phase.TotalTMU * 100) / verDetail.Percent, 3));
                                            verDetail.CreatedDate = phase.CreatedDate;
                                            verDetail.CreatedUser = phase.CreatedUser;


                                            qt.TimeCompletePerCommo = Math.Round((qt.TimeCompletePerCommo + verDetail.TimeByPercent), 3);
                                            qt.PacedProduction = (qt.NumberOfWorkers == 0 ? 0 : (Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3)));
                                            qt.ProOfGroupPerHour = Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3);
                                            qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                            qt.ProOfPersonPerDay = (qt.NumberOfWorkers == 0 ? 0 : (Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3)));
                                            //foreach (var item in allDetails)
                                            //    item.Worker = (qt.PacedProduction == 0 ? 0 : (Math.Round(((item.TimeByPercent / qt.PacedProduction)), 3)));

                                            verDetail.Worker = (qt.PacedProduction == 0 ? 0 : (Math.Round(((verDetail.TimeByPercent / qt.PacedProduction)), 3)));
                                            db.T_TechProcessVersionDetail.Add(verDetail);
                                        }
                                    }
                                }

                                db.SaveChanges();
                                ReOrderPhase(db, phase.ParentId, phase.Index, phase.Id);
                                result.IsSuccess = true;

                            }
                            #endregion
                        }

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase UpdateName(int phaseId, string newName, bool isOwner, int actionUser)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    #region update
                    var phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == phaseId);
                    if (phase != null)
                    {
                        if (!checkPermis(phase, actionUser, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo công đoạn này nên bạn không cập nhật được thông tin cho công đoạn này." });
                        }
                        else
                        {
                            phase.Name = newName;
                            phase.UpdatedDate = DateTime.Now;
                            phase.UpdatedUser = actionUser;
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                    }
                    #endregion
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Approve(int actionUserId, int phaseId, bool isApprove)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var rs = new ResponseBase();
                    var phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == phaseId);
                    if (phase == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã tồn tại hoặc đã bị xóa trước đó. Vui lòng kiểm tra lại dữ liệu!." });
                    }
                    else
                    {
                        if (isApprove)
                        {
                            phase.IsApprove = isApprove;
                            phase.Approver = actionUserId;
                            phase.ApprovedDate = DateTime.Now;
                            phase.Status = eStatus.Approved;

                            //ktra xem co qtcn chua 
                            int paId = (phase.Node.Substring(0, phase.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                            var qt = (from x in db.T_TechProcessVersion
                                      where !x.IsDeleted && x.ParentId == paId
                                      select x).FirstOrDefault();
                            if (qt != null)
                            {
                                var allDetails = (from x in db.T_TechProcessVersionDetail
                                                  where !x.IsDeleted && x.TechProcessVersionId == qt.Id
                                                  select x);

                                var verDetail = new T_TechProcessVersionDetail();
                                verDetail.TechProcessVersionId = qt.Id;
                                verDetail.CA_PhaseId = phase.Id;
                                verDetail.StandardTMU = phase.TotalTMU;
                                verDetail.Percent = allDetails.First().Percent;
                                verDetail.TimeByPercent = Math.Round((phase.TotalTMU * 100) / verDetail.Percent, 3);
                                verDetail.CreatedDate = phase.CreatedDate;
                                verDetail.CreatedUser = phase.CreatedUser;

                                qt.TimeCompletePerCommo = Math.Round((qt.TimeCompletePerCommo + verDetail.TimeByPercent), 3);
                                qt.PacedProduction = (qt.TimeCompletePerCommo > 0 && qt.NumberOfWorkers > 0 ? Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3) : 0);
                                qt.ProOfGroupPerHour = Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3);
                                qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                qt.ProOfPersonPerDay = (qt.ProOfGroupPerDay > 0 && qt.NumberOfWorkers > 0 ? Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3) : 0);
                                foreach (var item in allDetails)
                                    item.Worker = (item.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((item.TimeByPercent / qt.PacedProduction)), 3) : 0);

                                verDetail.Worker = (verDetail.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((verDetail.TimeByPercent / qt.PacedProduction)), 3) : 0);
                                db.T_TechProcessVersionDetail.Add(verDetail);
                                db.Entry<T_TechProcessVersion>(qt).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        if (!isApprove)
                        {
                            phase.Status = eStatus.Editor;
                            phase.UpdatedUser = actionUserId;
                            phase.UpdatedDate = DateTime.Now;
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReOrderPhase(IEDEntities db, int parentId, int stt, int phaseId)
        {
            //sap xiep lai thứ tự
            var phases = (from x in db.T_CA_Phase where !x.IsDeleted && x.ParentId == parentId && x.Index >= stt && x.Id != phaseId select x).OrderBy(x => x.Index);
            if (phases != null && phases.Count() > 0)
            {
                int _index = stt + 1;
                foreach (var item in phases)
                {
                    item.Index = _index;
                    if (item.Code.Split('-').Length > 1)
                    {
                        string code = item.Code.Substring(0, item.Code.LastIndexOf('-'));
                        item.Code = code + "-" + item.Index;
                    }
                    else
                    {
                        item.Code = item.Index.ToString();
                    }
                    _index++;
                }
            }
            db.SaveChanges();
        }

        private bool CheckExists(string keyword, int id, int parentId, bool isCheckCode)
        {
            try
            {
                T_CA_Phase phase = null;
                keyword = keyword.Trim().ToUpper();
                if (isCheckCode)
                    phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Code.Trim().ToUpper().Equals(keyword) && x.Id != id && x.ParentId == parentId);
                else
                    phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Name.Trim().ToUpper().Equals(keyword) && x.Id != id && x.ParentId == parentId);

                if (phase == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_CA_Phase> GetPhasesByParentIds(List<int> parentIds)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var phases = db.T_CA_Phase.Where(x => !x.IsDeleted && parentIds.Contains(x.ParentId));
                    if (phases != null && phases.Count() > 0)
                        return phases.ToList();
                    return new List<T_CA_Phase>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<PhaseLibModel> GetsWhichNotLib(string keyword, int startIndexRecord, int pageSize, string sorting)
        {
            var phases = new List<PhaseLibModel>();
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Name ASC";

                    var temps = (from x in db.T_CA_Phase
                                 where
                                 !x.IsDeleted &&
                                 !x.T_PhaseGroup.IsDeleted &&
                                 !x.IsLibrary &&
                                 x.IsApprove &&
                                  (x.Code.Trim().ToUpper().Contains(keyword) ||
                                   x.Name.Trim().ToUpper().Contains(keyword))
                                 select new PhaseLibModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Code = x.Code,
                                     TotalTMU = x.TotalTMU,
                                     EquipName = x.EquipmentId.HasValue ? x.T_Equipment.Name : "",
                                     Node = x.Node,
                                     Product = "",
                                     GroupPhase = ""
                                 }).ToList();
                    if (temps.Count > 0)
                    {
                        var masters = (from x in db.T_CommodityAnalysis where !x.IsDeleted select new { id = x.Id, name = x.Name, OType = x.ObjectType, OId = x.ObjectId }).ToList();
                        var products = (from x in db.T_Product where !x.IsDeleted select new { Id = x.Id, Name = x.Name }).ToList();
                        var phaseGroups = (from x in db.T_PhaseGroup where !x.IsDeleted select new { Id = x.Id, Name = x.Name }).ToList();
                        foreach (var item in temps)
                        {
                            int[] nodeArr = item.Node.Replace("0,0,", "0,").Split(',').Select(x => !string.IsNullOrEmpty(x) ? Convert.ToInt32(x) : 0).ToArray();
                            var found = masters.FirstOrDefault(x => x.id == nodeArr[1]);
                            if (found != null)
                            {
                                var foundPro = products.FirstOrDefault(x => x.Id == found.OId);
                                if (foundPro != null)
                                    item.Product = found.name;
                            }

                            found = masters.FirstOrDefault(x => x.id == nodeArr[4]);
                            if (found != null)
                            {
                                if (phaseGroups.FirstOrDefault(x => x.Id == found.OId) != null)
                                    item.GroupPhase = found.name;
                            }
                            if (!string.IsNullOrEmpty(item.Product) && !string.IsNullOrEmpty(item.GroupPhase))
                                phases.Add(item);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            switch (sorting)
            {
                case "Product ASC": phases = phases.OrderBy(x => x.Product).ToList(); break;
                case "Product DESC": phases = phases.OrderByDescending(x => x.Product).ToList(); break;
                case "GroupPhase ASC": phases = phases.OrderBy(x => x.GroupPhase).ToList(); break;
                case "GroupPhase DESC": phases = phases.OrderByDescending(x => x.GroupPhase).ToList(); break;
                case "Code ASC": phases = phases.OrderBy(x => x.Code).ToList(); break;
                case "Code DESC": phases = phases.OrderByDescending(x => x.Code).ToList(); break;
                case "EquipName ASC": phases = phases.OrderBy(x => x.EquipName).ToList(); break;
                case "EquipName DESC": phases = phases.OrderByDescending(x => x.EquipName).ToList(); break;
                case "Name DESC": phases = phases.OrderByDescending(x => x.Code).ToList(); break;
                default: phases = phases.OrderBy(x => x.Name).ToList(); break;
            }

            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<PhaseLibModel>(phases, pageNumber, pageSize);
        }

        public PagedList<PhaseLibModel> GetsWhichIsLib(string keyword, int startIndexRecord, int pageSize, string sorting)
        {
            var phases = new List<PhaseLibModel>();
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Name ASC";

                    var temps = (from x in db.T_CA_Phase
                                 where
                                 !x.IsDeleted &&
                                 !x.T_PhaseGroup.IsDeleted &&
                                 x.IsLibrary &&
                                 x.IsApprove &&
                                 (x.Code.Trim().ToUpper().Contains(keyword) ||
                                   x.Name.Trim().ToUpper().Contains(keyword))
                                 select new PhaseLibModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Code = x.Code,
                                     TotalTMU = x.TotalTMU,
                                     EquipName = x.EquipmentId.HasValue ? x.T_Equipment.Name : "",
                                     Node = x.Node,
                                     Product = "",
                                     GroupPhase = ""
                                 }).ToList();
                    if (temps.Count > 0)
                    {
                        var masters = (from x in db.T_CommodityAnalysis where !x.IsDeleted select new { id = x.Id, name = x.Name, OType = x.ObjectType, OId = x.ObjectId }).ToList();
                        var products = (from x in db.T_Product where !x.IsDeleted select new { Id = x.Id, Name = x.Name }).ToList();
                        var phaseGroups = (from x in db.T_PhaseGroup where !x.IsDeleted select new { Id = x.Id, Name = x.Name }).ToList();
                        foreach (var item in temps)
                        {
                            int[] nodeArr = item.Node.Replace("0,0,", "0,").Split(',').Select(x => !string.IsNullOrEmpty(x) ? Convert.ToInt32(x) : 0).ToArray();
                            var found = masters.FirstOrDefault(x => x.id == nodeArr[1]);
                            if (found != null)
                            {
                                var foundPro = products.FirstOrDefault(x => x.Id == found.OId);
                                if (foundPro != null)
                                    item.Product = found.name;
                            }

                            found = masters.FirstOrDefault(x => x.id == nodeArr[4]);
                            if (found != null)
                            {
                                if (phaseGroups.FirstOrDefault(x => x.Id == found.OId) != null)
                                    item.GroupPhase = found.name;
                            }


                            if (!string.IsNullOrEmpty(item.Product) && !string.IsNullOrEmpty(item.GroupPhase))
                                phases.Add(item);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            switch (sorting)
            {
                case "Product ASC": phases = phases.OrderBy(x => x.Product).ToList(); break;
                case "Product DESC": phases = phases.OrderByDescending(x => x.Product).ToList(); break;
                case "GroupPhase ASC": phases = phases.OrderBy(x => x.GroupPhase).ToList(); break;
                case "GroupPhase DESC": phases = phases.OrderByDescending(x => x.GroupPhase).ToList(); break;
                case "Code ASC": phases = phases.OrderBy(x => x.Code).ToList(); break;
                case "Code DESC": phases = phases.OrderByDescending(x => x.Code).ToList(); break;
                case "EquipName ASC": phases = phases.OrderBy(x => x.EquipName).ToList(); break;
                case "EquipName DESC": phases = phases.OrderByDescending(x => x.EquipName).ToList(); break;
                case "Name DESC": phases = phases.OrderByDescending(x => x.Code).ToList(); break;
                default: phases = phases.OrderBy(x => x.Name).ToList(); break;
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<PhaseLibModel>(phases.OrderBy(x => x.Product), pageNumber, pageSize);
        }

        public ResponseBase UpdateLibs(string ids, bool isLibrary)
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    string query = " ";
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] arr = ids.Split(',');
                        query += " UPDATE [dbo].[T_CA_Phase] SET [IsLibrary]=" + (isLibrary ? 1 : 0) + " where ";
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(arr[i]))
                            {
                                if (i == 0)
                                    query += " Id =" + arr[i];
                                if (i > 0)
                                    query += " or Id =" + arr[i];
                            }
                        }
                    }
                    db.Database.ExecuteSqlCommand(query);
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
            }
            return rs;
        }

        //load lai 
        public PagedList<Commo_Ana_PhaseModel_PL> GetListByNode(int currentUserId, bool isApprover, string node, int startIndexRecord, int pageSize, string sorting, bool isMDGVersion)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index ASC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;

                    var _phases = db.T_CA_Phase.Where(x => !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Node.Trim() == (node.Trim()));
                    if (!isApprover)
                        _phases = _phases.Where(x => x.CreatedUser == currentUserId || x.IsApprove);

                    int cawk = Convert.ToInt32(node.Split(',')[1]);
                    var proObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);
                    cawk = Convert.ToInt32(node.Split(',')[2]);
                    var wsObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);
                    cawk = Convert.ToInt32(node.Split(',')[4]);
                    var phaseGroupObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);

                    var phases = _phases.OrderBy(x => x.Index)
                                    .Select(x => new Commo_Ana_PhaseModel_PL()
                                    {
                                        Id = x.Id,
                                        Index = x.Index,
                                        Name = x.Name,
                                        //Code = proObj.Code + "" + wsObj.Code + "" + x.Code,
                                        Code = x.Code,
                                        TotalTMU = x.TotalTMU,
                                        WorkerLevelName = x.SWorkerLevel.Name,
                                        EquipName = x.T_Equipment.Name,
                                        Status = x.Status,
                                        //WorkshopId = wsObj.ObjectId,
                                        //WorkshopName = wsObj.Name,
                                        //ProductName = proObj.Name,

                                        //Description = x.Description,
                                        //EquipmentId = x.EquipmentId,

                                        //EquipDes = x.T_Equipment.Description,
                                        //EquipTypeDefaultId = x.EquipmentId != null ? x.T_Equipment.T_EquipmentType.EquipTypeDefaultId ?? 0 : 0,
                                        //WorkerLevelId = x.WorkerLevelId,

                                        //ParentId = x.ParentId,
                                        //PhaseGroupId = x.PhaseGroupId,
                                        //ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                                        //PercentWasteEquipment = x.PercentWasteEquipment,
                                        //PercentWasteManipulation = x.PercentWasteManipulation,
                                        //PercentWasteMaterial = x.PercentWasteMaterial,
                                        //PercentWasteSpecial = x.PercentWasteSpecial,
                                        //Video = x.Video,
                                        IsLibrary = x.IsLibrary,

                                        IsApprove = x.IsApprove,
                                        //Approver = x.Approver,
                                        //ApproverName = x.IsApprove ? x.SUser.UserName : "",
                                        //ApproveDate = x.ApprovedDate,
                                    }).OrderBy(sorting).ToList();

                    var pageListReturn = new PagedList<Commo_Ana_PhaseModel_PL>(phases, pageNumber, pageSize);
                    if (pageListReturn != null && pageListReturn.Count > 0)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);

                        //tao code
                        string[] wsDes = (!string.IsNullOrEmpty(wsObj.Description) ? wsObj.Description : "").Split('|').ToArray();
                        foreach (var item in pageListReturn)
                        {
                            if (isMDGVersion)
                                item.FullCode = item.Code;
                            else
                                item.FullCode = GenerateCode(item.Code, ((wsDes.Length > 1 && !string.IsNullOrEmpty(wsDes[1]) ? wsDes[1] : "") + "" + proObj.Code + "" + phaseGroupObj.Code));
                            /*
                            item.timePrepares.AddRange((from x in db.T_CA_Phase_TimePrepare
                                                        where !x.IsDeleted && x.Commo_Ana_PhaseId == item.Id
                                                        select new Commo_Ana_Phase_TimePrepareModel()
                                                        {
                                                            Id = x.Id,
                                                            TimePrepareId = x.TimePrepareId,
                                                            Name = x.T_TimePrepare.Name,
                                                            Code = x.T_TimePrepare.Code,
                                                            TimeTypePrepareName = x.T_TimePrepare.T_TimeTypePrepare.Name,
                                                            TMUNumber = x.T_TimePrepare.TMUNumber,
                                                            Description = x.T_TimePrepare.Description,
                                                        }));
                            item.TimePrepareTMU = item.timePrepares.Sum(x => x.TMUNumber);
                            item.actions.AddRange((from x in db.T_CA_Phase_Mani
                                                   where !x.IsDeleted && x.CA_PhaseId == item.Id
                                                   orderby x.OrderIndex
                                                   select new Commo_Ana_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       CA_PhaseId = x.CA_PhaseId,
                                                       ManipulationId = x.ManipulationId,
                                                       ManipulationName = x.ManipulationName,
                                                       ManipulationCode = x.ManipulationCode,
                                                       TMUEquipment = x.TMUEquipment,
                                                       TMUManipulation = x.TMUManipulation,
                                                       Loop = x.Loop,
                                                       TotalTMU = x.TotalTMU,
                                                       OrderIndex = x.OrderIndex
                                                   }));
                            item.ManiVerTMU = item.actions.Sum(x => ((x.TMUEquipment ?? 0 * x.Loop) + (x.TMUManipulation ?? 0 * x.Loop)));
                            */
                            if (isMDGVersion)
                            {
                                item.IsApprove = false;
                                item.Status = eStatus.Editor;
                            }
                        }
                    }
                    return pageListReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateCode(string so, string ma)
        {
            switch (so.Length)
            {
                case 1:
                    return ma + "00000" + so;
                case 2:
                    return ma + "0000" + so;
                case 3:
                    return ma + "000" + so;
                case 4:
                    return ma + "00" + so;
                case 5:
                    return ma + "0" + so;
                case 6:
                    return ma + so;
            }
            return "";
        }

        public PagedList<Commo_Ana_PhaseModel> Gets(string phaseName, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index ASC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    var iquery = db.T_CA_Phase.Where(x => !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Status == eStatus.Submit);
                    if (!string.IsNullOrEmpty(phaseName))
                        iquery = iquery.Where(x => x.Name.Trim().ToUpper().Contains(phaseName.Trim().ToUpper()) || x.T_PhaseGroup.Name.Trim().ToUpper().Contains(phaseName.Trim().ToUpper()));


                    var phases = iquery.OrderBy(x => x.Index)
                        .Select(x => new Commo_Ana_PhaseModel()
                        {
                            Id = x.Id,
                            Index = x.Index,
                            Name = x.Name,
                            Code = x.Code,
                            TotalTMU = x.TotalTMU,
                            Description = x.Description,
                            EquipmentId = x.EquipmentId,
                            EquipName = x.T_Equipment.Name,
                            EquipDes = x.T_Equipment.Description,
                            EquipTypeDefaultId = x.EquipmentId != null ? x.T_Equipment.T_EquipmentType.EquipTypeDefaultId ?? 0 : 0,
                            WorkerLevelId = x.WorkerLevelId,
                            WorkerLevelName = x.SWorkerLevel.Name,
                            ParentId = x.ParentId,
                            PhaseGroupId = x.PhaseGroupId,
                            PhaseGroupName = x.T_PhaseGroup.Name,
                            ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                            PercentWasteEquipment = x.PercentWasteEquipment,
                            PercentWasteManipulation = x.PercentWasteManipulation,
                            PercentWasteMaterial = x.PercentWasteMaterial,
                            PercentWasteSpecial = x.PercentWasteSpecial,
                            Video = x.Video,
                            IsLibrary = x.IsLibrary,
                            Status = x.Status,
                        }).OrderBy(x => x.ParentId).ThenBy(x => x.Index).ToList();

                    var pageListReturn = new PagedList<Commo_Ana_PhaseModel>(phases, pageNumber, pageSize);
                    if (pageListReturn != null && pageListReturn.Count > 0)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);

                        var phasegroupIds = pageListReturn.Select(x => x.ParentId).Distinct().ToList();
                        var phasegroups = db.T_CommodityAnalysis
                            .Where(x => !x.IsDeleted && phasegroupIds.Contains(x.Id))
                            .Select(x => new ModelSelectItem
                            {
                                Value = x.Id,
                                Code = x.Node,
                                Name = x.Name,
                                Data = 0,
                                Double = 0
                            }).ToList();
                        var ids = new List<int>();
                        for (int i = 0; i < phasegroups.Count; i++)
                        {
                            ids.Add(phasegroups[i].Value);
                            var _ids = phasegroups[i].Code.Split(',');

                            phasegroups[i].Data = (Convert.ToInt32(_ids[1]));
                            phasegroups[i].Double = (Convert.ToDouble(_ids[2]));
                            ids.Add(phasegroups[i].Data);
                            ids.Add((int)phasegroups[i].Double);
                        }

                        var allItems = db.T_CommodityAnalysis
                          .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                          .Select(x => new
                          {
                              Id = x.Id,
                              Name = x.Name,
                          }).ToList();

                        foreach (var item in pageListReturn)
                        {
                            item.timePrepares.AddRange((from x in db.T_CA_Phase_TimePrepare
                                                        where !x.IsDeleted && x.Commo_Ana_PhaseId == item.Id
                                                        select new Commo_Ana_Phase_TimePrepareModel()
                                                        {
                                                            Id = x.Id,
                                                            TimePrepareId = x.TimePrepareId,
                                                            Name = x.T_TimePrepare.Name,
                                                            Code = x.T_TimePrepare.Code,
                                                            TimeTypePrepareName = x.T_TimePrepare.T_TimeTypePrepare.Name,
                                                            TMUNumber = x.T_TimePrepare.TMUNumber,
                                                            Description = x.T_TimePrepare.Description,
                                                        }));
                            item.TimePrepareTMU = item.timePrepares.Sum(x => x.TMUNumber);
                            item.actions.AddRange((from x in db.T_CA_Phase_Mani
                                                   where !x.IsDeleted && x.CA_PhaseId == item.Id
                                                   orderby x.OrderIndex
                                                   select new Commo_Ana_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       CA_PhaseId = x.CA_PhaseId,
                                                       ManipulationId = x.ManipulationId,
                                                       ManipulationName = x.ManipulationName,
                                                       ManipulationCode = x.ManipulationCode,
                                                       TMUEquipment = x.TMUEquipment,
                                                       TMUManipulation = x.TMUManipulation,
                                                       Loop = x.Loop,
                                                       TotalTMU = x.TotalTMU,
                                                       OrderIndex = x.OrderIndex
                                                   }));
                            item.ManiVerTMU = item.actions.Sum(x => ((x.TMUEquipment ?? 0 * x.Loop) + (x.TMUManipulation ?? 0 * x.Loop)));

                            var ps = phasegroups.FirstOrDefault(x => x.Value == item.ParentId);
                            if (ps != null)
                            {
                                var _pro = allItems.FirstOrDefault(x => x.Id == ps.Data);
                                if (_pro != null)
                                {
                                    item.ProductName = _pro.Name;
                                }

                                _pro = allItems.FirstOrDefault(x => x.Id == ps.Double);
                                if (_pro != null)
                                {
                                    item.WorkshopName = _pro.Name;
                                }
                            }
                        }
                    }
                    return pageListReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Commo_Ana_Phase_ManiModel> GetPhaseVersionManipulationByManipulationVersionId(int Id)
        {
            try
            {
                //var phaseVerMani = db.T_CA_Phase_ManiVer_De.Where(x => !x.IsDeleted && x.Commo_Ana_Phase_ManiVerId == Id).Select(x => new Commo_Ana_Phase_ManiVer_DetailModel()
                //{
                //    Id = x.Id,
                //    ManipulationId = x.ManipulationId,
                //    ManipulationName = x.T_ManipulationLibrary.Name,
                //    OrderIndex = x.OrderIndex,
                //    Commo_Ana_Phase_ManiVerId = x.Commo_Ana_Phase_ManiVerId,
                //    TMUEquipment = x.TMUEquipment,
                //    TMUManipulation = x.TMUManipulation,
                //    Description = x.Description
                //});
                //if (phaseVerMani != null && phaseVerMani.Count() > 0)
                //{
                //    return phaseVerMani.ToList();
                //}
                return new List<Commo_Ana_Phase_ManiModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Delete(int Id, int actionUserId, bool isOwner, bool IsMDGVersion)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                    if (phase == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã tồn tại hoặc đã bị xóa trước đó. Vui lòng kiểm tra lại dữ liệu!." });
                    }
                    else
                    {
                        if (!checkPermis(phase, actionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete ", Message = "Bạn không phải là người tạo công đoạn này nên bạn không được xóa công đoạn này." });
                            return result;
                        }
                        //TODO MDG
                        if (phase.Status == eStatus.Submit && !IsMDGVersion)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "update", Message = "Công đoạn này đang chờ duyệt nên bạn không được xóa công đoạn này." });
                            return result;
                        }
                        //TODO MDG
                        if (phase.Status == eStatus.Approved && !IsMDGVersion)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "update", Message = "Công đoạn này đã được duyệt nên bạn không được xóa công đoạn này." });
                            return result;
                        }
                        phase.IsDeleted = true;
                        phase.DeletedUser = actionUserId;
                        phase.DeletedDate = DateTime.Now;

                        int paId = (phase.Node.Substring(0, phase.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                        var qt = db.T_TechProcessVersion.FirstOrDefault(x => !x.IsDeleted && x.ParentId == paId);
                        if (qt != null)
                        {
                            var deleteObj = db.T_TechProcessVersionDetail.FirstOrDefault(x => !x.IsDeleted && x.TechProcessVersionId == qt.Id && x.CA_PhaseId == phase.Id);
                            if (deleteObj != null)
                            {
                                deleteObj.IsDeleted = true;
                                deleteObj.DeletedUser = actionUserId;
                                deleteObj.DeletedDate = DateTime.Now;
                            }

                            var allDetails = db.T_TechProcessVersionDetail.Where(x => !x.IsDeleted && x.TechProcessVersionId == qt.Id);
                            if (allDetails != null && allDetails.Count() > 0)
                            {
                                qt.TimeCompletePerCommo = Math.Round(allDetails.Sum(x => x.TimeByPercent), 3);
                                qt.PacedProduction = qt.NumberOfWorkers != 0 ? Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3) : 0;
                                qt.ProOfGroupPerHour = qt.NumberOfWorkers != 0 ? Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3) : 0;
                                qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                qt.ProOfPersonPerDay = qt.NumberOfWorkers != 0 ? Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3) : 0;
                                foreach (var item in allDetails)
                                    item.Worker = qt.PacedProduction != 0 ? Math.Round(((item.TimeByPercent / qt.PacedProduction)), 3) : 0;
                            }
                            else
                            {
                                qt.IsDeleted = true;
                                qt.DeletedUser = actionUserId;
                                qt.DeletedDate = DateTime.Now;
                            }
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase RemovePhaseVideo(int Id, int actionUserId, bool isOwner, ref string videoPath)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var phase = db.T_CA_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                    if (phase == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã tồn tại hoặc đã bị xóa trước đó. Vui lòng kiểm tra lại dữ liệu!." });
                    }
                    else
                    {
                        if (!checkPermis(phase, actionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete ", Message = "Bạn không phải là người tạo công đoạn này nên bạn không xóa được xóa công đoạn này." });
                        }
                        else
                        {
                            videoPath = phase.Video;
                            phase.Video = "";
                            phase.UpdatedUser = actionUserId;
                            phase.UpdatedDate = DateTime.Now;
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Copy(int Id, int actionUserId, bool isMDGVersion)
        {
            var rs = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    var phase = (from x in db.T_CA_Phase
                                 where !x.IsDeleted && x.Id == Id
                                 select x).FirstOrDefault();
                    if (phase != null)
                    {
                        var phaseAcc = (from x in db.T_CA_Phase_Mani
                                        where !x.IsDeleted && x.CA_PhaseId == Id
                                        select x);
                        var times = (from x in db.T_CA_Phase_TimePrepare
                                     where !x.IsDeleted && x.Commo_Ana_PhaseId == Id
                                     select x);

                        var lastPhase = (from x in db.T_CA_Phase
                                         where !x.IsDeleted && x.ParentId == phase.ParentId
                                         orderby x.Index descending
                                         select x).FirstOrDefault();

                        T_CA_Phase phaseC;
                        T_CA_Phase_Mani maniC;
                        T_CA_Phase_TimePrepare timeC;
                        var now = DateTime.Now;
                        phaseC = new T_CA_Phase();
                        phaseC.Index = lastPhase.Index + 1;
                        phaseC.Name = phase.Name + "-Copy";
                        phaseC.Code = (phase.Code.LastIndexOf('-') == -1 ? phaseC.Index.ToString() : (phase.Code.Substring(0, phase.Code.LastIndexOf('-')) + "-" + phaseC.Index));
                        phaseC.PhaseGroupId = phase.PhaseGroupId;
                        phaseC.Description = phase.Description;
                        phaseC.EquipmentId = phase.EquipmentId;
                        phaseC.PhaseGroupId = phase.PhaseGroupId;
                        phaseC.WorkerLevelId = phase.WorkerLevelId;
                        phaseC.ParentId = phase.ParentId;
                        phaseC.TotalTMU = phase.TotalTMU;
                        phaseC.ApplyPressuresId = phase.ApplyPressuresId;
                        phaseC.PercentWasteEquipment = phase.PercentWasteEquipment;
                        phaseC.PercentWasteManipulation = phase.PercentWasteManipulation;
                        phaseC.PercentWasteMaterial = phase.PercentWasteMaterial;
                        phaseC.PercentWasteSpecial = phase.PercentWasteSpecial;
                        phaseC.Node = phase.Node;
                        phaseC.Video = phase.Video;
                        phaseC.CreatedUser = actionUserId;
                        phaseC.CreatedDate = now;
                        phaseC.Status = eStatus.Editor;

                        //TODO MDG
                        if (isMDGVersion)
                        {
                            phaseC.IsApprove = true;
                            phaseC.Approver = actionUserId;
                            phaseC.ApprovedDate = now;
                            phaseC.Status = eStatus.Approved;
                        }

                        //  db.T_CA_Phase.Add(phaseC);
                        //  db.SaveChanges();

                        if (times != null && times.Count() > 0)
                        {
                            phaseC.T_CA_Phase_TimePrepare = new Collection<T_CA_Phase_TimePrepare>();
                            foreach (var item in times)
                            {
                                timeC = new T_CA_Phase_TimePrepare();
                                timeC.TimePrepareId = item.TimePrepareId;
                                timeC.CreatedUser = actionUserId;
                                timeC.CreatedDate = now;
                                timeC.T_CA_Phase = phaseC;
                                phaseC.T_CA_Phase_TimePrepare.Add(timeC);
                            }
                        }

                        if (phaseAcc != null && phaseAcc.Count() > 0)
                        {
                            phaseC.T_CA_Phase_Mani = new Collection<T_CA_Phase_Mani>();
                            foreach (var item in phaseAcc)
                            {
                                maniC = new T_CA_Phase_Mani();
                                maniC.OrderIndex = item.OrderIndex;
                                maniC.ManipulationId = item.ManipulationId;
                                maniC.ManipulationCode = item.ManipulationCode;
                                maniC.ManipulationName = item.ManipulationName;
                                maniC.TMUEquipment = item.TMUEquipment;
                                maniC.TMUManipulation = item.TMUManipulation;
                                maniC.Loop = item.Loop;
                                maniC.TotalTMU = item.TotalTMU;
                                maniC.CreatedUser = actionUserId;
                                maniC.CreatedDate = now;
                                maniC.T_CA_Phase = phaseC;
                                phaseC.T_CA_Phase_Mani.Add(maniC);
                            }
                        }
                        db.T_CA_Phase.Add(phaseC);
                        db.SaveChanges();

                        //TODO MDG
                        if (isMDGVersion)
                        {
                            //ktra xem co qtcn chua 
                            int paId = (phase.Node.Substring(0, phase.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                            var qt = (from x in db.T_TechProcessVersion
                                      where !x.IsDeleted && x.ParentId == paId
                                      select x).FirstOrDefault();
                            if (qt != null)
                            {
                                var allDetails = (from x in db.T_TechProcessVersionDetail
                                                  where !x.IsDeleted && x.TechProcessVersionId == qt.Id
                                                  select x);

                                var verDetail = new T_TechProcessVersionDetail();
                                verDetail.TechProcessVersionId = qt.Id;
                                verDetail.CA_PhaseId = phase.Id;
                                verDetail.StandardTMU = phase.TotalTMU;
                                verDetail.Percent = allDetails.First().Percent;
                                verDetail.TimeByPercent = Math.Round((phase.TotalTMU * 100) / verDetail.Percent, 3);
                                verDetail.CreatedDate = phase.CreatedDate;
                                verDetail.CreatedUser = phase.CreatedUser;

                                qt.TimeCompletePerCommo = Math.Round((qt.TimeCompletePerCommo + verDetail.TimeByPercent), 3);
                                qt.PacedProduction = (qt.TimeCompletePerCommo > 0 && qt.NumberOfWorkers > 0 ? Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3) : 0);
                                qt.ProOfGroupPerHour = Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3);
                                qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                qt.ProOfPersonPerDay = (qt.ProOfGroupPerDay > 0 && qt.NumberOfWorkers > 0 ? Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3) : 0);
                                foreach (var item in allDetails)
                                    item.Worker = (item.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((item.TimeByPercent / qt.PacedProduction)), 3) : 0);

                                verDetail.Worker = (verDetail.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((verDetail.TimeByPercent / qt.PacedProduction)), 3) : 0);
                                db.T_TechProcessVersionDetail.Add(verDetail);
                                db.Entry<T_TechProcessVersion>(qt).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Errors.Add(new Error() { MemberName = "", Message = "Lỗi SQL" });
            }
            return rs;
        }

        public ExportPhaseActionsModel Export_CommoAnaPhaseManiVer(int Id)
        {
            ExportPhaseActionsModel exportObj = null;
            try
            {
                using (db = new IEDEntities())
                {
                    var phase = (from x in db.T_CA_Phase
                                 where !x.IsDeleted && x.Id == Id
                                 select x).FirstOrDefault();
                    if (phase != null)
                    {
                        exportObj = new ExportPhaseActionsModel();
                        exportObj.Details.AddRange((from x in db.T_CA_Phase_Mani
                                                    where !x.IsDeleted && x.CA_PhaseId == Id
                                                    orderby x.OrderIndex
                                                    select new Commo_Ana_Phase_ManiModel()
                                                    {
                                                        Id = x.Id,
                                                        CA_PhaseId = x.CA_PhaseId,
                                                        ManipulationId = x.ManipulationId,
                                                        ManipulationName = x.ManipulationName,
                                                        ManipulationCode = x.ManipulationCode,
                                                        TMUEquipment = x.TMUEquipment,
                                                        TMUManipulation = x.TMUManipulation,
                                                        Loop = x.Loop,
                                                        TotalTMU = x.TotalTMU,
                                                        OrderIndex = x.OrderIndex,
                                                    }));
                        exportObj.TotalTMU = phase.TotalTMU;
                        exportObj.PhaseName = phase.Name;

                        int cAnaId = Convert.ToInt32(phase.Node.Split(',')[1]);
                        var cAnaObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cAnaId);
                        if (cAnaObj != null)
                        {
                            var proObj = db.T_Product.FirstOrDefault(x => x.Id == cAnaObj.ObjectId);
                            if (proObj != null)
                            {
                                exportObj.ProductName = proObj.Name;
                                exportObj.CustomerName = proObj.Code;
                            }
                            else
                            {
                                exportObj.ProductName = "";
                                exportObj.CustomerName = "";
                            }
                        }

                        //get workshop info
                        cAnaId = Convert.ToInt32(phase.Node.Split(',')[2]);
                        cAnaObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cAnaId);
                        if (cAnaObj != null)
                        {
                            var ws = db.T_WorkShop.FirstOrDefault(x => x.Id == cAnaObj.ObjectId);
                            if (ws != null)
                            {
                                exportObj.WorkshopId = ws.Id;
                                exportObj.WorkshopName = ws.Name;
                            }
                            else
                            {
                                exportObj.WorkshopId = 0;
                                exportObj.WorkshopName = "";
                            }
                        }


                        //var timePrepares = db.T_CA_Phase_TimePrepare.Where(x => !x.IsDeleted && x.Commo_Ana_PhaseId ==  Id).Select(x => new Commo_Ana_Phase_TimePrepareModel()
                        //{
                        //    Id = x.Id,
                        //    Commo_Ana_PhaseId = x.Commo_Ana_PhaseId,
                        //    TMUNumber = x.T_TimePrepare.TMUNumber
                        //}).ToList();
                        //if (timePrepares.Count > 0)
                        //{
                        //    double tmu = 0, time = 0;
                        //    string strTmu = bllIEDConfig.GetValueByCode(eIEDConfigName.TMU);
                        //    if (!string.IsNullOrEmpty(strTmu))
                        //        double.TryParse(strTmu, out tmu);
                        //    time = timePrepares.Sum(x => x.TMUNumber);
                        //    exportObj.TimePrepare = time > 0 ? time / tmu : 0;
                        //}
                        //else
                        exportObj.TimePrepare = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exportObj;
        }

        public int GetLastIndex(int ParentId)
        {
            using (db = new IEDEntities())
            {
                var obj = (from x in db.T_CA_Phase where !x.IsDeleted && x.ParentId == ParentId orderby x.Index descending select x.Index).FirstOrDefault();
                if (obj != null)
                    return obj;
                return 0;
            }
        }

        public List<ModelSelectItem> GetAllPhasesForSuggest()
        {
            var list = new List<ModelSelectItem>();
            using (var db = new IEDEntities())
            {
                var phases = (from x in db.T_CA_Phase
                              where !x.IsDeleted && !x.T_CommodityAnalysis.IsDeleted &&
                              !x.T_PhaseGroup.IsDeleted &&
                              x.IsLibrary && x.IsApprove
                              select new // ModelSelectItem()
                              {
                                  Value = x.Id,
                                  Name = x.Name,
                                  Code = x.Code,
                                  Double = x.TotalTMU,
                                  Node = x.Node
                              }).ToList();
                if (phases.Count > 0)
                {
                    var proanas = (from x in db.T_CommodityAnalysis
                                   where
                                      !x.IsDeleted &&
                                      (x.ObjectType == (int)eObjectType.isCommodity || x.ObjectType == (int)eObjectType.isPhaseGroup)
                                   select new
                                   {
                                       Id = x.Id,
                                       objId = x.ObjectId,
                                       objType = x.ObjectType
                                   }).ToList();

                    var products = (from x in db.T_Product
                                    where !x.IsDeleted
                                    select new
                                    {
                                        Id = x.Id,
                                        name = x.Name
                                    }).ToList();
                    var phaseGroups = (from x in db.T_PhaseGroup
                                       where !x.IsDeleted
                                       select new
                                       {
                                           Id = x.Id,
                                           name = x.Name
                                       }).ToList();

                    foreach (var item in phases)
                    {
                        var arr = item.Node.Replace("0,0,", "0,").Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
                        var proObj = proanas.FirstOrDefault(x => x.Id == arr[1]);
                        if (proObj != null)
                        {
                            //check product
                            var pro = products.FirstOrDefault(x => x.Id == proObj.objId);
                            if (pro != null)
                            {
                                proObj = proanas.FirstOrDefault(x => x.Id == arr[4]);
                                if (proObj != null)
                                {
                                    //check phase group
                                    var pg = phaseGroups.FirstOrDefault(x => x.Id == proObj.objId);
                                    if (pg != null)
                                    {
                                        list.Add(new ModelSelectItem()
                                        {
                                            Value = item.Value,
                                            Name = item.Name,
                                            Code = item.Code,
                                            Double = item.Double,
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        public Commo_Ana_PhaseModel GetPhase(int phaseId, bool isMDGVersion)
        {
            using (db = new IEDEntities())
            {
                try
                {
                    var phaseObj = (from x in db.T_CA_Phase
                                    where !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Id == phaseId
                                    select new Commo_Ana_PhaseModel()
                                    {
                                        Id = x.Id,
                                        Index = x.Index,
                                        Name = x.Name,
                                        //Code = proObj.Code + "" + wsObj.Code + "" + x.Code,
                                        Code = x.Code,
                                        FullCode = x.Code,
                                        TotalTMU = x.TotalTMU,
                                        WorkerLevelName = x.SWorkerLevel.Name,
                                        EquipName = x.T_Equipment.Name,
                                        Status = x.Status,
                                        //WorkshopId = wsObj.ObjectId,
                                        //WorkshopName = wsObj.Name,
                                       //ProductName = proObj.Name,

                                        Description = x.Description,
                                        EquipmentId = x.EquipmentId,

                                        EquipDes = x.T_Equipment.Description,
                                        EquipTypeDefaultId = x.EquipmentId != null ? x.T_Equipment.T_EquipmentType.EquipTypeDefaultId ?? 0 : 0,
                                        WorkerLevelId = x.WorkerLevelId,

                                        ParentId = x.ParentId,
                                        PhaseGroupId = x.PhaseGroupId,
                                        ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                                        PercentWasteEquipment = x.PercentWasteEquipment,
                                        PercentWasteManipulation = x.PercentWasteManipulation,
                                        PercentWasteMaterial = x.PercentWasteMaterial,
                                        PercentWasteSpecial = x.PercentWasteSpecial,
                                        Video = x.Video,
                                        IsLibrary = x.IsLibrary,

                                        IsApprove = x.IsApprove,
                                        Approver = x.Approver,
                                        ApproverName = x.IsApprove ? x.SUser.UserName : "",
                                        ApproveDate = x.ApprovedDate,
                                        Node = x.Node
                                    }).FirstOrDefault();
                    if (phaseObj != null)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);

                        int cawk = Convert.ToInt32(phaseObj.Node.Split(',')[1]);
                        var proObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);
                        phaseObj.ProductName = proObj.Name;

                        cawk = Convert.ToInt32(phaseObj.Node.Split(',')[2]);
                        var wsObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);
                        phaseObj.WorkshopName = wsObj.Name;

                        cawk = Convert.ToInt32(phaseObj.Node.Split(',')[4]);
                        var phaseGroupObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cawk);
                        phaseObj.PhaseGroupName = phaseGroupObj.Name;

                        string[] wsDes = (!string.IsNullOrEmpty(wsObj.Description) ? wsObj.Description : "").Split('|').ToArray();
                        phaseObj.timePrepares.AddRange((from x in db.T_CA_Phase_TimePrepare
                                                        where !x.IsDeleted && x.Commo_Ana_PhaseId == phaseObj.Id
                                                        select new Commo_Ana_Phase_TimePrepareModel()
                                                        {
                                                            Id = x.Id,
                                                            TimePrepareId = x.TimePrepareId,
                                                            Name = x.T_TimePrepare.Name,
                                                            Code = x.T_TimePrepare.Code,
                                                            TimeTypePrepareName = x.T_TimePrepare.T_TimeTypePrepare.Name,
                                                            TMUNumber = x.T_TimePrepare.TMUNumber,
                                                            Description = x.T_TimePrepare.Description,
                                                        }));
                        phaseObj.TimePrepareTMU = phaseObj.timePrepares.Sum(x => x.TMUNumber);
                        phaseObj.actions.AddRange((from x in db.T_CA_Phase_Mani
                                                   where !x.IsDeleted && x.CA_PhaseId == phaseObj.Id
                                                   orderby x.OrderIndex
                                                   select new Commo_Ana_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       CA_PhaseId = x.CA_PhaseId,
                                                       ManipulationId = x.ManipulationId,
                                                       ManipulationName = x.ManipulationName,
                                                       ManipulationCode = x.ManipulationCode,
                                                       TMUEquipment = x.TMUEquipment,
                                                       TMUManipulation = x.TMUManipulation,
                                                       Loop = x.Loop,
                                                       TotalTMU = x.TotalTMU,
                                                       OrderIndex = x.OrderIndex
                                                   }));
                        phaseObj.ManiVerTMU = phaseObj.actions.Sum(x => ((x.TMUEquipment ?? 0 * x.Loop) + (x.TMUManipulation ?? 0 * x.Loop)));
                        if (isMDGVersion)
                        {
                            phaseObj.IsApprove = false;
                            phaseObj.Status = eStatus.Editor;
                        }
                        else
                            phaseObj.FullCode = GenerateCode(phaseObj.Code, ((wsDes.Length > 1 && !string.IsNullOrEmpty(wsDes[1]) ? wsDes[1] : "") + "" + proObj.Code + "" + phaseGroupObj.Code));
                    }
                    return phaseObj;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ResponseBase TinhLaiCode(List<Commo_Ana_Phase_ManiModel> actions, int equipmentId, int equiptypedefaultId, int applyPressure)
        {
            ResponseBase result = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        if (actions[i].ManipulationCode.Substring(0, 1) == "C" || actions[i].ManipulationCode.Substring(0, 2) == "SE")
                        {
                            if (actions[i].ManipulationCode.Length > 4)
                            {
                                string stopPrecisionCode = actions[i].ManipulationCode.Substring(actions[i].ManipulationCode.Length - 1, 1);
                                var stopPrecision = db.T_StopPrecisionLibrary.FirstOrDefault(c => c.Code.Trim().ToUpper().Equals(stopPrecisionCode.Trim().ToUpper()) && !c.IsDeleted);
                                if (stopPrecision == null)
                                    result.Errors.Add(new Error() { Message = "Không tìm thấy thông tin độ dừng chính xác.", MemberName = "GetManipulationEquipmentInfoByCode" });
                                else
                                {
                                    float distance = 0;
                                    if (actions[i].ManipulationCode.Substring(0, 2).Equals("SE"))
                                    {
                                        float.TryParse(actions[i].ManipulationCode.Substring(2, actions[i].ManipulationCode.Length - 3), out distance);
                                        if (distance == 0)
                                            result.Errors.Add(new Error() { Message = "Thông tin khoảng cách may không chính xác, hoặc bằng 0.", MemberName = "GetManipulationEquipmentInfoByCode" });
                                        else
                                        {
                                            actions[i].TMUEquipment = BLLEquipment.Instance.CalculationMachineTMU(equipmentId, equiptypedefaultId, distance, stopPrecision.TMUNumber, 0, 0);
                                            actions[i].TotalTMU = (actions[i].TMUEquipment * actions[i].Loop) ?? 0;
                                        }
                                    }
                                    else if (actions[i].ManipulationCode.Substring(0, 1).Equals("C"))
                                    {
                                        float.TryParse(actions[i].ManipulationCode.Substring(1, actions[i].ManipulationCode.Length - 3), out distance);
                                        string natureCutCode = actions[i].ManipulationCode.Substring(actions[i].ManipulationCode.Length - 2, 1);
                                        var natureCut = db.T_NatureCutsLibrary.FirstOrDefault(c => c.Code.Trim().ToUpper().Equals(natureCutCode.Trim().ToUpper()) && !c.IsDeleted);
                                        if (applyPressure == 0)
                                            result.Errors.Add(new Error() { Message = "Không tìm thấy thông tin số lớp cắt.", MemberName = "GetManipulationEquipmentInfoByCode" });
                                        else if (natureCut == null)
                                            result.Errors.Add(new Error() { Message = "Không tìm thấy thông tin tính chất căt.", MemberName = "GetManipulationEquipmentInfoByCode" });
                                        else if (distance == 0)
                                            result.Errors.Add(new Error() { Message = "Thông tin khoảng cách may không chính xác, hoặc bằng 0.", MemberName = "GetManipulationEquipmentInfoByCode" });
                                        else
                                        {

                                            actions[i].TMUEquipment = BLLEquipment.Instance.CalculationMachineTMU(equipmentId, equiptypedefaultId, distance, stopPrecision.TMUNumber, applyPressure, natureCut.Factor);
                                            actions[i].TotalTMU = (actions[i].TMUEquipment * actions[i].Loop) ?? 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    result.Data = actions;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ExportPhaseGroupModel Export_CommoAnaPhaseGroup(int Id)
        {
            ExportPhaseGroupModel exportObj = null;
            try
            {
                using (db = new IEDEntities())
                {
                    exportObj = (from x in db.T_CommodityAnalysis
                                 where !x.IsDeleted && x.Id == Id
                                 select new ExportPhaseGroupModel() { ObjectId = x.ObjectId, Name = x.Name, Node = x.Node, }).FirstOrDefault();
                    if (exportObj == null)
                        return exportObj;

                    int cAnaId = Convert.ToInt32(exportObj.Node.Split(',')[1]);
                    var cAnaObj = db.T_CommodityAnalysis.FirstOrDefault(x => x.Id == cAnaId);
                    if (cAnaObj != null)
                    {
                        var proObj = db.T_Product.FirstOrDefault(x => x.Id == cAnaObj.ObjectId);
                        if (proObj != null)
                        {
                            exportObj.ProductName = proObj.Name;
                            exportObj.CustomerName = proObj.Code;
                        }
                        else
                        {
                            exportObj.ProductName = "";
                            exportObj.CustomerName = "";
                        }
                    }


                    exportObj.Phases.AddRange((from x in db.T_CA_Phase
                                               where !x.IsDeleted && x.ParentId == Id
                                               orderby x.Index
                                               select new ExportPhaseActionsModel()
                                               {
                                                   Id = x.Id,
                                                   PhaseName = x.Name,
                                                   TotalTMU = x.TotalTMU,
                                                   TimePrepare = 0, //x.T_CA_Phase_TimePrepare.Where(c => !c.IsDeleted && !c.T_TimePrepare.IsDeleted).Sum(c => c.T_TimePrepare.TMUNumber)
                                                   EquiptId = x.EquipmentId ?? 0,
                                                   EquiptName = x.EquipmentId.HasValue ? x.T_Equipment.Name : ""
                                               }).ToList());
                    if (exportObj.Phases.Count > 0)
                    {
                        var _phaseIds = exportObj.Phases.Select(x => x.Id).ToList();
                        var _actions = ((from x in db.T_CA_Phase_Mani
                                         where !x.IsDeleted && _phaseIds.Contains(x.CA_PhaseId)
                                         orderby x.OrderIndex
                                         select new Commo_Ana_Phase_ManiModel()
                                         {
                                             Id = x.Id,
                                             CA_PhaseId = x.CA_PhaseId,
                                             ManipulationId = x.ManipulationId,
                                             ManipulationName = x.ManipulationName,
                                             ManipulationCode = x.ManipulationCode,
                                             TMUEquipment = x.TMUEquipment,
                                             TMUManipulation = x.TMUManipulation,
                                             Loop = x.Loop,
                                             TotalTMU = x.TotalTMU,
                                             OrderIndex = x.OrderIndex,
                                         })).ToList();

                        if (_actions.Count > 0)
                        {
                            foreach (var item in exportObj.Phases)
                                item.Details.AddRange(_actions.Where(x => x.CA_PhaseId == item.Id).OrderBy(x => x.OrderIndex).ToList());
                        }


                        var listEquipmentId = exportObj.Phases.Where(c => c.EquiptId > 0).Select(c => c.EquiptId).Distinct().ToList();
                        if (listEquipmentId.Count > 0)
                        {
                            exportObj.Equipments = new List<ModelSelectItem>();
                            foreach (var equipmentId in listEquipmentId)
                            {
                                var equipments = exportObj.Phases.Where(c => c.EquiptId == equipmentId).Select(c => new ModelSelectItem()
                                {
                                    Value = c.EquiptId,
                                    Name = c.EquiptName
                                }).ToList();
                                if (equipments.Count > 0)
                                {
                                    var equipmentFirst = equipments[0];
                                    //equipmentFirst.QuantityUse = equipments.Count;
                                    exportObj.Equipments.Add(equipmentFirst);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exportObj;
        }

        public void InsertWorkshop()
        {
            using (db = new IEDEntities())
            {
                var allPhases = db.T_CA_Phase
                   .Where(x => !x.IsDeleted)
                  .GroupBy(x => x.ParentId)
                   .ToList();

                var allWorkshops = db.T_CommodityAnalysis
                    .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isWorkShop)
                    .Select(x => new
                    {
                        Id = x.ObjectId,
                        CreatedDate = x.CreatedDate,
                        node = (x.Node + x.Id + ","),
                        name = x.Name
                    })
                    .ToList();

                foreach (var item in allPhases)
                {
                    var node = item.FirstOrDefault().Node;
                    var wk = allWorkshops.FirstOrDefault(x => node.Contains(x.node));
                    if (wk != null)
                    {
                        foreach (var sub in item)
                        {
                            sub.WorkShopId = wk.Id;
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public ResponseBase MoveToLibrary(int _phaseId, int _phasegroupId, int actionUserId)
        {
            var result = new ResponseBase();
            using (db = new IEDEntities())
            {
                try
                {
                    var phaseObj = db.T_CA_Phase.
                                    FirstOrDefault(x => !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Id == _phaseId);
                    if (phaseObj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã tồn tại hoặc đã bị xóa trước đó. Vui lòng kiểm tra lại dữ liệu!." });
                        return result;
                    }

                    if (!phaseObj.IsApprove || phaseObj.Status != eStatus.Approved)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã chưa được duyệt. Bạn phải duyệt trước khi đưa vào thư viện!." });
                        return result;
                    }

                    var newObj = new T_PhaseGroup_Phase();
                    Parse.CopyObject(phaseObj, ref newObj);
                    newObj.T_PhaseGroup = null;
                    newObj.PhaseGroupId = _phasegroupId;
                    newObj.IsLibrary = phaseObj.IsLibrary;
                    newObj.CreatedUser = actionUserId;
                    newObj.CreatedDate = DateTime.Now;
                    newObj.Node = _phasegroupId + ",";
                    newObj.IsApprove = phaseObj.IsApprove;
                    newObj.Status = phaseObj.Status;
                    newObj.Approver = phaseObj.Approver;
                    newObj.ApprovedDate = phaseObj.ApprovedDate;
                    newObj.ProductIds = "0";

                    var time = db.T_CA_Phase_TimePrepare.FirstOrDefault(x => !x.IsDeleted && x.Commo_Ana_PhaseId == phaseObj.Id);
                    if (time != null)
                        newObj.TimePrepareId = time.TimePrepareId;

                    var actions = db.T_CA_Phase_Mani
                        .Where(x => !x.IsDeleted && x.CA_PhaseId == phaseObj.Id)
                                                  .OrderBy(x => x.OrderIndex).ToList();

                    if (actions.Count > 0)
                    {
                        newObj.T_PhaseGroup_Phase_Mani = new List<T_PhaseGroup_Phase_Mani>();
                        T_PhaseGroup_Phase_Mani actionObj = null;
                        foreach (var item in actions)
                        {
                            actionObj = new T_PhaseGroup_Phase_Mani();
                            Parse.CopyObject(item, ref actionObj);
                            actionObj.CreatedUser = actionUserId;
                            actionObj.CreatedDate = newObj.CreatedDate;
                            actionObj.T_PhaseGroup_Phase = newObj;
                            newObj.T_PhaseGroup_Phase_Mani.Add(actionObj);
                        }
                    }
                    db.T_PhaseGroup_Phase.Add(newObj);
                    db.SaveChanges();
                    result.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public ResponseBase ImportFromLibrary(int commoAnaId, List<int> phaseIds, int actionUserId)
        {

            var rs = new ResponseBase();
            using (db = new IEDEntities())
            {
                try
                {
                    var noName = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == commoAnaId);
                    if (noName != null)
                    {
                        //lay cong doan mau
                        var phases = db.T_PhaseGroup_Phase.Where(x => !x.IsDeleted && x.PhaseGroupId == noName.ObjectId && phaseIds.Contains(x.Id)).ToList();

                        if (phases.Count > 0)
                        {
                            T_CA_Phase _phaseObj;
                            T_CA_Phase_Mani _phaseManiObj;
                            T_CA_Phase_TimePrepare _phaseTimeObj;
                            var now = DateTime.Now;

                            int _id = Int32.Parse(noName.Node.Split(',')[2]);
                            var parentObj = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == _id);
                            var lastChild = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == _id).OrderByDescending(x => x.Index).FirstOrDefault();
                            int _index = lastChild != null ? lastChild.Index : 0;

                            foreach (var item in phases)
                            {
                                _index++;
                                _phaseObj = new T_CA_Phase();
                                Parse.CopyObject(item, ref _phaseObj);
                                _phaseObj.Id = 0;
                                _phaseObj.Index = _index;
                                _phaseObj.Code = _index.ToString();
                                _phaseObj.Node = noName.Node + noName.Id + ",";
                                _phaseObj.ParentId = noName.Id;
                                _phaseObj.IsLibrary = false;
                                _phaseObj.T_CA_Phase_Mani = new List<T_CA_Phase_Mani>();
                                _phaseObj.T_CA_Phase_TimePrepare = new List<T_CA_Phase_TimePrepare>();
                                _phaseObj.WorkShopId = parentObj.ObjectId;
                                _phaseObj.Status = eStatus.Editor;
                                _phaseObj.IsApprove = false;
                                _phaseObj.CreatedDate = now;
                                _phaseObj.CreatedUser = actionUserId;

                                if (item.T_PhaseGroup_Phase_Mani != null && item.T_PhaseGroup_Phase_Mani.Count > 0)
                                {
                                    foreach (var itemMani in item.T_PhaseGroup_Phase_Mani)
                                    {
                                        _phaseManiObj = new T_CA_Phase_Mani();
                                        Parse.CopyObject(itemMani, ref _phaseManiObj);
                                        _phaseManiObj.Id = 0;
                                        _phaseManiObj.CreatedDate = now;
                                        _phaseManiObj.CreatedUser = actionUserId;
                                        _phaseObj.T_CA_Phase_Mani.Add(_phaseManiObj);
                                        _phaseManiObj.T_CA_Phase = _phaseObj;
                                    }
                                }

                                _phaseTimeObj = new T_CA_Phase_TimePrepare();
                                _phaseTimeObj.TimePrepareId = item.TimePrepareId;
                                _phaseTimeObj.CreatedDate = now;
                                _phaseTimeObj.CreatedUser = actionUserId;
                                _phaseTimeObj.T_CA_Phase = _phaseObj;
                                _phaseObj.T_CA_Phase_TimePrepare.Add(_phaseTimeObj);

                                db.T_CA_Phase.Add(_phaseObj);
                                db.SaveChanges();

                                int paId = (_phaseObj.Node.Substring(0, _phaseObj.Node.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList()[2] + 1);
                                var qt = (from x in db.T_TechProcessVersion
                                          where !x.IsDeleted && x.ParentId == paId
                                          select x).FirstOrDefault();
                                if (qt != null)
                                {
                                    var allDetails = (from x in db.T_TechProcessVersionDetail
                                                      where !x.IsDeleted && x.TechProcessVersionId == qt.Id
                                                      select x);

                                    var verDetail = new T_TechProcessVersionDetail();
                                    verDetail.TechProcessVersionId = qt.Id;
                                    verDetail.CA_PhaseId = _phaseObj.Id;
                                    verDetail.StandardTMU = _phaseObj.TotalTMU;
                                    verDetail.Percent = allDetails.First().Percent;
                                    verDetail.TimeByPercent = Math.Round((_phaseObj.TotalTMU * 100) / verDetail.Percent, 3);
                                    verDetail.CreatedDate = _phaseObj.CreatedDate;
                                    verDetail.CreatedUser = _phaseObj.CreatedUser;

                                    qt.TimeCompletePerCommo = Math.Round((qt.TimeCompletePerCommo + verDetail.TimeByPercent), 3);
                                    qt.PacedProduction = (qt.TimeCompletePerCommo > 0 && qt.NumberOfWorkers > 0 ? Math.Round(((qt.TimeCompletePerCommo / qt.NumberOfWorkers)), 3) : 0);
                                    qt.ProOfGroupPerHour = Math.Round(((3600 / qt.TimeCompletePerCommo) * qt.NumberOfWorkers), 3);
                                    qt.ProOfGroupPerDay = Math.Round((qt.ProOfGroupPerHour * qt.WorkingTimePerDay), 3);
                                    qt.ProOfPersonPerDay = (qt.ProOfGroupPerDay > 0 && qt.NumberOfWorkers > 0 ? Math.Round((qt.ProOfGroupPerDay / qt.NumberOfWorkers), 3) : 0);
                                    foreach (var subitem in allDetails)
                                        subitem.Worker = (subitem.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((subitem.TimeByPercent / qt.PacedProduction)), 3) : 0);

                                    verDetail.Worker = (verDetail.TimeByPercent > 0 && qt.PacedProduction > 0 ? Math.Round(((verDetail.TimeByPercent / qt.PacedProduction)), 3) : 0);
                                    db.T_TechProcessVersionDetail.Add(verDetail);
                                    db.Entry<T_TechProcessVersion>(qt).State = System.Data.Entity.EntityState.Modified;
                                }
                                db.SaveChanges();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                }
                rs.IsSuccess = true;
                return rs;
            }
        }

        public List<UserPhaseModel> GetReport(int userId, DateTime from, DateTime to)
        {
            List<UserPhaseModel> _report = null;
            using (var db = new IEDEntities())
            {
                _report = db.T_CA_Phase.Where(x => !x.IsDeleted && x.CreatedUser == userId && x.CreatedDate >= from && x.CreatedDate <= to)
                    .OrderBy(x => x.ParentId)
                     .Select(x => new UserPhaseModel()
                     {
                         PhaseName = x.Name,
                         Type = "Bài phân tích",
                         CreatedDate = x.CreatedDate,
                         Node = x.Node,
                         ParentId = x.ParentId,
                         PhaseGroupName = "",
                         ProductName = "",
                         TotalTMU = x.TotalTMU,
                         Status = x.Status
                     }).ToList();
                if (_report.Count > 0)
                {
                    int _id = 0;
                    var commos = db.T_CommodityAnalysis.Where(x => x.ObjectType == (int)eObjectType.isCommodity || x.ObjectType == (int)eObjectType.isPhaseGroup).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
                    for (int i = 0; i < _report.Count; i++)
                    {
                        //lay cumcđ
                        _id = _report[i].ParentId;
                        var found = commos.FirstOrDefault(x => x.Id == _id);
                        if (found != null)
                            _report[i].PhaseGroupName = found.Name;

                        //lay mahang
                        _id = Convert.ToInt32(_report[i].Node.Split(',')[1]);
                        found = commos.FirstOrDefault(x => x.Id == _id);
                        if (found != null)
                            _report[i].ProductName = found.Name;
                    }
                }
            }
            return _report;
        }
    }
}
