﻿using GPRO_IED_A.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPRO_IED_A.Business;
using GPRO.Core.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Gets(string keyword,  int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    var productTypes = BLLProduct.Instance.GetList(keyword,   UserContext.CompanyId, UserContext.ChildCompanyId, jtStartIndex, jtPageSize, jtSorting);
                    JsonDataResult.Records = productTypes;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = productTypes.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Save(ProductModel model)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    model.CompanyId = null;
                    if (!model.IsPrivate)
                        model.CompanyId = UserContext.CompanyId;
                    model.ActionUser = UserContext.UserID;
                    responseResult = BLLProduct.Instance.InsertOrUpdate(model, isOwner);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                        JsonDataResult.Result = "OK";
                }
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
                if (isAuthenticate)
                {
                    result = BLLProduct.Instance.Delete(Id, UserContext.UserID, isOwner);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
                        result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult DeleteFile(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLProduct.Instance.DeleteFile(Id, UserContext.UserID, isOwner);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
                        result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        

        [HttpPost]
        public JsonResult GetSelectList()
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = BLLProduct.Instance.GetSelectItem(UserContext.CompanyId, UserContext.ChildCompanyId);
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
        public JsonResult GetSelectListByProGroupId(int proGroupId, bool findByCustomer)
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = BLLProduct.Instance.GetSelectItem(proGroupId, UserContext.CompanyId, UserContext.ChildCompanyId, findByCustomer);
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