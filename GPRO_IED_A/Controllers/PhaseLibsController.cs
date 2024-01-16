using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using PagedList;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class PhaseLibsController : BaseController
    {
        public ActionResult Index()
        {
            var per = this.UserContext.Permissions.Where(x => x.Contains("Create-workshop")).ToArray();
            var per1 = this.UserContext.Permissions.Where(x => x.Contains("All Allow")).ToArray();
            ViewBag.hasPer = (per.Length > 0 && per1.Length == 0 ? "hide" : "");
            ViewBag.TMU = BLLIEDConfig.Instance.GetValueByCode("TMU");
            ViewBag.GetTMUType = BLLIEDConfig.Instance.GetValueByCode("GetTMUType");
            ViewBag.ListManipulationCode = BLLManipulationLibrary.Instance.GetListManipulationCode();
            ViewBag.ManipulationExpendDefault = !string.IsNullOrEmpty(BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend")) ? BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend") : "0";
            return View();
        }

        [HttpPost]
        public JsonResult GetWhichNotLibs(string keyword, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    PagedList<PhaseLibModel> phases = null;
                    if (ConfigurationManager.AppSettings["PhaseSusguestForm"] != null &&
                  ConfigurationManager.AppSettings["PhaseSusguestForm"] == "Library")
                        phases = BLLPhaseGroup_Phase.Instance.GetsWhichNotLib(keyword, jtStartIndex, jtPageSize, jtSorting);
                    else
                        phases = BLLCommo_Ana_Phase.Instance.GetsWhichNotLib(keyword, jtStartIndex, jtPageSize, jtSorting);

                    JsonDataResult.Records = phases;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = phases.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetWhichIsLibs(string keyword, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    PagedList<PhaseLibModel> phases = null;
                    if (ConfigurationManager.AppSettings["PhaseSusguestForm"] != null &&
                  ConfigurationManager.AppSettings["PhaseSusguestForm"] == "Library")
                        phases = BLLPhaseGroup_Phase.Instance.GetsWhichIsLib(keyword, jtStartIndex, jtPageSize, jtSorting);
                    else
                        phases = BLLCommo_Ana_Phase.Instance.GetsWhichIsLib(keyword, jtStartIndex, jtPageSize, jtSorting);

                    JsonDataResult.Records = phases;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = phases.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Update(string ids, bool isRemove)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    if (ConfigurationManager.AppSettings["PhaseSusguestForm"] != null &&
              ConfigurationManager.AppSettings["PhaseSusguestForm"] == "Library")
                    {
                        result = BLLPhaseGroup_Phase.Instance.UpdateLibs(ids, !isRemove);
                    }
                    else
                    {
                        result = BLLCommo_Ana_Phase.Instance.UpdateLibs(ids, !isRemove);
                    }


                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
                    {
                        JsonDataResult.Result = "OK";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }
    }
}