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
    public class BLLCompany
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLCompany _Instance;
        public static BLLCompany Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLCompany();

                return _Instance;
            }
        }
        private BLLCompany() { }
        #endregion

        public PagedList<CompanyModel> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "CreatedDate deSC";

                    var objs = db.SCompanies.Where(x => !x.IsDeleted);
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = objs.Where(x => x.CompanyName.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()));

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<CompanyModel>(objs
                        .Select(
                           x => new CompanyModel()
                           {
                               Id = x.Id,
                               CompanyName = x.CompanyName,
                               LevelCompanyId = x.LevelCompanyId,
                               LevelName = x.SLevelCompany.LevelName,
                               Address = x.Address,
                               Telephone = x.Telephone,
                               TaxCode = x.TaxCode,
                               ParentId = x.ParentId,
                               ParentNode = x.ParentNode,
                               Logo = x.Logo,
                               CreatedDate = x.CreatedDate
                           }).OrderBy(sorting).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(CompanyModel model)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }

                    SCompany obj;
                    if (model.Id == 0)
                    {
                        obj = new SCompany();
                        Parse.CopyObject(model, ref obj);
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedUser = model.ActionUser;
                        db.SCompanies.Add(obj);
                        db.SaveChanges();
                        result.IsSuccess = true;
                        return result;
                    }

                    obj = db.SCompanies.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Update  ", Message = "Công ty bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                        return result;
                    }

                    obj.LevelCompanyId = model.LevelCompanyId;
                    obj.CompanyName = model.CompanyName;
                    obj.TaxCode = model.TaxCode;
                    obj.Telephone = model.Telephone;
                    obj.Address = model.Address;
                    obj.UpdatedUser = model.ActionUser;
                    obj.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckExists(CompanyModel model)
        {
            try
            {
                SCompany obj = null;
                obj = db.SCompanies.FirstOrDefault(x => !x.IsDeleted && x.CompanyName.Trim().ToUpper().Equals(model.CompanyName) && x.Id != model.Id && x.LevelCompanyId == model.LevelCompanyId);

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
                    var obj = db.SCompanies.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete ", Message = "Công ty bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                        return result;
                    }
                    obj.IsDeleted = true;
                    obj.DeletedUser = acctionUserId;
                    obj.DeletedDate = DateTime.Now;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelSelectItem> GetSelectItem()
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var objs = new List<ModelSelectItem>();
                    var CustomerTypes = db.SCompanies.Where(x => !x.IsDeleted).Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.CompanyName
                        }).ToList();

                    if (CustomerTypes != null && CustomerTypes.Count() > 0)
                    {
                        objs.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn công tỵ  - - " });
                        objs.AddRange(CustomerTypes);
                    }
                    else
                        objs.Add(new ModelSelectItem() { Value = 0, Name = "  Không có công ty  " });
                    return objs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
