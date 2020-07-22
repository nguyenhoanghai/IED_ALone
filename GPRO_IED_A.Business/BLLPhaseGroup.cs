using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLPhaseGroup
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLPhaseGroup _Instance;
        public static BLLPhaseGroup Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhaseGroup();

                return _Instance;
            }
        }
        private BLLPhaseGroup() { }
        #endregion

        bool checkPermis(T_PhaseGroup obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public PagedList<PhaseGroupModel> GetList(string keyWord, int searchBy, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                    {
                        sorting = "CreatedDate DESC";
                    }
                    IQueryable<T_PhaseGroup> phaseGroups = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        keyWord = keyWord.Trim().ToUpper();
                        switch (searchBy)
                        {
                            case 1:
                                phaseGroups = db.T_PhaseGroup.Where(c => !c.IsDeleted && c.Name.Trim().ToUpper().Contains(keyWord));
                                break;
                            case 2:
                                phaseGroups = db.T_PhaseGroup.Where(c => !c.IsDeleted && c.Code.Trim().ToUpper().Contains(keyWord));
                                break;
                        }
                    }
                    else
                        phaseGroups = db.T_PhaseGroup.Where(c => !c.IsDeleted);

                    if (phaseGroups != null && phaseGroups.Count() > 0)
                    {
                        var list = phaseGroups.OrderByDescending(x => x.CreatedDate).Select(x => new PhaseGroupModel()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            MinLevel = x.MinLevel,
                            MaxLevel = x.MaxLevel,
                            Description = x.Description
                        }).ToList();
                        return new PagedList<PhaseGroupModel>(list, pageNumber, pageSize);
                    }
                    else
                        return new PagedList<PhaseGroupModel>(new List<PhaseGroupModel>(), pageNumber, pageSize);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(PhaseGroupModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    bool flag = false;
                    T_PhaseGroup obj = null;
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, true))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên Cụm Công Đoạn này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(model.Code))
                    {
                        if (CheckExists(model.Code.Trim().ToUpper(), model.Id, false))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Insert", Message = "Mã Cụm Công Đoạn này đã tồn tại. Vui lòng chọn lại Mã khác !." });
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        if (model.Id == 0)
                        {
                            obj = new T_PhaseGroup();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_PhaseGroup.Add(obj);
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.T_PhaseGroup.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                if (!checkPermis(obj, model.ActionUser,isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo Cụm Công Đoạn này nên bạn không cập nhật được thông tin cho Cụm Công Đoạn này." });
                                }
                                else
                                {
                                    obj.Name = model.Name;
                                    obj.Code = model.Code;
                                    obj.MinLevel = model.MinLevel;
                                    obj.MaxLevel = model.MaxLevel;
                                    obj.Description = model.Description;
                                    obj.UpdatedUser = model.ActionUser;
                                    obj.UpdatedDate = DateTime.Now;

                                    //  cap nhat ben phan tich mat hang
                                    var commoAna = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectId == obj.Id && x.ObjectType == (int)eObjectType.isPhaseGroup);
                                    if (commoAna != null && commoAna.Count() > 0)
                                    {
                                        foreach (var item in commoAna)
                                        {
                                            item.Name = model.Name;
                                            item.UpdatedUser = model.ActionUser;
                                            item.UpdatedDate = DateTime.Now;

                                        }
                                    }
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

        private bool CheckExists(string keyword, int PhaseGroupId, bool isCheckName)
        {
            try
            {
                T_PhaseGroup phaseGroup = null;
                if (isCheckName)
                    phaseGroup = db.T_PhaseGroup.FirstOrDefault(x => !x.IsDeleted && x.Name.Trim().ToUpper().Equals(keyword) && x.Id != PhaseGroupId);
                else
                    phaseGroup = db.T_PhaseGroup.FirstOrDefault(x => !x.IsDeleted && x.Code.Trim().ToUpper().Equals(keyword) && x.Id != PhaseGroupId);
                if (phaseGroup == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Delete(int id, int acctionUserId, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var phasegroup = db.T_PhaseGroup.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (phasegroup == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete ", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        if (!checkPermis(phasegroup, acctionUserId,isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete", Message = "Bạn không phải là người tạo Cụm Công Đoạn này nên bạn không xóa được Cụm Công Đoạn này." });
                        }
                        else
                        {
                            phasegroup.IsDeleted = true;
                            phasegroup.DeletedUser = acctionUserId;
                            phasegroup.DeletedDate = DateTime.Now;
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

        public List<ModelSelectItem> Gets()
        {
            using (db = new IEDEntities())
            {
                List<ModelSelectItem> objs = new List<ModelSelectItem>();
                objs.Add(new ModelSelectItem() { Value = 0, Name = " - Chọn Cụm Công Đoạn - " });
                try
                {
                    objs.AddRange(db.T_PhaseGroup.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objs;
            }
        }

    }
}
