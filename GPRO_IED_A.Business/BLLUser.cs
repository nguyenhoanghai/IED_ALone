﻿using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business
{
    public class BLLUser
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLUser _Instance;
        public static BLLUser Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUser();

                return _Instance;
            }
        }
        private BLLUser() { }
        #endregion

        public ResponseBase Login(string userName, string password)
        {
            ResponseBase rs = null;
            try
            {
                rs = new ResponseBase();
                userName = userName.Trim().ToUpper();
                using (db = new IEDEntities())
                {
                    SUser user = db.SUsers.Where(x => x.UserName.Trim().ToUpper().Equals(userName) && !x.IsDeleted).FirstOrDefault();
                    if (user == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Login Fail", Message = "Tên Đăng Nhập không tồn tại. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        if (!user.PassWord.Equals(GlobalFunction.EncryptMD5(password)))
                        {
                            rs.IsSuccess = false;
                            rs.Errors.Add(new Error() { MemberName = "Error", Message = "Mật Khẩu không đúng. Vui lòng kiểm tra lại." });
                        }
                        else
                        {
                            rs.IsSuccess = true;
                            rs.Data = user.Id;
                        }
                    }
                };
            }
            catch (Exception)
            { }
            return rs;
        }

        public UserService GetUserService(int userId, string moduleName)
        {
            UserService user = null;
            try
            {
                using (db = new IEDEntities())
                {
                    user = db.SUsers.Where(c => c.Id == userId && !c.IsDeleted && !c.SCompany.IsDeleted).Select(c => new UserService()
                                   {
                                       CompanyId = c.CompanyId,
                                       CompanyName = c.SCompany.CompanyName,
                                       UserID = c.Id,
                                       IsOwner = c.IsOwner,
                                       LogoCompany = c.SCompany.Logo,
                                       ImagePath = c.ImagePath,
                                       Email = c.Email,
                                       EmployeeName = c.FisrtName + " " + c.LastName,
                                       UserName = c.UserName
                                   }).FirstOrDefault();
                    if (user != null)
                    {
                        // lay quyen he thong
                        var permisUrl = new List<string>();
                        var systemPer = db.SPermissions.Where(x => !x.IsDeleted && !x.SFeature.IsDeleted && x.SFeature.SystemName.Trim().ToUpper().Equals("systemfeature")).Select(x => x.Url);
                        if (systemPer != null && systemPer.Count() > 0)
                            foreach (var item in systemPer)
                                permisUrl.AddRange(item.Split('|').ToList());

                        // lay quyen theo module 
                        List<int> userRoleIds = db.SUserRoles.Where(x => !x.IsDeleted && x.UserId == user.UserID && !x.SRoLe.SCompany.IsDeleted).Select(x => x.RoleId).ToList();
                        List<ModelPermission> permis = new List<ModelPermission>();
                        var permissionIds = db.SRolePermissions.Where(x => !x.IsDeleted && !x.SPermission.IsDeleted && !x.SPermission.SFeature.IsDeleted && !x.SPermission.SFeature.SModule.IsDeleted && (userRoleIds.Contains(x.RoleId) || (x.SFeature.IsDefault || x.SPermission.IsDefault)) && x.SModule.SystemName.Trim().ToUpper() == moduleName.Trim().ToUpper()).Select(c => c.PermissionId).Distinct().ToList();
                        if (permissionIds != null)
                        {
                            permis.AddRange(db.SPermissions.Where(c => permissionIds.Contains(c.Id)).Select(x => new ModelPermission()
                            {
                                Id = x.Id,
                                PermissionName = x.PermissionName,
                                FeatureId = x.FeatureId,
                                SystemName = x.SystemName,
                                Url = x.Url,
                                IsDefault = x.IsDefault
                            }));
                            if (permis.Count > 0)
                            {
                                permisUrl.AddRange(permis.Select(x => x.SystemName));
                                for (int i = 0; i < permis.Count; i++)
                                    permisUrl.AddRange(permis[i].Url.Split('|'));
                            }
                        }
                        user.Permissions = permisUrl.ToArray();
                        var moduleIds = db.SRolePermissions.Where(x => userRoleIds.Contains(x.RoleId) && !x.IsDeleted && !x.SModule.IsDeleted && x.SModule.IsShow && x.SModule.IsSystem).Select(c => c.ModuleId).Distinct();
                        if (moduleIds != null)
                        {
                            user.ListModule.AddRange(db.SModules.Where(c => moduleIds.Contains(c.Id)).Select(c => new Module()
                            {
                                Id = c.Id,
                                IsSystem = c.IsSystem,
                                SystemName = c.SystemName,
                                ModuleName = c.ModuleName,
                                ModuleUrl = c.ModuleUrl,
                                OrderIndex = c.OrderIndex,
                                Description = c.Description
                            }).OrderBy(x => x.OrderIndex));
                        }

                        if (user.ListModule.Count > 0)
                        {
                            user.ListMenu = new List<MenuCategory>();
                            var permiUrls = permis.Select(x => x.Url).ToList();
                            var moduleids = user.ListModule.Select(x => x.Id);
                            var categories = db.SCategories.Where(x => !x.IsDeleted && !x.SModule.IsDeleted && moduleids.Contains(x.ModuleId) && (x.CompanyId == null || x.CompanyId == user.CompanyId)).Select(x => new MenuCategory()
                                                            {
                                                                Id = x.Id,
                                                                Category = x.Name,
                                                                Position = x.Position,
                                                                OrderIndex = x.OrderIndex,
                                                                Description = x.Description,
                                                                Icon = x.Icon,
                                                                IsViewIcon = x.IsViewIcon,
                                                                Link = x.Link,
                                                                ModuleId = x.ModuleId,
                                                                ModuleName = x.SModule.SystemName,
                                                            }).OrderBy(x => x.OrderIndex).ToList();
                            if (categories.Count > 0)
                            {
                                var menus = db.SMenus.Where(x => !x.IsDeleted && x.IsShow && moduleids.Contains(x.ModuleId) && !x.SModule.IsDeleted && ((x.CompanyId == null && permiUrls.Contains(x.Link.Trim())) || (x.CompanyId != null && x.CompanyId.Value == user.CompanyId))).Select(x => new Menu()
                                                {
                                                    MenuName = x.MenuName,
                                                    Icon = x.Icon,
                                                    OrderIndex = x.OrderIndex,
                                                    Link = x.Link,
                                                    IsShow = x.IsShow,
                                                    IsViewIcon = x.IsViewIcon,
                                                    Description = x.Description,
                                                    CategoryId = x.MenuCategoryId
                                                }).ToList();
                                if (menus.Count > 0)
                                {
                                    for (int i = 0; i < categories.Count; i++)
                                    {
                                        categories[i].ListMenu.AddRange(menus.Where(x => x.CategoryId == categories[i].Id));
                                        user.ListMenu.Add(categories[i]);
                                    }
                                }
                            }
                        }
                        user.ChildCompanyId = db.SCompanies.Where(x => !x.IsDeleted && x.ParentId != null && x.ParentId.Value == user.CompanyId).Select(x => x.Id).ToArray();
                        if (user.ChildCompanyId == null)
                            user.ChildCompanyId = new int[] { };
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return user;
        }

        public UserModel Get(int userId)
        {
            using (db = new IEDEntities())
            {
                UserModel user = null;
                user = db.SUsers.Where(x => x.Id == userId && !x.IsDeleted).Select(x => new UserModel()
                {
                    Id = x.Id,
                    IsOwner = x.IsOwner,
                    UserName = x.UserName,
                    PassWord = x.PassWord,
                    IsLock = x.IsLock,
                    IsRequireChangePW = x.IsRequireChangePW,
                    IsForgotPassword = x.IsForgotPassword,
                    NoteForgotPassword = x.NoteForgotPassword,
                    Email = x.Email,
                    ImagePath = x.ImagePath,
                    LockedTime = x.LockedTime,
                    FisrtName = x.FisrtName,
                    LastName = x.LastName
                }).FirstOrDefault();
                return user;
            }
        }

        public ResponseBase InsertOrUpdate(UserModel model)
        {
            ResponseBase result = null;
            try
            {
                using (db = new IEDEntities())
                {
                    result = new ResponseBase();
                    SUser obj = null;
                    SUserRole userRoleObj = null;
                    if (CheckName(model.UserName, model.Id, db))
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { Message = "Tên đăng nhập đã tồn tại. Vui lòng chọn lại tên khác!", MemberName = "Thêm Mới" });
                    }
                    else
                    {
                        if (model.Id == 0)
                        {
                            #region add user
                            obj = new SUser();
                            Parse.CopyObject(model, ref obj);
                            if (!string.IsNullOrEmpty(model.ImagePath))
                                obj.ImagePath = model.ImagePath != "0" ? model.ImagePath.Split(',').ToList().First() : null;
                            obj.IsLock = false;
                            obj.IsRequireChangePW = true;
                            obj.NoteForgotPassword = null;
                            obj.PassWord = GlobalFunction.EncryptMD5(model.PassWord);
                            obj.CreatedUser = model.ActionUser;
                            obj.CreatedDate = DateTime.Now;
                            obj.CompanyId = model.CompanyId;
                            obj.SUserRoles = new Collection<SUserRole>();

                            if (model.NoteForgotPassword != null)
                            {
                                foreach (var item in model.NoteForgotPassword.Split(',').ToList())
                                {
                                    userRoleObj = new SUserRole();
                                    userRoleObj.RoleId = int.Parse(item);
                                    userRoleObj.CreatedDate = obj.CreatedDate;
                                    userRoleObj.CreatedUser = obj.CreatedUser;
                                    userRoleObj.SUser = obj;
                                    obj.SUserRoles.Add(userRoleObj);
                                }
                            }
                            db.SUsers.Add(obj);
                            #endregion
                        }
                        else
                        {
                            obj = db.SUsers.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj != null)
                            {
                                #region update user detail
                                if (!string.IsNullOrEmpty(model.PassWord))
                                    obj.PassWord = obj.IsRequireChangePW ? GlobalFunction.EncryptMD5(model.PassWord) : obj.PassWord;

                                obj.FisrtName = model.FisrtName;
                                obj.LastName = model.LastName;
                                obj.NoteForgotPassword = null;
                                if (model.ImagePath != null && model.ImagePath != "0")
                                    obj.ImagePath = model.ImagePath.Split(',').ToList().First();
                                obj.Email = model.Email;
                                obj.UpdatedUser = model.ActionUser;
                                obj.UpdatedDate = DateTime.Now;
                                #endregion

                                var oldRoles = db.SUserRoles.Where(x => !x.IsDeleted && x.UserId == model.Id);
                                SUserRole userRole;
                                if (model.NoteForgotPassword != null)
                                {
                                    model.UserRoleIds = model.NoteForgotPassword.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                                    #region sử lý nếu list user role new != null
                                    if (oldRoles != null && oldRoles.Count() > 0)
                                    {
                                        foreach (var oldItem in oldRoles)
                                        {
                                            var userRoleFind = model.UserRoleIds.Find(x => x == oldItem.Id);
                                            if (userRoleFind == 0)
                                            {
                                                oldItem.IsDeleted = true;
                                                oldItem.DeletedUser = obj.UpdatedUser;
                                                oldItem.DeletedDate = obj.UpdatedDate;
                                            }
                                            else
                                                model.UserRoleIds.Remove(userRoleFind);
                                        }

                                        if (model.UserRoleIds != null && model.UserRoleIds.Count > 0)
                                        {
                                            foreach (var item in model.UserRoleIds)
                                            {
                                                userRoleObj = new SUserRole();
                                                userRoleObj.RoleId = item;
                                                userRoleObj.CreatedDate = DateTime.Now;
                                                userRoleObj.CreatedUser = model.ActionUser;
                                                userRoleObj.UserId = model.Id;
                                                obj.SUserRoles.Add(userRoleObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in model.NoteForgotPassword.Split(',').Select(x => Convert.ToInt32(x)).ToList())
                                        {
                                            userRoleObj = new SUserRole();
                                            userRoleObj.RoleId = item;
                                            userRoleObj.CreatedDate = DateTime.Now;
                                            userRoleObj.CreatedUser = model.ActionUser;
                                            userRoleObj.UserId = model.Id;
                                            obj.SUserRoles.Add(userRoleObj);
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region sử lý nếu list user role new is null
                                    foreach (var oldItem in oldRoles)
                                    {
                                        oldItem.IsDeleted = true;
                                        oldItem.DeletedUser = obj.UpdatedUser;
                                        oldItem.DeletedDate = obj.UpdatedDate;
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
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

        private bool CheckName(string userName, int Id, IEDEntities db)
        {
            try
            {
                var user = db.SUsers.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));
                if (user != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ResponseBase Delete(int accountId, int actionUserId)
        {
            ResponseBase rs = null;
            try
            {
                using (db = new IEDEntities())
                {
                    rs = new ResponseBase();
                    SUser user = db.SUsers.FirstOrDefault(x => x.Id == accountId && !x.IsDeleted);
                    if (user != null)
                    {
                        user.IsDeleted = true;
                        user.DeletedUser = actionUserId;
                        user.DeletedDate = DateTime.Now;
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "Delete Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        public ResponseBase UpdatePassword(int userId, int accountId, string password)
        {
            ResponseBase userResult = null;
            try
            {
                using (db = new IEDEntities())
                {
                    userResult = new ResponseBase();
                    SUser user = db.SUsers.FirstOrDefault(x => x.Id == accountId && !x.IsDeleted);
                    if (user == null)
                    {
                        userResult.IsSuccess = false;
                        userResult.Errors.Add(new Error() { MemberName = "Mở Khóa Thời Gian", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        //GlobalFunction.RandomPass(lengh); // function random mat khau truyen vao length cua pass sau do kiem tra neu co mail thi chuyen wa mail neu ko co mail yeu cau admin change pass
                        user.IsRequireChangePW = true;
                        user.NoteForgotPassword = null;
                        user.IsForgotPassword = false;
                        user.PassWord = GlobalFunction.EncryptMD5(password);
                        user.UpdatedUser = userId;
                        user.UpdatedDate = DateTime.Now;
                        db.SaveChanges();
                        userResult.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return userResult;
        }
        public ResponseBase ChangeAvatar(int userId, string avatar)
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    SUser user = db.SUsers.FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
                    if (user == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "change avatar", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        user.ImagePath = avatar;
                        user.UpdatedUser = userId;
                        user.UpdatedDate = DateTime.Now;
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }
        public ResponseBase ChangeInfo(int userId, string mail, string first, string last)
        {
            ResponseBase rs = new ResponseBase();
            try
            {
                using (db = new IEDEntities())
                {
                    SUser user = db.SUsers.FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
                    if (user == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "change info", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        user.Email = mail;
                        user.FisrtName = first;
                        user.LastName = last;
                        user.UpdatedUser = userId;
                        user.UpdatedDate = DateTime.Now;
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }
        public ResponseBase ChangePassword(int userId, string oldPass, string password)
        {
            ResponseBase rs = null;
            try
            {
                using (db = new IEDEntities())
                {
                    rs = new ResponseBase();
                    SUser user = db.SUsers.FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
                    if (user == null)
                    {
                        rs.IsSuccess = false;
                        rs.Errors.Add(new Error() { MemberName = "change pass", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                    }
                    else
                    {
                        var old = GlobalFunction.EncryptMD5(oldPass);
                        if (user.PassWord.Equals(old))
                        {
                            user.IsRequireChangePW = true;
                            user.NoteForgotPassword = null;
                            user.IsForgotPassword = false;
                            user.PassWord = GlobalFunction.EncryptMD5(password); ;
                            user.UpdatedUser = userId;
                            user.UpdatedDate = DateTime.Now;
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.Errors.Add(new Error() { MemberName = "change pass", Message = "Mật khẩu cũ không đúng. Vui lòng kiểm tra lại." });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return rs;
        }
        public PagedList<UserModel> Gets(string keyWord, int searchBy, bool isBlock, bool isRequiredChangePass, bool isTimeBlock, bool isForgotPass, int startIndexRecord, int pageSize, string sorting, int userId, int companyId, int[] relationCompanyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    IQueryable<SUser> users = null;
                    PagedList<UserModel> usersReturn = null;
                    if (string.IsNullOrEmpty(sorting))
                    {
                        sorting = "CreatedDate DESC";
                    }
                    var pageNumber = (startIndexRecord / pageSize) + 1;

                    if (!string.IsNullOrEmpty(keyWord.Trim()))
                    {
                        switch (searchBy)
                        {
                            case 0:
                                keyWord = keyWord.Trim().ToUpper();
                                users = db.SUsers.Where(x => !x.IsDeleted && !x.SCompany.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && (x.FisrtName.Trim().ToUpper().Contains(keyWord) || x.LastName.Trim().ToUpper().Contains(keyWord)));
                                break;
                            case 1:
                                keyWord = keyWord.Trim().ToUpper();
                                users = db.SUsers.Where(x => !x.IsDeleted && !x.SCompany.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && x.UserName.Trim().ToUpper().Contains(keyWord));
                                break;
                            case 2:
                                keyWord = keyWord.Trim().ToUpper();
                                users = db.SUsers.Where(x => !x.IsDeleted && !x.SCompany.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)) && x.Email.Trim().ToUpper().Contains(keyWord));
                                break;
                        }
                    }
                    else
                        users = db.SUsers.Where(x => !x.IsDeleted && !x.SCompany.IsDeleted && (x.CompanyId == companyId || relationCompanyId.Contains(x.CompanyId)));

                    if (isBlock)
                        users = users.Where(x => x.IsLock);
                    if (isTimeBlock)
                        users = users.Where(x => x.LockedTime.HasValue && x.LockedTime.Value >= DateTime.Now);
                    if (isRequiredChangePass)
                        users = users.Where(x => x.IsRequireChangePW);
                    if (isForgotPass)
                        users = users.Where(x => x.IsForgotPassword);

                    if (users != null && users.Count() > 0)
                    {
                        var usersModel = users.OrderByDescending(x => x.CreatedDate).Select(x => new UserModel()
                        {
                            Id = x.Id,
                            IsOwner = x.IsOwner,
                            UserName = x.UserName,
                            PassWord = x.PassWord,
                            IsLock = x.IsLock,
                            IsRequireChangePW = x.IsRequireChangePW,
                            IsForgotPassword = x.IsForgotPassword,
                            NoteForgotPassword = x.NoteForgotPassword,
                            Email = x.Email,
                            ImagePath = x.ImagePath,
                            LockedTime = x.LockedTime,
                            FisrtName = x.FisrtName,
                            LastName = x.LastName 
                        }).ToList();
                        usersReturn = new PagedList<UserModel>(usersModel, pageNumber, pageSize);
                    }
                    else
                        usersReturn = new PagedList<UserModel>(new List<UserModel>(), pageNumber, pageSize);

                    if (usersReturn != null && usersReturn.Count > 0)
                    {
                        var uroles = db.SUserRoles.Where(x => !x.IsDeleted).Select(x => new UserRoleModel() { Id = x.Id, RoleId = x.RoleId, RoleName = x.SRoLe.RoleName, UserId = x.UserId }).ToList();
                        if ((uroles != null && uroles.Count() > 0) || usersReturn != null && usersReturn.Count > 0)
                        {
                            foreach (var item in usersReturn)
                            {
                                item.RoleNames = "";
                                var objs = uroles.Where(x => x.UserId == item.Id).ToList();
                                item.UserRoleIds = objs.Select(x => x.RoleId).Distinct().ToList();
                                for (int i = 0; i < objs.Count; i++)
                                {
                                    item.RoleNames += objs[i].RoleName;
                                    if (i < (objs.Count - 1))
                                        item.RoleNames += " ; ";
                                }
                            }
                        }
                    }
                    return usersReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}