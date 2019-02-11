using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;

namespace GPRO_IED_A.Controllers
{
    public class TimePrepareController : BaseController
    {
         
        public ActionResult Index()
        {
            return View();
        }

        #region Time Repare
        [HttpPost]
        public JsonResult Gets(int timeTypeId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                var timePrepares = BLLTimePrepare.Instance.Gets(timeTypeId, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = timePrepares;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = timePrepares.TotalItemCount;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetLists(string keyword, int searchBy, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                var timePrepares = BLLTimePrepare.Instance.Gets(keyword, searchBy, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = timePrepares;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = timePrepares.TotalItemCount;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Save(TimePrepareModel timePrepare)
        {
            ResponseBase rs;
            try
            {
                timePrepare.ActionUser = UserContext.UserID;
                rs = BLLTimePrepare.Instance.InsertOrUpdate(timePrepare);
                if (!rs.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(rs.Errors);
                }
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            ResponseBase result;
            try
            {
                result = BLLTimePrepare.Instance.Delete(Id, UserContext.UserID);
                if (!result.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(result.Errors);
                }
                else
                    result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        #endregion

        #region Time Type Prepare
        [HttpPost]
        public JsonResult Gets_(string keyword, int searchBy, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                var timeTypes = BLLTimeTypePrepare.Instance.Gets(keyword, searchBy, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = timeTypes;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = timeTypes.TotalItemCount;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Save_(TimeTypePrepareModel model)
        {
            ResponseBase rs;
            try
            {
                model.ActionUser = UserContext.UserID;
                rs = BLLTimeTypePrepare.Instance.InsertOrUpdate(model);
                if (!rs.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(rs.Errors);
                }
                else
                    JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Delete_(int Id)
        {
            ResponseBase result;
            try
            {
                result = BLLTimeTypePrepare.Instance.Delete(Id, UserContext.UserID);
                if (!result.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(result.Errors);
                }
                else
                    JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }


        [HttpPost]
        public JsonResult GetTimeTypePreparesSelectList()
        {
            try
            {
                JsonDataResult.Data = BLLTimeTypePrepare.Instance.GetListTimeTypePrepareSelect(UserContext.CompanyId, UserContext.ChildCompanyId);
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }


        #endregion
    }
}