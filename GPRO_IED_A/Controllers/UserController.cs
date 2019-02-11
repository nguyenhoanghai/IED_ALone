﻿using GPRO.Core.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class UserController : BaseController
    { 
        public ActionResult Index()
        {
            List<ModelSelectItem> roles = null;
            List<SelectListItem> rolesItem = new List<SelectListItem>();
            try
            {
                roles = BLLUserRole.Instance.GetUserRolesModelByUserId(UserContext.UserID, UserContext.IsOwner, UserContext.CompanyId);
                if (roles == null)
                {
                    //return Error Page

                }
                rolesItem.AddRange(roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Value.ToString() }).ToList());
                ViewData["roles"] = rolesItem;
            }
            catch (Exception ex)
            {
                // add Error
                throw ex;
            }
            return View();
        }
        public ActionResult Profile()
        {
            var obj = BLLUser.Instance.Get(UserContext.UserID);
            return View(obj);
        }

        [HttpPost]
        public JsonResult Gets(string keyWord, int searchBy, bool isBlock, bool isRequiredChangePass, bool isTimeBlock, bool isForgotPass, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            { 
                    var Users = BLLUser.Instance.Gets(keyWord, searchBy, isBlock, isRequiredChangePass, isTimeBlock, isForgotPass, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID, UserContext.CompanyId, UserContext.ChildCompanyId);
                    JsonDataResult.Records = Users;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = Users.TotalItemCount;
             }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Save(UserModel model)
        {
            ResponseBase responseResult = null;
            try
            {
                 
                    model.CompanyId = UserContext.CompanyId  ;
                    model.ActionUser = UserContext.UserID; 
                    responseResult = BLLUser.Instance.InsertOrUpdate(model);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    else
                        JsonDataResult.Result = "OK"; 
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponseBase responseResult;
            try
            { 
                    responseResult = new ResponseBase();
                    responseResult = BLLUser.Instance.Delete(id, UserContext.UserID);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    else
                        JsonDataResult.Result = "OK";
              }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
 
        [HttpPost]
        public JsonResult ChangePassword(string id, string Password)
        {
            ResponseBase responseResult = null;
            try
            { 
                    responseResult = BLLUser.Instance.UpdatePassword(UserContext.UserID, int.Parse(id), Password);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    else
                        JsonDataResult.Result = "OK";
              }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult ChangeAvatar(string img)
        {
            ResponseBase responseResult = null;
            try
            {
                responseResult =  BLLUser.Instance.ChangeAvatar(UserContext.UserID, img);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                }
                else
                    JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult ChangeInfo(string mail, string first, string last)
        {
            ResponseBase responseResult = null;
            try
            {
                responseResult = BLLUser.Instance.ChangeInfo(UserContext.UserID, mail, first, last);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                }
                else
                    JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult ChangePass(string oldPass, string newPass)
        {
            ResponseBase responseResult = null;
            try
            {
                responseResult = BLLUser.Instance.ChangePassword(UserContext.UserID, oldPass, newPass);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                }
                else
                    JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
    }
}