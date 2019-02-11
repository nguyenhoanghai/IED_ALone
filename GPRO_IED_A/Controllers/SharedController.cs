﻿using GPRO_IED_A.App_Global;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class SharedController : BaseController
    {
        public SharedController() { }

        public ActionResult HeadPartial()
        {
            ViewData["Module"] = UserContext.ListModule;
            ViewData["Category"] = UserContext.ListMenu.Where(c => c.Position.Trim().ToUpper().Equals("LEFT") && c.ModuleName.Trim().ToUpper().Equals(AppGlobal.MODULE_NAME.ToUpper())).ToList();
            var modelE = new UserInfo();
            modelE.EmployeeName = UserContext.EmployeeName.ToString();
            modelE.LogoCompany = UserContext.LogoCompany != null ? UserContext.LogoCompany.ToString() : "";
            modelE.Email = UserContext.Email == null ? "" : UserContext.Email.ToString();
            modelE.ImagePath = UserContext.ImagePath == null ? "" : UserContext.ImagePath.ToString();
            ViewData["userInfo"] = modelE;
            return PartialView(UserContext);
        }

        public ActionResult UnActived()
        {
            return View();
        }
    }
}