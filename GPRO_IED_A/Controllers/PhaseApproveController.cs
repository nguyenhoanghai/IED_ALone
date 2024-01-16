using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class PhaseApproveController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.TMU = BLLIEDConfig.Instance.GetValueByCode("TMU");
            ViewBag.GetTMUType = BLLIEDConfig.Instance.GetValueByCode("GetTMUType");
            ViewBag.ListManipulationCode = BLLManipulationLibrary.Instance.GetListManipulationCode();
            ViewBag.ManipulationExpendDefault = !string.IsNullOrEmpty(BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend")) ? BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend") : "0";

            return View();
        }

        public JsonResult Gets(bool fromLib, string phaseName, int jtStartIndex = 0, int jtPageSize = 100, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    JsonDataResult.Result = "OK";
                    if (fromLib)
                    {
                        var phases = BLLPhaseGroup_Phase.Instance.Gets(phaseName, jtStartIndex, jtPageSize, jtSorting);
                        JsonDataResult.Records = phases;
                        JsonDataResult.TotalRecordCount = phases.TotalItemCount;
                    }
                    else
                    {
                        var phases = BLLCommo_Ana_Phase.Instance.Gets(phaseName, jtStartIndex, jtPageSize, jtSorting);
                        JsonDataResult.Records = phases;
                        JsonDataResult.TotalRecordCount = phases.TotalItemCount;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }


        [HttpPost]
        public JsonResult Approve(int Id, bool isApprove, bool phaseLib)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    if (isPhaseApprover)
                    {
                        if (phaseLib)
                            result = BLLPhaseGroup_Phase.Instance.Approve(UserContext.UserID, Id, isApprove);
                        else
                            result = BLLCommo_Ana_Phase.Instance.Approve(UserContext.UserID, Id, isApprove);
                    }
                    else
                    {
                        result = new ResponseBase();
                        result.IsSuccess = false;
                        result.Errors.Add(new GPRO.Core.Mvc.Error() { MemberName = "", Message = "Tài khoản của bạn chưa được cấp quyền duyệt công đoạn. Vui lòng liên hệ Admin để cấp quyền." });
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