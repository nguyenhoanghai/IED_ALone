using GPRO.Core.Mvc;
using GPRO.Ultilities;
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
    public class BLLLine
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLLine _Instance;
        public static BLLLine Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLine();

                return _Instance;
            }
        }
        private BLLLine() { }
        #endregion

        private bool CheckExists(string name, string code, int Id, int WorkShopId, IEDEntities db)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    T_Line objectExists = null;
                    if (!string.IsNullOrEmpty(name))
                        objectExists = db.T_Line.FirstOrDefault(c => !c.IsDeleted && c.Id != Id && c.WorkShopId == WorkShopId && c.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
                    else
                        objectExists = db.T_Line.FirstOrDefault(c => !c.IsDeleted && c.Id != Id && c.WorkShopId == WorkShopId && c.Code.Trim().ToUpper().Equals(code.Trim().ToUpper()));
                    if (objectExists == null)
                        return false;
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(LineModel model)
        {
            ResponseBase result = new ResponseBase();
            var flag = false;
            try
            {
                using (db = new IEDEntities())
                {
                    if (CheckExists(model.Name, null, model.Id, model.WorkShopId, db))
                    {
                        flag = true;
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create  ", Message = "Tên Chuyền này Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                    else if (!string.IsNullOrEmpty(model.Code))
                    {
                        if (CheckExists(null, model.Code, model.Id, model.WorkShopId, db))
                        {
                            flag = true;
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Create  ", Message = "Mã Chuyền này Đã Tồn Tại,Vui Lòng Chọn Mã Khác" });
                        }
                    }

                    if (!flag)
                    {
                        T_Line obj;
                        if (model.Id == 0)
                        {
                            obj = new T_Line();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedUser = model.ActionUser;
                            db.T_Line.Add(obj);
                        }
                        else
                        {
                            obj = db.T_Line.FirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
                            if (obj != null)
                            {
                                obj.Code = model.Code;
                                obj.Name = model.Name;
                                obj.CountOfLabours = model.CountOfLabours;
                                obj.WorkShopId = model.WorkShopId;
                                obj.Description = model.Description;
                                obj.UpdatedDate = DateTime.Now;
                                obj.UpdatedUser = model.ActionUser;
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "UpdateLine", Message = "Chuyền này Không tồn tại hoặc đã bị xóa. Vui lòng kiểm tra lại!" });
                            }
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Delete(int id, int userId)
        {
            ResponseBase responResult;
            try
            {
                using (db = new IEDEntities())
                {
                    responResult = new ResponseBase();
                    var obj = db.T_Line.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.IsDeleted = true;
                        obj.DeletedUser = userId;
                        obj.DeletedDate = DateTime.Now;
                        db.SaveChanges();
                        responResult.IsSuccess = true;
                    }
                    else
                    {
                        responResult.IsSuccess = false;
                        responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responResult;
        }

        public List<ModelSelectItem> Gets(int workShopId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    var listModelSelect = new List<ModelSelectItem>();
                    var lines = db.T_Line.Where(x => !x.IsDeleted && x.WorkShopId == workShopId).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name, Data = x.CountOfLabours });
                    if (lines != null && lines.Count() > 0)
                        listModelSelect.AddRange(lines);
                    else
                        listModelSelect.Add(new ModelSelectItem() { Value = 0, Name = " Không có dữ Liệu " });

                    return listModelSelect;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<LineModel> Gets(string keyWord, int searchBy, int startIndexRecord, int pageSize, string sorting, int companyId, int[] relationCompanyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "CreatedDate DESC";

                    IQueryable<T_Line> Lines = null;
                    List<LineModel> lines = null;
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        keyWord = keyWord.Trim().ToUpper();
                        switch (searchBy)
                        {
                            case 1: Lines = db.T_Line.Where(c => !c.IsDeleted && (c.T_WorkShop.CompanyId == null || c.T_WorkShop.CompanyId == companyId || relationCompanyId.Contains(c.T_WorkShop.CompanyId)) && c.Name.Trim().ToUpper().Contains(keyWord));
                                break;
                            case 2: Lines = db.T_Line.Where(c => !c.IsDeleted && (c.T_WorkShop.CompanyId == null || c.T_WorkShop.CompanyId == companyId || relationCompanyId.Contains(c.T_WorkShop.CompanyId)) && c.Code.Trim().ToUpper().Contains(keyWord));
                                break;
                        }
                    }
                    else
                        Lines = db.T_Line.Where(c => !c.IsDeleted && (c.T_WorkShop.CompanyId == null || c.T_WorkShop.CompanyId == companyId || relationCompanyId.Contains(c.T_WorkShop.CompanyId)));
                    if (Lines != null && Lines.Count() > 0)
                    {
                        lines = Lines.OrderByDescending(x => x.CreatedDate).Select(c => new LineModel()
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Name = c.Name,
                            WorkShopId = c.WorkShopId,
                            Description = c.Description,
                            CountOfLabours = c.CountOfLabours,
                            WorkShopName = c.T_WorkShop.Name,
                        }).ToList();
                    }
                    else
                        lines = new List<LineModel>();
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<LineModel>(lines, pageNumber, pageSize);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
