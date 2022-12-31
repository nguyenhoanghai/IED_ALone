using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Hugate.Framework;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLProduct
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLProduct _Instance;
        public static BLLProduct Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLProduct();

                return _Instance;
            }
        }
        private BLLProduct() { }
        #endregion
        bool checkPermis(T_Product obj, int actionUser, bool isOwner)
        {
            if (isOwner) return true;
            return obj.CreatedUser == actionUser;
        }

        public PagedList<ProductModel> GetList(string keyWord, int companyId, int[] relationCompanyId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "Id DESC";

                    IQueryable<T_Product> _products = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)));
                    if (!string.IsNullOrEmpty(keyWord))
                        _products = _products.Where(x => (x.Code.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()) || x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())));

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    var objs = new PagedList<ProductModel>(_products.Select(x => new ProductModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.T_Customer.Name,
                        Description = x.Description,
                        IsPrivate = (x.CompanyId == null ? true : false),
                        CompanyId = x.CompanyId,
                        CustomerId = x.CustomerId,
                        ProductGroupId = x.ProductGroupId ?? 0,
                        ProGroupName = (x.ProductGroupId.HasValue ? x.T_ProductGroup.Name : "")
                    }).OrderBy(sorting).ToList(), pageNumber, pageSize);
                    if (objs.Count > 0)
                    {
                        var ids = objs.Select(x => x.Id).ToList();
                        var files = db.T_ProductFile
                            .Where(x => !x.IsDeleted && ids.Contains(x.ProductId))
                            .Select(x => new ModelSelectItem() { Value = x.Id, Name = x.FileName, Code = x.Path, Data = x.ProductId })
                            .ToList();
                        for (int i = 0; i < objs.Count; i++)
                        {
                            objs[i].Files = files.Where(x => x.Data == objs[i].Id).ToList();
                        }
                    }
                    return objs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(ProductModel model, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.CompanyId, true))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert Product Type", Message = "Tên mã hàng này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }
                    if (model.CustomerId == 0)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert Product Type", Message = "Khách hàng không được để trống.!" });
                        return result;
                    }

                    T_Product obj;
                    if (model.ProductGroupId == 0)
                        model.ProductGroupId = null;
                    if (model.Id == 0)
                    {
                        obj = new T_Product();
                        Parse.CopyObject(model, ref obj);
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedUser = model.ActionUser;
                        db.T_Product.Add(obj);
                        db.SaveChanges();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        obj = db.T_Product.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (obj == null)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Update Product Type", Message = "Loại mã hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                            return result;
                        }
                        else
                        {
                            if (!checkPermis(obj, model.ActionUser, isOwner))
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "update", Message = "Bạn không phải là người tạo mã hàng này nên bạn không cập nhật được thông tin cho mã hàng này." });
                            }
                            else
                            {
                                obj.CompanyId = model.CompanyId;
                                obj.Name = model.Name;
                                obj.Code = model.Code;
                                obj.CustomerId = model.CustomerId;
                                obj.ProductGroupId = model.ProductGroupId;
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

                    if (!string.IsNullOrEmpty(model.Img))
                    {
                        List<T_ProductFile> files = JsonConvert.DeserializeObject<List<T_ProductFile>>(model.Img);
                        if (files != null && files.Count > 0)
                        {
                            foreach (var file in files)
                            {
                                file.ProductId = obj.Id;
                                file.CreatedDate = DateTime.Now;
                                file.CreatedUser = model.ActionUser;
                                db.T_ProductFile.Add(file);
                            }
                            db.SaveChanges();
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

        private bool CheckExists(string code, int? id, int? companyId, bool isName)
        {
            try
            {
                T_Product obj = null;
                if (!isName)
                    obj = db.T_Product.FirstOrDefault(x => !x.IsDeleted && x.CompanyId == companyId && x.Code.Trim().ToUpper().Equals(code) && x.Id != id);
                else
                    obj = db.T_Product.FirstOrDefault(x => !x.IsDeleted && x.CompanyId == companyId && x.Name.Trim().ToUpper().Equals(code) && x.Id != id);

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
                    var productType = db.T_Product.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (productType == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Product Type", Message = "Loại mã hàng bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
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

                                    string _node = "0," + item.Id + ",";
                                    var _children = (from x in db.T_CommodityAnalysis where !x.IsDeleted && x.Node.Contains(_node) select x);
                                    if (_children != null && _children.Count() > 0)
                                    {
                                        foreach (var _child in _children)
                                        {
                                            _child.IsDeleted = true;
                                            _child.DeletedUser = acctionUserId;
                                            _child.DeletedDate = item.DeletedDate;

                                            if (_child.ObjectType == (int)eObjectType.isPhaseGroup)
                                            {
                                                var _phases = db.T_CA_Phase.Where(x => !x.IsDeleted && x.ParentId == _child.Id);
                                                if (_phases != null && _phases.Count() > 0)
                                                {
                                                    foreach (var _obj in _phases)
                                                    {
                                                        _obj.IsDeleted = true;
                                                        _obj.DeletedDate = item.DeletedDate;
                                                        _obj.DeletedUser = acctionUserId;
                                                    }
                                                }
                                            }
                                        }
                                    }
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

        public ResponseBase DeleteFile(int id, int acctionUserId, bool isOwner)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var obj = db.T_ProductFile.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Product Type", Message = "Hình ảnh đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        if (obj.CreatedUser == acctionUserId || isOwner)
                        {
                            obj.IsDeleted = true;
                            obj.DeletedUser = acctionUserId;
                            obj.DeletedDate = DateTime.Now;

                            db.SaveChanges();
                            result.IsSuccess = true;
                            if (File.Exists(obj.Path))
                                File.Delete(obj.Path);
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Delete", Message = "Bạn không phải là người tải ảnh này nên bạn không xóa được ảnh này." });
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
                    var productTypes = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)))
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    if (productTypes != null && productTypes.Count() > 0)
                    {
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn mã hàng  - - " });
                        listModelSelect.AddRange(productTypes);
                    }
                    else
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = "  Không có mã hàng  " });
                    return listModelSelect;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelSelectItem> GetSelectItem(int _objectId, int companyId, int[] relationCompanyId, bool findByCustomer)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var listModelSelect = new List<ModelSelectItem>();
                    var _objs = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)));
                    if (findByCustomer)
                        _objs = _objs.Where(x => x.CustomerId == _objectId);
                    if (!findByCustomer)
                        _objs = _objs.Where(x => x.ProductGroupId == _objectId);

                    var productTypes = _objs.OrderByDescending(x => x.CreatedDate)
                        .Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    if (productTypes != null && productTypes.Count() > 0)
                    {
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn mã hàng  - - " });
                        listModelSelect.AddRange(productTypes);
                    }
                    else
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = "  Không có mã hàng  " });
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
