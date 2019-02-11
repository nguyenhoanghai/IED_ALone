using GPRO.Core.Mvc;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPRO_IED_A.Business;

namespace GPRO_IED_A.Controllers
{
    public class EquipmentController : BaseController
    {
        // GET: Equipment
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
                rs = BLLEquipment.Instance.Delete(Id, UserContext.UserID);
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
        public JsonResult Gets(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var objs = BLLEquipment.Instance.GetList(keyword, jtStartIndex, jtPageSize, jtSorting, UserContext.CompanyId  );
                JsonDataResult.Records = objs;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = objs.TotalItemCount;
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult Save(ModelEquipment modelEquipment, List<string> a)
        {
            ResponseBase rs;
            try
            {
                if (modelEquipment.Id == 0)
                {
                    modelEquipment.CompanyId = UserContext.CompanyId  ;
                    modelEquipment.ActionUser = UserContext.UserID;
                    rs = BLLEquipment.Instance.Create(modelEquipment, UserContext.CompanyId  , a);
                }
                else
                {
                    modelEquipment.CompanyId = UserContext.CompanyId  ;
                    modelEquipment.ActionUser = UserContext.UserID;
                    rs = BLLEquipment.Instance.Update(modelEquipment, UserContext.CompanyId  , a);
                }
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
        public JsonResult GetListEquipmentTypeAttribute(int id)
        {
            try
            {
                List<ModelEquipmentTypeAttribute> listParent = new List<ModelEquipmentTypeAttribute>();
                listParent = BLLEquipmentTypeAttribute.Instance.GetEquipmentTypeAttributeByEquipmentTypeId(id);
                return Json(listParent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}