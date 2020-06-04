using GPRO.Core.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using System;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class WorkShopController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    responseResult = BLLWorkshop.Instance.Delete(Id, UserContext.UserID);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
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

        [HttpPost]
        public JsonResult Gets(string keyword, int searchBy, int jtStartIndex=0, int jtPageSize=1000, string jtSorting="")
        {
            try
            {
                if (isAuthenticate)
                {
                    var listWorkShop = BLLWorkshop.Instance.GetList(keyword, searchBy, jtStartIndex, jtPageSize, jtSorting, UserContext.CompanyId);
                    JsonDataResult.Records = listWorkShop;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = listWorkShop.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult Save(WorkShopModel model)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    model.CompanyId = UserContext.CompanyId;
                    model.ActionUser = UserContext.UserID;
                    rs = BLLWorkshop.Instance.InsertOrUpdate(model);
                    if (!rs.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(rs.Errors);
                    }
                    else
                        JsonDataResult.Result = "OK";
                }
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
        public JsonResult GetSelect()
        {
            try
            {
                JsonDataResult.Data = BLLWorkshop.Instance.GetListWorkShop();
                JsonDataResult.Result = "OK";
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