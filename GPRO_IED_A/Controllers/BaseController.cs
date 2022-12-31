using GPRO.Core.Mvc;
using GPRO.Core.Security;
using System;
using System.Linq;
using System.Web.Routing;

namespace GPRO_IED_A.Controllers
{
    public class BaseController : ControllerCore
    {
        public string defaultPage = string.Empty;
        public bool isAuthenticate = false;
        public bool isOwner = false;
        public bool isPhaseApprover = false;
        public BaseController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                var routeDefault = ((System.Web.Routing.Route)requestContext.RouteData.Route).Defaults;
                if (routeDefault != null)
                {
                    var valuesDefault = routeDefault.Values.ToList();
                    defaultPage = "/" + valuesDefault[0].ToString() + "/" + valuesDefault[1].ToString();
                }
                CheckLogin(requestContext, App_Global.AppGlobal.ProductCode);
                isAuthenticate = Authentication.isAuthenticate;
                isOwner = Authentication.IsOwner;
                isPhaseApprover = Authentication.IsPhaseApprover;
            }
            catch { }
        }
    }
}