using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PagedList<ProductModel> GetList(string keyWord, string searchBy, int companyId, int[] relationCompanyId, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "CreatedDate DESC";

                    List<ProductModel> productTypes = null;
                    if (string.IsNullOrEmpty(keyWord))
                        productTypes = GetAll(sorting, companyId, relationCompanyId);
                    else
                        productTypes = GetByKeyword(keyWord, searchBy, companyId, relationCompanyId, sorting);

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<ProductModel>(productTypes, pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductModel> GetByKeyword(string keyWord, string searchBy, int companyId, int[] relationCompanyId, string sorting)
        {
            try
            {
                List<ProductModel> productTypes = null;
                switch (searchBy)
                {
                    case "1":
                        productTypes = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)) && x.Code.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())).OrderByDescending(x => x.CreatedDate).Select(
                            x => new ProductModel()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code,
                                Description = x.Description,
                                IsPrivate = (x.CompanyId == null ? true :false ),
                                CompanyId = x.CompanyId
                            }).ToList();
                        break;
                    case "2":
                        productTypes = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0)) && x.Name.Trim().ToUpper().Contains(keyWord.Trim().ToUpper())).OrderByDescending(x => x.CreatedDate).Select(
                            x => new ProductModel()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code,
                                Description = x.Description,
                                IsPrivate = (x.CompanyId == null ?  true:false ),
                                CompanyId = x.CompanyId
                            }).ToList();
                        break;
                }
                if (productTypes != null && productTypes.Count > 0)
                    return productTypes;
                return new List<ProductModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductModel> GetAll(string sorting, int companyId, int[] relationCompanyId)
        {
            try
            {
                var productTypes = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0))).OrderByDescending(x => x.CreatedDate).Select(
                    x => new ProductModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        CompanyId = x.CompanyId,
                        IsPrivate = (x.CompanyId == null ? true : false)
                    }).ToList();
                if (productTypes != null && productTypes.Count > 0)
                    return productTypes;
                return new List<ProductModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(ProductModel model)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.Name.Trim().ToUpper(), model.Id, model.CompanyId , true))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert Product Type", Message = "Tên  Sản Phẩm này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Code))
                        {
                            if (CheckExists(model.Code.Trim().ToUpper(), model.Id, model.CompanyId , false))
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Insert Product Type", Message = "Mã  Sản Phẩm này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                                return result;
                            }
                        }
                        T_Product obj;
                        if (model.Id == 0)
                        {
                            obj = new T_Product();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_Product.Add(obj);
                        }
                        else
                        {
                            obj = db.T_Product.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj == null)
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update Product Type", Message = "Loại Sản Phẩm bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                                return result;
                            }
                            else
                            {
                                obj.CompanyId = model.CompanyId;
                                obj.Name = model.Name;
                                obj.Code = model.Code;
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
                            }
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;
                        return result;
                    }
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

        public ResponseBase Delete(int id, int acctionUserId)
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
                        result.Errors.Add(new Error() { MemberName = "Delete Product Type", Message = "Loại Sản Phẩm bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                    }
                    else
                    {
                        productType.IsDeleted = true;
                        productType.DeletedUser = acctionUserId;
                        productType.DeletedDate = DateTime.Now;
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
        public List<ModelSelectItem> GetSelectItem(int companyId, int[] relationCompanyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var listModelSelect = new List<ModelSelectItem>();
                    var productTypes = db.T_Product.Where(x => !x.IsDeleted && (x.CompanyId == null || x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId ?? 0))).Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.Name
                        }).ToList();

                    if (productTypes != null && productTypes.Count() > 0)
                    {
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn Sản Phẩm  - - " });
                        listModelSelect.AddRange(productTypes);
                    }
                    else
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = "  Không có Sản Phẩm  " });
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
