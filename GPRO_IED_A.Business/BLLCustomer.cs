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
    public class BLLCustomer
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLCustomer _Instance;
        public static BLLCustomer Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCustomer();

                return _Instance;
            }
        }
        private BLLCustomer() { }
        #endregion

        public PagedList<CustomerModel> GetList(string keyWord, int companyId, int[] relationCompanyId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    var _custs = db.T_Customer.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)));
                    if (!string.IsNullOrEmpty(keyWord))
                        _custs = _custs.Where(x => x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()));
                     
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<CustomerModel>(_custs.OrderBy(sorting)
                        .Select(
                           x => new CustomerModel()
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Description = x.Description,
                               IsPrivate = (x.CompanyId == null ? true : false),
                               CompanyId = x.CompanyId
                           }).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public ResponseBase InsertOrUpdate(CustomerModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.CompanyId))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert Customer Type", Message = "Khách hàng này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }
                    else
                    {
                        T_Customer obj;
                        if (model.Id == 0)
                        {
                            obj = new T_Customer();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_Customer.Add(obj);
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.T_Customer.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update Customer Type", Message = "Khách hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                if (!checkPermis(obj, model.ActionUser, isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo khách hàng này nên bạn không cập nhật được thông tin cho khách hàng này." });
                                }
                                else
                                {
                                    obj.CompanyId = model.CompanyId;
                                    obj.Name = model.Name;
                                    obj.Description = model.Description;
                                    obj.UpdatedUser = model.ActionUser;
                                    obj.UpdatedDate = DateTime.Now;

                                    //  cap nhat ben phan tich mat hang
                                    var commoAna = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectId == obj.Id && x.ObjectType == (int)eObjectType.isCommodity);
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

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckExists(string code, int? id, int? companyId)
        {
            try
            {
                T_Customer obj = null;
                obj = db.T_Customer.FirstOrDefault(x => !x.IsDeleted && x.CompanyId == companyId && x.Name.Trim().ToUpper().Equals(code) && x.Id != id);

                if (obj == null)
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
                    var obj = db.T_Customer.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Customer Type", Message = "Khách hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        if (!checkPermis(obj, acctionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete", Message = "Bạn không phải là người tạo khách hàng này nên bạn không xóa được khách hàng này." });
                        }
                        else
                        {
                            obj.IsDeleted = true;
                            obj.DeletedUser = acctionUserId;
                            obj.DeletedDate = DateTime.Now;
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

        public List<ModelSelectItem> GetSelectItem(int companyId, int[] relationCompanyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var objs = new List<ModelSelectItem>();
                    var CustomerTypes = db.T_Customer.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0))).Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    if (CustomerTypes != null && CustomerTypes.Count() > 0)
                    {
                        objs.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn khách hàng  - - " });
                        objs.AddRange(CustomerTypes);
                    }
                    else
                        objs.Add(new ModelSelectItem() { Value = 0, Name = "  Không có khách hàng  " });
                    return objs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool checkPermis(T_Customer obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }
    }
}
