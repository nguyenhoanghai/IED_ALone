using GPRO.Core.Interface;
using GPRO_IED_A.App_Global;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GPRO_IED_A
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var membership = Application[GPRO.Core.Constant.IMEMBERSHIP_SERVICE] as GPRO.Core.Interface.IPermissionService;
            if (membership == null)
                Application[GPRO.Core.Constant.IMEMBERSHIP_SERVICE] = new InnerMembershipService();
            var encryptor = Application[GPRO.Core.Constant.GETENCRYPTOR] as GPRO.Core.Interface.IEncryptor;
            if (encryptor == null)
                Application[GPRO.Core.Constant.GETENCRYPTOR] = new InnerEncryptor();
        }

        protected void Application_End()
        {
            GPRO.Core.Security.Authentication.Logout();
        }

        //protected void Session_Start()
        //{
        //    Session.Timeout = 600;
        //}
        protected void Session_End()
        {
            GPRO.Core.Security.Authentication.Logout();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)        
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        public class InnerMembershipService : GPRO.Core.Interface.IMembershipService
        {
            public GPRO.Core.Interface.IUserService GetUserService(int userId)
            {
                var moduleName = AppGlobal.MODULE_NAME;
                  return new InnerUserService(BLLUser.Instance.GetUserService(userId, moduleName)); 
            }

            public GPRO.Core.Interface.IPermissionService[] GetPermissionService(string featureName)
            {
                return null;
            }
        }

        public class InnerUserService : UserService, IUserService
        {
            public InnerUserService(UserService userService)
            {
                this.CompanyId = userService.CompanyId;
                this.CompanyName = userService.CompanyName;
                this.DepartmentName = userService.DepartmentName;
                this.Description = userService.Description;
                this.Email = userService.Email;
                this.EmployeeName = userService.EmployeeName;
                this.Features = userService.Features;
                this.ImagePath = userService.ImagePath;
                this.IsOwner = userService.IsOwner;
                this.LogoCompany = userService.LogoCompany;
                this.Permissions = userService.Permissions;
                this.UserID = userService.UserID;
                this.ChildCompanyId = userService.ChildCompanyId;
                this.UserName = userService.UserName;
                this.ListModule = GetListModuleService(userService.ListModule).ToList();
                this.ListMenu = GetListMenuCategoryService(userService.ListMenu).ToList();
                State = new object();
            }
            public int StoreID { get; set; }
            public List<IPermissionService> PermissionServices { get; set; }
            public string SesssionId { get; set; }
            public object State { get; set; }
            public List<IModule> ListModule { get; set; }
            public List<IMenuCategory> ListMenu { get; set; }
            public IModule[] GetListModuleService(List<Module> listModule)
            {
                var result = new List<InnerModule>();
                if (listModule != null && listModule.Count > 0)
                    listModule.ForEach(p => result.Add(new InnerModule(p)));
                return result.ToArray();
            }

            public IMenuCategory[] GetListMenuCategoryService(List<MenuCategory> listMenuCategory)
            {
                var result = new List<InnerMenuCategory>();
                if (listMenuCategory != null && listMenuCategory.Count > 0)
                    listMenuCategory.ForEach(p => result.Add(new InnerMenuCategory(p)));
                return result.ToArray();
            }

            private class InnerModule : Module, IModule
            {
                public InnerModule(Module module)
                {
                    this.IsSystem = module.IsSystem;
                    this.ModuleName = module.ModuleName;
                    this.ModuleUrl = module.ModuleUrl;
                    this.OrderIndex = module.OrderIndex;
                    this.SystemName = module.SystemName;
                    this.Description = module.Description;
                }
            }

            private class InnerMenu : Menu, IMenu
            {
                public InnerMenu(Menu menu)
                {
                    this.Description = menu.Description;
                    this.Icon = menu.Icon;
                    this.IsShow = menu.IsShow;
                    this.IsViewIcon = menu.IsViewIcon;
                    this.Link = menu.Link;
                    this.MenuName = menu.MenuName;
                    this.OrderIndex = menu.OrderIndex;
                }
            }
            private class InnerMenuCategory : MenuCategory, IMenuCategory
            {
                public InnerMenuCategory(MenuCategory menuCategory)
                {
                    this.Description = menuCategory.Description;
                    this.Icon = menuCategory.Icon;
                    this.IsViewIcon = menuCategory.IsViewIcon;
                    this.Link = menuCategory.Link;
                    this.OrderIndex = menuCategory.OrderIndex;
                    this.Category = menuCategory.Category;
                    this.Position = menuCategory.Position;
                    this.ModuleId = menuCategory.ModuleId;
                    this.ModuleName = menuCategory.ModuleName;
                    this.ListMenu = GetListMenuService(menuCategory.ListMenu).ToList();
                }
                public List<IMenu> ListMenu { get; set; }

                public IMenu[] GetListMenuService(List<Menu> listMenu)
                {
                    var result = new List<InnerMenu>();
                    if (listMenu != null && listMenu.Count > 0)
                        listMenu.ForEach(p => result.Add(new InnerMenu(p)));
                    return result.ToArray();
                }
            }
        }

        private class InnerEncryptor : GPRO.Core.Interface.IEncryptor
        {
            public string Encrypt(byte[] data)
            {
                return Hugate.Framework.Security.SerializeObject.Encrypt(data);
            }

            public byte[] Decrypt(string data)
            {
                return Hugate.Framework.Security.SerializeObject.Decrypt(data);
            }

            public object Deserialize(byte[] bytes)
            {
                return Hugate.Framework.Security.SerializeObject.Deserialize(bytes);
            }


            public byte[] Serialize(object obj)
            {
                return Hugate.Framework.Security.SerializeObject.Serialize(obj);
            }
        }

        //public class UserService
        //{
        //    public int? CompanyID { get; set; }
        //    public string CompanyName { get; set; }
        //    public string DepartmentName { get; set; }
        //    public string Description { get; set; }
        //    public string Email { get; set; }
        //    public string EmployeeName { get; set; }
        //    public int[] Features { get; set; }
        //    public string ImagePath { get; set; }
        //    public bool IsOwner { get; set; }
        //    public string LogoCompany { get; set; }
        //    public string[] Permissions { get; set; }
        //    public int UserID { get; set; }
        //    public int[] ChildCompanyId { get; set; }
        //    public string UserName { get; set; }
        //    public List<Module> ListModule { get; set; }
        //    public List<MenuCategory> ListMenu { get; set; }
        //}

        //public class Module
        //{
        //    public string SystemName { get; set; }
        //    public string ModuleName { get; set; }
        //    public bool IsSystem { get; set; }
        //    public int OrderIndex { get; set; }
        //    public string Description { get; set; }
        //    public string ModuleUrl { get; set; }
        //}
        //public class MenuCategory
        //{
        //    public string Category { get; set; }
        //    public string Position { get; set; }
        //    public int OrderIndex { get; set; }
        //    public string Description { get; set; }
        //    public string Icon { get; set; }
        //    public bool IsViewIcon { get; set; }
        //    public string Link { get; set; }
        //    public List<Menu> ListMenu { get; set; }
        //    public int ModuleId { get; set; }
        //    public string ModuleName { get; set; }
        //}
        //public class Menu
        //{
        //    public string MenuName { get; set; }
        //    public int OrderIndex { get; set; }
        //    public string Link { get; set; }
        //    public bool IsShow { get; set; }
        //    public bool IsViewIcon { get; set; }
        //    public string Icon { get; set; }
        //    public string Description { get; set; }
        //}

    }


}
