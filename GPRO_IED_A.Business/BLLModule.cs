using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLModule
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLModule _Instance;
        public static BLLModule Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLModule();

                return _Instance;
            }
        }
        private BLLModule() { }
        #endregion

        //    private bool CheckModuleName(string systemName, int Id)
        //    {
        //        var checkResult = false;
        //        try
        //        {
        //            var checkName = repModule.GetMany(c => !c.IsDeleted && c.Id != Id && c.SystemName.Trim().ToUpper().Equals(systemName.Trim().ToUpper())).FirstOrDefault();
        //            if (checkName == null)
        //                checkResult = true;
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //        return checkResult;
        //    }

        //    public ResponseBase Create(ModelModule obj, int userId)
        //    {
        //        ResponseBase result = new ResponseBase();
        //        result.IsSuccess = false;
        //        try
        //        {

        //            if (obj != null)
        //            {
        //                if (CheckModuleName(obj.SystemName, obj.Id))
        //                {

        //                    var moduleModule = new SModule();
        //                    Parse.CopyObject(obj, ref moduleModule);
        //                    moduleModule.CreatedDate = DateTime.Now;
        //                    moduleModule.CreatedUser = userId;
        //                    repModule.Add(moduleModule);
        //                    SaveChange();
        //                    result.IsSuccess = true;
        //                    result.Data = obj;
        //                }
        //                else
        //                {
        //                    result.IsSuccess = false;
        //                    result.Errors.Add(new Error() { MemberName = "Create Module", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
        //                }
        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Errors.Add(new Error() { MemberName = "Create Module", Message = "Đối Tượng Không tồn tại" });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return result;
        //    }

        //    public ResponseBase Update(ModelModule obj, int userId)
        //    {

        //        ResponseBase result = new ResponseBase();
        //        result.IsSuccess = false;
        //        try
        //        {
        //            if (!CheckModuleName(obj.SystemName, obj.Id))
        //            {
        //                result.IsSuccess = false;
        //                result.Errors.Add(new Error() { MemberName = "UpdateModule", Message = "Trùng Tên. Vui lòng chọn lại" });
        //            }
        //            else
        //            {
        //                SModule moduleModule = repModule.GetMany(x => x.Id == obj.Id && !x.IsDeleted).FirstOrDefault();
        //                if (moduleModule != null)
        //                {
        //                    moduleModule.SystemName = obj.SystemName;
        //                    moduleModule.ModuleName = obj.ModuleName;
        //                    moduleModule.OrderIndex = obj.OrderIndex;
        //                    moduleModule.Description = obj.Description;
        //                    moduleModule.ModuleUrl = obj.ModuleUrl;
        //                    moduleModule.UpdatedDate = DateTime.Now;
        //                    moduleModule.UpdatedUser = userId;
        //                    repModule.Update(moduleModule);
        //                    SaveChange();
        //                    result.IsSuccess = true;
        //                }
        //                else
        //                {
        //                    result.IsSuccess = false;
        //                    result.Errors.Add(new Error() { MemberName = "UpdateModule", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return result;
        //    }

        //    public ResponseBase DeleteById(int id, int userId)
        //    {
        //        ResponseBase responResult;

        //        try
        //        {
        //            responResult = new ResponseBase();
        //            var module = repModule.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
        //            if (module != null)
        //            {
        //                module.IsDeleted = true;
        //                module.DeletedUser = userId;
        //                module.DeletedDate = DateTime.Now;
        //                repModule.Update(module);
        //                SaveChange();
        //                responResult.IsSuccess = true;
        //            }
        //            else
        //            {
        //                responResult.IsSuccess = false;
        //                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return responResult;
        //    }

        //    public ResponseBase DeleteByListId(List<int> listId, int userId)
        //    {
        //        ResponseBase responResult = null;
        //        try
        //        {
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //        return responResult;
        //    }        

        //    public PagedList<ModelModule> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(sorting))
        //            {
        //                sorting = "CreatedDate DESC";
        //            }
        //            var Modules = repModule.GetMany(c => !c.IsDeleted).Select(c => new ModelModule()
        //            {
        //                Id = c.Id,
        //                SystemName = c.SystemName,
        //                ModuleName = c.ModuleName,
        //                IsSystem = c.IsSystem,
        //                OrderIndex = c.OrderIndex,
        //                Description = c.Description,
        //                ModuleUrl = c.ModuleUrl,
        //                CreatedDate = c.CreatedDate,
        //            }).OrderBy(sorting);
        //            if (Modules != null)
        //            {
        //                if (!string.IsNullOrEmpty(keyWord))
        //                    Modules = Modules.Where((c => c.ModuleName.Contains(keyWord) || c.SystemName.Contains(keyWord)));
        //            }
        //            var pageNumber = (startIndexRecord / pageSize) + 1;
        //            return new PagedList<ModelModule>(Modules.ToList(), pageNumber, pageSize);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public List<ModelModule> GetListModuleByCreatedUser(int userId, string sorting)
        //    {
        //        List<ModelModule> modules = null;
        //        try
        //        {
        //            if (string.IsNullOrEmpty(sorting))
        //            {
        //                sorting = "CreatedDate DESC";
        //            }
        //            modules = repModule.GetMany(x => x.CreatedUser == userId && !x.IsDeleted).Select(x => new ModelModule()
        //            {
        //                Id = x.Id,
        //                ModuleName = x.ModuleName,
        //                SystemName = x.SystemName,
        //                OrderIndex = x.OrderIndex,
        //                Description = x.Description,
        //                ModuleUrl = x.ModuleUrl,
        //                IsSystem = x.IsSystem,
        //                SFeatures = x.SFeatures
        //            }).ToList();

        //            if (modules != null && modules.Count > 0)
        //            {
        //                var listPermission = repPermission.GetMany(x => !x.IsDeleted).Select(x => new ModelPermission()
        //                    {
        //                        Id = x.Id,
        //                        PermissionName = x.PermissionName,
        //                        SystemName = x.SystemName,
        //                        Description = x.Description,
        //                        Url = x.Url,
        //                        FeatureId = x.SFeature.Id,
        //                        SFeature = x.SFeature
        //                    }).ToList();
        //                foreach (var module in modules)
        //                {
        //                    module.Permissions = listPermission.Where(x => module.SFeatures.Contains(x.SFeature)).ToList();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return modules;
        //    }

        public int GetModuleIdBySystemName(string systemName, IEDEntities db)
        {
            int moduleId = 0;
            try
            {
                var module = db.SModules.FirstOrDefault(c => c.SystemName.Trim().ToUpper().Equals(systemName.Trim().ToUpper()) && !c.IsDeleted);
                if (module != null)
                    moduleId = module.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return moduleId;
        }

        public List<ModelSelectItem> GetSelectListModuleByCompanyId(int companyId)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    var selectItems = new List<ModelSelectItem>();
                    selectItems.Add(new ModelSelectItem() { Value = 0, Name = " Không có Dữ Liệu " });
                    var roleIds = BLLRole.Instance.GetListRoleIdByCompanyId(companyId);
                    var moduleIds = BLLRolePermission.Instance.GetListModuleIdByListRoleId(roleIds, db);
                    if (moduleIds != null || moduleIds.Count > 0)
                    {
                        var modules = (from x in db.SModules
                                       where !x.IsDeleted && moduleIds.Contains(x.Id) && x.IsShow
                                       select new ModelSelectItem() { Name = x.ModuleName, Value = x.Id }).ToList();
                        if (modules != null && modules.Count > 0)
                        {
                            selectItems[0].Name = " - - Chọn Hệ Thống - - ";
                            selectItems.AddRange(modules);
                        }
                    }
                    return selectItems;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //    public List<ModelModule> GetListModuleByCompanyId(int companyId)
        //    {
        //        try
        //        {
        //            List<ModelModule> modules = null;
        //            var roleIds = bllRole.GetListRoleIdByCompanyId(companyId);
        //            var moduleIds = bllRolePermission.GetListModuleIdByListRoleId(roleIds);

        //            if (moduleIds != null || moduleIds.Count > 0)
        //            {   modules = repModule.GetMany(x => !x.IsDeleted && moduleIds.Contains(x.Id) && x.IsShow).Select( x => new ModelModule() { 
        //                   Id = x.Id,
        //                    ModuleName = x.ModuleName,
        //                    SystemName = x.SystemName,
        //                    IsSystem = x.IsSystem,
        //                    OrderIndex = x.OrderIndex,
        //                    Description = x.Description,
        //                    ModuleUrl = x.ModuleUrl
        //                }).ToList();
        //            }
        //            if (modules != null && modules.Count > 0)
        //                return modules;
        //            else
        //                return new List<ModelModule>();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
    }
}
