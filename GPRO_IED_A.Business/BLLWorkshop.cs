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
    public class BLLWorkshop
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLWorkshop _Instance;
        public static BLLWorkshop Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWorkshop();

                return _Instance;
            }
        }
        private BLLWorkshop() { }
        #endregion

        private bool CheckExists(string name, string code, int Id, int CompanyId, IEDEntities db)
        {
            try
            {
                T_WorkShop objectExists = null;
                if (!string.IsNullOrEmpty(name))
                    objectExists = db.T_WorkShop.FirstOrDefault(c => !c.IsDeleted && c.Id != Id && c.CompanyId == CompanyId && c.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
                else
                    objectExists = db.T_WorkShop.FirstOrDefault(c => !c.IsDeleted && c.Id != Id && c.CompanyId == CompanyId && c.Code.Trim().ToUpper().Equals(code.Trim().ToUpper()));

                if (objectExists == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(WorkShopModel model)
        {
            ResponseBase result = new ResponseBase();
            result.IsSuccess = false; var flag = false;
            try
            {
                using (db = new IEDEntities())
                {
                    if (CheckExists(model.Name.Trim().ToUpper(), null, model.Id, model.CompanyId, db))
                    {
                        flag = true;
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create  ", Message = "Tên Phân Xưởng này Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                    else if (!string.IsNullOrEmpty(model.Code))
                    {
                        if (CheckExists(null, model.Code.Trim().ToUpper(), model.Id, model.CompanyId, db))
                        {
                            flag = true;
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Create  ", Message = "Mã Phân Xưởng này Đã Tồn Tại,Vui Lòng Chọn Mã Khác" });
                        }
                    }

                    if (!flag)
                    {
                        T_WorkShop obj;
                        if (model.Id == 0)
                        {
                            obj = new T_WorkShop();
                            Parse.CopyObject(model, ref obj);
                            obj.CreatedDate = DateTime.Now;
                            db.T_WorkShop.Add(obj);
                        }
                        else
                        {
                            obj = db.T_WorkShop.FirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
                            if (obj != null)
                            {
                                obj.Code = model.Code;
                                obj.Name = model.Name;
                                obj.Description = model.Description;
                                obj.UpdatedDate = DateTime.Now;
                                obj.UpdatedUser = model.ActionUser;

                                // cap nhat ben phan tich mat hang
                                var commoAna = db.T_CommodityAnalysis.Where(x => !x.IsDeleted && x.ObjectId == obj.Id && x.ObjectType == (int)eObjectType.isWorkShop);
                                if (commoAna != null && commoAna.Count() > 0)
                                {
                                    foreach (var item in commoAna)
                                    {
                                        item.Name = obj.Name;
                                        item.UpdatedUser = model.ActionUser;
                                        item.UpdatedDate = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "UpdateWorkShop", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
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
            ResponseBase rs;

            try
            {
                using (db = new IEDEntities())
                {
                    rs = new ResponseBase();
                    var WorkShop = db.T_WorkShop.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                    if (WorkShop != null)
                    {
                        WorkShop.IsDeleted = true;
                        WorkShop.DeletedUser = userId;
                        WorkShop.DeletedDate = DateTime.Now;
                        db.SaveChanges(); ;
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        public List<ModelSelectItem> GetListWorkShop()
        {

            try
            {
                using (db = new IEDEntities())
                {
                    List<ModelSelectItem> objs = new List<ModelSelectItem>();
                    var workshops = db.T_WorkShop.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name });
                    if (workshops != null && workshops.Count() > 0)
                        objs.AddRange(workshops);
                    else
                        objs.Add(new ModelSelectItem() { Value = 0, Name = " Không có Dữ Liệu " });
                    return objs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public PagedList<WorkShopModel> GetList(string keyWord, int searchBy, int startIndexRecord, int pageSize, string sorting, int companyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    if (string.IsNullOrEmpty(sorting))
                        sorting = "CreatedDate DESC";

                    IQueryable<T_WorkShop> workshops = null;
                    var pageNumber = (startIndexRecord / pageSize) + 1;
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        keyWord = keyWord.Trim().ToUpper();
                        switch (searchBy)
                        {
                            case 1:
                                workshops = db.T_WorkShop.Where(c => !c.IsDeleted && c.CompanyId == companyId && c.Name.Trim().ToUpper().Contains(keyWord));
                                break;
                            case 2:
                                workshops = db.T_WorkShop.Where(c => !c.IsDeleted && c.CompanyId == companyId && c.Code.Trim().ToUpper().Contains(keyWord));
                                break;
                        }
                    }
                    else
                        workshops = db.T_WorkShop.Where(c => !c.IsDeleted && c.CompanyId == companyId);

                    if (workshops != null && workshops.Count() > 0)
                    {
                        var WorkShops = workshops.OrderByDescending(x => x.CreatedDate).Select(c => new WorkShopModel()
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Name = c.Name,
                        Description = c.Description,
                    }).ToList();
                        return new PagedList<WorkShopModel>(WorkShops, pageNumber, pageSize);
                    }
                    else
                        return new PagedList<WorkShopModel>(new List<WorkShopModel>(), pageNumber, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
