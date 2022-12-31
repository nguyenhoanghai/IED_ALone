using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using Hugate.Framework;

namespace GPRO_IED_A.Business
{
    public class BLLApprover
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLApprover _Instance;
        public static BLLApprover Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLApprover();

                return _Instance;
            }
        }
        private BLLApprover() { }
        #endregion

        public PagedList<ApproverModel> GetList(string keyword, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    var _custs = db.SApprovers.AsQueryable();
                    if (!string.IsNullOrEmpty(keyword))
                        _custs = _custs.Where(x => x.SUser.UserName.Trim().ToUpper().Contains(keyword.Trim().ToUpper()));

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<ApproverModel>(_custs.OrderBy(sorting)
                        .Select(
                           x => new ApproverModel()
                           {
                               Id = x.Id,
                               UserId = x.UserId,
                               UserName = x.SUser.UserName +" ("+ x.SUser.Name+")",
                               ApproveRoles = x.ApproveRoles
                           }).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(ApproverModel model )
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.Id, model.UserId))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert Approver Type", Message = "Tài khoản này đã tồn tại. Vui lòng chọn lại tài khoản khác !." });
                        return result;
                    }
                    else
                    {
                        SApprover obj;
                        if (model.Id == 0)
                        {
                            obj = new SApprover();
                            Parse.CopyObject(model, ref obj);
                            db.SApprovers.Add(obj);
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.SApprovers.FirstOrDefault(x => x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update Approver Type", Message = "Tài khoản bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                obj.ApproveRoles = model.ApproveRoles;
                                db.SaveChanges();
                                result.IsSuccess = true;
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckExists(int id, int userId)
        {
            try
            {
                SApprover obj = null;
                obj = db.SApprovers.FirstOrDefault(x => x.UserId == userId && x.Id != id);

                if (obj == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Delete(int id )
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var obj = db.SApprovers.FirstOrDefault(x => x.Id == id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Approver Type", Message = "Tài khoản bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        db.SApprovers.Remove(obj);
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

        public ModelSelectItem Get(int userId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    return db.SApprovers.Where(x => x.UserId == userId).Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.UserId,
                            Name = x.ApproveRoles
                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
