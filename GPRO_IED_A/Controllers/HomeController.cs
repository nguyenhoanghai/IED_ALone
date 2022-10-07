using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            //   Configuration conf = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            //   SessionStateSection section = (SessionStateSection)conf.GetSection("system.web/sessionState");
            //  int timeout = (int)section.Timeout.TotalMinutes;
            //  section.Timeout = new TimeSpan(8, 0, 0);
            //System.Web.HttpContext.Current.Session.Timeout
            return View();
        }

        [HttpPost]
        public JsonResult KeepAlive( )
        {
            JsonDataResult.Result = "Keep alive.!";
            return Json(JsonDataResult);
        }
    }
}