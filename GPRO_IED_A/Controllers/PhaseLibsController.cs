using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using PagedList;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class PhaseLibsController : BaseController
    {
        public ActionResult Index()
        {
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
                    if (!isRemove)
                        result = BLLCommo_Ana_Phase.Instance.UpdateLibs(ids);
                    else
                        result = BLLCommo_Ana_Phase.Instance.RemoveLibs(ids);
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