using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace GPRO_IED_A.Business
{
    public class BLLCommodityAnalysis
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLCommodityAnalysis _Instance;
        public static BLLCommodityAnalysis Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCommodityAnalysis();

                return _Instance;
            }
        }
        private BLLCommodityAnalysis() { }
        #endregion
        bool checkPermis(T_CommodityAnalysis obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public CommodityAnalysisModel GetList()
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var commoAnaModel = new CommodityAnalysisModel();
                    var commoAnalysis = (from x in db.T_CommodityAnalysis
                                         where !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity
                                         select new ProAnaModel()
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Node = x.Node,
                                             ObjectId = x.ObjectId,
                                             ObjectType = x.ObjectType,
                                             ParentId = x.ParentId,
                                             Description = x.Description,
                                             CreatedDate = x.CreatedDate
                                         });
                    if (commoAnalysis != null)
                    {
                        commoAnaModel.CommoAna.AddRange(commoAnalysis);
                        commoAnaModel.years.AddRange(commoAnalysis.Select(x => x.CreatedDate.Month).Distinct());
                    }
                    return commoAnaModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommodityAnalysisModel GetCommoAnaItemByParentId(int parentId, string value, int Type, int companyId, int[] relationCompanyId, int year, int[] workshopIds)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var commoAnaModel = new CommodityAnalysisModel();
                    int value_ = 0;
                    switch (Type)
                    {
                        case (int)eObjectType.getYear:
                            commoAnaModel.years.AddRange((from x in db.T_CommodityAnalysis where !x.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && x.ObjectType == (int)eObjectType.isCommodity select x.CreatedDate.Year).Distinct());
                            break;
                        case (int)eObjectType.getMonth:
                            value_ = int.Parse(value);
                            commoAnaModel.years.AddRange((from x in db.T_CommodityAnalysis where !x.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && x.ObjectType == (int)eObjectType.isCommodity && x.CreatedDate.Year == value_ select x.CreatedDate.Month).Distinct());
                            break;
                        case (int)eObjectType.isCommodity:
                            value_ = int.Parse(value);
                            commoAnaModel.CommoAna.AddRange((from x in db.T_CommodityAnalysis
                                                             where !x.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && x.ObjectType == (int)eObjectType.isCommodity && x.CreatedDate.Year == year && x.CreatedDate.Month == value_
                                                             select new ProAnaModel()
                                                             {
                                                                 Id = x.Id,
                                                                 Name = x.Name,
                                                                 Node = x.Node,
                                                                 ObjectId = x.ObjectId,
                                                                 ObjectType = x.ObjectType,
                                                                 ParentId = x.ParentId,
                                                                 Description = x.Description,
                                                                 CreatedDate = x.CreatedDate
                                                             }));
                            break;
                        case (int)eObjectType.isWorkShop:
                            if (workshopIds != null && workshopIds.Length > 0)
                                commoAnaModel.CommoAna.AddRange((from x in db.T_CommodityAnalysis
                                                                 where !x.IsDeleted
                                                                 && x.ParentId == parentId
                                                                 && workshopIds.Contains(x.ObjectId)
                                                                 select new ProAnaModel()
                                                                 {
                                                                     Id = x.Id,
                                                                     Name = x.Name,
                                                                     Node = x.Node,
                                                                     ObjectId = x.ObjectId,
                                                                     ObjectType = x.ObjectType,
                                                                     ParentId = x.ParentId,
                                                                     Description = x.Description,
                                                                     CreatedDate = x.CreatedDate,
                                                                 }));
                            break;
                        case (int)eObjectType.isComponent:
                        case (int)eObjectType.isGroupVersion:
                        case (int)eObjectType.isPhaseGroup:
                            commoAnaModel.CommoAna.AddRange((from x in db.T_CommodityAnalysis
                                                             where !x.IsDeleted && x.ParentId == parentId
                                                             select new ProAnaModel()
                                                             {
                                                                 Id = x.Id,
                                                                 Name = x.Name,
                                                                 Node = x.Node,
                                                                 ObjectId = x.ObjectId,
                                                                 ObjectType = x.ObjectType,
                                                                 ParentId = x.ParentId,
                                                                 Description = x.Description,
                                                                 CreatedDate = x.CreatedDate,
                                                             }));
                            break;
                    }

                    if (commoAnaModel.CommoAna.Count > 0)
                    {
                        switch (commoAnaModel.CommoAna.First().ObjectType)
                        {
                            case (int)eObjectType.isCommodity:
                                var commoIds = commoAnaModel.CommoAna.Select(x => x.ObjectId);
                                var commoCheck = db.T_Product.Where(x => !x.IsDeleted && commoIds.Contains(x.Id)).Select(x => x.Id);
                                commoAnaModel.CommoAna = commoAnaModel.CommoAna.Where(x => commoCheck.Contains(x.ObjectId)).ToList();
                                break;
                            case (int)eObjectType.isWorkShop:
                                var _workshopIds = commoAnaModel.CommoAna.Select(x => x.ObjectId);
                                var workshopCheck = db.T_WorkShop.Where(x => !x.IsDeleted && _workshopIds.Contains(x.Id)).Select(x => x.Id);
                                commoAnaModel.CommoAna = commoAnaModel.CommoAna.Where(x => workshopCheck.Contains(x.ObjectId)).ToList();
                                break;
                            case (int)eObjectType.isPhaseGroup:
                                var phaseGroupIds = commoAnaModel.CommoAna.Select(x => x.ObjectId);
                                var phaseGroupCheck = db.T_PhaseGroup.Where(x => !x.IsDeleted && phaseGroupIds.Contains(x.Id)).Select(x => x.Id);
                                commoAnaModel.CommoAna = commoAnaModel.CommoAna.Where(x => phaseGroupCheck.Contains(x.ObjectId)).ToList();

                                break;
                        }
                    }
                    return commoAnaModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(T_CommodityAnalysis noNameModel, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();

                    if (noNameModel.ObjectType == (int)eObjectType.isPhaseGroup && noNameModel.ObjectId == 0)
                    {
                        //tao nhóm cong doan
                        var newPhaseGroup = db.T_PhaseGroup.FirstOrDefault(x => x.Name == noNameModel.Name && !x.IsDeleted);
                        if (newPhaseGroup != null)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên Cụm Công Đoạn này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        }
                        else
                        {
                            //getparent
                            int _parentId = Int32.Parse(noNameModel.Node.Split(',')[2]);
                            var parentObj = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == _parentId);

                            newPhaseGroup = new T_PhaseGroup();
                            newPhaseGroup.Name = noNameModel.Name;
                            newPhaseGroup.MinLevel = 1;
                            newPhaseGroup.MaxLevel = 1;
                            newPhaseGroup.CreatedUser = noNameModel.CreatedUser;
                            newPhaseGroup.CreatedDate = noNameModel.CreatedDate;
                            newPhaseGroup.WorkshopIds = parentObj.ObjectId.ToString();
                            db.T_PhaseGroup.Add(newPhaseGroup);
                            db.SaveChanges();
                            noNameModel.ObjectId = newPhaseGroup.Id;
                        }
                    }

                    if (noNameModel.ObjectType == (int)eObjectType.isWorkShop && noNameModel.ObjectId == 0)
                    {
                        //tao phan xuong
                        var newWorkShop = db.T_WorkShop.FirstOrDefault(x => x.Name == noNameModel.Name && !x.IsDeleted);
                        if (newWorkShop != null)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên phân xưởng này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        }
                        else
                        {
                            newWorkShop = new T_WorkShop();
                            newWorkShop.Name = noNameModel.Name;
                            newWorkShop.CreatedUser = noNameModel.CreatedUser;
                            newWorkShop.CreatedDate = noNameModel.CreatedDate;
                            newWorkShop.CompanyId = noNameModel.CompanyId;
                            db.T_WorkShop.Add(newWorkShop);
                            db.SaveChanges();
                            noNameModel.ObjectId = newWorkShop.Id;
                        }
                    }

                    T_CommodityAnalysis noName = null;
                    if (CheckObjectExists(noNameModel))
                    {
                        result.IsSuccess = false;
                        switch (noNameModel.ObjectType)
                        {
                            case (int)eObjectType.isCommodity:
                                result.Errors.Add(new Error() { MemberName = "Insert  ", Message = "Mã hàng này đã có bài phân tích. Vui lòng chọn lại Mã hàng khác !." });
                                break;
                            case (int)eObjectType.isWorkShop:
                                result.Errors.Add(new Error() { MemberName = "Insert  ", Message = "Phân xưởng này đã được chọn. Vui lòng chọn lại Phân xưởng khác !." });
                                break;
                            case (int)eObjectType.isPhaseGroup:
                                result.Errors.Add(new Error() { MemberName = "Insert  ", Message = "Nhóm Công Đoạn này đã được chọn. Vui lòng chọn lại Nhóm Công Đoạn khác !." });
                                break;
                        }
                    }
                    else
                    {
                        if (noNameModel.Id == 0)
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                noName = new T_CommodityAnalysis();
                                Parse.CopyObject(noNameModel, ref noName);
                                noName.Node = noName.Node == "0" ? noName.Id.ToString() : noName.Node + noName.ParentId + ",";

                                if (noName.ObjectType == (int)eObjectType.isVersion)
                                {
                                    noName.ObjectId = FindLastedVersion(noName.ParentId, noName.Node);
                                }
                                db.T_CommodityAnalysis.Add(noName);
                                db.SaveChanges();
                                if (noName.ObjectType == (int)eObjectType.isWorkShop)
                                {
                                    T_CommodityAnalysis newObject;
                                    for (int i = 0; i < 3; i++)
                                    {
                                        newObject = new T_CommodityAnalysis();
                                        newObject.Id = 0;
                                        newObject.Node = noName.Node + noName.Id + ",";
                                        newObject.ParentId = noName.Id;
                                        newObject.ObjectId = 0;
                                        switch (i)
                                        {
                                            case 0:
                                                newObject.Name = "Quy trình công nghệ";
                                                newObject.ObjectType = (int)eObjectType.isGroupVersion;
                                                break;
                                            case 1:
                                                newObject.Name = "Thiết kế chuyền";
                                                newObject.ObjectType = (int)eObjectType.isLabourDivision;
                                                break;
                                            case 2:
                                                newObject.Name = "Thành Phần";
                                                newObject.ObjectType = (int)eObjectType.isComponent;
                                                break;
                                        }
                                        newObject.Description = "";
                                        newObject.CreatedUser = noName.CreatedUser;
                                        newObject.CreatedDate = noName.CreatedDate;
                                        db.T_CommodityAnalysis.Add(newObject);
                                    }
                                }
                                db.SaveChanges();
                                scope.Complete();
                                result.IsSuccess = true;
                            }
                        }
                        else
                        {
                            //Update                     
                            noName = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == noNameModel.Id);
                            if (noName == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update  ", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                if (!checkPermis(noName, noNameModel.UpdatedUser.Value, isOwner))
                                {
                                    result.IsSuccess = false;
                                    switch (noName.ObjectType)
                                    {
                                        case (int)eObjectType.isCommodity:
                                            result.Errors.Add(new Error() { MemberName = "update  ", Message = "Bạn không phải là người tạo mã hàng này nên bạn không cập nhật được thông tin cho mã hàng này." });
                                            break;
                                        case (int)eObjectType.isWorkShop:
                                            result.Errors.Add(new Error() { MemberName = "update  ", Message = "Bạn không phải là người tạo phân xưởng này nên bạn không cập nhật được thông tin cho phân xưởng này." });
                                            break;
                                        case (int)eObjectType.isPhaseGroup:
                                            result.Errors.Add(new Error() { MemberName = "update  ", Message = "Bạn không phải là người tạo nhóm công đoạn này nên bạn không cập nhật được thông tin cho nhóm công đoạn này." });
                                            break;
                                    }
                                }
                                else
                                {
                                    if (noName.ObjectType == (int)eObjectType.isPhaseGroup && noName.Description != noNameModel.Description)
                                    {
                                        var phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == noName.Id);
                                        if (phases != null && phases.Count() > 0)
                                            foreach (var item in phases)
                                                item.Code = noNameModel.Description == null || noNameModel.Description == "" ? item.Index.ToString() : noNameModel.Description + "-" + item.Index;

                                    }
                                    noName.Name = noNameModel.Name;
                                    noName.Description = noNameModel.Description;
                                    noName.UpdatedUser = noNameModel.UpdatedUser;
                                    noName.UpdatedDate = noNameModel.UpdatedDate;
                                    db.SaveChanges();
                                    result.IsSuccess = true;
                                }
                            }
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

        private bool CheckObjectExists(T_CommodityAnalysis model)
        {
            try
            {
                T_CommodityAnalysis noName = null;
                noName = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.ObjectId == model.ObjectId && x.ObjectType == model.ObjectType && x.ParentId == model.ParentId && x.Id != model.Id);
                if (noName == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int FindLastedVersion(int parentId, string Node)
        {
            try
            {
                var noName = db.T_CommodityAnalysis.Where(x => x.ParentId == parentId && x.Node.Trim().Equals(Node.Trim())).OrderByDescending(x => x.ObjectId);
                if (noName != null && noName.Count() > 0)
                    return noName.FirstOrDefault().ObjectId + 1;
                return 1;
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
                    var commoAna = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                    if (commoAna != null)
                    {
                        if (!checkPermis(commoAna, actionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            switch (commoAna.ObjectType)
                            {
                                case (int)eObjectType.isCommodity:
                                    result.Errors.Add(new Error() { MemberName = "delete  ", Message = "Bạn không phải là người tạo mã hàng này nên bạn không xóa được xóa mã hàng này." });
                                    break;
                                case (int)eObjectType.isWorkShop:
                                    result.Errors.Add(new Error() { MemberName = "delete  ", Message = "Bạn không phải là người tạo phân xưởng này nên bạn không xóa được xóa phân xưởng này." });
                                    break;
                                case (int)eObjectType.isPhaseGroup:
                                    result.Errors.Add(new Error() { MemberName = "delete  ", Message = "Bạn không phải là người tạo nhóm công đoạn này nên bạn không xóa được xóa nhóm công đoạn này." });
                                    break;
                            }
                        }
                        else
                        {
                            commoAna.IsDeleted = true;
                            commoAna.DeletedUser = actionUserId;
                            commoAna.DeletedDate = DateTime.Now;
                            var node = commoAna.Node + Id + ",";
                            var childCommoAna = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.Node.Trim().ToUpper().Contains(node.Trim().ToUpper()));
                            if (childCommoAna != null && childCommoAna.Count() > 0)
                            {
                                foreach (var item in childCommoAna)
                                {
                                    item.IsDeleted = true;
                                    item.DeletedUser = actionUserId;
                                    item.DeletedDate = DateTime.Now;
                                }
                            }

                            if (commoAna.ObjectType == (int)eObjectType.isPhaseGroup)
                            {
                                var phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == commoAna.Id);
                                if (phases != null && phases.Count() > 0)
                                {
                                    foreach (var item in phases)
                                    {
                                        item.IsDeleted = true;
                                        item.DeletedUser = actionUserId;
                                        item.DeletedDate = DateTime.Now;
                                    }
                                }
                            }
                            db.SaveChanges();
                            result.IsSuccess = true;
                            result.Errors.Add(new Error() { MemberName = "", Message = "Xóa Thành Công.!" });
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại.\nVui lòng kiểm tra lại.!" });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Ktra lai
        public ResponseBase Copy_CommoAnaPhaseGroup(int fromId, int toId, int actionUserId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var commoAna = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == toId);
                    if (commoAna != null)
                    {
                        //ktra object copy có còn tồn tại hay không
                        var objCopy = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == fromId);
                        if (objCopy != null)
                        {

                            // ktra coi mat hag dang phan tich co tồn tại nhóm công đoạn này chưa
                            var node = (commoAna.Node + commoAna.Id + ",").Trim();
                            var objExists = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Node.Trim().Equals(node) && x.ObjectId == objCopy.ObjectId);
                            if (objExists != null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "", Message = "Mã hàng Bạn đang Phân Tích đã Tồn Tại Nhóm Công Đoạn này rồi.\nVui lòng kiểm tra lại.!" });
                            }
                            else
                            {
                                using (TransactionScope scope = new TransactionScope())
                                {
                                    //step 1  -  copy phase group
                                    var new_commoAna = new T_CommodityAnalysis();
                                    new_commoAna.Name = objCopy.Name;
                                    new_commoAna.ObjectType = objCopy.ObjectType;
                                    new_commoAna.ObjectId = objCopy.ObjectId;
                                    new_commoAna.ParentId = commoAna.Id;
                                    new_commoAna.Node = node;
                                    new_commoAna.Description = objCopy.Description;
                                    new_commoAna.CreatedUser = actionUserId;
                                    new_commoAna.CreatedDate = DateTime.Now;
                                    db.T_CommodityAnalysis.Add(new_commoAna);
                                    db.SaveChanges();
                                    var parentid = new_commoAna.Id;
                                    //step 2  - copy phase
                                    var commo_ana_phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == objCopy.Id).OrderBy(x => x.CreatedDate);
                                    if (commo_ana_phases != null && commo_ana_phases.Count() > 0)
                                    {
                                        foreach (var item in commo_ana_phases)
                                        {
                                            var new_commoAnaPhase = new T_CA_Phase();
                                            new_commoAnaPhase.Index = item.Index;
                                            new_commoAnaPhase.Name = item.Name;
                                            new_commoAnaPhase.Code = item.Code;
                                            new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                            new_commoAnaPhase.Description = item.Description;
                                            new_commoAnaPhase.EquipmentId = item.EquipmentId;
                                            new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                            new_commoAnaPhase.WorkerLevelId = item.WorkerLevelId;
                                            new_commoAnaPhase.ParentId = item.ParentId;
                                            new_commoAnaPhase.TotalTMU = item.TotalTMU;
                                            new_commoAnaPhase.ApplyPressuresId = item.ApplyPressuresId;
                                            new_commoAnaPhase.PercentWasteEquipment = item.PercentWasteEquipment;
                                            new_commoAnaPhase.PercentWasteManipulation = item.PercentWasteManipulation;
                                            new_commoAnaPhase.PercentWasteMaterial = item.PercentWasteMaterial;
                                            new_commoAnaPhase.PercentWasteSpecial = item.PercentWasteSpecial;
                                            new_commoAnaPhase.Node = item.Node;
                                            new_commoAnaPhase.Video = item.Video;
                                            new_commoAnaPhase.CreatedUser = actionUserId;
                                            new_commoAnaPhase.CreatedDate = new_commoAna.CreatedDate;
                                            // step 3 - copy active timeprepare                                            
                                            var listTimePrepareExist = db.T_CA_Phase_TimePrepare.Where(c => c.Commo_Ana_PhaseId == item.Id && !c.IsDeleted).ToList();
                                            if (listTimePrepareExist.Count > 0)
                                            {
                                                var listTimePrepareNew = new Collection<T_CA_Phase_TimePrepare>();
                                                foreach (var timePrepare in listTimePrepareExist)
                                                {
                                                    listTimePrepareNew.Add(new T_CA_Phase_TimePrepare()
                                                    {
                                                        Commo_Ana_PhaseId = item.Id,
                                                        TimePrepareId = timePrepare.TimePrepareId,
                                                        CreatedUser = actionUserId,
                                                        CreatedDate = new_commoAnaPhase.CreatedDate
                                                    });
                                                }
                                                new_commoAnaPhase.T_CA_Phase_TimePrepare = listTimePrepareNew;
                                            }
                                            //check         // step 4  - copy active manipulation version
                                            var phaseAcc = db.T_CA_Phase_Mani.Where(x => !x.IsDeleted && x.CA_PhaseId == item.Id).ToList();
                                            if (phaseAcc != null && phaseAcc.Count() > 0)
                                            {
                                                new_commoAnaPhase.T_CA_Phase_Mani = new Collection<T_CA_Phase_Mani>();
                                                foreach (var acc in phaseAcc)
                                                {
                                                    var maniC = new T_CA_Phase_Mani();
                                                    maniC.OrderIndex = acc.OrderIndex;
                                                    maniC.ManipulationId = acc.ManipulationId;
                                                    maniC.ManipulationCode = acc.ManipulationCode;
                                                    maniC.ManipulationName = acc.ManipulationName;
                                                    maniC.TMUEquipment = acc.TMUEquipment;
                                                    maniC.TMUManipulation = acc.TMUManipulation;
                                                    maniC.Loop = acc.Loop;
                                                    maniC.TotalTMU = acc.TotalTMU;
                                                    maniC.CreatedUser = actionUserId;
                                                    maniC.CreatedDate = new_commoAnaPhase.CreatedDate;
                                                    maniC.T_CA_Phase = new_commoAnaPhase;
                                                    new_commoAnaPhase.T_CA_Phase_Mani.Add(maniC);
                                                }
                                            }

                                            new_commoAnaPhase.T_CommodityAnalysis = new_commoAna;
                                            // new_commoAnaPhase.ParentId = parentid;
                                            new_commoAnaPhase.Node = new_commoAna.Node + new_commoAnaPhase.T_CommodityAnalysis.Id + ",";
                                            new_commoAnaPhase.CreatedUser = actionUserId;
                                            new_commoAnaPhase.CreatedDate = new_commoAna.CreatedDate;
                                            db.T_CA_Phase.Add(new_commoAnaPhase);
                                            db.SaveChanges();
                                        }
                                    }
                                    scope.Complete();
                                    result.IsSuccess = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại.\nVui lòng kiểm tra lại.!" });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommodityAnalysisModel GetProductByCustomerId(int customerId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var productIds = db.T_Product.Where(x => !x.IsDeleted && !x.T_Customer.IsDeleted && x.CustomerId == customerId).Select(x => x.Id).ToList();

                    var commoAnaModel = new CommodityAnalysisModel();
                    var commoAnalysis = (from x in db.T_CommodityAnalysis
                                         where
                                         !x.IsDeleted &&
                                         x.ObjectType == (int)eObjectType.isCommodity &&
                                         productIds.Contains(x.ObjectId)
                                         select new ProAnaModel()
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Node = x.Node,
                                             ObjectId = x.ObjectId,
                                             ObjectType = x.ObjectType,
                                             ParentId = x.ParentId,
                                             Description = x.Description,
                                             CreatedDate = x.CreatedDate
                                         });
                    if (commoAnalysis != null)
                    {
                        commoAnaModel.CommoAna.AddRange(commoAnalysis);
                        commoAnaModel.years.AddRange(commoAnalysis.Select(x => x.CreatedDate.Month).Distinct());
                    }
                    return commoAnaModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommodityAnalysisModel GetProductByProductGroupId(int proGroupId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var productIds = db.T_Product.Where(x => !x.IsDeleted && !x.T_ProductGroup.IsDeleted && x.ProductGroupId == proGroupId).Select(x => x.Id).ToList();

                    var commoAnaModel = new CommodityAnalysisModel();
                    var commoAnalysis = (from x in db.T_CommodityAnalysis
                                         where
                                         !x.IsDeleted &&
                                         x.ObjectType == (int)eObjectType.isCommodity &&
                                         productIds.Contains(x.ObjectId)
                                         select new ProAnaModel()
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Node = x.Node,
                                             ObjectId = x.ObjectId,
                                             ObjectType = x.ObjectType,
                                             ParentId = x.ParentId,
                                             Description = x.Description,
                                             CreatedDate = x.CreatedDate
                                         });
                    if (commoAnalysis != null)
                    {
                        commoAnaModel.CommoAna.AddRange(commoAnalysis);
                        commoAnaModel.years.AddRange(commoAnalysis.Select(x => x.CreatedDate.Month).Distinct());
                    }
                    return commoAnaModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ResponseBase Copy_CommoAna(int fromId, int toId, int actionUserId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var commoAna = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == fromId);
                    if (commoAna != null)
                    {
                        //ktra object copy có còn tồn tại hay không
                        var objCopy = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == toId);
                        if (objCopy != null)
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                string phanxuongNode = "0," + fromId + ",";
                                var phanxuongs = db.T_CommodityAnalysis
                                    .Where(x => !x.IsDeleted && x.Node == phanxuongNode)
                                    .Select(x => new { Id = x.Id, Name = x.Name, Node = x.Node, ObjectId = x.ObjectId, ObjectType = x.ObjectType, ParentId = x.ParentId, Description = x.Description, CompanyId = x.CompanyId })
                                    .ToList();
                                if (phanxuongs.Count > 0)
                                {
                                    T_CommodityAnalysis _phanxuong, _qtcn, _tkc, _tp;
                                    string _node = ("0," + toId + ","),
                                         sourceNode = ("0," + fromId + ",");
                                    for (int i = 0; i < phanxuongs.Count; i++)
                                    {
                                        _node = ("0," + toId + ",");
                                        //phan xuong
                                        _phanxuong = new T_CommodityAnalysis();
                                        _phanxuong.Name = phanxuongs[i].Name;
                                        _phanxuong.ObjectType = phanxuongs[i].ObjectType;
                                        _phanxuong.ObjectId = phanxuongs[i].ObjectId;
                                        _phanxuong.ParentId = toId;
                                        _phanxuong.Node = _node;
                                        _phanxuong.Description = phanxuongs[i].Description;
                                        _phanxuong.CreatedUser = actionUserId;
                                        _phanxuong.CreatedDate = DateTime.Now;
                                        db.T_CommodityAnalysis.Add(_phanxuong);
                                        db.SaveChanges();

                                        _node = (_phanxuong.Node + "" + _phanxuong.Id + ",");
                                        //quy trinh cong nghe
                                        _qtcn = new T_CommodityAnalysis();
                                        _qtcn.Name = "Quy trình công nghệ";
                                        _qtcn.ObjectType = (int)eObjectType.isGroupVersion;
                                        _qtcn.ObjectId = 0;
                                        _qtcn.ParentId = _phanxuong.Id;
                                        _qtcn.Node = _node;
                                        _qtcn.CreatedUser = actionUserId;
                                        _qtcn.CreatedDate = DateTime.Now;
                                        db.T_CommodityAnalysis.Add(_qtcn);

                                        //tkc
                                        _tkc = new T_CommodityAnalysis();
                                        _tkc.Name = "Thiết kế chuyền";
                                        _tkc.ObjectType = (int)eObjectType.isLabourDivision;
                                        _tkc.ObjectId = 0;
                                        _tkc.ParentId = _phanxuong.Id;
                                        _tkc.Node = _node;
                                        _tkc.CreatedUser = actionUserId;
                                        _tkc.CreatedDate = DateTime.Now;
                                        db.T_CommodityAnalysis.Add(_tkc);

                                        //thanh pham
                                        _tp = new T_CommodityAnalysis();
                                        _tp.Name = "Thành Phần";
                                        _tp.ObjectType = (int)eObjectType.isComponent;
                                        _tp.ObjectId = 0;
                                        _tp.ParentId = _phanxuong.Id;
                                        _tp.Node = _node;
                                        _tp.CreatedUser = actionUserId;
                                        _tp.CreatedDate = DateTime.Now;
                                        db.T_CommodityAnalysis.Add(_tp);
                                        db.SaveChanges();

                                        //lay ds nhom cong doan
                                        _node = phanxuongs[i].Node + "" + phanxuongs[i].Id + ",";
                                        var phaseGroups = db.T_CommodityAnalysis
                                                            .Where(x => !x.IsDeleted && x.Node.Contains(_node) && x.ObjectType == (int)eObjectType.isPhaseGroup)
                                                            .Select(x => new { Id = x.Id, Name = x.Name, Node = x.Node, ObjectId = x.ObjectId, ObjectType = x.ObjectType, ParentId = x.ParentId, Description = x.Description, CompanyId = x.CompanyId })
                                                            .ToList();
                                        if (phaseGroups.Count > 0)
                                        {
                                            _node = (_tp.Node + "" + _tp.Id + ",");
                                            for (int ii = 0; ii < phaseGroups.Count; ii++)
                                            {
                                                //step 1  -  copy phase group
                                                var new_commoAna = new T_CommodityAnalysis();
                                                new_commoAna.Name = phaseGroups[ii].Name;
                                                new_commoAna.ObjectType = phaseGroups[ii].ObjectType;
                                                new_commoAna.ObjectId = phaseGroups[ii].ObjectId;
                                                new_commoAna.ParentId = _tp.Id;
                                                new_commoAna.Node = _node;
                                                new_commoAna.Description = phaseGroups[ii].Description;
                                                new_commoAna.CreatedUser = actionUserId;
                                                new_commoAna.CreatedDate = DateTime.Now;
                                                db.T_CommodityAnalysis.Add(new_commoAna);
                                                db.SaveChanges();
                                                var parentid = new_commoAna.Id;
                                                //step 2  - copy phase
                                                int _id = phaseGroups[ii].Id;
                                                var commo_ana_phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == _id).OrderBy(x => x.CreatedDate);
                                                if (commo_ana_phases != null && commo_ana_phases.Count() > 0)
                                                {
                                                    foreach (var item in commo_ana_phases)
                                                    {
                                                        var new_commoAnaPhase = new T_CA_Phase();
                                                        new_commoAnaPhase.Index = item.Index;
                                                        new_commoAnaPhase.Name = item.Name;
                                                        new_commoAnaPhase.Code = item.Code;
                                                        new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                                        new_commoAnaPhase.Description = item.Description;
                                                        new_commoAnaPhase.EquipmentId = item.EquipmentId;
                                                        new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                                        new_commoAnaPhase.WorkerLevelId = item.WorkerLevelId;
                                                        new_commoAnaPhase.ParentId = new_commoAna.Id;
                                                        new_commoAnaPhase.TotalTMU = item.TotalTMU;
                                                        new_commoAnaPhase.ApplyPressuresId = item.ApplyPressuresId;
                                                        new_commoAnaPhase.PercentWasteEquipment = item.PercentWasteEquipment;
                                                        new_commoAnaPhase.PercentWasteManipulation = item.PercentWasteManipulation;
                                                        new_commoAnaPhase.PercentWasteMaterial = item.PercentWasteMaterial;
                                                        new_commoAnaPhase.PercentWasteSpecial = item.PercentWasteSpecial;
                                                        new_commoAnaPhase.Node = item.Node;
                                                        new_commoAnaPhase.Video = item.Video;
                                                        new_commoAnaPhase.CreatedUser = actionUserId;
                                                        new_commoAnaPhase.CreatedDate = new_commoAna.CreatedDate;
                                                        // step 3 - copy active timeprepare                                            
                                                        var listTimePrepareExist = db.T_CA_Phase_TimePrepare.Where(c => c.Commo_Ana_PhaseId == item.Id && !c.IsDeleted).ToList();
                                                        if (listTimePrepareExist.Count > 0)
                                                        {
                                                            var listTimePrepareNew = new Collection<T_CA_Phase_TimePrepare>();
                                                            foreach (var timePrepare in listTimePrepareExist)
                                                            {
                                                                listTimePrepareNew.Add(new T_CA_Phase_TimePrepare()
                                                                {
                                                                    Commo_Ana_PhaseId = item.Id,
                                                                    TimePrepareId = timePrepare.TimePrepareId,
                                                                    CreatedUser = actionUserId,
                                                                    CreatedDate = new_commoAnaPhase.CreatedDate
                                                                });
                                                            }
                                                            new_commoAnaPhase.T_CA_Phase_TimePrepare = listTimePrepareNew;
                                                        }
                                                        //check        
                                                        // step 4  - copy active manipulation version
                                                        var phaseAcc = db.T_CA_Phase_Mani.Where(x => !x.IsDeleted && x.CA_PhaseId == item.Id).ToList();
                                                        if (phaseAcc != null && phaseAcc.Count() > 0)
                                                        {
                                                            new_commoAnaPhase.T_CA_Phase_Mani = new Collection<T_CA_Phase_Mani>();
                                                            foreach (var acc in phaseAcc)
                                                            {
                                                                var maniC = new T_CA_Phase_Mani();
                                                                maniC.OrderIndex = acc.OrderIndex;
                                                                maniC.ManipulationId = acc.ManipulationId;
                                                                maniC.ManipulationCode = acc.ManipulationCode;
                                                                maniC.ManipulationName = acc.ManipulationName;
                                                                maniC.TMUEquipment = acc.TMUEquipment;
                                                                maniC.TMUManipulation = acc.TMUManipulation;
                                                                maniC.Loop = acc.Loop;
                                                                maniC.TotalTMU = acc.TotalTMU;
                                                                maniC.CreatedUser = actionUserId;
                                                                maniC.CreatedDate = new_commoAnaPhase.CreatedDate;
                                                                maniC.T_CA_Phase = new_commoAnaPhase;
                                                                new_commoAnaPhase.T_CA_Phase_Mani.Add(maniC);
                                                            }
                                                        }

                                                        new_commoAnaPhase.T_CommodityAnalysis = new_commoAna;
                                                        // new_commoAnaPhase.ParentId = parentid;
                                                        new_commoAnaPhase.Node = new_commoAna.Node + new_commoAnaPhase.T_CommodityAnalysis.Id + ",";
                                                        new_commoAnaPhase.CreatedUser = actionUserId;
                                                        new_commoAnaPhase.CreatedDate = new_commoAna.CreatedDate;
                                                        db.T_CA_Phase.Add(new_commoAnaPhase);
                                                        db.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }



                                scope.Complete();
                                result.IsSuccess = true;
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại.\nVui lòng kiểm tra lại.!" });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Copy_CommoAnaPhaseGroupContents(int fromId, int toId, int actionUserId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var commoAna = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == toId && x.ObjectType == (int)eObjectType.isPhaseGroup);
                    if (commoAna != null)
                    {
                        //ktra nhóm cong doan có còn tồn tại hay không
                        var objCopy = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.Id == fromId && x.ObjectType == (int)eObjectType.isPhaseGroup);
                        if (objCopy != null)
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                var commo_ana_phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == fromId).OrderBy(x => x.CreatedDate);
                                if (commo_ana_phases != null && commo_ana_phases.Count() > 0)
                                {
                                    var now = DateTime.Now;
                                    foreach (var item in commo_ana_phases)
                                    {
                                        var new_commoAnaPhase = new T_CA_Phase();
                                        new_commoAnaPhase.Index = item.Index;
                                        new_commoAnaPhase.Name = item.Name;
                                        new_commoAnaPhase.Code = item.Code;
                                        new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                        new_commoAnaPhase.Description = item.Description;
                                        new_commoAnaPhase.EquipmentId = item.EquipmentId;
                                        new_commoAnaPhase.PhaseGroupId = item.PhaseGroupId;
                                        new_commoAnaPhase.WorkerLevelId = item.WorkerLevelId;
                                        new_commoAnaPhase.ParentId = toId;
                                        new_commoAnaPhase.TotalTMU = item.TotalTMU;
                                        new_commoAnaPhase.ApplyPressuresId = item.ApplyPressuresId;
                                        new_commoAnaPhase.PercentWasteEquipment = item.PercentWasteEquipment;
                                        new_commoAnaPhase.PercentWasteManipulation = item.PercentWasteManipulation;
                                        new_commoAnaPhase.PercentWasteMaterial = item.PercentWasteMaterial;
                                        new_commoAnaPhase.PercentWasteSpecial = item.PercentWasteSpecial;
                                        new_commoAnaPhase.Node = item.Node;
                                        new_commoAnaPhase.Video = item.Video;
                                        new_commoAnaPhase.CreatedUser = actionUserId;
                                        new_commoAnaPhase.CreatedDate = now;
                                        // step 3 - copy active timeprepare                                            
                                        var listTimePrepareExist = db.T_CA_Phase_TimePrepare.Where(c => c.Commo_Ana_PhaseId == item.Id && !c.IsDeleted).ToList();
                                        if (listTimePrepareExist.Count > 0)
                                        {
                                            var listTimePrepareNew = new Collection<T_CA_Phase_TimePrepare>();
                                            foreach (var timePrepare in listTimePrepareExist)
                                            {
                                                listTimePrepareNew.Add(new T_CA_Phase_TimePrepare()
                                                {
                                                    Commo_Ana_PhaseId = item.Id,
                                                    TimePrepareId = timePrepare.TimePrepareId,
                                                    CreatedUser = actionUserId,
                                                    CreatedDate = new_commoAnaPhase.CreatedDate
                                                });
                                            }
                                            new_commoAnaPhase.T_CA_Phase_TimePrepare = listTimePrepareNew;
                                        }
                                        //check         // step 4  - copy active manipulation version
                                        var phaseAcc = db.T_CA_Phase_Mani.Where(x => !x.IsDeleted && x.CA_PhaseId == item.Id).ToList();
                                        if (phaseAcc != null && phaseAcc.Count() > 0)
                                        {
                                            new_commoAnaPhase.T_CA_Phase_Mani = new Collection<T_CA_Phase_Mani>();
                                            foreach (var acc in phaseAcc)
                                            {
                                                var maniC = new T_CA_Phase_Mani();
                                                maniC.OrderIndex = acc.OrderIndex;
                                                maniC.ManipulationId = acc.ManipulationId;
                                                maniC.ManipulationCode = acc.ManipulationCode;
                                                maniC.ManipulationName = acc.ManipulationName;
                                                maniC.TMUEquipment = acc.TMUEquipment;
                                                maniC.TMUManipulation = acc.TMUManipulation;
                                                maniC.Loop = acc.Loop;
                                                maniC.TotalTMU = acc.TotalTMU;
                                                maniC.CreatedUser = actionUserId;
                                                maniC.CreatedDate = new_commoAnaPhase.CreatedDate;
                                                maniC.T_CA_Phase = new_commoAnaPhase;
                                                new_commoAnaPhase.T_CA_Phase_Mani.Add(maniC);
                                            }
                                        }

                                        new_commoAnaPhase.Node = commoAna.Node + commoAna.Id + ",";
                                        new_commoAnaPhase.CreatedUser = actionUserId;
                                        new_commoAnaPhase.CreatedDate = now;
                                        db.T_CA_Phase.Add(new_commoAnaPhase);
                                        db.SaveChanges();
                                    }
                                }
                                scope.Complete();
                                result.IsSuccess = true;
                            }

                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại.\nVui lòng kiểm tra lại.!" });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelSelectItem> GetSelectItems(int type, int parentId)
        {
            var objs = new List<ModelSelectItem>();
            try
            {
                using (var db = new IEDEntities())
                {
                    IQueryable<T_CommodityAnalysis> _iObjs = null;
                    switch (type)
                    {
                        case (int)eObjectType.isCommodity:
                            _iObjs = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity); break;
                        case (int)eObjectType.isWorkShop:
                            _iObjs = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isWorkShop && x.ParentId == parentId); break;
                        case (int)eObjectType.isPhaseGroup:
                            var _thanhPhanObj = db.T_CommodityAnalysis.FirstOrDefault(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isComponent && x.ParentId == parentId);
                            if (_thanhPhanObj != null)
                                _iObjs = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isPhaseGroup && x.ParentId == _thanhPhanObj.Id);
                            break;
                    }
                    if (_iObjs != null)
                        objs.AddRange(_iObjs.Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name, Data = x.ObjectId }));

                    if (type == (int)eObjectType.isPhaseGroup && objs.Count > 0)
                    {
                        var _phaseGroupIds = objs.Select(x => x.Data).ToList();
                        var _commoIds = objs.Select(x => x.Value).ToList();
                        //lay san luong
                        var _phaseGroupPro = db.T_PhaseGroupDailyProduction
                            .Where(x => !x.IsDeleted && _phaseGroupIds.Contains(x.PhaseGroupId) && _commoIds.Contains(x.ComAnaId))
                            .Select(x => new
                            {
                                Date = x.Date,
                                CreatedDate = x.CreatedDate,
                                PhaseGroupId = x.PhaseGroupId,
                                CommoId = x.ComAnaId,
                                CType = x.ComandType,
                                Quantities = x.Quantities
                            }).ToList();
                        int _increase = 0, _reduce = 0;
                        for (int i = 0; i < objs.Count; i++)
                        {
                            _increase = _phaseGroupPro.Where(x => x.PhaseGroupId == objs[i].Data && x.CommoId == objs[i].Value && x.CType == (int)eCommandType.Increase).Sum(x => x.Quantities);
                            _reduce = 0; _phaseGroupPro.Where(x => x.PhaseGroupId == objs[i].Data && x.CommoId == objs[i].Value && x.CType == (int)eCommandType.Reduce).Sum(x => x.Quantities);
                            objs[i].Double = (_increase - _reduce);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objs;
        }

        public ResponseBase GetSampleProAna(int productId, int[] workshopIds)
        {
            ResponseBase rs = new ResponseBase();
            rs.IsSuccess = false;

            using (var db = new IEDEntities())
            {
                var sampleProAna = db.T_CommodityAnalysis
                     .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isCommodity && x.ObjectId == productId)
                     .Select(x => new SampleProAna()
                     {
                         Id = x.Id,
                         ObjectId = x.ObjectId,
                         ObjectType = x.ObjectType,
                         Name = x.Name,
                         Note = x.Node,
                         Node = x.Node,
                         ParentId = x.ParentId
                     }).FirstOrDefault();
                if (sampleProAna != null)
                {
                    if (workshopIds == null)
                        workshopIds = new int[] { };
                    //phan xuong
                    sampleProAna.Details.AddRange(db.T_CommodityAnalysis
                    .Where(x => !x.IsDeleted && x.ObjectType == (int)eObjectType.isWorkShop && x.ParentId == sampleProAna.Id && workshopIds.Contains(x.ObjectId))
                    .Select(x => new SampleProAna()
                    {
                        Id = x.Id,
                        ObjectId = x.ObjectId,
                        ObjectType = x.ObjectType,
                        Name = x.Name,
                        Note = x.Description,
                        Node = x.Node,
                        ParentId = x.ParentId
                    }).ToList());
                    if (sampleProAna.Details.Count > 0)
                    {
                        //tkc - qtcn - tp
                        var _ids = sampleProAna.Details.Select(x => x.Id);
                        var tkc_qtcn_tps = db.T_CommodityAnalysis
                                        .Where(x => !x.IsDeleted && _ids.Contains(x.ParentId))
                                        .Select(x => new SampleProAna()
                                        {
                                            Id = x.Id,
                                            ParentId = x.ParentId,
                                            ObjectId = x.ObjectId,
                                            ObjectType = x.ObjectType,
                                            Name = x.Name,
                                            Note = x.Description,
                                            Node = x.Node
                                        }).ToList();

                        // cum cd
                        _ids = tkc_qtcn_tps.Where(x => x.ObjectType == (int)eObjectType.isComponent).Select(x => x.Id);
                        var _phaseGroups = db.T_CommodityAnalysis
                                        .Where(x => !x.IsDeleted && _ids.Contains(x.ParentId))
                                        .Select(x => new SampleProAna()
                                        {
                                            Id = x.Id,
                                            ObjectId = x.ObjectId,
                                            ParentId = x.ParentId,
                                            Name = x.Name,
                                            Note = x.Description,
                                            Node = x.Node,
                                            ObjectType = x.ObjectType,
                                        }).ToList();

                        for (int i = 0; i < sampleProAna.Details.Count; i++)
                        {
                            var _id = sampleProAna.Details[i].Id;

                            var _components = tkc_qtcn_tps.Where(x => x.ParentId == _id).ToList();

                            var _tp = _components.FirstOrDefault(x => x.ObjectType == (int)eObjectType.isComponent);
                            if (_tp != null)
                            {
                                _tp.Details.AddRange(_phaseGroups.Where(x => x.ParentId == _tp.Id).ToList());
                            }
                            sampleProAna.Details[i].Details.AddRange(_components);
                        }

                    }

                    rs.IsSuccess = true;
                    rs.Records = sampleProAna;
                }
            }
            return rs;
        }
    }
}
