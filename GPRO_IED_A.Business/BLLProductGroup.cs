using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Hugate.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLProductGroup
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLProductGroup _Instance;
        public static BLLProductGroup Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLProductGroup();

                return _Instance;
            }
        }
        private BLLProductGroup() { }
        #endregion
        bool checkPermis(T_ProductGroup obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public PagedList<ProductGroupModel> GetList(string keyWord,  int companyId, int[] relationCompanyId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    IQueryable<T_ProductGroup> objs = null;
                    if (string.IsNullOrEmpty(keyWord))
                        objs = db.T_ProductGroup.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)) ).OrderByDescending(x => x.CreatedDate);
                    else
                        objs = db.T_ProductGroup.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)) && x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())).OrderByDescending(x => x.CreatedDate);

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<ProductGroupModel>(objs.Select(x => new ProductGroupModel()
                    {
                        Id = x.Id,
                        Name = x.Name, 
                        Description = x.Description,
                        IsPrivate = (x.CompanyId == null ? true : false),
                        CompanyId = x.CompanyId 
                    }).OrderBy(sorting).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public ResponseBase InsertOrUpdate(ProductGroupModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.CompanyId ))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert", Message = "Tên nhóm mã hàng này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }
                    else
                    { 
                        T_ProductGroup obj;
                        if (model.Id == 0)
                        {
                            obj = new T_ProductGroup();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_ProductGroup.Add(obj);
                            db.SaveChanges();
                            result.IsSuccess = true;
                        }
                        else
                        {
                            obj = db.T_ProductGroup.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update Product Type", Message = "Nhóm mã hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                if (!checkPermis(obj, model.ActionUser, isOwner))
                                {
                                    result.IsSuccess = false;
                                    result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo nhóm mã hàng này nên bạn không cập nhật được thông tin cho mã hàng này." });
                                }
                                else
                                {
                                    obj.CompanyId = model.CompanyId;
                                    obj.Name = model.Name; 
                                    obj.Description = model.Description;
                                    obj.UpdatedUser = model.ActionUser;
                                    obj.UpdatedDate = DateTime.Now;
                                     
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

        private bool CheckExists(string code, int? id, int? companyId )
        {
            try
            {
                T_ProductGroup obj = null; 
                    obj = db.T_ProductGroup.FirstOrDefault(x => !x.IsDeleted && x.CompanyId == companyId && x.Name.Trim().ToUpper().Equals(code) && x.Id != id);

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
                    var productType = db.T_ProductGroup.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (productType == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Product Type", Message = "Loại nhóm mã hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        if (!checkPermis(productType, acctionUserId, isOwner))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete", Message = "Bạn không phải là người tạo mã hàng này nên bạn không xóa được mã hàng này." });
                        }
                        else
                        {
                            productType.IsDeleted = true;
                            productType.DeletedUser = acctionUserId;
                            productType.DeletedDate = DateTime.Now;

                            var proanas = (from x in db.T_CommodityAnalysis where !x.IsDeleted && x.ObjectType == 1 && x.ObjectId == productType.Id select x);
                            if (proanas != null && proanas.Count() > 0)
                                foreach (var item in proanas)
                                {
                                    item.IsDeleted = true;
                                    item.DeletedUser = acctionUserId;
                                    item.DeletedDate = DateTime.Now;
                                }

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
                    var listModelSelect = new List<ModelSelectItem>();
                    var productTypes = db.T_ProductGroup.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)))
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    //if (productTypes != null && productTypes.Count() > 0)
                    //{
                    //    listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn nhóm mã hàng  - - " });
                       listModelSelect.AddRange(productTypes);
                    //}
                    //else
                    //    listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = "  Không có nhóm mã hàng  " });
                    return listModelSelect;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ModelSelectItem> GetSelectItem(int productGroupId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var listModelSelect = new List<ModelSelectItem>();
                    var productTypes = db.T_ProductGroup.Where(x => !x.IsDeleted && x.Id == productGroupId)
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    //if (productTypes != null && productTypes.Count() > 0)
                    //{
                    //    listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn nhóm mã hàng  - - " });
                    listModelSelect.AddRange(productTypes);
                    //}
                    //else
                    //    listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = "  Không có nhóm mã hàng  " });
                    return listModelSelect;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
