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
    public class BLLLevelCompany
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLLevelCompany _Instance;
        public static BLLLevelCompany Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLevelCompany();

                return _Instance;
            }
        }
        private BLLLevelCompany() { }
        #endregion

        public PagedList<LevelCompanyModel> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "OrderIndex ASC";

                    var objs = db.SLevelCompanies.Where(x => !x.IsDeleted);
                    if (!string.IsNullOrEmpty(keyWord))
                        objs = objs.Where(x => x.LevelName.Trim().ToUpper().Contains(keyWord.Trim().ToUpper()));

                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<LevelCompanyModel>(objs.OrderBy(sorting)
                        .Select(
                           x => new LevelCompanyModel()
                           {
                               Id = x.Id,
                               LevelName = x.LevelName,
                               Description = x.Description,
                               OrderIndex = x.OrderIndex
                           }).ToList(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(LevelCompanyModel model)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    if (CheckExists(model.LevelName.Trim().ToUpper(), model.Id))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Insert ", Message = "Tên này đã tồn tại. Vui lòng chọn lại Tên khác !." });
                        return result;
                    }

                    SLevelCompany obj;
                    if (model.Id == 0)
                    {
                        obj = new SLevelCompany();
                        Parse.CopyObject(model, ref obj);
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedUser = model.ActionUser;
                        db.SLevelCompanies.Add(obj);
                        db.SaveChanges();
                        result.IsSuccess = true;
                        return result;
                    }

                    obj = db.SLevelCompanies.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Update  ", Message = "Cấp bậc bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
                        return result;
                    }

                    obj.OrderIndex = model.OrderIndex;
                    obj.LevelName = model.LevelName;
                    obj.Description = model.Description;
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

        private bool CheckExists(string code, int? id)
        {
            try
            {
                SLevelCompany obj = null;
                obj = db.SLevelCompanies.FirstOrDefault(x => !x.IsDeleted && x.LevelName.Trim().ToUpper().Equals(code) && x.Id != id);

                if (obj == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase Delete(int id, int acctionUserId )
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var result = new ResponseBase();
                    var obj = db.SLevelCompanies.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete ", Message = "Cấp bậc bạn đang thao tác đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại !." });
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

        public List<ModelSelectItem> GetSelectItem( )
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var objs = new List<ModelSelectItem>();
                    var CustomerTypes = db.SLevelCompanies.Where(x => !x.IsDeleted  ).Select(
                        x => new ModelSelectItem()
                        {
                            Value = x.Id,
                            Name = x.LevelName
                        }).ToList();

                    if (CustomerTypes != null && CustomerTypes.Count() > 0)
                    {
                        objs.Add(new ModelSelectItem() { Value = 0, Name = " - -  Chọn cấp bậc công tỵ  - - " });
                        objs.AddRange(CustomerTypes);
                    }
                    else
                        objs.Add(new ModelSelectItem() { Value = 0, Name = "  Không có Cấp bậc  " });
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
