﻿using GPRO.Core.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Enum;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class ProAnaController : BaseController
    {
        // GET: ProAna
        #region Commo Ana
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
        public JsonResult Gets()
        {
            try
            {
                if (isAuthenticate)
                {
                    var noName = BLLCommodityAnalysis.Instance.GetList();
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
        public JsonResult GetByParentId(int parentId, string value, int Type, int year)
        {
            try
            {
                if (isAuthenticate)
                {
                    int[] relationCompanyId = new int[] { };
                    if (UserContext.ChildCompanyId != null)
                        relationCompanyId = UserContext.ChildCompanyId;
                    var noName = BLLCommodityAnalysis.Instance.GetCommoAnaItemByParentId(parentId, value, Type, UserContext.CompanyId, relationCompanyId, year, UserContext.WorkshopIds);
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
        public JsonResult SaveProduct(T_CommodityAnalysis noName)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Save(noName);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult SaveWorkshop(T_CommodityAnalysis noName)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Save(noName);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult SavePhaseGroup(T_CommodityAnalysis noName)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Save(noName);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        private ResponseBase Save(T_CommodityAnalysis noName)
        {
            ResponseBase responseResult;
            try
            {

                noName.CompanyId = UserContext.CompanyId;
                if (noName.Id == 0)
                {
                    noName.CreatedUser = UserContext.UserID;
                    noName.CreatedDate = DateTime.Now;
                }
                else
                {
                    noName.UpdatedUser = UserContext.UserID;
                    noName.UpdatedDate = DateTime.Now;
                }

                responseResult = BLLCommodityAnalysis.Instance.InsertOrUpdate(noName, isOwner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseResult;
        }

        [HttpPost]
        public JsonResult DeleteProduct(int Id)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Delete(Id);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult DeleteWorkshop(int Id)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Delete(Id);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult DeletePhaseGroup(int Id)
        {
            ResponseBase rs;
            try
            {
                if (isAuthenticate)
                {
                    rs = Delete(Id);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        private ResponseBase Delete(int Id)
        {
            ResponseBase result;
            try
            {
                result = BLLCommodityAnalysis.Instance.Delete(Id, UserContext.UserID, isOwner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region Commo Ana Phase
        [HttpPost]
        public JsonResult SavePhase(Commo_Ana_PhaseModel phase, List<Commo_Ana_Phase_TimePrepareModel> timePrepares)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    phase.ActionUser = UserContext.UserID;
                    responseResult = BLLCommo_Ana_Phase.Instance.InsertOrUpdate(phase, timePrepares, (isOwner || isPhaseApprover ? true : false), IsMDG);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                    {
                        JsonDataResult.Result = "OK";
                        JsonDataResult.Data = responseResult.Data;
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
        public JsonResult UpdatePhaseName(int phaseId, string newName)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    responseResult = BLLCommo_Ana_Phase.Instance.UpdateName(phaseId, newName, (isOwner || isPhaseApprover ? true : false), UserContext.UserID);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetPhases(string node, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    var phases = BLLCommo_Ana_Phase.Instance.GetListByNode(UserContext.UserID, isPhaseApprover, node + ",", jtStartIndex, jtPageSize, jtSorting, IsMDG);
                    JsonDataResult.Records = phases;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = phases.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                var ss = ex.Message;
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult ImportFromLibrary(int Id, List<int> phaseIds)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLCommo_Ana_Phase.Instance.ImportFromLibrary(Id, phaseIds, UserContext.UserID);
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

        [HttpPost]
        public JsonResult DeletePhase(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLCommo_Ana_Phase.Instance.Delete(Id, UserContext.UserID, isOwner, IsMDG);
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

        public JsonResult MoveToLibrary(int phaseId, int phasegroupId)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLCommo_Ana_Phase.Instance.MoveToLibrary(phaseId, phasegroupId, UserContext.UserID);
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

        [HttpPost]
        public JsonResult RemovePhaseVideo(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    string videoPath = "";
                    result = BLLCommo_Ana_Phase.Instance.RemovePhaseVideo(Id, UserContext.UserID, isOwner, ref videoPath);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(videoPath))
                            {
                                string directoryPath = Server.MapPath(("~" + videoPath.Split('|')[0]));
                                if (!System.IO.File.Exists(directoryPath))
                                    System.IO.File.Delete(directoryPath);
                            }
                        }
                        catch (Exception)
                        { }

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

        [HttpPost]
        public JsonResult GetPhaseLastIndex(int Id)
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Records = BLLCommo_Ana_Phase.Instance.GetLastIndex(Id);
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult CopyPhase(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLCommo_Ana_Phase.Instance.Copy(Id, UserContext.UserID, IsMDG);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
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
        public JsonResult Copy_CommoAnaPhaseGroup(int CopyObjectId, int ObjectId, bool copyFull)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    if (copyFull)
                        result = BLLCommodityAnalysis.Instance.Copy_CommoAnaPhaseGroup(CopyObjectId, ObjectId, UserContext.UserID, IsMDG);
                    else
                        result = BLLCommodityAnalysis.Instance.Copy_CommoAnaPhaseGroupContents(CopyObjectId, ObjectId, UserContext.UserID, IsMDG);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
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
        public JsonResult Copy_CommoAna(int CopyObjectId, int ObjectId)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLCommodityAnalysis.Instance.Copy_CommoAna(CopyObjectId, ObjectId, UserContext.UserID);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
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
        public async Task<JsonResult> UploadVideo()
        {
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname, returnName;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1].Replace(' ', '-');
                }
                else
                    fname = file.FileName.Replace(' ', '-');
                Guid g;
                g = Guid.NewGuid();
                var str = g.ToString().Replace('-', 'a') + file.FileName.Substring(file.FileName.LastIndexOf('.')) + "|" + fname;
                returnName = ("/Videos/" + str);

                string directoryPath = Server.MapPath(("~/Videos/"));
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);

                fname = Path.Combine(Server.MapPath(("~/Videos/")), str.Split('|')[0]);
                file.SaveAs(fname);
                return Json(returnName);
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult GetPhaseById(int phaseId)
        {
            try
            {
                var phase = BLLCommo_Ana_Phase.Instance.GetPhase(phaseId,IsMDG);
                JsonDataResult.Records = phase;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }


        #endregion

        #region Commo Ana Phase Mani Version
        [HttpPost]
        public JsonResult GetPhasesForSuggest()
        {
            try
            {
                JsonDataResult.Result = "OK";
                if (ConfigurationManager.AppSettings["PhaseSusguestForm"] != null &&
                    ConfigurationManager.AppSettings["PhaseSusguestForm"] == "Library")
                    JsonDataResult.Records = BLLPhaseGroup_Phase.Instance.GetAllPhasesForSuggest();
                else
                    JsonDataResult.Records = BLLCommo_Ana_Phase.Instance.GetAllPhasesForSuggest();
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetPhaseSusgestById(int phaseId)
        {
            try
            {
                //var phase = BLLCommo_Ana_Phase.Instance.GetPhase(phaseId,IsMDG);
                if (ConfigurationManager.AppSettings["PhaseSusguestForm"] != null &&
                     ConfigurationManager.AppSettings["PhaseSusguestForm"] == "Library")
                    JsonDataResult.Records = BLLPhaseGroup_Phase.Instance.GetPhase(phaseId);
                else
                    JsonDataResult.Records = BLLCommo_Ana_Phase.Instance.GetPhase(phaseId,IsMDG);
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult TinhLaiCode(List<Commo_Ana_Phase_ManiModel> actions, int equipmentId, int equiptypedefaultId, int applyPressure)
        {
            try
            {
                JsonDataResult.Result = "OK";
                var rs = BLLCommo_Ana_Phase.Instance.TinhLaiCode(actions, equipmentId, equiptypedefaultId, applyPressure);
                JsonDataResult.Records = rs.Data;
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }

        #region export excel
        public void export_PhaseManiVersion(int Id)
        {
            if (isAuthenticate)
            {
                try
                {
                    var excelPackage = new ExcelPackage();
                    excelPackage.Workbook.Properties.Author = "IED";
                    excelPackage.Workbook.Properties.Title = "Phân Tích Công Đoạn";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PTCĐ");
                    sheet.Name = "Phân Tích Công Đoạn";
                    sheet.Cells.Style.Font.Size = 12;
                    sheet.Cells.Style.Font.Name = "Times New Roman";

                    sheet.Cells[1, 2].Value = "PHÂN TÍCH CÔNG ĐOẠN";
                    sheet.Cells[1, 2].Style.Font.Size = 14;
                    sheet.Cells[1, 2, 1, 7].Merge = true;
                    sheet.Cells[1, 2, 1, 7].Style.Font.Bold = true;
                    sheet.Cells[1, 2, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // sheet.Cells[1, 2, 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //  sheet.Cells[1, 2, 1, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    //  sheet.Cells[1, 2, 1, 7].Style.Font.Color.SetColor(Color.White);

                    var result = BLLCommo_Ana_Phase.Instance.Export_CommoAnaPhaseManiVer(Id);
                    if (result != null)
                    {
                        sheet.Cells[1, 2].Value = "PHÂN TÍCH CÔNG ĐOẠN  ";

                        sheet.Cells[2, 2].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy");
                        sheet.Cells[2, 2, 2, 4].Merge = true;
                        sheet.Cells[2, 2, 2, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[3, 2].Value = "Khách hàng : " + result.CustomerName;
                        sheet.Cells[3, 2, 3, 4].Merge = true;
                        sheet.Cells[3, 2, 3, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[4, 2, 5, 4].Merge = true;
                        sheet.Cells[4, 2, 5, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[4, 2, 5, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Cells[4, 2, 5, 4].Value = "Mã hàng : " + result.ProductName;

                        double basicTime = Math.Round((result.TotalTMU + result.TimePrepare), 2);
                        sheet.Cells[2, 5].Value = "Thời gian chuẩn (giây) : ";
                        sheet.Cells[2, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[2, 8].Value = basicTime;
                        sheet.Cells[2, 8, 2, 9].Merge = true;
                        sheet.Cells[2, 8, 2, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[3, 5].Value = "Hiệu suất : ";
                        sheet.Cells[3, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[3, 8].Value = "100 %";
                        sheet.Cells[3, 8, 3, 9].Merge = true;
                        sheet.Cells[3, 8, 3, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[4, 5].Value = "Định mức / 1h / 1 LĐ ";
                        sheet.Cells[4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[4, 8].Value = Math.Round(3600 / basicTime) + " SP";
                        sheet.Cells[4, 8, 4, 9].Merge = true;
                        sheet.Cells[4, 8, 4, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[5, 5].Value = "Định mức / 9h / 1 LĐ ";
                        sheet.Cells[5, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[5, 8].Value = Math.Round((3600 / basicTime) * 9) + " SP";
                        sheet.Cells[5, 8, 5, 9].Merge = true;
                        sheet.Cells[5, 8, 5, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[2, 9, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //  sheet.Cells[2, 2, 5, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //  sheet.Cells[2, 2, 5, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));


                        sheet.Cells[6, 2, 6, 4].Merge = true;
                        sheet.Cells[6, 2].Value = "Tên công đoạn";
                        sheet.Cells[6, 2, 6, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[6, 2, 6, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[6, 5].Value = result.PhaseName.Trim().ToUpper();
                        sheet.Cells[6, 5, 6, 7].Merge = true;
                        sheet.Cells[6, 5, 6, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[6, 5, 6, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[7, 2].Value = "STT";
                        sheet.Cells[7, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 3].Value = "Mã số";
                        sheet.Cells[7, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 4].Value = "Tần suất";
                        sheet.Cells[7, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 5].Value = "Mô tả";
                        sheet.Cells[7, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[7, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[7, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 6].Value = "TMU thiết bị (chuẩn)";

                        sheet.Cells[7, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[7, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 7].Value = "TMU thao tác (chuẩn)";

                        sheet.Cells[7, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[7, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 8].Value = "TMU * Tần suất";

                        sheet.Cells[7, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[7, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[7, 9].Value = "Tổng thời gian(giây)";

                        sheet.Cells[6, 2, 7, 9].Style.Font.Bold = true;
                        //  sheet.Cells[6, 2, 7, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //  sheet.Cells[6, 2, 7, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        var row = 8;
                        double tmu = 0;
                        if (result.Details != null && result.Details.Count > 0)
                        {
                            foreach (var item in result.Details)
                            {

                                sheet.Cells[row, 2].Value = item.OrderIndex;
                                sheet.Cells[row, 3].Value = item.ManipulationCode.Trim();
                                sheet.Cells[row, 4].Value = item.Loop;
                                sheet.Cells[row, 5].Value = item.ManipulationName;
                                sheet.Cells[row, 6].Value = item.TMUEquipment;
                                sheet.Cells[row, 7].Value = item.TMUManipulation;
                                tmu = ((item.TMUManipulation.Value * item.Loop) + (item.TMUEquipment.Value * item.Loop));
                                sheet.Cells[row, 8].Value = tmu;
                                sheet.Cells[row, 9].Value = Math.Round((tmu / 27.8), 2);

                                AddCellBorder(sheet, row);
                                row++;
                            }
                        }
                        // sheet.Cells[8, 2, row - 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //  sheet.Cells[8, 2, row - 1, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                        //sheet.Cells[7, 2, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //sheet.Cells[row, 5].Value = "Tổng TMU";
                        //sheet.Cells[row, 6].Value = result.Details.Sum(x => x.TMUEquipment);
                        //sheet.Cells[row, 7].Value = result.Details.Sum(x => x.TMUManipulation);
                        //tmu = result.Details.Sum(x => (x.TMUManipulation.Value * x.Loop) + (x.TMUEquipment.Value * x.Loop));
                        //sheet.Cells[row, 8].Value = tmu;
                        //sheet.Cells[row, 9].Value = Math.Round((tmu / 27.8), 2);
                        //sheet.Cells[row, 5, row, 9].Style.Font.Bold = true;

                        //AddCellBorder(sheet, row);
                    }
                    sheet.Cells.AutoFitColumns();
                    sheet.Column(6).Width = 16;
                    sheet.Column(7).Width = 16;
                    sheet.Column(14).Style.WrapText = true;
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "PTCĐ_" + Regex.Replace( result.PhaseName, "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                    //TODO
                    //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                    //{
                    //    CreatedDate = dateNow,
                    //    UserId = UserContext.UserID,
                    //    PhaseId = Id,
                    //    WorkShopId = UserContext.WorkshopId,
                    //    Type = (int)eObjectType.isPhase,
                    //    Note = "Mã hàng: " + result.ProductName + " - Công đoạn: " + result.PhaseName
                    //});

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void AddCellBorder(ExcelWorksheet sheet, int rowIndex)
        {
            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        #endregion

        public void Export_CommoAnaPhaseGroup(int Id)
        {
            if (isAuthenticate)
            {
                try
                {
                    var excelPackage = new ExcelPackage();
                    excelPackage.Workbook.Properties.Author = "IED";
                    excelPackage.Workbook.Properties.Title = "Phân Tích cụm Công Đoạn";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PTCĐ");
                    sheet.Name = "Phân Tích cụm Công Đoạn";
                    sheet.Cells.Style.Font.Size = 12;
                    sheet.Cells.Style.Font.Name = "Times New Roman";

                    sheet.Cells[1, 2].Value = "PHÂN TÍCH CỤM CÔNG ĐOẠN";
                    sheet.Cells[1, 2].Style.Font.Size = 14;
                    sheet.Cells[1, 2, 1, 9].Merge = true;
                    sheet.Cells[1, 2, 1, 9].Style.Font.Bold = true;
                    sheet.Cells[1, 2, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //sheet.Cells[1, 2, 1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    // sheet.Cells[1, 2, 1, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    //sheet.Cells[1, 2, 1, 9].Style.Font.Color.SetColor(Color.White);

                    var result = BLLCommo_Ana_Phase.Instance.Export_CommoAnaPhaseGroup(Id);
                    if (result != null)
                    {
                        sheet.Cells[1, 2].Value = ("PHÂN TÍCH " + result.Name).ToUpper();

                        sheet.Cells[2, 2].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy");
                        sheet.Cells[2, 2, 2, 5].Merge = true;
                        //sheet.Cells[2, 2, 2, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[2, 8].Value = "Thời gian chuẩn (giây) :";
                        sheet.Cells[2, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        double basicTime = Math.Round(result.Phases.Sum(x => x.TotalTMU + x.TimePrepare), 2);
                        sheet.Cells[2, 9].Value = basicTime;
                        sheet.Cells[2, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        sheet.Cells[3, 2].Value = "Khách hàng : " + result.CustomerName;
                        sheet.Cells[3, 2, 3, 5].Merge = true;
                        // sheet.Cells[3, 2, 3, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[3, 8].Value = "Hiệu suất : ";
                        sheet.Cells[3, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[3, 9].Value = "100%";
                        sheet.Cells[3, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[4, 2, 4, 5].Merge = true;
                        //sheet.Cells[4, 2, 4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        // sheet.Cells[4, 2, 4, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Cells[4, 2, 4, 5].Value = "Mã hàng : " + result.ProductName;

                        sheet.Cells[4, 8].Value = "Định mức / 1h / 1 LĐ ";
                        sheet.Cells[4, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[4, 9].Value = Math.Round(3600 / basicTime) + " SP";
                        sheet.Cells[4, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[5, 8].Value = "Định mức / 9h / 1 LĐ ";
                        sheet.Cells[5, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[5, 9].Value = Math.Round((3600 / basicTime) * 9) + " SP";
                        sheet.Cells[5, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        int _row = 7;
                        if (result.Phases.Count > 0)
                        {
                            foreach (var item in result.Phases)
                            {
                                sheet.Cells[_row, 2, _row + 3, 4].Merge = true;
                                sheet.Cells[_row, 2].Value = "Tên công đoạn";
                                sheet.Cells[_row, 2, _row + 3, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 2, _row + 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 2, _row + 3, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                sheet.Cells[_row, 5].Value = item.PhaseName.Trim().ToUpper();
                                sheet.Cells[_row, 5, _row + 3, 7].Merge = true;
                                sheet.Cells[_row, 5, _row + 3, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 5, _row + 3, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 5, _row + 3, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                basicTime = Math.Round((item.TotalTMU + item.TimePrepare), 2);
                                sheet.Cells[_row, 8].Value = "Thời gian chuẩn (giây) : ";
                                sheet.Cells[_row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 9].Value = basicTime;
                                sheet.Cells[_row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                _row++;
                                sheet.Cells[_row, 8].Value = "Hiệu suất : ";
                                sheet.Cells[_row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 9].Value = "100 %";
                                sheet.Cells[_row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                _row++;
                                sheet.Cells[_row, 8].Value = "Định mức / 1h";
                                sheet.Cells[_row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 9].Value = Math.Round(3600 / basicTime) + " SP";
                                sheet.Cells[_row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                _row++;
                                sheet.Cells[_row, 8].Value = "Định mức / 9h ";
                                sheet.Cells[_row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 9].Value = Math.Round((3600 / basicTime) * 9) + " SP";
                                sheet.Cells[_row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //sheet.Cells[2, 9, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;                                 
                                //sheet.Cells[2, 2, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //sheet.Cells[2, 2, 5, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                                _row++;
                                sheet.Cells[_row, 2].Value = "STT";
                                sheet.Cells[_row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 3].Value = "Mã số";
                                sheet.Cells[_row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 4].Value = "Tần suất";
                                sheet.Cells[_row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 5].Value = "Mô tả";
                                sheet.Cells[_row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                sheet.Cells[_row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 6].Value = "TMU thiết bị (chuẩn)";

                                sheet.Cells[_row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 7].Value = "TMU thao tác (chuẩn)";

                                sheet.Cells[_row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 8].Value = "TMU * Tần suất";

                                sheet.Cells[_row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 9].Value = "Tổng thời gian(giây)";

                                sheet.Cells[_row, 2, _row, 9].Style.Font.Bold = true;
                                sheet.Cells[_row, 2, _row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[_row, 2, _row, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                sheet.Cells[_row, 2, _row, 9].Style.Font.Color.SetColor(Color.White);
                                //  sheet.Cells[6, 2, 7, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //  sheet.Cells[6, 2, 7, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                _row++;
                                double tmu = 0;
                                if (item.Details != null && item.Details.Count > 0)
                                {
                                    foreach (var action in item.Details)
                                    {

                                        sheet.Cells[_row, 2].Value = action.OrderIndex;
                                        sheet.Cells[_row, 3].Value = action.ManipulationCode.Trim();
                                        sheet.Cells[_row, 4].Value = action.Loop;
                                        sheet.Cells[_row, 5].Value = action.ManipulationName;
                                        sheet.Cells[_row, 6].Value = action.TMUEquipment;
                                        sheet.Cells[_row, 7].Value = action.TMUManipulation;
                                        tmu = ((action.TMUManipulation.Value * action.Loop) + (action.TMUEquipment.Value * action.Loop));
                                        sheet.Cells[_row, 8].Value = tmu;
                                        sheet.Cells[_row, 9].Value = Math.Round((tmu / 27.8), 2);

                                        AddCellBorder(sheet, _row);
                                        _row++;
                                    }
                                }
                                // sheet.Cells[8, 2, row - 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //  sheet.Cells[8, 2, row - 1, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                                sheet.Cells[6, 2, _row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[_row, 5].Value = "Tổng TMU";
                                sheet.Cells[_row, 6].Value = item.Details.Sum(x => x.TMUEquipment);
                                sheet.Cells[_row, 7].Value = item.Details.Sum(x => x.TMUManipulation);
                                tmu = item.Details.Sum(x => (x.TMUManipulation.Value * x.Loop) + (x.TMUEquipment.Value * x.Loop));
                                sheet.Cells[_row, 8].Value = tmu;
                                sheet.Cells[_row, 9].Value = Math.Round((tmu / 27.8), 2);
                                sheet.Cells[_row, 5, _row, 9].Style.Font.Bold = true;

                                AddCellBorder(sheet, _row);
                                _row += 2;
                            }
                        }

                        _row++;
                        #region Thông Tin Thiết Bi

                        sheet.Cells[_row, 2].Value = "TỔNG HỢP THIẾT BỊ SỬ DỤNG THEO CỤM CÔNG ĐOẠN";
                        sheet.Cells[_row, 2, _row, 6].Merge = true;
                        sheet.Cells[_row, 2, _row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[_row, 2, _row, 6].Style.Font.Bold = true;
                        sheet.Cells[_row, 2, _row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        _row++;
                        sheet.Cells[_row, 2].Value = "STT";
                        sheet.Cells[_row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[_row, 3].Value = "Tên Thiết Bị";
                        sheet.Cells[_row, 3, _row, 5].Merge = true;
                        sheet.Cells[_row, 3, _row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[_row, 6].Value = "Số lượng";
                        sheet.Cells[_row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[_row, 2, _row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[_row, 2, _row, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[_row, 2, _row, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                        sheet.Cells[_row, 2, _row, 6].Style.Font.Bold = true;
                        sheet.Cells[_row, 2, _row, 6].Style.Font.Color.SetColor(Color.White);

                        _row++;
                        if (result.Equipments != null && result.Equipments.Count > 0)
                        {
                            int tongTB = 0, tong = 0;
                            for (int i = 0; i < result.Equipments.Count; i++)
                            {
                                var equipment = result.Equipments[i];
                                sheet.Cells[_row, 2].Value = i + 1;
                                sheet.Cells[_row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 3].Value = equipment.Name;
                                sheet.Cells[_row, 3, _row, 5].Merge = true;
                                sheet.Cells[_row, 3, _row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                tong = result.Phases.Count(x => x.EquiptId == equipment.Value);
                                sheet.Cells[_row, 6].Value = tong;
                                sheet.Cells[_row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[_row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                _row++;
                                tongTB += tong;
                            }
                            sheet.Cells[_row, 2].Value = "Tổng";
                            sheet.Cells[_row, 2, _row, 5].Merge = true;
                            sheet.Cells[_row, 2, _row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet.Cells[_row, 6].Value = tongTB;
                            sheet.Cells[_row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                        sheet.Cells[_row, 2, _row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //sheet.Cells[_row, 4, _row, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //sheet.Cells[_row + 2, 2, _row, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        sheet.Cells[_row, 2, _row, 6].Style.Font.Bold = true;
                        #endregion

                    }
                    sheet.Cells.AutoFitColumns();
                    sheet.Column(6).Width = 16;
                    sheet.Column(7).Width = 16;
                    sheet.Column(14).Style.WrapText = true;
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "PTCĐ_" + Regex.Replace( result.Name , "[^\\w]", "_")+ "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();

                    //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                    //{
                    //    CreatedDate = dateNow,
                    //    UserId = UserContext.UserID,
                    //    PhaseGroupId = result.ObjectId,
                    //    Type = (int)eObjectType.isPhaseGroup,
                    //    Note = "Mã hàng: " + result.ProductName + " - Cụm công đoạn: " + result.Name
                    //});
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public JsonResult InsertViewLog(int phaseId, int wsId, string note)
        {
            try
            {
                JsonDataResult.Data = BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                {
                    CreatedDate = DateTime.Now,
                    UserId = UserContext.UserID,
                    PhaseId = phaseId,
                    WorkShopId = UserContext.WorkshopId,
                    Type = (int)eObjectType.isPhase,
                    IsView = true,
                    Note = note //"Mã hàng: " + result.ProductName + " - Công đoạn: " + result.PhaseName
                });
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }
        #endregion

        #region Techprocess
        [HttpPost]
        public JsonResult SaveTech(TechProcessVersionModel version, string details)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    var _details = JsonConvert.DeserializeObject<List<TechProcessVerDetailModel>>(details);
                    version.details = _details;
                    version.ActionUser = UserContext.UserID;
                    responseResult = BLLTechProcessVersion.Instance.InsertOrUpdate(version, isOwner);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                        JsonDataResult.Result = responseResult.Data.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
            return Json(JsonDataResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTech(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLTechProcessVersion.Instance.Delete(Id, UserContext.UserID, isOwner);
                    if (!result.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(result.Errors);
                    }
                    else
                        JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetTech(int parentId, string node, bool isGetNull)
        {
            try
            {
                if (isAuthenticate)
                {
                    JsonDataResult.Data = BLLTechProcessVersion.Instance.Get(parentId, node, isGetNull,IsMDG);
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
            return Json(JsonDataResult);
        }

        #region Excel
        public void ExportToExcel(int parentId, int fileType, bool isGetNull)
        {
            switch (fileType)
            {
                case 1: ExportToExcel_1(parentId, isGetNull); break;
                case 2: ExportToExcel_2(parentId, isGetNull); break;
                case 3: ExportToExcel_3(parentId, isGetNull); break;
                case 4: ExportToExcel_4(parentId, isGetNull); break;
                case 6: ExportToExcel_qtcn_dongtien(parentId, isGetNull); break;
                case 7: ExportToExcel_7(parentId, isGetNull); break;
                case 8: ExportToExcel_8(parentId, isGetNull); break;
            }
        }

        /// <summary>
        ///  Quy Trình Công Nghệ mẫu 1 thông tin phía trên
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_1(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull,IsMDG);
                var company = BLLCompany.Instance.GetById(UserContext.CompanyId);
                var excelPackage = new ExcelPackage();
                excelPackage.Workbook.Properties.Author = "IED";
                excelPackage.Workbook.Properties.Title = "Quy trình công nghệ";
                var sheet = excelPackage.Workbook.Worksheets.Add("Quy trình công nghệ");
                sheet.Name = "Quy trình công nghệ";
                sheet.Cells.Style.Font.Size = 12;
                sheet.Cells.Style.Font.Name = "Times New Roman";

                sheet.Cells[1, 2].Value = company != null ? company.CompanyName : "";
                sheet.Cells[1, 2].Style.Font.Size = 13;
                sheet.Cells[1, 2, 1, 10].Merge = true;
                sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[2, 2].Value = company != null ? company.Address : "";
                sheet.Cells[2, 2].Style.Font.Size = 10;
                sheet.Cells[2, 2, 2, 10].Merge = true;
                sheet.Cells[2, 2, 2, 10].Style.Font.Bold = true;
                sheet.Row(1).Height = 20;
                sheet.Cells[4, 2].Value = "TIME STUDY - QUY TRÌNH CÔNG NGHỆ SẢN XUẤT : " + techProcessInfo.ProductName.ToUpper();
                sheet.Cells[4, 2].Style.Font.Size = 14;
                sheet.Cells[4, 2, 4, 10].Merge = true;
                sheet.Cells[4, 2, 4, 10].Style.Font.Bold = true;
                sheet.Cells[4, 2].Style.WrapText = true;
                sheet.Cells[4, 2, 4, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[4, 2, 4, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[4, 2, 4, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[4, 2, 4, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[4, 2, 4, 10].Style.Font.Color.SetColor(Color.White);
                sheet.Row(4).Height = 40;

                sheet.Cells[5, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                sheet.Cells[5, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells[5, 2].Style.Font.Bold = true;
                sheet.Cells[5, 2, 5, 9].Merge = true;


                int rowIndex = 6, row = 0, dem = 1;
                double tongTG = 0, TongLĐ = 0;
                #region TTchung
                if (techProcessInfo != null)
                {
                    row = rowIndex;

                    sheet.Cells[rowIndex, 3].Value = "Thời gian làm việc trong ngày  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.WorkingTimePerDay * 3600;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/Ngày";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Chuyền SX ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    //sheet.Cells[rowIndex, 7].Value = techProcessInfo.LineName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Sản Lượng";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Thời gian hoàn thành 1 sản phẩm ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.TimeCompletePerCommo);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "K.Hàng";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    // sheet.Cells[rowIndex, 7].Value = techProcessInfo.CustomerName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "số ngày SX";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Nhịp độ sản xuất Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.PacedProduction);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/SP/LĐ";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Mã Hàng";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = techProcessInfo.ProductName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Ngày vào";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất bình quân / Người";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfPersonPerDay);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "/Người";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Chủng Loại";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Ngày giao";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[row, 6, rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất của Chuyền / giờ  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerHour);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc Thợ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "Đơn Giá";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Font.Color.SetColor(Color.White);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất Chuyền / ngày  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerDay);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 2";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Tổng Lao Động tham gia Sản Xuất";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 3";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Lao Động trực tiếp";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 4";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Quản Lý Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = "";
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 5";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Thợ dự trữ / Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = "";
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = " ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[row, 4, rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[row, 4, rowIndex, 4].Style.Font.Bold = true;
                    sheet.Cells[row, 5, rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet.Cells[row, 7, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Cụm";
                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 3].Value = "Tên Cụm";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 4].Value = "Phương Án Phân Công QL";
                    sheet.Cells[rowIndex, 4, rowIndex, 5].Merge = true;
                    sheet.Cells[rowIndex, 4, rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = "TG Cụm";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "HSLĐ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Color.SetColor(Color.White);

                    foreach (var group in techProcessInfo.ListTechProcessGroup)
                    {
                        rowIndex++;
                        sheet.Cells[rowIndex, 2].Value = dem;
                        sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName;
                        sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[rowIndex, 4, rowIndex, 5].Merge = true;
                        tongTG += group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent);
                        sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                        sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        TongLĐ += group.ListTechProcessVerDetail.Sum(x => x.Worker);
                        sheet.Cells[rowIndex, 7].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker));
                        sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        dem++;
                    }
                    rowIndex++;

                    sheet.Cells[rowIndex, 2].Value = "Tổng";
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = Math.Round(tongTG);
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = Math.Round(TongLĐ);
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Bold = true;

                    sheet.Cells[2, 2, rowIndex, 10].Style.Font.Size = 9;
                    sheet.Cells[row, 2, rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    rowIndex += 2;
                }
                #endregion

                #region Thong tin QTCN
                if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                {
                    row = rowIndex;
                    sheet.Cells[rowIndex, 2].Value = "STT";
                    sheet.Cells[rowIndex, 3].Value = "Tên Công Đoạn";
                    sheet.Cells[rowIndex, 4].Value = "Thiết Bị";
                    sheet.Cells[rowIndex, 5].Value = "Bậc Thợ";
                    sheet.Cells[rowIndex, 6].Value = "TG Chuẩn";
                    sheet.Cells[rowIndex, 7].Value = "Tỉ Lệ %";
                    sheet.Cells[rowIndex, 8].Value = "TG theo %";
                    sheet.Cells[rowIndex, 9].Value = "Lao Động";
                    sheet.Cells[rowIndex, 10].Value = "Ghi Chú";
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                    sheet.Row(rowIndex).Height = 25;
                    AddCellBorder(sheet, rowIndex, 2, 10);
                    techProcessInfo.CompanyName = UserContext.CompanyName;
                    int stt = 1, stt_group = 1;
                    rowIndex++;
                    foreach (var group in techProcessInfo.ListTechProcessGroup)
                    {
                        sheet.Cells[rowIndex, 2].Value = stt_group;
                        sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName.ToUpper();
                        sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.StandardTMU), 2);
                        sheet.Cells[rowIndex, 8].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                        //sheet.Cells[rowIndex, 9].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);
                        sheet.Cells[rowIndex, 9].Formula = "=SUM(I" + (rowIndex + 1) + ":I" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                        AddCellBorder(sheet, rowIndex, 2, 10);
                        rowIndex++;
                        if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                        {
                            foreach (var detail in group.ListTechProcessVerDetail)
                            {
                                sheet.Cells[rowIndex, 2].Value = stt;
                                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 3].Value = detail.PhaseName;
                                sheet.Cells[rowIndex, 4].Value = detail.EquipmentName;
                                sheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 5].Value = detail.WorkerLevelName;
                                sheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 6].Value = Math.Round(detail.StandardTMU, 2);
                                sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 7].Value = detail.Percent;
                                sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 8].Value = Math.Round(detail.TimeByPercent, 2);
                                sheet.Cells[rowIndex, 8].Style.Font.Color.SetColor(Color.Blue);
                                sheet.Cells[rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 9].Value = Math.Round(detail.Worker, 2);
                                sheet.Cells[rowIndex, 9].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 9].Style.Font.Color.SetColor(Color.Red);
                                sheet.Cells[rowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 10].Value = detail.Description;
                                AddCellBorder(sheet, rowIndex, 2, 10);
                                stt++;
                                rowIndex++;
                            }
                        }
                        stt_group++;
                    }
                }
                sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                sheet.Cells[row, 2, rowIndex - 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                #endregion

                row = rowIndex;
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "NĂNG SUẤT THEO PHẦN TRĂM";
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                AddCellBorder(sheet, rowIndex, 2, 10);
                sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "STT";
                sheet.Cells[rowIndex, 3].Value = "";
                sheet.Cells[rowIndex, 4].Value = "70%";
                sheet.Cells[rowIndex, 5].Value = "75%";
                sheet.Cells[rowIndex, 6].Value = "80%";
                sheet.Cells[rowIndex, 7].Value = "85%";
                sheet.Cells[rowIndex, 8].Value = "90%";
                sheet.Cells[rowIndex, 9].Value = "100%";
                AddCellBorder(sheet, rowIndex, 2, 10);

                string[] percents = new string[] { "Năng suất bình quân / Người", "Năng suất của Chuyền / giờ ", "Năng suất Chuyền / ngày" };
                for (int iii = 0; iii < percents.Length; iii++)
                {
                    double num = 0;
                    switch (iii)
                    {
                        case 0: num = techProcessInfo.ProOfPersonPerDay; break;
                        case 1: num = techProcessInfo.ProOfGroupPerHour; break;
                        case 2: num = techProcessInfo.ProOfGroupPerDay; break;
                    }
                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = (iii + 1);
                    sheet.Cells[rowIndex, 3].Value = percents[iii];
                    sheet.Cells[rowIndex, 4].Value = (num > 0 ? Math.Round((num * 70) / 100) : 0).ToString();
                    sheet.Cells[rowIndex, 5].Value = (num > 0 ? Math.Round((num * 75) / 100) : 0).ToString();
                    sheet.Cells[rowIndex, 6].Value = (num > 0 ? Math.Round((num * 80) / 100) : 0).ToString();
                    sheet.Cells[rowIndex, 7].Value = (num > 0 ? Math.Round((num * 85) / 100) : 0).ToString();
                    sheet.Cells[rowIndex, 8].Value = (num > 0 ? Math.Round((num * 90) / 100) : 0).ToString();
                    sheet.Cells[rowIndex, 9].Value = Math.Round(num);
                    AddCellBorder(sheet, rowIndex, 2, 10);
                }

                #region Thông Tin Thiết Bi
                row = rowIndex;
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "TỔNG HỢP THIẾT BỊ SỬ DỤNG THEO QUY TRÌNH";
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "STT";
                sheet.Cells[rowIndex, 3].Value = "Tên Thiết Bị";
                sheet.Cells[rowIndex, 4].Value = "Mã Thiết Bị";
                sheet.Cells[rowIndex, 5].Value = "QT";
                sheet.Cells[rowIndex, 6].Value = "Làm Tròn";
                sheet.Cells[rowIndex, 7].Value = "Thực Tế";

                AddCellBorder(sheet, rowIndex, 2, 10);

                sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);

                rowIndex++;
                if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
                {
                    TongLĐ = 0;
                    for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                    {
                        var equipment = techProcessInfo.equipments[i];
                        double tong = 0;
                        foreach (var item in techProcessInfo.ListTechProcessGroup)
                        {
                            tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                        }
                        sheet.Cells[rowIndex, 2].Value = i + 1;
                        sheet.Cells[rowIndex, 3].Value = equipment.Name;
                        sheet.Cells[rowIndex, 4].Value = equipment.Code;
                        sheet.Cells[rowIndex, 5].Value = Math.Round(tong, 2);
                        sheet.Cells[rowIndex, 6].Value = Math.Round(tong);

                        AddCellBorder(sheet, rowIndex, 2, 10);
                        rowIndex++;
                        TongLĐ += tong;
                    }
                    sheet.Cells[rowIndex, 2].Value = "Tổng";
                    sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                    sheet.Cells[rowIndex, 5].Value = Math.Round(TongLĐ, 2);
                    sheet.Cells[rowIndex, 6].Value = Math.Round(TongLĐ);
                    AddCellBorder(sheet, rowIndex, 2, 10);
                }

                sheet.Cells[row, 2, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 4, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row + 2, 2, rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                #endregion

                if (techProcessInfo.productImgs.Count > 0)
                {
                    int top = 65,
                        left = 700,
                        iWidth = 320,
                        iHeight = 320;
                    for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                    {
                        try
                        {
                            Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                            // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                            ExcelPicture excelPicture = sheet.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                            excelPicture.SetPosition(top, left);
                            excelPicture.SetSize(iWidth, iHeight);

                            top += 330;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                rowIndex += 2;
                sheet.Cells[rowIndex, 2].Value = "               GĐ ĐH 	                  GIÁM ĐỐC 	                     TP/KỸ THUẬT 	                   TRƯỞNG NHÓM QT 	                              NGƯỜI LẬP QT".ToUpper();
                sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;

                rowIndex += 7;
                sheet.Cells[rowIndex, 2].Value = "             PT.KỸ THUẬT XN 	                     KT.TRIỂN KHAI".ToUpper();
                sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;

                sheet.Cells.AutoFitColumns(5);
                sheet.Column(3).Width = 50;
                sheet.Column(14).Style.WrapText = true;
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                DateTime dateNow = DateTime.Now;
                string fileName = "QTC_" + Regex.Replace(techProcessInfo.ProductName , "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Flush();
                Response.End();

                //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                //{
                //    CreatedDate = dateNow,
                //    UserId = UserContext.UserID,
                //    QTCNId = techProcessInfo.Id,
                //    Type = (int)eObjectType.isQTCN,
                //    Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu 1 "
                //});
            }
        }

        private static void AddCellBorder(ExcelWorksheet sheet, int rowIndex, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                sheet.Cells[rowIndex, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }

        private static void AlignCenter(ExcelWorksheet sheet, int rowIndex, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                sheet.Cells[rowIndex, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// Quy Trình Công Nghệ mẫu 2 thông tin phía dưới
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_2(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
                var excelPackage = new ExcelPackage();
                excelPackage.Workbook.Properties.Author = "IED";
                excelPackage.Workbook.Properties.Title = "Quy trình công nghệ";
                var sheet = excelPackage.Workbook.Worksheets.Add("Quy trình công nghệ");
                sheet.Name = "Quy trình công nghệ";
                sheet.Cells.Style.Font.Size = 12;
                sheet.Cells.Style.Font.Name = "Times New Roman";

                sheet.Cells[1, 2].Value = "QUY TRÌNH CÔNG NGHỆ SẢN XUẤT " + techProcessInfo.ProductName.ToUpper();
                sheet.Cells[1, 2].Style.Font.Size = 14;
                sheet.Cells[1, 2, 1, 10].Merge = true;
                sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 2, 1, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);
                sheet.Row(1).Height = 50;

                sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells[2, 2].Style.Font.Bold = true;
                sheet.Cells[2, 2, 2, 9].Merge = true;


                int rowIndex = 3, row = 0, dem = 1;
                double tongTG = 0, TongLĐ = 0;


                #region Thong tin QTCN
                if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                {
                    row = rowIndex;
                    sheet.Cells[rowIndex, 2].Value = "STT";
                    sheet.Cells[rowIndex, 3].Value = "Tên Công Đoạn";
                    sheet.Cells[rowIndex, 4].Value = "Thiết Bị";
                    sheet.Cells[rowIndex, 5].Value = "Bậc Thợ";
                    sheet.Cells[rowIndex, 6].Value = "TG Chuẩn";
                    sheet.Cells[rowIndex, 7].Value = "Tỉ Lệ %";
                    sheet.Cells[rowIndex, 8].Value = "TG theo %";
                    sheet.Cells[rowIndex, 9].Value = "Lao Động";
                    sheet.Cells[rowIndex, 10].Value = "Ghi Chú";
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                    sheet.Row(rowIndex).Height = 25;
                    AddCellBorder(sheet, rowIndex, 2, 10);
                    techProcessInfo.CompanyName = UserContext.CompanyName;
                    int stt = 1;
                    int stt_group = 1;
                    rowIndex++;
                    foreach (var group in techProcessInfo.ListTechProcessGroup)
                    {
                        sheet.Cells[rowIndex, 2].Value = stt_group;
                        sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName.ToUpper();
                        sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.StandardTMU), 2);
                        sheet.Cells[rowIndex, 8].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                        sheet.Cells[rowIndex, 9].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;

                        AddCellBorder(sheet, rowIndex, 2, 10);
                        rowIndex++;
                        if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                        {
                            foreach (var detail in group.ListTechProcessVerDetail)
                            {
                                sheet.Cells[rowIndex, 2].Value = stt;
                                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 3].Value = detail.PhaseName;
                                sheet.Cells[rowIndex, 4].Value = detail.EquipmentName;
                                sheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 5].Value = detail.WorkerLevelName;
                                sheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 6].Value = Math.Round(detail.StandardTMU, 2);
                                sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 7].Value = detail.Percent;
                                sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 8].Value = Math.Round(detail.TimeByPercent, 2);
                                sheet.Cells[rowIndex, 8].Style.Font.Color.SetColor(Color.Blue);
                                sheet.Cells[rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 9].Value = Math.Round(detail.Worker, 2);
                                sheet.Cells[rowIndex, 9].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 9].Style.Font.Color.SetColor(Color.Red);
                                sheet.Cells[rowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 10].Value = detail.Description;
                                AddCellBorder(sheet, rowIndex, 2, 10);
                                stt++;
                                rowIndex++;
                            }
                        }
                        stt_group++;
                    }
                }
                sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                sheet.Cells[row, 2, rowIndex - 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                #endregion

                #region Thông Tin Thiết Bi
                row = rowIndex;
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "TỔNG HỢP THIẾT BỊ SỬ DỤNG THEO QUY TRÌNH";
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "STT";
                sheet.Cells[rowIndex, 3].Value = "Tên Thiết Bị";
                sheet.Cells[rowIndex, 4].Value = "Mã Thiết Bị";
                sheet.Cells[rowIndex, 5].Value = "QT";
                sheet.Cells[rowIndex, 6].Value = "Làm Tròn";
                sheet.Cells[rowIndex, 7].Value = "Thực Tế";

                AddCellBorder(sheet, rowIndex, 2, 10);

                sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);

                rowIndex++;
                if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
                {
                    TongLĐ = 0;
                    for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                    {
                        var equipment = techProcessInfo.equipments[i];
                        double tong = 0;
                        foreach (var item in techProcessInfo.ListTechProcessGroup)
                        {
                            tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                        }
                        sheet.Cells[rowIndex, 2].Value = i + 1;
                        sheet.Cells[rowIndex, 3].Value = equipment.Name;
                        sheet.Cells[rowIndex, 4].Value = equipment.Code;
                        sheet.Cells[rowIndex, 5].Value = Math.Round(tong, 2);
                        sheet.Cells[rowIndex, 6].Value = Math.Round(tong);

                        AddCellBorder(sheet, rowIndex, 2, 10);
                        rowIndex++;
                        TongLĐ += tong;
                    }
                    sheet.Cells[rowIndex, 2].Value = "Tổng";
                    sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                    sheet.Cells[rowIndex, 5].Value = Math.Round(TongLĐ, 2);
                    sheet.Cells[rowIndex, 6].Value = Math.Round(TongLĐ);
                    AddCellBorder(sheet, rowIndex, 2, 10);
                }

                sheet.Cells[row, 2, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 4, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row + 2, 2, rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                rowIndex += 2;
                #endregion

                #region TTchung
                if (techProcessInfo != null)
                {
                    row = rowIndex;
                    sheet.Cells[rowIndex, 3].Value = "Thời gian làm việc trong ngày  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.WorkingTimePerDay * 3600;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/Ngày";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Chuyền SX ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    // sheet.Cells[rowIndex, 7].Value = techProcessInfo.LineName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Sản Lượng";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Thời gian hoàn thành 1 sản phẩm ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.TimeCompletePerCommo);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "K.Hàng";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    //    sheet.Cells[rowIndex, 7].Value = techProcessInfo.CustomerName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "số ngày SX";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Nhịp độ sản xuất Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.PacedProduction);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "Giây/SP/LĐ";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Mã Hàng";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = techProcessInfo.ProductName;
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Ngày vào";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất bình quân / Người";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfPersonPerDay);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "/Người";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Chủng Loại";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "Ngày giao";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[row, 6, rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất của Chuyền / giờ  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerHour);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc Thợ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "Đơn Giá";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.Font.Color.SetColor(Color.White);

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Năng suất Chuyền / ngày  ";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerDay);
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Value = "SP";
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 2";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Tổng Lao Động tham gia Sản Xuất";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 3";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Lao Động trực tiếp";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 4";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Quản Lý Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6].Value = "Bậc 5";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 3].Value = "Thợ dự trữ / Chuyền";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = " ";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = " ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[row, 4, rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[row, 4, rowIndex, 4].Style.Font.Bold = true;
                    sheet.Cells[row, 5, rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet.Cells[row, 7, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Cụm";
                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 3].Value = "Tên Cụm";
                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 4].Value = "Phương Án Phân Công QL";
                    sheet.Cells[rowIndex, 4, rowIndex, 5].Merge = true;
                    sheet.Cells[rowIndex, 4, rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = "TG Cụm";
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = "HSLĐ";
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Color.SetColor(Color.White);


                    foreach (var group in techProcessInfo.ListTechProcessGroup)
                    {
                        rowIndex++;
                        sheet.Cells[rowIndex, 2].Value = dem;
                        sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName;
                        sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[rowIndex, 4, rowIndex, 5].Merge = true;
                        tongTG += group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent);
                        sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent));
                        sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //  TongLĐ += group.ListTechProcessVerDetail.Sum(x => x.Worker);
                        sheet.Cells[rowIndex, 7].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker));
                        sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        dem++;
                    }
                    rowIndex++;

                    sheet.Cells[rowIndex, 2].Value = "Tổng";
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowIndex, 6].Value = Math.Round(tongTG);
                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 7].Value = Math.Round(TongLĐ);
                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 7].Style.Font.Bold = true;

                    sheet.Cells[2, 2, rowIndex, 10].Style.Font.Size = 9;
                    sheet.Cells[row, 2, rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    rowIndex++;
                }
                #endregion

                if (techProcessInfo.productImgs.Count > 0)
                {
                    int top = 65,
                        left = 700,
                        iWidth = 320,
                        iHeight = 320;
                    for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                    {
                        Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                        // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                        ExcelPicture excelPicture = sheet.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                        excelPicture.SetPosition(top, left);
                        excelPicture.SetSize(iWidth, iHeight);

                        top += 330;
                    }
                }

                rowIndex += 2;
                sheet.Cells[rowIndex, 2].Value = "                      GĐ ĐH 	                           GIÁM ĐỐC 	                                TP/KỸ THUẬT 	                           TRƯỞNG NHÓM QT 	                              NGƯỜI LẬP QT".ToUpper();
                sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                sheet.Cells[rowIndex, 2].Style.Font.Size = 10;

                rowIndex += 7;
                sheet.Cells[rowIndex, 2].Value = "                        PT.KỸ THUẬT XN 	                     KT.TRIỂN KHAI".ToUpper();
                sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                sheet.Cells[rowIndex, 2].Style.Font.Size = 10;

                sheet.Cells.AutoFitColumns(5);
                sheet.Column(3).Width = 50;
                sheet.Column(14).Style.WrapText = true;
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                DateTime dateNow = DateTime.Now;
                string fileName = "QTC_" + Regex.Replace(techProcessInfo.ProductName , "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();

                //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                //{
                //    CreatedDate = dateNow,
                //    UserId = UserContext.UserID,
                //    QTCNId = techProcessInfo.Id,
                //    Type = (int)eObjectType.isQTCN,
                //    Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu 2 "
                //});
            }
        }

        /// <summary>
        /// mẫu may tex-giang
        /// </summary>
        /// <param name="parentId"></param>
        public void ExportToExcel_3(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
                var company = BLLCompany.Instance.GetById(UserContext.CompanyId);
                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\QTCN_TexGiang.xlsx"));
                int rowIndex = 8;
                using (var package = new ExcelPackage(_file))
                {
                    var workbook = package.Workbook;
                    var sheet = workbook.Worksheets.First();

                    sheet.Cells[1, 2].Value = company != null ? company.CompanyName : "";

                    //worksheet.Cells[5, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                    sheet.Cells[5, 2].Value = ("mã hàng " + techProcessInfo.ProductName + "_kh " + techProcessInfo.CustomerName).ToUpper();


                    #region Thong tin QTCN
                    if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                    {
                        int stt = 1, stt_group = 1;
                        foreach (var group in techProcessInfo.ListTechProcessGroup)
                        {
                            //sheet.Cells[rowIndex, 2].Value = stt_group;
                            //sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName.ToUpper();
                            //sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.StandardTMU), 2);
                            //sheet.Cells[rowIndex, 8].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                            //sheet.Cells[rowIndex, 9].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);
                            //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                            //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                            //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                            //AddCellBorder(sheet, rowIndex, 2, 10);
                            //rowIndex++;
                            if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                            {
                                foreach (var detail in group.ListTechProcessVerDetail)
                                {
                                    sheet.Cells[rowIndex, 1].Value = stt;
                                    sheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[rowIndex, 2].Value = detail.PhaseName;
                                    sheet.Cells[rowIndex, 5].Value = detail.EquipmentName;
                                    sheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    sheet.Cells[rowIndex, 6].Value = detail.WorkerLevelName;
                                    sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[rowIndex, 7].Value = Math.Round(detail.StandardTMU, 2);
                                    sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[rowIndex, 7].Style.Font.Color.SetColor(Color.Red);

                                    sheet.Cells[rowIndex, 8].Value = Math.Round(detail.Worker, 2);
                                    sheet.Cells[rowIndex, 8].Style.Font.Bold = true;
                                    AddCellBorder(sheet, rowIndex, 1, 8);
                                    stt++;
                                    rowIndex++;
                                }
                            }
                            stt_group++;
                        }
                    }
                    sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                    sheet.Cells[8, 2, rowIndex - 1, 8].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    #endregion

                    #region thông tin ns theo %

                    rowIndex++;
                    sheet.Cells[rowIndex, 1].Value = "NĂNG SUẤT THEO PHẦN TRĂM";
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Merge = true;
                    AddCellBorder(sheet, rowIndex, 1, 8);
                    sheet.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Color.SetColor(Color.White);

                    rowIndex++;
                    sheet.Cells[rowIndex, 1].Value = "STT";
                    sheet.Cells[rowIndex, 2].Value = "";
                    sheet.Cells[rowIndex, 3].Value = "70%";
                    sheet.Cells[rowIndex, 4].Value = "75%";
                    sheet.Cells[rowIndex, 5].Value = "80%";
                    sheet.Cells[rowIndex, 6].Value = "85%";
                    sheet.Cells[rowIndex, 7].Value = "90%";
                    sheet.Cells[rowIndex, 8].Value = "100%";
                    AddCellBorder(sheet, rowIndex, 2, 8);

                    string[] percents = new string[] { "Năng suất bình quân / Người", "Năng suất của Chuyền / giờ ", "Năng suất Chuyền / ngày" };
                    for (int iii = 0; iii < percents.Length; iii++)
                    {
                        double num = 0;
                        switch (iii)
                        {
                            case 0: num = techProcessInfo.ProOfPersonPerDay; break;
                            case 1: num = techProcessInfo.ProOfGroupPerHour; break;
                            case 2: num = techProcessInfo.ProOfGroupPerDay; break;
                        }
                        rowIndex++;
                        sheet.Cells[rowIndex, 1].Value = (iii + 1);
                        sheet.Cells[rowIndex, 2].Value = percents[iii];
                        sheet.Cells[rowIndex, 3].Value = (num > 0 ? Math.Round((num * 70) / 100) : 0).ToString();
                        sheet.Cells[rowIndex, 4].Value = (num > 0 ? Math.Round((num * 75) / 100) : 0).ToString();
                        sheet.Cells[rowIndex, 5].Value = (num > 0 ? Math.Round((num * 80) / 100) : 0).ToString();
                        sheet.Cells[rowIndex, 6].Value = (num > 0 ? Math.Round((num * 85) / 100) : 0).ToString();
                        sheet.Cells[rowIndex, 7].Value = (num > 0 ? Math.Round((num * 90) / 100) : 0).ToString();
                        sheet.Cells[rowIndex, 8].Value = Math.Round(num);
                        AddCellBorder(sheet, rowIndex, 1, 8);
                    }

                    #endregion

                    rowIndex += 2;
                    sheet.Cells[rowIndex, 2].Value = "Năng suất lao động bình quân đầu người :";
                    sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.ProOfPersonPerDay);
                    sheet.Cells[rowIndex, 7].Value = "sp/lđ";
                    sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 7].Style.Font.Color.SetColor(Color.Blue);
                    sheet.Cells[rowIndex, 7].Style.Font.Size = 14;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Năng suất lao động tổ:";
                    sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.ProOfGroupPerHour);
                    sheet.Cells[rowIndex, 7].Value = "sp/h/lđ";
                    sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 7].Style.Font.Color.SetColor(Color.Blue);
                    sheet.Cells[rowIndex, 7].Style.Font.Size = 14;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Nhịp sản suất :";
                    sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.PacedProduction);
                    sheet.Cells[rowIndex, 7].Value = "s/lđ";
                    sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 7].Style.Font.Size = 14;
                    sheet.Cells[rowIndex, 7].Style.Font.Color.SetColor(Color.Red);

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Công cụ";
                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Công đoạn ngoài";
                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Thiết bị";

                    rowIndex++;
                    sheet.Cells[rowIndex, 5].Value = "                            Ngày .... tháng .... năm ......";
                    sheet.Cells[rowIndex, 5, rowIndex, 8].Merge = true;
                    sheet.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "  BLĐ                                            GĐXN                    ";

                    sheet.Cells[rowIndex, 3].Value = "P.KT-CN";
                    sheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 3].Style.Font.Color.SetColor(Color.Red);

                    sheet.Cells[rowIndex, 6].Value = "Người lập";
                    sheet.Cells[rowIndex, 6, rowIndex, 8].Merge = true;
                    sheet.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Size = 12;


                    if (techProcessInfo.productImgs.Count > 0)
                    {
                        int top = 5,
                            left = 980,
                            iWidth = 320,
                            iHeight = 320;
                        for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                        {
                            Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                            // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                            ExcelPicture excelPicture = sheet.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                            excelPicture.SetPosition(top, left);
                            excelPicture.SetSize(iWidth, iHeight);

                            top += 330;
                        }
                        //https://www.youtube.com/watch?v=xwZW3-E4gBw
                    }

                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "QTC_" + Regex.Replace( techProcessInfo.ProductName, "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Flush();
                    Response.End();

                    //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                    //{
                    //    CreatedDate = dateNow,
                    //    UserId = UserContext.UserID,
                    //    QTCNId = techProcessInfo.Id,
                    //    Type = (int)eObjectType.isQTCN,
                    //    Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu 3"
                    //});
                }

            }
        }


        /// <summary>
        /// Quy Trình Công Nghệ mẫu 5 MDG
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_5(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
                var excelPackage = new ExcelPackage();
                excelPackage.Workbook.Properties.Author = "IED";
                excelPackage.Workbook.Properties.Title = "Quy trình công nghệ";

                #region sheet 1 
                var sheet = excelPackage.Workbook.Worksheets.Add("PT");
                sheet.Name = "PT";
                sheet.Cells.Style.Font.Size = 9;
                sheet.Cells.Style.Font.Name = "Times New Roman";

                sheet.Cells[1, 2].Value = "QUY TRÌNH CÔNG NGHỆ SẢN XUẤT " + techProcessInfo.ProductName.ToUpper();
                sheet.Cells[1, 2].Style.Font.Size = 14;
                sheet.Cells[1, 2, 1, 10].Merge = true;
                sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 2, 1, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);
                sheet.Row(1).Height = 50;

                sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells[2, 2].Style.Font.Bold = true;
                sheet.Cells[2, 2, 2, 9].Merge = true;


                int rowIndex = 3, row = 0, dem = 1;
                double tongTG = 0, TongLĐ = 0;
                string sumFormula = "";

                #region Thong tin QTCN
                if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                {
                    row = rowIndex;
                    sheet.Cells[rowIndex, 2].Value = "STT";
                    sheet.Cells[rowIndex, 3].Value = "Tên Công Đoạn";
                    sheet.Cells[rowIndex, 4].Value = "Thiết Bị";
                    sheet.Cells[rowIndex, 5].Value = "Bậc Thợ";
                    sheet.Cells[rowIndex, 6].Value = "TG Chuẩn";
                    sheet.Cells[rowIndex, 7].Value = "Tỉ Lệ %";
                    sheet.Cells[rowIndex, 8].Value = "TG theo %";
                    sheet.Cells[rowIndex, 9].Value = "Lao Động TH";
                    sheet.Cells[rowIndex, 10].Value = "Đơn giá";
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                    sheet.Row(rowIndex).Height = 25;
                    AddCellBorder(sheet, rowIndex, 2, 10);
                    techProcessInfo.CompanyName = UserContext.CompanyName;
                    int stt = 1,
                        stt_group = 1;

                    rowIndex++;
                    foreach (var group in techProcessInfo.ListTechProcessGroup)
                    {
                        sheet.Cells[rowIndex, 2].Value = stt_group;
                        sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName.ToUpper();
                        sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.StandardTMU), 2);
                        if (rowIndex == 4)
                        {
                            sheet.Cells[rowIndex, 7].Value = techProcessInfo.PercentWorker;
                            sumFormula += "H" + rowIndex;
                        }
                        else
                            sumFormula += ",H" + rowIndex;

                        sheet.Cells[rowIndex, 8].Formula = "SUM(H" + (rowIndex + 1) + ":H" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";// Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                        sheet.Cells[rowIndex, 9].Formula = "SUM(I" + (rowIndex + 1) + ":I" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";//Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);
                        sheet.Cells[rowIndex, 10].Formula = "SUM(J" + (rowIndex + 1) + ":J" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";//Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent) * techProcessInfo.PricePerSecond, 2);

                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;


                        AddCellBorder(sheet, rowIndex, 2, 10);
                        rowIndex++;
                        if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                        {
                            foreach (var detail in group.ListTechProcessVerDetail)
                            {
                                sheet.Cells[rowIndex, 2].Value = stt;
                                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 3].Value = detail.PhaseName;
                                sheet.Cells[rowIndex, 4].Value = detail.EquipmentName;
                                sheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 5].Value = detail.WorkerLevelName;
                                sheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 6].Value = Math.Round(detail.StandardTMU, 2);
                                sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 7].Formula = "$G$4";
                                //sheet.Cells[rowIndex, 7].Value = detail.Percent;
                                sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 8].Formula = "F" + rowIndex + "*100/G" + rowIndex; //Math.Round(detail.TimeByPercent, 2);
                                sheet.Cells[rowIndex, 8].Style.Font.Color.SetColor(Color.Blue);
                                sheet.Cells[rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 9].Formula = "H" + rowIndex + "*25/$H$" + techProcessInfo.LastRow; // Math.Round(detail.Worker, 2);
                                sheet.Cells[rowIndex, 9].Style.Font.Bold = true;
                                sheet.Cells[rowIndex, 9].Style.Font.Color.SetColor(Color.Red);
                                sheet.Cells[rowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[rowIndex, 10].Formula = "H" + rowIndex + "*" + techProcessInfo.PricePerSecond; //detail.TimeByPercent * techProcessInfo.PricePerSecond;
                                AddCellBorder(sheet, rowIndex, 2, 10);
                                stt++;
                                rowIndex++;
                            }
                        }
                        stt_group++;
                    }
                }
                sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                sheet.Cells[row, 2, rowIndex - 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                int timePerProduct_RowIndex = rowIndex;
                sheet.Cells[rowIndex, 3].Value = ("THỜI GIAN HOÀN THÀNH SẢN PHẨM").ToUpper();
                sheet.Cells[rowIndex, 8].Formula = "sum(" + sumFormula + ")"; // Math.Round(techProcessInfo.TimeCompletePerCommo);

                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;

                AddCellBorder(sheet, rowIndex, 2, 10);

                #endregion

                sheet.Cells.AutoFitColumns(5);
                sheet.Column(3).Width = 50;
                sheet.Column(14).Style.WrapText = true;
                #endregion

                #region sheet 2
                var sheetCN = excelPackage.Workbook.Worksheets.Add("CN");
                sheetCN.Name = "CN";
                sheetCN.Cells.Style.Font.Size = 12;
                sheetCN.Cells.Style.Font.Name = "Times New Roman";

                sheetCN.Cells[1, 2].Value = "DỰ KIẾN THỜI GIAN + NĂNG SUẤT";
                sheetCN.Cells[1, 2].Style.Font.Size = 14;
                sheetCN.Cells[1, 2, 1, 5].Merge = true;
                sheetCN.Cells[1, 2, 1, 5].Style.Font.Bold = true;
                sheetCN.Cells[1, 2].Style.WrapText = true;
                sheetCN.Cells[1, 2, 1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetCN.Cells[1, 2, 1, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //sheetCN.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheetCN.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                //sheetCN.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);
                sheetCN.Row(1).Height = 20;

                sheetCN.Cells[2, 2].Value = "Mô tả: " + techProcessInfo.Note;
                sheetCN.Cells[2, 2].Style.WrapText = true;
                sheetCN.Cells[2, 2, 2, 5].Merge = true;

                sheetCN.Cells[4, 2].Value = "Khách hàng: " + techProcessInfo.CustomerName;
                sheetCN.Cells[4, 2].Style.Font.Bold = true;
                sheetCN.Cells[4, 2].Style.WrapText = true;
                sheetCN.Cells[4, 2, 4, 5].Merge = true;

                sheetCN.Cells[5, 2].Value = "Mã hàng: " + techProcessInfo.ProductName;
                sheetCN.Cells[5, 2].Style.Font.Bold = true;
                sheetCN.Cells[5, 2].Style.WrapText = true;
                sheetCN.Cells[5, 2, 5, 5].Merge = true;

                double tgCT_minute = techProcessInfo.TimeCompletePerCommo / 60;

                sheetCN.Cells[6, 2].Value = "Thời gian chế tạo (s)";
                sheetCN.Cells[6, 3].Formula = "PT!H" + timePerProduct_RowIndex;// techProcessInfo.TimeCompletePerCommo;
                sheetCN.Cells[6, 4].Value = "Thời gian chế tạo (phút)";
                sheetCN.Cells[6, 5].Formula = "C6/60";// Math.Round(tgCT_minute, 2);

                sheetCN.Cells[7, 2].Value = "LĐ trực tiếp";
                sheetCN.Cells[7, 3].Value = techProcessInfo.NumberOfWorkers;
                sheetCN.Cells[7, 4].Value = "Đơn giá / phút";
                sheetCN.Cells[7, 5].Value = techProcessInfo.PricePerMinute;

                sheetCN.Cells[8, 2].Value = "Nhịp";
                sheetCN.Cells[8, 3].Formula = "C6/C7";// techProcessInfo.PacedProduction;
                sheetCN.Cells[8, 4].Value = "Tổng giá";
                sheetCN.Cells[8, 5].Formula = "=E6*E7";

                sheetCN.Cells[9, 2].Value = "NS tổ/h";
                sheetCN.Cells[9, 3].Formula = "3600/C8";// Math.Ceiling(techProcessInfo.ProOfGroupPerHour);
                sheetCN.Cells[9, 4].Value = "Doanh thu ngày ";
                sheetCN.Cells[9, 5].Formula = "=E8*C10";

                sheetCN.Cells[10, 2].Value = "NSTT/ngày/" + techProcessInfo.WorkingTimePerDay + "h";
                sheetCN.Cells[10, 3].Formula = "C9*" + techProcessInfo.WorkingTimePerDay;// Math.Ceiling(techProcessInfo.ProOfGroupPerDay);
                sheetCN.Cells[10, 4].Value = "Doanh thu Tháng";
                sheetCN.Cells[10, 5].Formula = "E9*26";

                sheetCN.Cells[11, 2].Value = "NSBQ/N/LĐ";
                sheetCN.Cells[11, 3].Formula = "=C10/C7";
                sheetCN.Cells[11, 4].Value = "DTBQ/N/LĐ";
                sheetCN.Cells[11, 5].Formula = "=E9/C7";
                for (int i = 4; i < 12; i++)
                    AddCellBorder(sheetCN, i, 2, 5);


                if (techProcessInfo.productImgs.Count > 0)
                {
                    int top = 50,
                        left = 350,
                        iWidth = 320,
                        iHeight = 320;
                    for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                    {
                        try
                        {
                            Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                            // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                            ExcelPicture excelPicture = sheetCN.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                            excelPicture.SetPosition(top, left);
                            excelPicture.SetSize(iWidth, iHeight);

                            top += 330;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    //https://www.youtube.com/watch?v=xwZW3-E4gBw
                }

                sheetCN.Cells[13, 2].Value = "PHÂN TÍCH CÔNG NGHỆ ";
                sheetCN.Cells[13, 2, 13, 5].Merge = true;
                sheetCN.Cells[13, 2, 13, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                AddCellBorder(sheetCN, 13, 2, 5);
                #region Thông Tin Thiết Bi 
                rowIndex = 14;
                sheetCN.Cells[rowIndex, 2].Value = "PHÂN TÍCH CÔNG NGHỆ  THEO QUI TRÌNH";
                sheetCN.Cells[rowIndex, 3].Value = "VẬT LIỆU";
                sheetCN.Cells[rowIndex, 4].Value = "THIẾT BỊ ";
                sheetCN.Cells[rowIndex, 5].Value = "SL MÁY";
                AddCellBorder(sheetCN, rowIndex, 2, 5);

                sheetCN.Cells[rowIndex, 2, rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                //sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);

                rowIndex++;
                if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
                {
                    for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                    {
                        var equipment = techProcessInfo.equipments[i];
                        double tong = 0;
                        foreach (var item in techProcessInfo.ListTechProcessGroup)
                        {
                            tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                        }
                        sheetCN.Cells[rowIndex, 2].Value = equipment.Name;
                        sheetCN.Cells[rowIndex, 5].Value = Math.Round(tong);

                        AddCellBorder(sheetCN, rowIndex, 2, 5);
                        rowIndex++;
                    }
                }

                //sheetCN.Cells[row, 2, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //sheetCN.Cells[row, 4, rowIndex,6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //sheetCN.Cells[row + 2, 2, rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                //sheetCN.Cells[rowIndex, 2, rowIndex, 6].Style.Font.Bold = true;
                sheetCN.Cells.AutoFitColumns(5);
                // sheetCN.Column(3).Width = 50;
                // sheetCN.Column(14).Style.WrapText = true;
                #endregion

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                DateTime dateNow = DateTime.Now;
                string fileName = "QTC_" + Regex.Replace( techProcessInfo.ProductName, "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();

                //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                //{
                //    CreatedDate = dateNow,
                //    UserId = UserContext.UserID,
                //    QTCNId = techProcessInfo.Id,
                //    Type = (int)eObjectType.isQTCN,
                //    Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu 5 "
                //});
            }
        }

        public void ExportToExcel_qtcn_dongtien(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\qtcn-dong-tien-template.xlsx"));

                using (var excelPackage = new ExcelPackage(_file))
                {
                    var workbook = excelPackage.Workbook;
                    var sheet = workbook.Worksheets.First();

                    sheet.Cells[3, 4].Value = techProcessInfo.ProductName;
                    sheet.Cells[4, 4].Value = DateTime.Now.ToString("MM/yyyy");
                    sheet.Cells[4, 9].Value = techProcessInfo.PricePerSecond;

                    int rowIndex = 6, row = 0, dem = 1, tongRow = 0;
                    double tongTG = 0, TongLĐ = 0;


                    #region Thong tin QTCN
                    if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                    {
                        row = rowIndex;

                        techProcessInfo.CompanyName = UserContext.CompanyName;
                        int stt = 1;
                        int stt_group = 1;
                        tongRow = rowIndex;
                        for (int i = 0; i < techProcessInfo.ListTechProcessGroup.Count; i++)
                        {
                            tongRow++;
                            tongRow += techProcessInfo.ListTechProcessGroup[i].ListTechProcessVerDetail.Count;
                        }


                        foreach (var group in techProcessInfo.ListTechProcessGroup)
                        {
                            sheet.Cells[rowIndex, 1].Value = stt_group;
                            sheet.Cells[rowIndex, 2].Value = group.PhaseGroupName.ToUpper();
                            //sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.StandardTMU), 2);
                            //sheet.Cells[rowIndex, 8].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                            //sheet.Cells[rowIndex, 9].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);
                            sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                            sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Font.Color.SetColor(Color.White);
                            sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Font.Bold = true;

                            AddCellBorder(sheet, rowIndex, 1, 9);
                            rowIndex++;
                            if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                            {
                                foreach (var detail in group.ListTechProcessVerDetail)
                                {
                                    sheet.Cells[rowIndex, 1].Value = stt;
                                    sheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[rowIndex, 2].Value = detail.PhaseName;
                                    sheet.Cells[rowIndex, 3].Value = detail.EquipmentName;
                                    try
                                    {
                                        sheet.Cells[rowIndex, 4].Value = Convert.ToInt32(detail.WorkerLevelName);
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    sheet.Cells[rowIndex, 5].Value = detail.StandardTMU;

                                    sheet.Cells[rowIndex, 6].Formula = "+E" + rowIndex + "/($E$" + tongRow + "/" + techProcessInfo.NumberOfWorkers + ")";

                                    // sheet.Cells[rowIndex, 7].Value = detail.Coefficient  ;
                                    sheet.Cells[rowIndex, 7].Formula = "IF(D" + rowIndex + "=2,0.9,IF(D" + rowIndex + "=3,1,IF(D" + rowIndex + "=4,1.1,0)))";


                                    //sheet.Cells[rowIndex, 8].Value = Math.Round(detail.Worker, 2);
                                    sheet.Cells[rowIndex, 8].Formula = "+E" + rowIndex + "*G" + rowIndex;

                                    sheet.Cells[rowIndex, 9].Formula = "+H" + rowIndex + "*$I$4";
                                    AddCellBorder(sheet, rowIndex, 1, 9);
                                    AlignCenter(sheet, rowIndex, 3, 9);
                                    stt++;
                                    rowIndex++;
                                }
                            }
                            stt_group++;
                        }
                    }

                    sheet.Cells[rowIndex, 2].Value = "TỔNG";
                    sheet.Cells[rowIndex, 2].Style.Font.Color.SetColor(Color.Red);
                    sheet.Cells[rowIndex, 5].Formula = "SUM(E7:E" + (rowIndex - 1) + ")";
                    sheet.Cells[rowIndex, 6].Formula = "SUM(F7:F" + (rowIndex - 1) + ")";
                    sheet.Cells[rowIndex, 8].Formula = "SUM(H7:H" + (rowIndex - 1) + ")";
                    sheet.Cells[rowIndex, 9].Formula = "SUM(I7:I" + (rowIndex - 1) + ")";
                    sheet.Cells[rowIndex, 5, rowIndex, 9].Style.Font.Color.SetColor(Color.Blue);
                    //sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    //sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Font.Color.SetColor(Color.White);
                    sheet.Cells[rowIndex, 1, rowIndex, 9].Style.Font.Bold = true;

                    AddCellBorder(sheet, rowIndex, 1, 9);
                    AlignCenter(sheet, rowIndex, 1, 9);

                    // int tongRow = rowIndex;
                    rowIndex += 2;

                    sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                    sheet.Cells[row, 2, rowIndex - 2, 9].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    sheet.Cells[rowIndex, 8].Formula = "+H" + tongRow + "/60";
                    sheet.Cells[rowIndex, 9].Value = "phút";

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Năng suất thiết kế:";
                    sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Cells[rowIndex, 3].Formula = "+C" + (rowIndex + 1) + "*F" + tongRow + "/H" + tongRow;
                    sheet.Cells[rowIndex, 4].Value = "SP/NGÀY";
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Size = 14;
                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Blue);

                    sheet.Cells[rowIndex, 6].Value = "SÓ LƯỢNG";
                    sheet.Cells[rowIndex, 7].Value = "HIỆU SUẤT";
                    sheet.Cells[rowIndex, 8].Value = "NĂNG SUẤT";
                    sheet.Cells[rowIndex, 9].Value = "NGÀY LÀM VIỆC";
                    sheet.Cells[rowIndex, 4, rowIndex, 9].Style.Font.Size = 11;
                    AddCellBorder(sheet, rowIndex, 6, 9);

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Tổng thời gian làm việc:";
                    sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Cells[rowIndex, 3].Value = techProcessInfo.WorkingTimePerDay * 3600;
                    sheet.Cells[rowIndex, 4].Value = "Giây";
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Size = 14;
                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Blue);

                    sheet.Cells[rowIndex, 6].Value = "500";
                    sheet.Cells[rowIndex, 7].Value = "35%";
                    sheet.Cells[rowIndex, 8].Formula = "+$C$" + (rowIndex - 1) + "*G" + (rowIndex);
                    sheet.Cells[rowIndex, 9].Formula = "+F" + rowIndex + "/H" + rowIndex;
                    sheet.Cells[rowIndex, 4, rowIndex, 9].Style.Font.Size = 11;
                    AddCellBorder(sheet, rowIndex, 6, 9);

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Nhịp độ sản xuất:";
                    sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Cells[rowIndex, 3].Formula = "+C" + (rowIndex - 1) + "/C" + (rowIndex - 2);
                    sheet.Cells[rowIndex, 4].Value = "Giây";
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Size = 14;
                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Blue);
                    sheet.Cells[rowIndex, 4].Style.Font.Size = 11;

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "Năng suất bình quân đầu người:";
                    sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Cells[rowIndex, 3].Formula = "+$C$" + (rowIndex - 3) + "/$F$" + tongRow;
                    sheet.Cells[rowIndex, 4].Value = "SP/NGƯỜI";
                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Blue);
                    sheet.Cells[rowIndex, 4].Style.Font.Size = 11;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 3].Style.Font.Size = 14;

                    rowIndex += 2;

                    sheet.Cells[rowIndex, 1].Value = "* LƯU Ý :  - THỜI GIAN TRONG QUY TRÌNH TRÊN CHỈ Ở MỨC ĐỘ TƯƠNG ĐỐI. - THỜI GIAN MỘT SỐ CÔNG ĐOẠN CÓ THỂ KHÔNG PHÙ HỢP VỚI THỰC TẾ SẢN XUẤT TẠI TỪNG ĐƠN VỊ.NHÂN VIÊN QTCN XÍ NGHIỆP VUI LÒNG CẬP NHẬT THỜI GIAN Ở CỘT(F5) CHO PHÙ HỢP VỚI THỰC TẾ.";
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Merge = true;
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Style.Font.Size = 14;
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Style.WrapText = true;
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    sheet.Cells[rowIndex, 1, rowIndex + 3, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    AddCellBorder(sheet, rowIndex, 1, 9);

                    rowIndex += 5;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                    sheet.Cells[rowIndex, 2, rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowIndex, 2, rowIndex, 6].Value = "BẢNG CẬP NHẬT LỊCH SỬ MÃ HÀNG";

                    rowIndex++;
                    sheet.Cells[rowIndex, 2].Value = "LỊCH SỬ MÃ HÀNG (Ngày/tháng/năm)";
                    sheet.Cells[rowIndex, 3].Value = "ĐƠN VỊ";
                    sheet.Cells[rowIndex, 4].Value = "TỔ";
                    sheet.Cells[rowIndex, 5].Value = "NĂNG SUẤT";
                    sheet.Cells[rowIndex, 6].Value = "BÌNH QUÂN NS/LĐ";
                    AddCellBorder(sheet, rowIndex, 2, 6);
                    AlignCenter(sheet, rowIndex, 2, 6);

                    rowIndex++;
                    AddCellBorder(sheet, rowIndex, 2, 6);
                    AlignCenter(sheet, rowIndex, 2, 6);

                    rowIndex++;
                    #endregion

                    #region Thông Tin Thiết Bi
                    row = rowIndex;
                    rowIndex = 21;
                    if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
                    {
                        for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                        {
                            var equipment = techProcessInfo.equipments[i];
                            double tong = 0;
                            foreach (var item in techProcessInfo.ListTechProcessGroup)
                            {
                                tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                            }
                            sheet.Cells[rowIndex, 11].Value = equipment.Name;
                            sheet.Cells[rowIndex, 12].Value = tong;
                            sheet.Cells[rowIndex, 13].Formula = "IF(AND(L" + rowIndex + ">=1,L" + rowIndex + "<=5),1,IF(AND(L" + rowIndex + ">5,L" + rowIndex + "<=10),2,IF(L" + rowIndex + ">10,3,0)))";


                            AddCellBorder(sheet, rowIndex, 11, 13);
                            AlignCenter(sheet, rowIndex, 11, 13);
                            rowIndex++;
                        }
                        sheet.Cells[rowIndex, 11].Value = "Tổng";
                        sheet.Cells[rowIndex, 12].Formula = "SUM(L20:L" + (rowIndex - 1) + ")";
                        sheet.Cells[rowIndex, 13].Formula = "SUM(M20:M" + (rowIndex - 1) + ")";
                        sheet.Cells[rowIndex, 11, rowIndex, 13].Style.Font.Color.SetColor(Color.Blue);
                        AddCellBorder(sheet, rowIndex, 11, 13);
                        AlignCenter(sheet, rowIndex, 11, 13);
                    }
                    rowIndex += 2;
                    #endregion

                    sheet.Cells[rowIndex, 12].Value = "Bậc ";
                    sheet.Cells[rowIndex, 13].Value = "Hệ số";
                    AddCellBorder(sheet, rowIndex, 12, 13);
                    AlignCenter(sheet, rowIndex, 12, 13);
                    rowIndex++;

                    var bactho = BLLWorkerLevel.Instance.Gets(UserContext.CompanyId, UserContext.ChildCompanyId);
                    if (bactho.Count > 0)
                    {
                        for (int i = 0; i < bactho.Count; i++)
                        {
                            if (bactho[i].Value > 0)
                            {
                                sheet.Cells[rowIndex, 12].Value = bactho[i].Name;
                                sheet.Cells[rowIndex, 13].Value = bactho[i].Double;
                                AddCellBorder(sheet, rowIndex, 12, 13);
                                AlignCenter(sheet, rowIndex, 12, 13);
                                rowIndex++;
                            }
                        }
                    }


                    if (techProcessInfo.productImgs.Count > 0)
                    {
                        int top = 10,
                            left = 1030,
                            iWidth = 320,
                            iHeight = 320;
                        for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                        {
                            Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                            // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                            ExcelPicture excelPicture = sheet.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                            excelPicture.SetPosition(top, left);
                            excelPicture.SetSize(iWidth, iHeight);

                            top += 330;
                        }
                    }

                    rowIndex += 2;

                    sheet.Cells.AutoFitColumns(5);
                    sheet.Column(3).Width = 50;
                    sheet.Column(14).Style.WrapText = true;
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "QTC_" + Regex.Replace( techProcessInfo.ProductName, "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        /// <summary>
        /// Quy Trình Công Nghệ mẫu 7 việt thành
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_7(int parentId, bool isGetNull)
        {
            var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
            var company = BLLCompany.Instance.GetById(UserContext.CompanyId);

            var excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "IED";
            excelPackage.Workbook.Properties.Title = "Quy trình công nghệ làm lương";
            var sheet = excelPackage.Workbook.Worksheets.Add("Quy trình công nghệ làm lương");
            sheet.Name = "Quy trình công nghệ làm lương";
            sheet.Cells.Style.Font.Size = 12;
            sheet.Cells.Style.Font.Name = "Times New Roman";

            sheet.Cells[1, 1].Value = company != null ? company.CompanyName.ToUpper() : "";
            sheet.Cells[1, 1].Style.Font.Size = 13;
            sheet.Cells[1, 1, 1, 6].Merge = true;

            sheet.Cells[2, 2].Value = ("xí nghiệp: ").ToUpper();
            sheet.Cells[2, 3].Value = ("mã hàng: " + techProcessInfo.ProductName).ToUpper();
            sheet.Cells[2, 3, 2, 6].Merge = true;
            sheet.Cells[3, 2].Value = ("khách hàng: " + techProcessInfo.CustomerName).ToUpper();
            sheet.Cells[3, 3].Value = ("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy")).ToUpper();
            sheet.Cells[3, 3, 3, 6].Merge = true;

            sheet.Cells[1, 1, 3, 3].Style.Font.Size = 14;
            sheet.Cells[2, 2, 3, 3].Style.Font.Bold = true;

            //sheet.Row(1).Height = 20;
            sheet.Cells[4, 1].Value = "QUY TRÌNH CÔNG NGHỆ SẢN XUẤT";
            sheet.Cells[4, 1].Style.Font.Size = 14;
            sheet.Cells[4, 1].Style.Font.Bold = true;
            sheet.Cells[4, 1, 4, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[4, 1, 4, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[4, 1, 4, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            sheet.Cells[4, 1, 4, 6].Style.Font.Color.SetColor(Color.White);
            sheet.Cells[4, 1, 4, 6].Merge = true;
            sheet.Row(4).Height = 20;

            int rowIndex = 5, row = 0;

            #region Thong tin QTCN
            if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
            {
                row = rowIndex;
                sheet.Cells[rowIndex, 1].Value = "STT";
                sheet.Cells[rowIndex, 2].Value = "Tên Công Đoạn";
                sheet.Cells[rowIndex, 3].Value = "Thời gian chuẩn";
                sheet.Cells[rowIndex, 4].Value = "Lao Động";
                sheet.Cells[rowIndex, 5].Value = "Thiết Bị ";
                sheet.Cells[rowIndex, 6].Value = "Bậc Thợ";
                sheet.Cells[rowIndex, 1, rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 1, rowIndex, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[rowIndex, 1, rowIndex, 6].Style.Font.Bold = true;
                sheet.Row(rowIndex).Height = 25;
                AddCellBorder(sheet, rowIndex, 2, 6);


                int stt = 1;
                int stt_group = 1;
                rowIndex++;
                foreach (var group in techProcessInfo.ListTechProcessGroup)
                {
                    //sheet.Cells[rowIndex, 1].Value = stt_group;
                    sheet.Cells[rowIndex, 2].Value = group.PhaseGroupName.ToUpper();
                    sheet.Cells[rowIndex, 3].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent), 2);
                    sheet.Cells[rowIndex, 4].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker), 2);

                    sheet.Cells[rowIndex, 1, rowIndex, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 1, rowIndex, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 1, rowIndex, 6].Style.Font.Color.SetColor(Color.White);
                    sheet.Cells[rowIndex, 1, rowIndex, 6].Style.Font.Bold = true;

                    AddCellBorder(sheet, rowIndex, 1, 6);
                    rowIndex++;
                    if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                    {
                        foreach (var detail in group.ListTechProcessVerDetail)
                        {
                            sheet.Cells[rowIndex, 1].Value = stt;
                            sheet.Cells[rowIndex, 2].Value = detail.PhaseName;
                            sheet.Cells[rowIndex, 3].Value = Math.Round(detail.TimeByPercent, 2);
                            sheet.Cells[rowIndex, 4].Value = Math.Round(detail.Worker, 2);
                            sheet.Cells[rowIndex, 5].Value = detail.EquipmentName;
                            sheet.Cells[rowIndex, 6].Value = detail.WorkerLevelName;

                            sheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[rowIndex, 3, rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
                            AddCellBorder(sheet, rowIndex, 1, 6);
                            stt++;
                            rowIndex++;
                        }
                    }
                    stt_group++;
                }
                sheet.Cells[rowIndex, 2].Value = "Tổng cộng".ToUpper();
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.TimeCompletePerCommo, 2);
                sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                sheet.Cells[rowIndex, 2, rowIndex, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 2, rowIndex, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                sheet.Cells[rowIndex, 3, rowIndex, 6].Style.Font.Bold = true;
                rowIndex += 2;

                sheet.Cells[rowIndex, 2].Value = "Thời gian làm việc trong ngày";
                sheet.Cells[rowIndex, 3].Value = techProcessInfo.WorkingTimePerDay * 3600;
                sheet.Cells[rowIndex, 4].Value = "Giây/ngày";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Tổng thời gian chế tạo";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.TimeCompletePerCommo, 2);
                sheet.Cells[rowIndex, 4].Value = "Giây/SP";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Nhịp độ sản xuất";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.PacedProduction, 0);
                sheet.Cells[rowIndex, 4].Value = "Giây/SP/LĐ";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Năng suất bình quân / Người";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfPersonPerDay, 0);
                sheet.Cells[rowIndex, 4].Value = "SP/người";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Năng suất của Chuyền / giờ  ";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfGroupPerHour, 0);
                sheet.Cells[rowIndex, 4].Value = "SP ";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Năng suất Chuyền / ngày  ";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfGroupPerDay, 0);
                sheet.Cells[rowIndex, 4].Value = "SP ";
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = "Số Lao động";
                sheet.Cells[rowIndex, 3].Value = techProcessInfo.NumberOfWorkers;
                sheet.Cells[rowIndex, 4].Value = "LĐ";
                sheet.Cells[rowIndex - 6, 2, rowIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex - 6, 2, rowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                sheet.Cells[rowIndex - 6, 3, rowIndex, 3].Style.Font.Bold = true;
                sheet.Cells[rowIndex - 6, 3, rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                AddCellBorder(sheet, rowIndex, 2, 4);
                rowIndex += 2;

                sheet.Cells[rowIndex, 1].Value = "NĂNG SUẤT THEO PHẦN TRĂM";
                sheet.Cells[rowIndex, 3].Value = "50%";
                sheet.Cells[rowIndex, 4].Value = "60%";
                sheet.Cells[rowIndex, 5].Value = "70%";
                sheet.Cells[rowIndex, 6].Value = "75%";
                sheet.Cells[rowIndex, 7].Value = "80%";
                sheet.Cells[rowIndex, 8].Value = "85%";
                sheet.Cells[rowIndex, 9].Value = "90%";
                sheet.Cells[rowIndex, 10].Value = "100%";
                sheet.Cells[rowIndex, 1, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 1, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                sheet.Cells[rowIndex, 1, rowIndex, 10].Style.Font.Bold = true;
                sheet.Cells[rowIndex, 1, rowIndex, 2].Merge = true;
                AddCellBorder(sheet, rowIndex, 1, 10);
                rowIndex++;

                sheet.Cells[rowIndex, 1].Value = "1";
                sheet.Cells[rowIndex, 2].Value = "Năng suất bình quân / Người";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.5, 0);
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.6, 0);
                sheet.Cells[rowIndex, 5].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.7, 0);
                sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.75, 0);
                sheet.Cells[rowIndex, 7].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.8, 0);
                sheet.Cells[rowIndex, 8].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.85, 0);
                sheet.Cells[rowIndex, 9].Value = Math.Round(techProcessInfo.ProOfPersonPerDay * 0.9, 0);
                sheet.Cells[rowIndex, 10].Value = Math.Round(techProcessInfo.ProOfPersonPerDay, 0);
                AddCellBorder(sheet, rowIndex, 1, 10);
                rowIndex++;
                sheet.Cells[rowIndex, 1].Value = "2";
                sheet.Cells[rowIndex, 2].Value = "Năng suất của Chuyền / giờ ";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.5, 0);
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.6, 0);
                sheet.Cells[rowIndex, 5].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.7, 0);
                sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.75, 0);
                sheet.Cells[rowIndex, 7].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.8, 0);
                sheet.Cells[rowIndex, 8].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.85, 0);
                sheet.Cells[rowIndex, 9].Value = Math.Round(techProcessInfo.ProOfGroupPerHour * 0.9, 0);
                sheet.Cells[rowIndex, 10].Value = Math.Round(techProcessInfo.ProOfGroupPerHour, 0);
                AddCellBorder(sheet, rowIndex, 1, 10);
                rowIndex++;

                sheet.Cells[rowIndex, 1].Value = "3";
                sheet.Cells[rowIndex, 2].Value = "Năng suất Chuyền / ngày";
                sheet.Cells[rowIndex, 3].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.5, 0);
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.6, 0);
                sheet.Cells[rowIndex, 5].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.7, 0);
                sheet.Cells[rowIndex, 6].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.75, 0);
                sheet.Cells[rowIndex, 7].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.8, 0);
                sheet.Cells[rowIndex, 8].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.85, 0);
                sheet.Cells[rowIndex, 9].Value = Math.Round(techProcessInfo.ProOfGroupPerDay * 0.9, 0);
                sheet.Cells[rowIndex, 10].Value = Math.Round(techProcessInfo.ProOfGroupPerDay, 0);
                AddCellBorder(sheet, rowIndex, 1, 10);
                rowIndex++;
            }

            #endregion
            rowIndex += 2;
            sheet.Cells[rowIndex, 2].Value = " TP. KỸ THUẬT CÔNG NGHỆ    	                              KT TRIỂN KHAI                                     NHÂN VIÊN QT".ToUpper();
            sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 2].Style.Font.Size = 12;
            sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;

            rowIndex += 5;
            sheet.Cells[rowIndex, 2].Value = "PHÒNG IE                                       KAIZEN/PHÓ GĐSX                                          GIÁM ĐỐC XN".ToUpper();
            sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 2].Style.Font.Size = 12;
            sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
            sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
            sheet.Cells[1, 1, rowIndex + 4, 13].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Blue);


            #region Thông Tin Thiết Bi
            sheet.Cells[4, 8].Value = "TỔNG HỢP THIẾT BỊ SỬ DỤNG THEO QUY TRÌNH";
            sheet.Cells[4, 8, 4, 12].Merge = true;
            sheet.Cells[4, 8].Style.Font.Size = 12;
            sheet.Cells[4, 8].Style.Font.Bold = true;

            sheet.Cells[5, 8].Value = "STT";
            sheet.Cells[5, 9].Value = "Tên Thiết Bị";
            sheet.Cells[5, 10].Value = "QT";
            sheet.Cells[5, 11].Value = "Làm tròn";
            sheet.Cells[5, 12].Value = "Thực tế";
            sheet.Cells[5, 8, 5, 12].Style.Font.Size = 12;
            sheet.Cells[5, 8, 5, 12].Style.Font.Bold = true;
            sheet.Cells[5, 8, 5, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[5, 8, 5, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[5, 8, 5, 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            sheet.Cells[5, 8, 5, 12].Style.Font.Color.SetColor(Color.White);
            AddCellBorder(sheet, 5, 8, 12);
            rowIndex = 6;
            if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
            {
                for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                {
                    var equipment = techProcessInfo.equipments[i];
                    double tong = 0;
                    foreach (var item in techProcessInfo.ListTechProcessGroup)
                    {
                        tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                    }
                    sheet.Cells[rowIndex, 8].Value = i + 1;
                    sheet.Cells[rowIndex, 9].Value = equipment.Name;
                    sheet.Cells[rowIndex, 10].Value = Math.Round(tong, 2);
                    sheet.Cells[rowIndex, 11].Value = Math.Round(tong);
                    AddCellBorder(sheet, rowIndex, 8, 12);
                    rowIndex++;
                }

                sheet.Cells[rowIndex, 8].Value = "TỔNG";
                sheet.Cells[rowIndex, 10].Formula = "SUM(J6:J" + (rowIndex - 1) + ")";
                sheet.Cells[rowIndex, 11].Formula = "SUM(K6:K" + (rowIndex - 1) + ")";
                sheet.Cells[rowIndex, 8, rowIndex, 12].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 8, 12);
                rowIndex += 2;
            }
            #endregion

            #region Phase Group     
            sheet.Cells[rowIndex, 8].Value = "Cụm";
            sheet.Cells[rowIndex, 9].Value = "Tên cụm";
            sheet.Cells[rowIndex, 10].Value = "TG cụm";
            sheet.Cells[rowIndex, 11].Value = "HSLĐ";
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Size = 12;
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Color.SetColor(Color.White);
            AddCellBorder(sheet, rowIndex, 8, 11);
            rowIndex++;
            if (techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
            {
                for (int i = 0; i < techProcessInfo.ListTechProcessGroup.Count; i++)
                {
                    var group = techProcessInfo.ListTechProcessGroup[i];
                    sheet.Cells[rowIndex, 8].Value = i + 1;
                    sheet.Cells[rowIndex, 9].Value = group.PhaseGroupName;
                    sheet.Cells[rowIndex, 10].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent));
                    sheet.Cells[rowIndex, 11].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.Worker));
                    AddCellBorder(sheet, rowIndex, 8, 11);
                    rowIndex++;
                }

                sheet.Cells[rowIndex, 8].Value = "TỔNG";
                sheet.Cells[rowIndex, 8, rowIndex, 9].Merge = true;
                sheet.Cells[rowIndex, 10].Formula = "SUM(J" + (rowIndex - techProcessInfo.ListTechProcessGroup.Count - 2) + ":J" + (rowIndex - 1) + ")";
                sheet.Cells[rowIndex, 11].Formula = "SUM(K" + (rowIndex - techProcessInfo.ListTechProcessGroup.Count - 2) + ":K" + (rowIndex - 1) + ")";
                sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 8, 11);
                rowIndex++;
            }
            #endregion


            sheet.Cells.AutoFitColumns(5);
            sheet.Column(3).Width = 50;
            sheet.Column(14).Style.WrapText = true;
            Response.ClearContent();
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            DateTime dateNow = DateTime.Now;
            //string fileName = "QTC_" + techProcessInfo. + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            string fileName = "QTC_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/excel";
            Response.Flush();
            Response.End();

            //BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
            //{
            //    CreatedDate = dateNow,
            //    UserId = UserContext.UserID,
            //    QTCNId = techProcessInfo.Id,
            //    Type = (int)eObjectType.isQTCN,
            //    Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu làm lương "
            //});
        }

        /// <summary>
        /// Quy Trình Công Nghệ mẫu 8 Genviet
        /// </summary> 
        public void ExportToExcel_8(int parentId, bool isGetNull)
        {
            if (isAuthenticate)
            {
                var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\qtcn-genviet-template.xlsx"));

                using (var excelPackage = new ExcelPackage(_file))
                {
                    var workbook = excelPackage.Workbook;
                    var sheet = workbook.Worksheets.First();

                    #region Sheet 1
                    sheet.Cells[3, 4].Value = techProcessInfo.ProductName;

                    sheet.Cells[5, 8].Value = techProcessInfo.PacedProduction;

                    sheet.Cells[6, 4].Value = techProcessInfo.Pro_PercentHelp;
                    sheet.Cells[6, 8].Value = techProcessInfo.NumberOfWorkers;

                    sheet.Cells[7, 4].Value = techProcessInfo.PricePerSecond;
                    sheet.Cells[7, 9].Value = techProcessInfo.ProOfPersonPerDay;

                    int rowIndex = 10, row = 0, tongRow = 0;
                    string TGTT = "", TGKhoan = "", TGLuong = "", DGia = "", tongDG = "", LD = "";
                    if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
                    {
                        row = rowIndex;
                        techProcessInfo.CompanyName = UserContext.CompanyName;
                        int stt = 1;
                        int stt_group = 1;
                        tongRow = rowIndex;
                        for (int i = 0; i < techProcessInfo.ListTechProcessGroup.Count; i++)
                        {
                            tongRow++;
                            tongRow += techProcessInfo.ListTechProcessGroup[i].ListTechProcessVerDetail.Count;
                        }

                        foreach (var group in techProcessInfo.ListTechProcessGroup)
                        {
                            //sheet.Cells[rowIndex, 1].Value = stt_group;
                            sheet.Cells[rowIndex, 4].Value = group.PhaseGroupName.ToUpper();
                            sheet.Cells[rowIndex, 4].Style.HorizontalAlignment =  ExcelHorizontalAlignment.Center;

                            sheet.Cells[rowIndex, 6].Formula = "SUM(F" + (rowIndex + 1) + ":F" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                            sheet.Cells[rowIndex, 10].Formula = "SUM(J" + (rowIndex + 1) + ":J" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                            sheet.Cells[rowIndex, 13].Formula = "SUM(M" + (rowIndex + 1) + ":M" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                            sheet.Cells[rowIndex, 14].Formula = "SUM(N" + (rowIndex + 1) + ":N" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                            sheet.Cells[rowIndex, 16].Formula = "SUM(P" + (rowIndex + 1) + ":P" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";
                            sheet.Cells[rowIndex, 17].Formula = "SUM(Q" + (rowIndex + 1) + ":Q" + (rowIndex + group.ListTechProcessVerDetail.Count) + ")";

                            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 191, 255));
                            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Font.Color.SetColor(Color.Black);
                            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Font.Bold = true;

                            TGTT += "+F" + rowIndex;
                            TGKhoan += "+J" + rowIndex;
                            TGLuong += "+M" + rowIndex;
                            DGia += "+N" + rowIndex;
                            tongDG += "+P" + rowIndex;
                            LD += "+Q" + rowIndex;

                            AddCellBorder(sheet, rowIndex, 1, 21);
                            rowIndex++;
                            if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                            {
                                foreach (var detail in group.ListTechProcessVerDetail)
                                {
                                    sheet.Cells[rowIndex, 1].Value = stt;                                   
                                    sheet.Cells[rowIndex, 2].Value = detail.PhaseCode;
                                    sheet.Cells[rowIndex, 3].Value = detail.PhaseGroupCode;
                                    sheet.Cells[rowIndex, 4].Value = detail.PhaseName;
                                    sheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    sheet.Cells[rowIndex, 4].Style.WrapText = true;
                                    sheet.Cells[rowIndex, 5].Value = detail.EquipmentName;
                                    sheet.Cells[rowIndex, 6].Value =Math.Round( detail.TMUThaoTac,1);
                                    sheet.Cells[rowIndex, 7].Value = Math.Round(detail.TimePrepare,1);
                                    sheet.Cells[rowIndex, 8].Formula = "(F" + rowIndex + "*" + detail.HaoPhiThaoTac + ")/100";
                                    sheet.Cells[rowIndex, 9].Formula = "(F" + rowIndex + "*" + detail.HaoPhiThietBi + ")/100"; 
                                    sheet.Cells[rowIndex, 10].Value = Math.Round(detail.StandardTMU,1);
                                    sheet.Cells[rowIndex, 11].Value = detail.WorkerLevelName;
                                    sheet.Cells[rowIndex, 12].Value = detail.Coefficient;
                                    sheet.Cells[rowIndex, 13].Formula = "J" + rowIndex + "*L" + rowIndex;
                                    sheet.Cells[rowIndex, 14].Formula = "M" + rowIndex + "*D7";
                                    sheet.Cells[rowIndex, 15].Formula = "D6";
                                    sheet.Cells[rowIndex, 16].Formula = "N" + rowIndex + "+((N" + rowIndex + "*O" + rowIndex + ")/100)";
                                    sheet.Cells[rowIndex, 17].Formula = "J" + rowIndex + "/H5";
                                    sheet.Cells[rowIndex, 18].Value = detail.Description;
                                    sheet.Cells[rowIndex, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
                                    sheet.Cells[rowIndex, 18].Style.WrapText = true;
                                    sheet.Cells[rowIndex, 19].Value = techProcessInfo.DonVi;
                                    sheet.Cells[rowIndex, 20].Value = techProcessInfo.WorkShopName;
                                    sheet.Cells[rowIndex, 21].Value = techProcessInfo.NhomMe;

                                    sheet.Cells[rowIndex, 13, rowIndex, 17].Style.Font.Color.SetColor(Color.Red);
                                    AddCellBorder(sheet, rowIndex, 1, 21);
                                    AlignCenter(sheet, rowIndex, 1, 3);
                                    AlignCenter(sheet, rowIndex, 5, 17);
                                    AlignCenter(sheet, rowIndex, 19, 21);
                                    stt++;
                                    rowIndex++;
                                }
                            }
                            stt_group++;
                        }
                    }

                    sheet.Cells[rowIndex, 4].Value = "TỔNG THỜI GIAN";
                    sheet.Cells[rowIndex, 6].Formula = TGTT;
                    sheet.Cells[rowIndex, 10].Formula = TGKhoan;
                    sheet.Cells[rowIndex, 13].Formula = TGLuong;
                    sheet.Cells[rowIndex, 14].Formula = DGia;
                    sheet.Cells[rowIndex, 16].Formula = tongDG;
                    sheet.Cells[rowIndex, 17].Formula = LD;
                    sheet.Cells[rowIndex, 13, rowIndex, 17].Style.Font.Color.SetColor(Color.Red);
                    sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                    //sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Font.Color.SetColor(Color.White);
                    sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Font.Bold = true;

                    sheet.Cells[3, 8].Formula = "P" + rowIndex;
                    sheet.Cells[4, 8].Formula = "J" + rowIndex;

                    AddCellBorder(sheet, rowIndex, 1, 21);
                    AlignCenter(sheet, rowIndex, 13, 21);
                    rowIndex+=2;

                    sheet.Cells[rowIndex, 13].Value = "Hà nam ,Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                    rowIndex++;

                    sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
                    //sheet.Cells[row, 2, rowIndex - 2, 9].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    rowIndex++;
                    sheet.Cells[rowIndex, 1].Value = "   BAN GIÁM ĐỐC                                                                               TP KỸ THUẬT                                                                                       IE ".ToUpper();
                    sheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 1].Style.Font.Size = 12;
                    sheet.Cells[rowIndex, 1, rowIndex, 21].Merge = true;
                    AlignCenter(sheet, rowIndex, 1, 21);
                    #endregion

                    sheet = workbook.Worksheets[2];
                    rowIndex = 6;
                    sheet.Cells[2, 2].Value = "Mã hàng : " + techProcessInfo.ProductName;
                    #region Sheet 2
                    if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
                    {
                        for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                        {
                            var equipment = techProcessInfo.equipments[i];
                            sheet.Cells[rowIndex, 1].Value = i + 1;
                            sheet.Cells[rowIndex, 2].Value = equipment.Name;

                            double tong = 0;
                            foreach (var item in techProcessInfo.ListTechProcessGroup)
                                tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
 
                            sheet.Cells[rowIndex, 4].Value = Math.Round(tong,2);
                            sheet.Cells[rowIndex, 5].Value = (int)Math.Round(tong,  MidpointRounding.AwayFromZero);

                            AddCellBorder(sheet, rowIndex, 1, 7);
                            AlignCenter(sheet, rowIndex, 1, 7);
                            rowIndex++;
                        }
                        sheet.Cells[rowIndex, 1].Value = "Tổng";
                        sheet.Cells[rowIndex, 1, rowIndex, 3].Merge = true;
                        sheet.Cells[rowIndex, 4].Formula = "SUM(D6:D" + (rowIndex - 1) + ")";
                        sheet.Cells[rowIndex, 5].Formula = "SUM(E6:E" + (rowIndex - 1) + ")";
                        sheet.Cells[rowIndex, 1, rowIndex, 7].Style.Font.Color.SetColor(Color.Blue);
                        AddCellBorder(sheet, rowIndex, 1, 7);
                        AlignCenter(sheet, rowIndex, 1, 7);

                        rowIndex++;
                        sheet.Cells[rowIndex, 4].Value = "Hà nam , Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                        rowIndex++;
                        sheet.Cells[rowIndex, 1].Value = "   BAN GIÁM ĐỐC                                                       TP KỸ THUẬT                                                            IE                                           ".ToUpper();
                        sheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                        sheet.Cells[rowIndex, 1].Style.Font.Size = 12;
                        sheet.Cells[rowIndex, 1, rowIndex, 7].Merge = true;
                        AlignCenter(sheet, rowIndex, 1, 7);
                    }
                    rowIndex += 2;

                    #endregion

                    if (techProcessInfo.productImgs.Count > 0)
                    {
                        int top = 10,
                            left = 1030,
                            iWidth = 320,
                            iHeight = 320;
                        for (int i = 0; i < techProcessInfo.productImgs.Count; i++)
                        {
                            Image img = Image.FromFile(Server.MapPath("~" + techProcessInfo.productImgs[i].Code));
                            // Image img = Image.FromFile(Server.MapPath("http://112.197.117.97:86/" + techProcessInfo.productImgs[0].Code));
                            ExcelPicture excelPicture = sheet.Drawings.AddPicture(techProcessInfo.productImgs[i].Name, img);
                            excelPicture.SetPosition(top, left);
                            excelPicture.SetSize(iWidth, iHeight);

                            top += 330;
                        }
                    }

                    rowIndex += 2;

                    sheet.Cells.AutoFitColumns(5);
                    sheet.Column(3).Width = 50;
                    sheet.Column(14).Style.WrapText = true;
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "QTC_" + Regex.Replace( techProcessInfo.ProductName, "[^\\w]", "_") + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
                }
            }
        }

        #endregion
        #endregion

        #region thiết kế chuyền
        [HttpPost]
        public JsonResult Gets_TKC(int parentId, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                var listWorkShop = BLLLabourDivision.Instance.GetList(parentId, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listWorkShop;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listWorkShop.TotalItemCount;
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Gets_TKC_Ver(int labourId, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                var listWorkShop = BLLLabourDivision.Instance.GetList_Version(labourId, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listWorkShop;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listWorkShop.TotalItemCount;
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult SaveTKC(string model, bool isSubmit)
        {
            ResponseBase rs = null;
            try
            {
                LabourDivisionModel obj = JsonConvert.DeserializeObject<LabourDivisionModel>(model);
                if (obj.TechProVer_Id == 0)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi", Message = "Quy trình công nghệ chưa được tạo. Bạn cần phải lưu quy trình công nghệ trước rồi mới có thể lưu thiết kế chuyền được !." });
                }
                else
                {
                    obj.ActionUser = UserContext.UserID;
                    if (obj.Id == 0)
                        rs = BLLLabourDivision.Instance.Insert(obj);
                    if (obj.Id != 0 && isSubmit)
                        rs = BLLLabourDivision.Instance.Update(obj);
                    if (obj.Id != 0 && !isSubmit)
                        rs = BLLLabourDivision.Instance.UpdateCover(obj);

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
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult DeleteTKC(int Id)
        {
            ResponseBase rs;
            try
            {
                rs = BLLLabourDivision.Instance.Delete(Id);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        public JsonResult GetTKCById(int labourId)
        {
            try
            {
                JsonDataResult.Result = "OK";
                var diagram = BLLLabourDivision.Instance.GetById(labourId);
                JsonDataResult.Records = diagram;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult RefreshTKCById(int labourId)
        {
            try
            {
                JsonDataResult.Result = "OK";
                BLLLabourDivision.Instance.RefreshById(labourId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Active(int labourVerId)
        {
            ResponseBase rs = null;
            try
            {
                rs = BLLLabourDivision.Instance.Active(labourVerId);
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


        #region Excel
        public void ExportDiagramToExcel(int Id)
        {
            #region GetData
            List<EmployeeSmallModel> employees = new List<EmployeeSmallModel>();
            var diagram = BLLLabourDivision.Instance.GetLinePositionById(Id);
            if (diagram != null)
            {
                #endregion

                var excelPackage = new ExcelPackage();
                excelPackage.Workbook.Properties.Author = "IED";
                excelPackage.Workbook.Properties.Title = "Sơ Đồ Phân Công";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sơ Đồ Phân Công");
                sheet.Name = "Sơ Đồ Phân Công";
                sheet.Cells.Style.Font.Size = 12;
                sheet.Cells.Style.Font.Name = "Times New Roman";
                var company = BLLCompany.Instance.GetById(UserContext.CompanyId);
                var tech = diagram.TechProcess;
                var linePos = diagram.LinePositions;// != null && diagram.LinePositions.Count > 0 ? SetEmployeeData(diagram.LinePositions) : new List<LinePositionModel>();

                #region
                sheet.Cells[1, 2].Value = company != null ? company.CompanyName : "";
                sheet.Cells[1, 2].Style.Font.Size = 13;
                sheet.Cells[1, 2, 1, 10].Merge = true;
                sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[2, 2].Value = company != null ? company.Address : "";
                sheet.Cells[2, 2].Style.Font.Size = 10;
                sheet.Cells[2, 2, 2, 10].Merge = true;
                sheet.Cells[2, 2, 2, 10].Style.Font.Bold = true;
                sheet.Row(1).Height = 20;
                sheet.Cells[4, 2].Value = "SƠ ĐỒ THIẾT KẾ CHUYỀN : " + linePos[0].LineName.ToUpper() + " - XÍ NGHIỆP : " + tech.WorkShopName.ToUpper() + " - SẢN PHẨM : " + tech.ProductName.ToUpper();
                sheet.Cells[4, 2].Style.Font.Size = 14;
                sheet.Cells[4, 2, 4, 18].Merge = true;
                sheet.Cells[4, 2, 4, 18].Style.WrapText = true;
                sheet.Cells[4, 2, 4, 18].Style.Font.Bold = true;
                sheet.Cells[4, 2, 4, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[4, 2, 4, 18].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[4, 2, 4, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[4, 2, 4, 18].Style.Fill.BackgroundColor.SetColor(Color.White);
                sheet.Cells[4, 2, 4, 18].Style.Font.Color.SetColor(Color.White);
                sheet.Cells[4, 2, 4, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[4, 2, 4, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[4, 2, 4, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Row(4).Height = 50;

                sheet.Cells[6, 3, 12, 6].Style.Font.Bold = true;
                sheet.Cells[6, 3, 13, 6].Style.Font.Size = 8;
                sheet.Cells[6, 3].Value = "XÍ NGHIỆP : " + tech.WorkShopName.ToUpper();
                sheet.Cells[6, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[6, 4].Value = "CHUYỀN : " + linePos[0].LineName.ToUpper();
                sheet.Cells[6, 4, 6, 5].Merge = true;
                sheet.Cells[6, 4, 6, 5].Style.Font.Color.SetColor(Color.Red);
                sheet.Cells[6, 4, 6, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[6, 4, 12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                sheet.Cells[7, 3].Value = "TỔNG THỜI GIAN";
                sheet.Cells[7, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[7, 4].Value = Math.Round(tech.TimeCompletePerCommo);
                sheet.Cells[7, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[7, 5].Value = "S";
                sheet.Cells[7, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[8, 3].Value = "NĂNG XUẤT BÌNH QUÂN ĐẦU NGƯỜI ";
                sheet.Cells[8, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[8, 4].Value = Math.Round(tech.ProOfPersonPerDay);
                sheet.Cells[8, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[8, 5].Value = "SP";
                sheet.Cells[8, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[9, 3].Value = "NĂNG XUẤT TỔ / 1H";
                sheet.Cells[9, 4].Value = Math.Round(tech.ProOfGroupPerHour);
                sheet.Cells[9, 5].Value = "SP";
                sheet.Cells[9, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[9, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[9, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[10, 3].Value = "NĂNG XUẤT TỔ / NGÀY (" + tech.WorkingTimePerDay + "H)";
                sheet.Cells[10, 4].Value = Math.Round(tech.ProOfGroupPerDay);
                sheet.Cells[10, 5].Value = "SP";
                sheet.Cells[10, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[10, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[10, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[11, 3].Value = "NHỊP SẢN XUẤT";
                sheet.Cells[11, 4].Value = Math.Round(tech.PacedProduction);
                sheet.Cells[11, 5].Value = "SP";
                sheet.Cells[11, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[11, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[11, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[12, 3].Value = "TỔNG LAO ĐỘNG";
                sheet.Cells[12, 4].Value = tech.NumberOfWorkers;
                sheet.Cells[12, 5].Value = "LĐ";
                sheet.Cells[12, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[12, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[12, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //sheet.Cells[13, 2].Value = "GHI CHÚ:" + tech.Note;
                //sheet.Cells[13, 2, 13, 16].Merge = true;
                //sheet.Cells[13, 2, 13, 16].Style.Font.Size = 8;

                // sheet.Cells[6, 3, 12, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[7, 4, 12, 4].Style.Font.Color.SetColor(Color.Red);
                sheet.Cells[7, 5, 12, 5].Style.Font.Color.SetColor(Color.DarkBlue);
                sheet.Cells[6, 3, 12, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[6, 3, 12, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                var tong = new List<string>();
                foreach (var line in linePos)
                {
                    var e = line.Details.Where(x => x.EquipmentId != 0).Select(x => x.EquipmentName).Distinct();
                    if (e != null && e.Count() > 0)
                        tong.AddRange(e);
                }
                sheet.Cells[6, 15].Value = "Thiết Bị Theo Chuyền";
                sheet.Cells[6, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[6, 16].Value = "SL";
                sheet.Cells[6, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[6, 17].Value = "ĐVT";
                sheet.Cells[6, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[6, 15, 6, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[6, 15, 6, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[6, 15, 6, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[6, 15, 6, 17].Style.Font.Bold = true;
                sheet.Cells[6, 15, 6, 17].Style.Font.Color.SetColor(Color.White);

                int r = 7;

                if (tong.Count > 0)
                {

                    foreach (var item in tong.Distinct())
                    {
                        var a = tong.Where(x => x.Equals(item));
                        sheet.Cells[r, 15].Value = item;
                        sheet.Cells[r, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[r, 16].Value = a.Count();
                        sheet.Cells[r, 16].Style.Font.Bold = true;
                        sheet.Cells[r, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[r, 17].Value = "Chiếc";
                        sheet.Cells[r, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        r++;
                    }
                    sheet.Cells[7, 16, r - 1, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[7, 15, r - 1, 17].Style.Font.Size = 8;
                    sheet.Cells[7, 15, r - 1, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[7, 15, r - 1, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                }
                #endregion

                #region detail
                int startRow = r > 14 ? r++ : 14;
                sheet.Cells[startRow, 6, startRow, 14].Merge = true;
                sheet.Cells[startRow, 6, startRow, 14].Value = "BÀN KSC";
                sheet.Cells[startRow, 6, startRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 6, startRow, 14].Style.Font.Bold = true;
                sheet.Cells[startRow, 6, startRow, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                startRow += 2;
                int endRow = startRow - 1;
                LinePositionModel po_Left = null;
                LinePositionModel po_right = null;
                double labor_left = 0;
                double labor_right = 0;
                double time_left = 0;
                double time_right = 0;
                string Equip_left = "";
                string Equip_right = "";
                var tongMachine = new List<int>();
                var d = 1;

                for (int dong2 = linePos.Count, dong = dong2 - 1; dong2 > 0; dong2 -= 2, dong -= 2)
                {
                    po_Left = linePos.Where(x => x.OrderIndex == dong).FirstOrDefault();
                    po_right = linePos.Where(x => x.OrderIndex == dong2).FirstOrDefault();

                    // lay so dong cong doan ben trai va ben phai ben nao >
                    int soCongDoan = po_Left != null && po_Left.Details.Count > 2 ? po_Left.Details.Count : 2;
                    soCongDoan = po_right != null && po_right.Details.Count > soCongDoan ? po_right.Details.Count : soCongDoan;

                    labor_left = 0;
                    labor_right = 0;
                    time_left = 0;
                    time_right = 0;
                    Equip_left = "";
                    Equip_right = "";
                    var E_left = new List<string>();
                    var E_right = new List<string>();
                    if (po_Left != null && po_Left.IsHasBTP)
                    {
                        sheet.Cells[endRow + 1, 1].Value = "BTP";
                        sheet.Cells[endRow + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[endRow + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 176, 240));
                    }
                    if (po_right != null && po_right.IsHasBTP)
                    {
                        sheet.Cells[endRow + 1, 19].Value = "BTP";
                        sheet.Cells[endRow + 1, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[endRow + 1, 19].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 176, 240));
                    }
                    for (int i = 1; i <= soCongDoan; i++)
                    {
                        endRow++;
                        #region left
                        if (po_Left != null && i <= po_Left.Details.Count)
                        {
                            sheet.Cells[endRow, 2].Value = po_Left.Details[i - 1].PhaseCode;
                            sheet.Cells[endRow, 3].Value = po_Left.Details[i - 1].PhaseName;
                            sheet.Cells[endRow, 3].Style.WrapText = true;
                            sheet.Cells[endRow, 4].Value = Math.Round(po_Left.Details[i - 1].TotalTMU);
                            sheet.Cells[endRow, 5].Value = po_Left.Details[i - 1].DevisionPercent;
                            sheet.Cells[endRow, 6].Value = Math.Round(po_Left.Details[i - 1].NumberOfLabor, 2);
                            labor_left += po_Left.Details[i - 1].NumberOfLabor;
                            time_left += po_Left.Details[i - 1].TotalTMU;
                            if (i <= po_Left.Details.Count)
                            {
                                if (E_left.Count > 0 && po_Left.Details[i - 1].EquipmentCode != null)
                                {
                                    if (E_left.FirstOrDefault(x => x.Trim().ToUpper().Equals(po_Left.Details[i - 1].EquipmentCode.Trim().ToUpper())) == null)
                                    {
                                        Equip_left += po_Left.Details[i - 1].EquipmentCode + "\n";
                                        E_left.Add(po_Left.Details[i - 1].EquipmentCode);
                                    }
                                }
                                else
                                {
                                    if (po_Left.Details[i - 1].EquipmentCode != null)
                                    {
                                        Equip_left += po_Left.Details[i - 1].EquipmentCode + "\n";
                                        E_left.Add(po_Left.Details[i - 1].EquipmentCode);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region right
                        if (po_right != null && i <= po_right.Details.Count)
                        {
                            sheet.Cells[endRow, 14].Value = po_right.Details[i - 1].PhaseCode;
                            sheet.Cells[endRow, 15].Value = po_right.Details[i - 1].PhaseName;
                            sheet.Cells[endRow, 15].Style.WrapText = true;
                            sheet.Cells[endRow, 16].Value = Math.Round(po_right.Details[i - 1].TotalTMU);
                            sheet.Cells[endRow, 17].Value = po_right.Details[i - 1].DevisionPercent;
                            sheet.Cells[endRow, 18].Value = Math.Round(po_right.Details[i - 1].NumberOfLabor, 2);
                            labor_right += po_right.Details[i - 1].NumberOfLabor;
                            time_right += po_right.Details[i - 1].TotalTMU;

                            if (i <= po_right.Details.Count)
                            {
                                if (E_right.Count > 0 && po_right.Details[i - 1].EquipmentCode != null)
                                {
                                    if (E_right.FirstOrDefault(x => x.Trim().ToUpper().Equals(po_right.Details[i - 1].EquipmentCode.Trim().ToUpper())) == null)
                                    {
                                        Equip_right += po_right.Details[i - 1].EquipmentCode + "\n";
                                        E_right.Add(po_right.Details[i - 1].EquipmentCode);
                                    }
                                }
                                else
                                {
                                    Equip_right += po_right.Details[i - 1].EquipmentCode + "\n";
                                    E_right.Add(po_right.Details[i - 1].EquipmentCode);
                                }
                            }
                        }
                        #endregion
                    }

                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 2, (endRow + 1), 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 14, (endRow + 1), 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    endRow++;



                    #region //left
                    sheet.Cells[endRow, 2].Value = dong;
                    sheet.Cells[endRow, 3].Value = (po_Left != null && po_Left.EmployeeId != null && po_Left.EmployeeId != 0) ? po_Left.EmployeeName : "";
                    sheet.Cells[endRow, 4].Value = Math.Round(time_left);
                    sheet.Cells[endRow, 6].Value = Math.Round(labor_left, 3);

                    sheet.Cells[endRow, 2, endRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[endRow, 2, endRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[endRow, 2, endRow, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 220, 219));
                    sheet.Cells[endRow, 2, endRow, 6].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 88));
                    sheet.Cells[endRow, 2, endRow, 6].Style.Font.Bold = true;
                    //equipment -left
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Merge = true;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Style.WrapText = true;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Value = Equip_left;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Style.TextRotation = 90;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(247, 150, 70));
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 8, endRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region //right
                    sheet.Cells[endRow, 14].Value = dong2;
                    sheet.Cells[endRow, 15].Value = (po_right != null && po_right.EmployeeId != null && po_right.EmployeeId != 0) ? po_right.EmployeeName : "";
                    sheet.Cells[endRow, 16].Value = Math.Round(time_right);
                    sheet.Cells[endRow, 18].Value = Math.Round(labor_right, 3);

                    sheet.Cells[endRow, 14, endRow, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[endRow, 14, endRow, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[endRow, 14, endRow, 18].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 220, 219));
                    sheet.Cells[endRow, 14, endRow, 18].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 88));
                    sheet.Cells[endRow, 14, endRow, 18].Style.Font.Bold = true;
                    // equipment - right
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Merge = true;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Style.WrapText = true;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Value = Equip_right;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Style.TextRotation = 90;
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(247, 150, 70));
                    sheet.Cells[(d == 1 ? startRow : endRow - soCongDoan), 12, endRow, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region ve exit line
                    if (po_Left != null && po_Left.IsHasExitLine)
                    {
                        endRow++;
                        sheet.Cells[endRow, 2, endRow, 6].Merge = true;
                        sheet.Cells[endRow, 2, endRow, 6].Value = "LỐI   ĐI";
                        sheet.Cells[endRow, 2, endRow, 6].Style.Font.Bold = true;
                        sheet.Cells[endRow, 2, endRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[endRow, 2, endRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[endRow, 2, endRow, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
                        sheet.Cells[endRow, 2, endRow, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet.Cells[endRow, 7, endRow, 13].Merge = true;
                        sheet.Cells[endRow, 7, endRow, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[endRow, 7, endRow, 13].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

                        sheet.Cells[endRow, 14, endRow, 18].Merge = true;
                        sheet.Cells[endRow, 14, endRow, 18].Value = "LỐI   ĐI";
                        sheet.Cells[endRow, 14, endRow, 18].Style.Font.Bold = true;
                        sheet.Cells[endRow, 14, endRow, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[endRow, 14, endRow, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[endRow, 14, endRow, 18].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
                        sheet.Cells[endRow, 14, endRow, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    #endregion


                    d++;
                }

                sheet.Cells[startRow, 2, endRow, 18].Style.Font.Size = 11;
                sheet.Cells[startRow, 2, endRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 4, endRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 14, endRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 15, endRow, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 2, endRow, 6].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheet.Cells[startRow, 14, endRow, 18].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                //sheet.Cells[15, 7, endRow, 7].Style.Font.Name = "Cambria";
                //sheet.Cells[15, 7, endRow, 7].Style.Font.Bold = true;
                sheet.Cells[startRow, 7, endRow, 12].Style.Font.Size = 9;
                //sheet.Cells[15, 11, endRow, 11].Style.Font.Name = "Cambria";
                //sheet.Cells[15, 11, endRow, 11].Style.Font.Bold = true;
                //sheet.Cells[15, 11, endRow, 11].Style.Font.Size = 9;

                //sheet.Cells[15, 9, endRow, 9].Merge = true;
                sheet.Cells[startRow, 10, endRow, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[startRow, 10, endRow, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                endRow += 2;
                sheet.Cells[endRow, 6, endRow, 14].Merge = true;
                sheet.Cells[endRow, 6, endRow, 14].Value = "BÀN TỔ TRƯỞNG";
                sheet.Cells[endRow, 6, endRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[endRow, 6, endRow, 14].Style.Font.Bold = true;
                sheet.Cells[endRow, 6, endRow, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                endRow++;
                sheet.Cells[endRow, 14, endRow, 16].Merge = true;
                sheet.Cells[endRow, 14, endRow, 16].Style.Font.Bold = true;
                sheet.Cells[endRow, 14, endRow, 16].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year + "            ";
                sheet.Cells[endRow, 14, endRow, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                endRow++;
                sheet.Cells[endRow, 2, endRow, 16].Merge = true;
                sheet.Cells[endRow, 2, endRow, 16].Value = "GĐ CN&QLCL                  Trưởng lean TK                             Tổ Trưởng                               TBP QT                          Người lập";
                sheet.Cells[endRow, 2, endRow, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                #endregion

                sheet.Cells.AutoFitColumns(5, 40);
                sheet.Column(15).Width = 40;
                sheet.Column(3).Width = 40;
                sheet.Column(7).AutoFit(1.5);
                sheet.Column(9).AutoFit(1.5);
                sheet.Column(11).AutoFit(1.5);
                sheet.Column(13).AutoFit(1.5);
                sheet.Column(8).AutoFit(8.5);
                sheet.Column(10).AutoFit(10);
                sheet.Column(12).AutoFit(8.5);
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                DateTime dateNow = DateTime.Now;
                string fileName = Regex.Replace( "TKC_" + (linePos[0] != null ? linePos[0].LineName : "_") + "_" + tech.ProductName  , "[^\\w]", "_")+ "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";

                fileName = fileName.Replace(',', '-');
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();

                BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
                {
                    CreatedDate = dateNow,
                    UserId = UserContext.UserID,
                    QTCNId = Id,
                    Type = (int)eObjectType.isLabourDivision,
                    Note = "Mã hàng: " + tech.ProductName + " - Thiết kế chuyền: " + (linePos[0] != null ? linePos[0].LineName : " ")
                });
            }

        }
        #endregion
        #endregion

        #region QT Công Nghệ ăn lương

        /// <summary>
        /// Quy Trình Công Nghệ Làm Lương
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_4(int parentId, bool isGetNull)
        {
            var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId, isGetNull, IsMDG);
            var company = BLLCompany.Instance.GetById(UserContext.CompanyId);

            var excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "IED";
            excelPackage.Workbook.Properties.Title = "Quy trình công nghệ làm lương";
            var sheet = excelPackage.Workbook.Worksheets.Add("Quy trình công nghệ làm lương");
            sheet.Name = "Quy trình công nghệ làm lương";
            sheet.Cells.Style.Font.Size = 12;
            sheet.Cells.Style.Font.Name = "Times New Roman";

            sheet.Cells[1, 2].Value = company != null ? company.CompanyName : "";
            sheet.Cells[1, 2].Style.Font.Size = 13;
            sheet.Cells[1, 2, 1, 10].Merge = true;
            sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
            sheet.Cells[1, 2].Style.WrapText = true;
            sheet.Cells[2, 2].Value = company != null ? company.Address : "";
            sheet.Cells[2, 2].Style.Font.Size = 10;
            sheet.Cells[2, 2, 2, 10].Merge = true;
            sheet.Cells[2, 2, 2, 10].Style.Font.Bold = true;
            sheet.Row(1).Height = 20;
            sheet.Cells[4, 2].Value = "TIME STUDY - QUY TRÌNH CÔNG NGHỆ SẢN XUẤT : " + techProcessInfo.ProductName.ToUpper();
            sheet.Cells[4, 2].Style.Font.Size = 14;
            sheet.Cells[4, 2, 4, 10].Merge = true;
            sheet.Cells[4, 2, 4, 10].Style.Font.Bold = true;
            sheet.Cells[4, 2].Style.WrapText = true;
            sheet.Cells[4, 2, 4, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[4, 2, 4, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[4, 2, 4, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[4, 2, 4, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            sheet.Cells[4, 2, 4, 10].Style.Font.Color.SetColor(Color.White);
            sheet.Row(4).Height = 40;

            sheet.Cells[5, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
            sheet.Cells[5, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            sheet.Cells[5, 2].Style.Font.Bold = true;
            sheet.Cells[5, 2, 5, 9].Merge = true;

            int rowIndex = 6, row = 0;
            double TongLĐ = 0;


            #region Thong tin QTCN
            if (techProcessInfo != null && techProcessInfo.ListTechProcessGroup != null && techProcessInfo.ListTechProcessGroup.Count > 0)
            {
                row = rowIndex;
                sheet.Cells[rowIndex, 2].Value = "STT";
                sheet.Cells[rowIndex, 3].Value = "Tên Công Đoạn";
                sheet.Cells[rowIndex, 4].Value = "Bậc Thợ";
                sheet.Cells[rowIndex, 5].Value = "Hệ số";
                sheet.Cells[rowIndex, 6].Value = "Thiết Bị ";
                sheet.Cells[rowIndex, 7].Value = "Lao Động";
                sheet.Cells[rowIndex, 8].Value = "Thời gian chuẩn";
                sheet.Cells[rowIndex, 9].Value = "TGC theo hệ số";
                sheet.Cells[rowIndex, 10].Value = "Tiền";
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                sheet.Row(rowIndex).Height = 25;

                AddCellBorder(sheet, rowIndex, 2, 10);

                techProcessInfo.CompanyName = UserContext.CompanyName;
                int stt = 1;
                int stt_group = 1;
                rowIndex++;
                double DMSL = 0, DMLD = 0, TGHP = 0, TGQD = 0, Money = 0;
                foreach (var group in techProcessInfo.ListTechProcessGroup)
                {
                    sheet.Cells[rowIndex, 2].Value = stt_group;
                    sheet.Cells[rowIndex, 3].Value = group.PhaseGroupName.ToUpper();
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Color.SetColor(Color.White);
                    sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;

                    AddCellBorder(sheet, rowIndex, 2, 10);
                    rowIndex++;
                    if (group.ListTechProcessVerDetail != null && group.ListTechProcessVerDetail.Count > 0)
                    {
                        foreach (var detail in group.ListTechProcessVerDetail)
                        {
                            sheet.Cells[rowIndex, 2].Value = stt;
                            sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[rowIndex, 3].Value = detail.PhaseName;
                            sheet.Cells[rowIndex, 4].Value = detail.WorkerLevelName;
                            sheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[rowIndex, 5].Value = detail.Coefficient;
                            sheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // sheet.Cells[rowIndex, 6].Value = detail.EquipmentGroupCode;
                            sheet.Cells[rowIndex, 6].Value = detail.EquipmentName;
                            sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            //var a = Math.Round((3600 * techProcessInfo.WorkingTimePerDay) / detail.TimeByPercent);
                            //DMSL += a;
                            //sheet.Cells[rowIndex, 6].Value = a;
                            //sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            DMLD += Math.Round(detail.Worker, 2);
                            sheet.Cells[rowIndex, 7].Value = Math.Round(detail.Worker, 2);
                            sheet.Cells[rowIndex, 7].Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 7].Style.Font.Color.SetColor(Color.Red);
                            sheet.Cells[rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            TGHP += Math.Round(detail.TimeByPercent, 1);
                            sheet.Cells[rowIndex, 8].Value = Math.Round(detail.TimeByPercent, 1);
                            sheet.Cells[rowIndex, 8].Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 8].Style.Font.Color.SetColor(Color.Blue);
                            sheet.Cells[rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            double a = 0;
                            a = Math.Round(detail.TimeByPercent * detail.Coefficient);
                            TGQD += a;
                            sheet.Cells[rowIndex, 9].Value = a;
                            sheet.Cells[rowIndex, 9].Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 9].Style.Font.Color.SetColor(Color.Red);
                            sheet.Cells[rowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            a = a * techProcessInfo.PricePerSecond;
                            Money += a;
                            sheet.Cells[rowIndex, 10].Value = Math.Round(a, 2);
                            sheet.Cells[rowIndex, 10].Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 10].Style.Font.Color.SetColor(Color.Blue);
                            sheet.Cells[rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            AddCellBorder(sheet, rowIndex, 2, 10);
                            stt++;
                            rowIndex++;
                        }
                    }
                    stt_group++;
                }
                sheet.Cells[rowIndex, 2].Value = "Đơn Giá May";
                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                sheet.Cells[rowIndex, 6].Value = Math.Round(DMSL);
                sheet.Cells[rowIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 7].Value = Math.Round(DMLD, 2);
                sheet.Cells[rowIndex, 8].Value = Math.Round(TGHP, 1);
                sheet.Cells[rowIndex, 9].Value = Math.Round(TGQD);
                sheet.Cells[rowIndex, 10].Value = Math.Round(Money, 2);
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 2, 10);
                rowIndex++;

                sheet.Cells[rowIndex, 2].Value = "Phụ cấp " + techProcessInfo.Allowance + "% Đơn Giá ";
                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                var all = Math.Round((Money * techProcessInfo.Allowance) / 100, 2);
                sheet.Cells[rowIndex, 10].Value = all;
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 2, 10);
                rowIndex++;

                sheet.Cells[rowIndex, 2].Value = "Tổng đơn giá đã có Phụ cấp ";
                sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;

                sheet.Cells[rowIndex, 10].Value = Math.Round((Money + all), 2);
                sheet.Cells[rowIndex, 2, rowIndex, 10].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 2, 10);
                rowIndex++;
            }
            sheet.Cells[7, 3, rowIndex, 3].Style.WrapText = true;
            sheet.Cells[row, 2, rowIndex - 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            #endregion

            #region Thông Tin Thiết Bi
            if (techProcessInfo.equipments != null && techProcessInfo.equipments.Count > 0)
            {
                TongLĐ = 0;
                for (int i = 0; i < techProcessInfo.equipments.Count; i++)
                {
                    var equipment = techProcessInfo.equipments[i];
                    double tong = 0;
                    foreach (var item in techProcessInfo.ListTechProcessGroup)
                    {
                        tong += item.ListTechProcessVerDetail.Where(x => x.EquipmentId == equipment.Id).Select(x => x.Worker).Sum();
                    }
                    TongLĐ += tong;
                }
            }
            #endregion

            #region TTchung
            rowIndex += 2;
            if (techProcessInfo != null)
            {
                row = rowIndex;
                sheet.Cells[rowIndex, 2].Value = "THÔNG TIN QUY TRÌNH ";
                sheet.Cells[rowIndex, 2, rowIndex, 5].Merge = true;
                sheet.Cells[rowIndex, 2, rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 2, rowIndex, 5].Style.Font.Bold = true;
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 1;
                sheet.Cells[rowIndex, 3].Value = "Thời gian hoàn thành 1 sản phẩm ";
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.TimeCompletePerCommo);
                sheet.Cells[rowIndex, 5].Value = "Giây/SP";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 2;
                sheet.Cells[rowIndex, 3].Value = "Tổng số lao động";
                sheet.Cells[rowIndex, 4].Value = techProcessInfo.NumberOfWorkers;
                sheet.Cells[rowIndex, 5].Value = "LĐ";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 3;
                sheet.Cells[rowIndex, 3].Value = "Nhịp độ sản xuất Chuyền";
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.PacedProduction);
                sheet.Cells[rowIndex, 5].Value = "Giây/SP/LĐ";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 4;
                sheet.Cells[rowIndex, 3].Value = "Năng suất của Chuyền / giờ  ";
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerHour);
                sheet.Cells[rowIndex, 5].Value = "SP";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 5;
                sheet.Cells[rowIndex, 3].Value = "Năng suất Lao Động / ngày  ";
                sheet.Cells[rowIndex, 4].Value = Math.Round((3600 * techProcessInfo.WorkingTimePerDay) / techProcessInfo.TimeCompletePerCommo);
                sheet.Cells[rowIndex, 5].Value = "SP";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 6;
                sheet.Cells[rowIndex, 3].Value = "Năng suất Chuyền / ngày  ";
                sheet.Cells[rowIndex, 4].Value = Math.Round(techProcessInfo.ProOfGroupPerDay);
                sheet.Cells[rowIndex, 5].Value = "SP";
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 7;
                sheet.Cells[rowIndex, 3].Value = "Tổng sản lượng Mã Hàng";
                sheet.Cells[rowIndex, 4].Value = techProcessInfo.Quantities;
                sheet.Cells[rowIndex, 5].Value = "SP";
                AddCellBorder(sheet, rowIndex, 2, 5);

                //var moneyObj =   App_Global.AppGlobal.Account.GetService().GetMoneyTypeById(techProcessInfo.MoneyTypeId);
                //var moneyName = moneyObj != null ? moneyObj.Name : "";
                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 8;
                sheet.Cells[rowIndex, 3].Value = "Đơn giá của Mã Hàng";
                sheet.Cells[rowIndex, 4].Value = techProcessInfo.Price;
                //sheet.Cells[rowIndex, 5].Value = moneyName;
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 9;
                sheet.Cells[rowIndex, 3].Value = "Đơn giá giây";
                sheet.Cells[rowIndex, 4].Value = techProcessInfo.PricePerSecond;
                //sheet.Cells[rowIndex, 5].Value = moneyName;
                AddCellBorder(sheet, rowIndex, 2, 5);

                rowIndex++;
                sheet.Cells[rowIndex, 2].Value = 10;
                sheet.Cells[rowIndex, 3].Value = "Tổng số máy trên chuyền";
                sheet.Cells[rowIndex, 4].Value = Math.Round(TongLĐ);
                sheet.Cells[rowIndex, 5].Value = "";
                sheet.Cells[row, 3, rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                AddCellBorder(sheet, rowIndex, 2, 5);
            }
            #endregion

            rowIndex += 2;
            sheet.Cells[rowIndex, 2].Value = "                      GĐ ĐH 	                           GIÁM ĐỐC 	                                TP/KỸ THUẬT 	                           TRƯỞNG NHÓM QT 	                              NGƯỜI LẬP QT".ToUpper();
            sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
            sheet.Cells[rowIndex, 2].Style.Font.Size = 10;

            rowIndex += 7;
            sheet.Cells[rowIndex, 2].Value = "                        PT.KỸ THUẬT XN 	                     KT.TRIỂN KHAI".ToUpper();
            sheet.Cells[rowIndex, 2].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
            sheet.Cells[rowIndex, 2].Style.Font.Size = 10;

            sheet.Cells.AutoFitColumns(5);
            sheet.Column(3).Width = 50;
            sheet.Column(14).Style.WrapText = true;
            Response.ClearContent();
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            DateTime dateNow = DateTime.Now;
            //string fileName = "QTC_" + techProcessInfo. + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            string fileName = "QTC_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/excel";
            Response.Flush();
            Response.End();

            BLLUsingTechLog.Instance.Insert(new T_UsingTechLog()
            {
                CreatedDate = dateNow,
                UserId = UserContext.UserID,
                QTCNId = techProcessInfo.Id,
                Type = (int)eObjectType.isQTCN,
                Note = "Mã hàng: " + techProcessInfo.ProductName + " - Quy trình công nghệ mẫu làm lương "
            });
        }

        #endregion

        [HttpPost]
        public JsonResult GetSelectList(int type, int parentId)
        {
            try
            {
                JsonDataResult.Data = BLLCommodityAnalysis.Instance.GetSelectItems(type, parentId);
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

    }
}