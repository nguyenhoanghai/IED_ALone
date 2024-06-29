using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class PhaseGroupAnaController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.TMU = BLLIEDConfig.Instance.GetValueByCode("TMU");
            ViewBag.GetTMUType = BLLIEDConfig.Instance.GetValueByCode("GetTMUType");
            ViewBag.ListManipulationCode = BLLManipulationLibrary.Instance.GetListManipulationCode();
            ViewBag.ManipulationExpendDefault = !string.IsNullOrEmpty(BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend")) ? BLLIEDConfig.Instance.GetValueByCode("ManipulationExpend") : "0";
            ViewBag.isApprover = isPhaseApprover;
            ViewBag.WorkshopId = UserContext.WorkshopId;

            return View();
        }

        #region Commo Ana Phase
        [HttpPost]
        public JsonResult Save(PhaseGroup_PhaseModel phase)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    phase.ActionUser = UserContext.UserID;
                    if (isPhaseApprover)
                        isOwner = true;
                    responseResult = BLLPhaseGroup_Phase.Instance.InsertOrUpdate(phase, isOwner);
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
        public JsonResult UpdateName(int phaseId, string newName)
        {
            ResponseBase responseResult;
            try
            {
                if (isAuthenticate)
                {
                    if (isPhaseApprover)
                        isOwner = true;
                    responseResult = BLLPhaseGroup_Phase.Instance.UpdateName(phaseId, newName, isOwner, UserContext.UserID);
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
        public JsonResult Gets(int phaseGroupId, string keyword, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    var phases = BLLPhaseGroup_Phase.Instance.Gets(phaseGroupId, keyword, jtStartIndex, jtPageSize, jtSorting);
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
        public JsonResult Delete(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    if (isPhaseApprover)
                        isOwner = true;
                    result = BLLPhaseGroup_Phase.Instance.Delete(Id, UserContext.UserID, isOwner);
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
        public JsonResult RemoveVideo(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    if (isPhaseApprover)
                        isOwner = true;
                    string videoPath = "";
                    result = BLLPhaseGroup_Phase.Instance.RemovePhaseVideo(Id, UserContext.UserID, isOwner, ref videoPath);
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
        public JsonResult GetLastIndex(int Id)
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Records = BLLPhaseGroup_Phase.Instance.GetLastIndex(Id);
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Copy(int Id)
        {
            ResponseBase result;
            try
            {
                if (isAuthenticate)
                {
                    result = BLLPhaseGroup_Phase.Instance.Copy(Id, UserContext.UserID);
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
        public JsonResult GetById(int phaseId)
        {
            try
            {
                var phase = BLLPhaseGroup_Phase.Instance.GetPhase(phaseId);
                JsonDataResult.Records = phase;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult TinhLaiCode(List<PhaseGroup_Phase_ManiModel> actions, int equipmentId, int equiptypedefaultId, int applyPressure)
        {
            try
            {
                JsonDataResult.Result = "OK";
                var rs = BLLPhaseGroup_Phase.Instance.TinhLaiCode(actions, equipmentId, equiptypedefaultId, applyPressure);
                JsonDataResult.Records = rs.Data;
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }

        public JsonResult SaveThamChieu(  int phaseId, string productIds )
        {
            try
            {
                JsonDataResult.Result = "OK";
                if (isPhaseApprover)
                    isOwner = true;
                var result = BLLPhaseGroup_Phase.Instance.UpdateProductRefer(phaseId, productIds, isOwner, UserContext.UserID);
                if (!result.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(result.Errors);
                }
                else
                {
                    JsonDataResult.Result = "OK"; 
                }
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }
        #endregion

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

                    var result = BLLPhaseGroup_Phase.Instance.Export_CommoAnaPhaseManiVer(Id);
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

                        sheet.Cells[7, 2, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[row, 5].Value = "Tổng TMU";
                        sheet.Cells[row, 6].Value = result.Details.Sum(x => x.TMUEquipment);
                        sheet.Cells[row, 7].Value = result.Details.Sum(x => x.TMUManipulation);
                        tmu = result.Details.Sum(x => (x.TMUManipulation.Value * x.Loop) + (x.TMUEquipment.Value * x.Loop));
                        sheet.Cells[row, 8].Value = tmu;
                        sheet.Cells[row, 9].Value = Math.Round((tmu / 27.8), 2);
                        sheet.Cells[row, 5, row, 9].Style.Font.Bold = true;

                        AddCellBorder(sheet, row);
                    }
                    sheet.Cells.AutoFitColumns();
                    sheet.Column(6).Width = 16;
                    sheet.Column(7).Width = 16;
                    sheet.Column(14).Style.WrapText = true;
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "PTCĐ_" + result.PhaseName + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.Flush();
                    Response.End();
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

                    var result = BLLPhaseGroup_Phase.Instance.Export_CommoAnaPhaseGroup(Id);
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
                    string fileName = "PTCĐ_" + result.Name + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
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
        #endregion
    }
}