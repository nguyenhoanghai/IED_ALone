using GPRO.Core.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace GPRO_IED_A.Controllers
{
    public class WorkerLevelController : BaseController
    {  
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            ResponseBase rs;
            try
            {
                rs = BLLWorkerLevel.Instance.Delete(Id, UserContext.UserID);
                if (rs.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(rs.Errors);
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete Area", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult GetList(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var listWorkersLevel = BLLWorkerLevel.Instance.Gets(keyword, jtStartIndex, jtPageSize, jtSorting, UserContext.CompanyId  , UserContext.ChildCompanyId);
                JsonDataResult.Records = listWorkersLevel;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listWorkersLevel.TotalItemCount;
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult Save(WorkerLevelModel obj)
        {
            ResponseBase rs;
            try
            { 
                obj.ActionUser =  UserContext.UserID;
                obj.CompanyId = UserContext.CompanyId;
                rs = BLLWorkerLevel.Instance.InsertOrUpdate(obj, UserContext.ChildCompanyId);
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
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetSelectList()
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = BLLWorkerLevel.Instance.Gets(UserContext.CompanyId , UserContext.ChildCompanyId);
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete Area", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}