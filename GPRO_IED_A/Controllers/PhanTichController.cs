﻿using GPRO_IED_A.Business;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class PhanTichController : BaseController
    {
        // GET: PhanTich
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
        public JsonResult GetProductByCustomerId(int customerId)
        {
            try
            {
                if (isAuthenticate)
                {
                    int[] relationCompanyId = new int[] { };
                    if (UserContext.ChildCompanyId != null)
                        relationCompanyId = UserContext.ChildCompanyId;
                    var noName = BLLCommodityAnalysis.Instance.GetProductByCustomerId(customerId);
                    JsonDataResult.Data = noName;
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        /// <summary>
        /// mau 3
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_3()
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
        public JsonResult GetProductByProductGroupId(int progroupId)
        {
            try
            {
                if (isAuthenticate)
                {
                    int[] relationCompanyId = new int[] { };
                    if (UserContext.ChildCompanyId != null)
                        relationCompanyId = UserContext.ChildCompanyId;
                    var noName = BLLCommodityAnalysis.Instance.GetProductByProductGroupId(progroupId);
                    JsonDataResult.Data = noName;
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetSampleProAnaByProductId(int productId)
        {
            try
            {
                if (isAuthenticate)
                {
                    var rs = BLLCommodityAnalysis.Instance.GetSampleProAna(productId, UserContext.WorkshopIds);
                    if (rs.IsSuccess)
                    {
                        JsonDataResult.Data = JsonConvert.SerializeObject(rs.Records);
                        JsonDataResult.Result = "OK";
                    }
                    else 
                        JsonDataResult.Result = "ERROR";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetProAnaByProductId(int productId)
        {
            try
            {
                if (isAuthenticate)
                {
                    var rs = BLLCommodityAnalysis.Instance.GetSampleProAna(productId, UserContext.WorkshopIds);
                    if (rs.IsSuccess)
                    {
                        JsonDataResult.Data = JsonConvert.SerializeObject(rs.Records);
                        JsonDataResult.Result = "OK";
                    }
                    else
                        JsonDataResult.Result = "ERROR";
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