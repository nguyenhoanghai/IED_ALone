using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Hugate.Framework;
using PagedList;
using System;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLTimePrepare
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLTimePrepare _Instance;
        public static BLLTimePrepare Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLTimePrepare();

                return _Instance;
            }
        }
        private BLLTimePrepare() { }
        #endregion

        bool checkPermis(T_TimePrepare obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }
        public ResponseBase InsertOrUpdate(TimePrepareModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    bool flag = false;
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, true, db))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert  ", Message = "Tên này đã được sử dụng. Vui lòng chọn lại Tên khác !." });
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(model.Code))
                    {
                        if (CheckExists(model.Code.Trim().ToUpper(), model.Id, false, db))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Insert  ", Message = "Mã này đã được sử dụng. Vui lòng chọn lại Mã khác !." });
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        T_TimePrepare obj = null;
                        if (model.Id == 0)
                        {
                            obj = new T_TimePrepare();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_TimePrepare.Add(obj);
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.T_TimePrepare.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                if (!checkPermis(obj, model.ActionUser, isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo thời gian chuẩn bị này nên bạn không cập nhật được thông tin cho thời gian chuẩn bị này." });
                                }
                                else
                                {
                                    obj.Name = model.Name;
                                    obj.Code = model.Code;
                                    obj.TimeTypePrepareId = model.TimeTypePrepareId;
                                    obj.TMUNumber = model.TMUNumber;
                                    obj.Description = model.Description;
                                    obj.UpdatedUser = model.ActionUser;
                                    obj.UpdatedDate = DateTime.Now;
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

        private bool CheckExists(string keyword, int id, bool isCheckName, IEDEntities db)
        {
            try
            {
                T_TimePrepare timeType = null;
                keyword = keyword.Trim().ToUpper();
                if (isCheckName)
                    timeType = db.T_TimePrepare.FirstOrDefault(x => !x.IsDeleted && x.Id != id && x.Name.Trim().ToUpper().Equals(keyword));
                else
                    timeType = db.T_TimePrepare.FirstOrDefault(x => !x.IsDeleted && x.Id != id && x.Code.Trim().ToUpper().Equals(keyword));

                if (timeType == null)
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
                    var timeType = db.T_TimePrepare.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (timeType == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete  ", Message = "Dữ liệu bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        if (!checkPermis(timeType, acctionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete", Message = "Bạn không phải là người tạo thời gian chuẩn bị này nên bạn không xóa được thời gian chuẩn bị này." });
                        }
                        else
                        {
                            timeType.IsDeleted = true;
                            timeType.DeletedUser = acctionUserId;
                            timeType.DeletedDate = DateTime.Now;
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

        public PagedList<TimePrepareModel> Gets(int timeTypeId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    var timeTypes = db.T_TimePrepare.Where(x => !x.IsDeleted && x.TimeTypePrepareId == timeTypeId && !x.T_TimeTypePrepare.IsDeleted).OrderByDescending(x => x.CreatedDate).
                         Select(x => new TimePrepareModel()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Code = x.Code,
                             TimeTypePrepareId = x.TimeTypePrepareId,
                             TimeTypePrepareName = x.T_TimeTypePrepare.Name,
                             TMUNumber = x.TMUNumber,
                             Description = x.Description,
                         }).OrderBy(sorting).ToList();
                    return new PagedList<TimePrepareModel>(timeTypes, pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedList<TimePrepareModel> Gets(string keyword, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "CreatedDate DESC";

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    IQueryable<T_TimePrepare> objs = db.T_TimePrepare.Where(x => !x.IsDeleted && !x.T_TimeTypePrepare.IsDeleted);
                    if (!string.IsNullOrEmpty(keyword))
                        objs = objs.Where(x => x.Code.Trim().ToUpper().Contains(keyword) || x.Name.Trim().ToUpper().Contains(keyword));
                    
                    return new PagedList<TimePrepareModel>(objs.Select(x => new TimePrepareModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        TimeTypePrepareId = x.TimeTypePrepareId,
                        TimeTypePrepareName = x.T_TimeTypePrepare.Name,
                        TMUNumber = x.TMUNumber,
                        Description = x.Description,
                        CreatedDate = x.CreatedDate
                    }).OrderBy(sorting).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
