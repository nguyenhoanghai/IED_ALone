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
    public class BLLPhaseGroup_Phase
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLPhaseGroup_Phase _Instance;
        public static BLLPhaseGroup_Phase Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhaseGroup_Phase();

                return _Instance;
            }
        }
        private BLLPhaseGroup_Phase() { }
        #endregion

        bool checkPermis(T_PhaseGroup_Phase obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public ResponseBase InsertOrUpdate(PhaseGroup_PhaseModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    T_PhaseGroup_Phase phase = null;
                    T_PhaseGroup_Phase_Mani maniVerDetail;
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.PhaseGroupId, false))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên Công Đoạn này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                    }
                    else
                    {
                        if (model.ApplyPressuresId == 0)
                            model.ApplyPressuresId = null;
                        if (model.Id == 0)
                        {
                            #region create
                            //  var lastPhase = db.T_PhaseGroup_Phase.Where(x => !x.IsDeleted && x.ParentId == phaseModel.ParentId).OrderByDescending(x => x.Index).FirstOrDefault();
                            phase = new T_PhaseGroup_Phase();
                            Parse.CopyObject(model, ref phase);
                            phase.Node = phase.Node + phase.PhaseGroupId + ",";
                            phase.CreatedDate = DateTime.Now;
                            phase.CreatedUser = model.ActionUser;
                            //   phase.Index = lastPhase != null ? (lastPhase.Index + 1) : 1;

                            if (model.actions != null && model.actions.Count > 0)
                            {
                                phase.T_PhaseGroup_Phase_Mani = new Collection<T_PhaseGroup_Phase_Mani>();

                                foreach (var item in model.actions)
                                {
                                    if (item.OrderIndex < model.actions.Count)
                                    {
                                        maniVerDetail = new T_PhaseGroup_Phase_Mani();
                                        Parse.CopyObject(item, ref maniVerDetail);
                                        maniVerDetail.ManipulationCode = maniVerDetail.ManipulationCode.Trim();
                                        maniVerDetail.ManipulationName = maniVerDetail.ManipulationName.Trim();
                                        maniVerDetail.ManipulationId = maniVerDetail.ManipulationId == 0 ? null : maniVerDetail.ManipulationId;
                                        maniVerDetail.CreatedUser = phase.CreatedUser;
                                        maniVerDetail.CreatedDate = phase.CreatedDate;
                                        maniVerDetail.T_PhaseGroup_Phase = phase;
                                        phase.T_PhaseGroup_Phase_Mani.Add(maniVerDetail);
                                    }
                                }
                            }

                            db.T_PhaseGroup_Phase.Add(phase);
                            db.SaveChanges();

                            ReOrderPhase(db, phase.PhaseGroupId, phase.Index, phase.Id);
                            result.IsSuccess = true;
                            result.Data = phase.Id;
                            #endregion
                        }
                        else
                        {
                            #region update
                            phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (phase != null)
                            {
                                if (!checkPermis(phase, model.ActionUser, isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo công đoạn này nên bạn không cập nhật được thông tin cho công đoạn này." });
                                }
                                else
                                {
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
                                    phase.UpdatedDate = DateTime.Now;
                                    phase.UpdatedUser = model.ActionUser;
                                    phase.IsLibrary = model.IsLibrary;
                                    phase.Status = model.Status;

                                    #region actions
                                    var oldDetails = db.T_PhaseGroup_Phase_Mani.Where(x => !x.IsDeleted && x.PhaseGroup_PhaseId == phase.Id);
                                    if (model.actions == null && model.actions.Count == 0 && oldDetails != null && oldDetails.Count() > 0)
                                    {
                                        foreach (var item in oldDetails)
                                        {
                                            item.IsDeleted = true;
                                            item.DeletedUser = phase.UpdatedUser;
                                            item.DeletedDate = phase.UpdatedDate;
                                        }
                                    }
                                    else if (model.actions != null && model.actions.Count > 0 && oldDetails != null && oldDetails.Count() > 0)
                                    {
                                        foreach (var item in oldDetails)
                                        {
                                            var obj = model.actions.FirstOrDefault(x => x.Id == item.Id);
                                            if (obj == null)
                                            {
                                                item.IsDeleted = true;
                                                item.DeletedUser = phase.UpdatedUser;
                                                item.DeletedDate = phase.UpdatedDate;
                                            }
                                            else
                                            {
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
                                                model.actions.Remove(obj);
                                            }
                                        }
                                        if (model.actions.Count > 0)
                                            for (int i = 0; i < model.actions.Count; i++)
                                            {
                                                if (i < (model.actions.Count - 1))
                                                {
                                                    maniVerDetail = new T_PhaseGroup_Phase_Mani();
                                                    Parse.CopyObject(model.actions[i], ref maniVerDetail);
                                                    maniVerDetail.ManipulationId = model.actions[i].ManipulationId == 0 ? null : model.actions[i].ManipulationId;
                                                    maniVerDetail.PhaseGroup_PhaseId = phase.Id;
                                                    maniVerDetail.CreatedUser = phase.UpdatedUser ?? 0;
                                                    maniVerDetail.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                                    db.T_PhaseGroup_Phase_Mani.Add(maniVerDetail);
                                                }
                                            }
                                    }
                                    else if ((oldDetails == null || oldDetails.Count() == 0) && model.actions != null && model.actions.Count > 0)
                                    {
                                        for (int i = 0; i < model.actions.Count; i++)
                                        {
                                            if (i < (model.actions.Count - 1))
                                            {
                                                maniVerDetail = new T_PhaseGroup_Phase_Mani();
                                                Parse.CopyObject(model.actions[i], ref maniVerDetail);
                                                maniVerDetail.ManipulationId = model.actions[i].ManipulationId == 0 ? null : model.actions[i].ManipulationId;
                                                maniVerDetail.PhaseGroup_PhaseId = phase.Id;
                                                maniVerDetail.CreatedUser = phase.UpdatedUser ?? 0;
                                                maniVerDetail.CreatedDate = phase.UpdatedDate ?? DateTime.Now;
                                                db.T_PhaseGroup_Phase_Mani.Add(maniVerDetail);
                                            }
                                        }
                                    }
                                    #endregion

                                    db.SaveChanges();
                                    ReOrderPhase(db, phase.PhaseGroupId, phase.Index, phase.Id);
                                    result.IsSuccess = true;
                                }
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

        public ResponseBase Approve(int actionUserId, int phaseId, bool isApprove)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var rs = new ResponseBase();
                    var phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == phaseId);
                    if (phase == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Approve", Message = "Công Đoạn này không tồn tại hoặc đã bị xóa trước đó. Vui lòng kiểm tra lại dữ liệu!." });
                    }
                    else
                    {
                        if (isApprove)
                        {
                            phase.IsApprove = isApprove;
                            phase.Approver = actionUserId;
                            phase.ApprovedDate = DateTime.Now;
                            phase.Status = eStatus.Approved;

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

        public ResponseBase UpdateName(int phaseId, string newName, bool isOwner, int actionUser)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    #region update
                    var phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == phaseId);
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


        public void ReOrderPhase(IEDEntities db, int parentId, int stt, int phaseId)
        {
            //sap xiep lai thứ tự
            var phases = (from x in db.T_PhaseGroup_Phase where !x.IsDeleted && x.PhaseGroupId == parentId && x.Index >= stt && x.Id != phaseId select x).OrderBy(x => x.Index);
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

        private bool CheckExists(string keyword, int id, int phaseGroupId, bool isCheckCode)
        {
            try
            {
                T_PhaseGroup_Phase phase = null;
                keyword = keyword.Trim().ToUpper();
                if (isCheckCode)
                    phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Code.Trim().ToUpper().Equals(keyword) && x.Id != id && x.PhaseGroupId == phaseGroupId);
                else
                    phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Name.Trim().ToUpper().Equals(keyword) && x.Id != id && x.PhaseGroupId == phaseGroupId);

                if (phase == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_PhaseGroup_Phase> GetPhasesByParentIds(List<int> parentIds)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var phases = db.T_PhaseGroup_Phase.Where(x => !x.IsDeleted && parentIds.Contains(x.PhaseGroupId));
                    if (phases != null && phases.Count() > 0)
                        return phases.ToList();
                    return new List<T_PhaseGroup_Phase>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<PhaseGroup_PhaseModel> Gets(int phasegroupId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index ASC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    var phases = (from x in db.T_PhaseGroup_Phase
                                  where !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.PhaseGroupId == phasegroupId
                                  orderby x.Index
                                  select new PhaseGroup_PhaseModel()
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

                                      PhaseGroupId = x.PhaseGroupId,
                                      ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                                      PercentWasteEquipment = x.PercentWasteEquipment,
                                      PercentWasteManipulation = x.PercentWasteManipulation,
                                      PercentWasteMaterial = x.PercentWasteMaterial,
                                      PercentWasteSpecial = x.PercentWasteSpecial,
                                      Video = x.Video,
                                      TimePrepareTMU = x.T_TimePrepare.TMUNumber,
                                      TimePrepareId = x.TimePrepareId,
                                      TimePrepareName = x.T_TimePrepare.Name,
                                      IsLibrary = x.IsLibrary,
                                      Status = x.Status,
                                      IsApprove = x.IsApprove,
                                      Approver = x.Approver,
                                      ApproverName = x.IsApprove ? x.SUser.UserName : "",
                                      ApproveDate = x.ApprovedDate
                                  }).OrderBy(sorting).ToList();

                    var pageListReturn = new PagedList<PhaseGroup_PhaseModel>(phases, pageNumber, pageSize);
                    if (pageListReturn != null && pageListReturn.Count > 0)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);
                        foreach (var item in pageListReturn)
                        {
                            item.actions.AddRange((from x in db.T_PhaseGroup_Phase_Mani
                                                   where !x.IsDeleted && x.PhaseGroup_PhaseId == item.Id
                                                   orderby x.OrderIndex
                                                   select new PhaseGroup_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       PhaseGroup_PhaseId = x.PhaseGroup_PhaseId,
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

        public PagedList<PhaseGroup_PhaseModel> Gets(int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Index ASC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    var phases = (from x in db.T_PhaseGroup_Phase
                                  where !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Status == eStatus.Submit
                                  orderby x.Index
                                  select new PhaseGroup_PhaseModel()
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

                                      PhaseGroupId = x.PhaseGroupId,
                                      PhaseGroupName = x.T_PhaseGroup.Name,
                                      ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                                      PercentWasteEquipment = x.PercentWasteEquipment,
                                      PercentWasteManipulation = x.PercentWasteManipulation,
                                      PercentWasteMaterial = x.PercentWasteMaterial,
                                      PercentWasteSpecial = x.PercentWasteSpecial,
                                      Video = x.Video,
                                      TimePrepareTMU = x.T_TimePrepare.TMUNumber,
                                      TimePrepareId = x.TimePrepareId,
                                      TimePrepareName = x.T_TimePrepare.Name,
                                      IsLibrary = x.IsLibrary,
                                  }).OrderBy(sorting).ToList();

                    var pageListReturn = new PagedList<PhaseGroup_PhaseModel>(phases, pageNumber, pageSize);
                    if (pageListReturn != null && pageListReturn.Count > 0)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);
                        foreach (var item in pageListReturn)
                        {
                            item.actions.AddRange((from x in db.T_PhaseGroup_Phase_Mani
                                                   where !x.IsDeleted && x.PhaseGroup_PhaseId == item.Id
                                                   orderby x.OrderIndex
                                                   select new PhaseGroup_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       PhaseGroup_PhaseId = x.PhaseGroup_PhaseId,
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

        public ResponseBase Delete(int Id, int actionUserId, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                    if (phase == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "delete ", Message = "Công Đoạn này đã tồn tại hoặc đã bị xóa trướ đó. Vui lòng kiểm tra lại dữ liệu!." });
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
                            phase.IsDeleted = true;
                            phase.DeletedUser = actionUserId;
                            phase.DeletedDate = DateTime.Now;

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

        public ResponseBase RemovePhaseVideo(int Id, int actionUserId, bool isOwner, ref string videoPath)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var phase = db.T_PhaseGroup_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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

        public ResponseBase Copy(int Id, int actionUserId)
        {
            var rs = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    var phase = (from x in db.T_PhaseGroup_Phase
                                 where !x.IsDeleted && x.Id == Id
                                 select x).FirstOrDefault();
                    if (phase != null)
                    {
                        var phaseAcc = (from x in db.T_PhaseGroup_Phase_Mani
                                        where !x.IsDeleted && x.PhaseGroup_PhaseId == Id
                                        select x);

                        var lastPhase = (from x in db.T_PhaseGroup_Phase
                                         where !x.IsDeleted && x.PhaseGroupId == phase.PhaseGroupId
                                         orderby x.Index descending
                                         select x).FirstOrDefault();

                        T_PhaseGroup_Phase phaseC;
                        T_PhaseGroup_Phase_Mani maniC;
                        var now = DateTime.Now;
                        phaseC = new T_PhaseGroup_Phase();
                        phaseC.Index = lastPhase.Index + 1;
                        phaseC.Name = phase.Name + "-Copy";
                        phaseC.Code = (phase.Code.LastIndexOf('-') == -1 ? phaseC.Index.ToString() : (phase.Code.Substring(0, phase.Code.LastIndexOf('-')) + "-" + phaseC.Index));
                        phaseC.PhaseGroupId = phase.PhaseGroupId;
                        phaseC.Description = phase.Description;
                        phaseC.EquipmentId = phase.EquipmentId;
                        phaseC.PhaseGroupId = phase.PhaseGroupId;
                        phaseC.WorkerLevelId = phase.WorkerLevelId;
                        phaseC.TotalTMU = phase.TotalTMU;
                        phaseC.ApplyPressuresId = phase.ApplyPressuresId;
                        phaseC.PercentWasteEquipment = phase.PercentWasteEquipment;
                        phaseC.PercentWasteManipulation = phase.PercentWasteManipulation;
                        phaseC.PercentWasteMaterial = phase.PercentWasteMaterial;
                        phaseC.PercentWasteSpecial = phase.PercentWasteSpecial;
                        phaseC.TimePrepareId = phase.TimePrepareId;
                        phaseC.Node = phase.Node;
                        phaseC.Video = phase.Video;
                        phaseC.CreatedUser = actionUserId;
                        phaseC.CreatedDate = now;


                        if (phaseAcc != null && phaseAcc.Count() > 0)
                        {
                            phaseC.T_PhaseGroup_Phase_Mani = new Collection<T_PhaseGroup_Phase_Mani>();
                            foreach (var item in phaseAcc)
                            {
                                maniC = new T_PhaseGroup_Phase_Mani();
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
                                maniC.T_PhaseGroup_Phase = phaseC;
                                phaseC.T_PhaseGroup_Phase_Mani.Add(maniC);
                            }
                        }
                        db.T_PhaseGroup_Phase.Add(phaseC);
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


        public int GetLastIndex(int ParentId)
        {
            using (db = new IEDEntities())
            {
                var obj = (from x in db.T_PhaseGroup_Phase where !x.IsDeleted && x.PhaseGroupId == ParentId orderby x.Index descending select x.Index).FirstOrDefault();
                if (obj != null)
                    return obj;
                return 0;
            }
        }

        public PhaseGroup_PhaseModel GetPhase(int phaseId)
        {
            using (db = new IEDEntities())
            {
                try
                {
                    var phaseObj = (from x in db.T_PhaseGroup_Phase
                                    where !x.IsDeleted && !x.T_PhaseGroup.IsDeleted && x.Id == phaseId
                                    select new PhaseGroup_PhaseModel()
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
                                        PhaseGroupId = x.PhaseGroupId,
                                        ApplyPressuresId = x.ApplyPressuresId != null ? x.ApplyPressuresId : 0,
                                        PercentWasteEquipment = x.PercentWasteEquipment,
                                        PercentWasteManipulation = x.PercentWasteManipulation,
                                        PercentWasteMaterial = x.PercentWasteMaterial,
                                        PercentWasteSpecial = x.PercentWasteSpecial,
                                        Video = x.Video,
                                        TimePrepareId = x.TimePrepareId,
                                        TimePrepareName = x.T_TimePrepare.Name,
                                        TimePrepareTMU = x.T_TimePrepare.TMUNumber
                                    }).FirstOrDefault();
                    if (phaseObj != null)
                    {
                        double tmu = 27.8;
                        var config = (from x in db.T_IEDConfig
                                      where !x.IsDeleted && x.Name.Trim().ToUpper().Equals(eIEDConfigName.TMU.Trim().ToUpper())
                                      select x).FirstOrDefault();
                        if (config != null)
                            double.TryParse(config.Value, out tmu);

                        phaseObj.timePrepares
                            .Add(new Commo_Ana_Phase_TimePrepareModel()
                            {
                                Id = phaseObj.TimePrepareId,
                                TimePrepareId = phaseObj.TimePrepareId,
                                Name = phaseObj.TimePrepareName,
                                TMUNumber = phaseObj.TimePrepareTMU,
                            });
                        phaseObj.TimePrepareTMU = phaseObj.timePrepares.Sum(x => x.TMUNumber);

                        phaseObj.actions.AddRange((from x in db.T_PhaseGroup_Phase_Mani
                                                   where !x.IsDeleted && x.PhaseGroup_PhaseId == phaseObj.Id
                                                   orderby x.OrderIndex
                                                   select new PhaseGroup_Phase_ManiModel()
                                                   {
                                                       Id = x.Id,
                                                       PhaseGroup_PhaseId = x.PhaseGroup_PhaseId,
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

                    }
                    return phaseObj;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public ResponseBase TinhLaiCode(List<PhaseGroup_Phase_ManiModel> actions, int equipmentId, int equiptypedefaultId, int applyPressure)
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
                        exportObj.Details.AddRange((from x in db.T_PhaseGroup_Phase_Mani
                                                    where !x.IsDeleted && x.PhaseGroup_PhaseId == Id
                                                    orderby x.OrderIndex
                                                    select new Commo_Ana_Phase_ManiModel()
                                                    {
                                                        Id = x.Id,
                                                        CA_PhaseId = x.PhaseGroup_PhaseId,
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


        public List<ModelSelectItem> GetAllPhasesForSuggest()
        {
            var list = new List<ModelSelectItem>();
            using (var db = new IEDEntities())
            {
                return (from x in db.T_PhaseGroup_Phase
                        where !x.IsDeleted && !x.T_PhaseGroup.IsDeleted &&
                          x.IsLibrary && x.IsApprove
                        select new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name,
                            Code = x.Code,
                            Double = x.TotalTMU,
                        }).ToList();
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

                    var temps = (from x in db.T_PhaseGroup_Phase
                                 where
                                 !x.IsDeleted &&
                                 !x.T_PhaseGroup.IsDeleted &&
                                 !x.IsLibrary &&
                                 x.IsApprove
                                 select new PhaseLibModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Code = x.Code,
                                     TotalTMU = x.TotalTMU,
                                     EquipName = x.EquipmentId.HasValue ? x.T_Equipment.Name : "",
                                     Node = x.Node,
                                     Product = "",
                                     GroupPhase = x.T_PhaseGroup.Name
                                 }).ToList();

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        temps = temps.Where(x => (x.Code.Trim().ToUpper().Contains(keyword) ||
                               x.Name.Trim().ToUpper().Contains(keyword))).ToList();
                    }
                    if (temps.Count > 0)
                        phases.AddRange(temps);
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

                    var temps = (from x in db.T_PhaseGroup_Phase
                                 where
                                 !x.IsDeleted &&
                                 !x.T_PhaseGroup.IsDeleted &&
                                 x.IsLibrary &&
                                 x.IsApprove
                                 select new PhaseLibModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Code = x.Code,
                                     TotalTMU = x.TotalTMU,
                                     EquipName = x.EquipmentId.HasValue ? x.T_Equipment.Name : "",
                                     Node = x.Node,
                                     Product = "",
                                     GroupPhase = x.T_PhaseGroup.Name
                                 }).ToList();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        temps = temps.Where(x => (x.Code.Trim().ToUpper().Contains(keyword) ||
                               x.Name.Trim().ToUpper().Contains(keyword))).ToList();
                    }
                    if (temps.Count > 0)
                        phases.AddRange(temps);

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


    }
}
