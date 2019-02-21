using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Data;
using GPRO_IED_A.Business.Model;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Configuration;
using System.Drawing;
using GPRO.Core.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GPRO_IED_A.Controllers
{
    public class ProAnaController : BaseController
    {
        // GET: ProAna
        #region Commo Ana
        public ActionResult Index()
        { 
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
                var noName = BLLCommodityAnalysis.Instance.GetList();
                JsonDataResult.Data = noName;
                JsonDataResult.Result = "OK";
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
                int[] relationCompanyId = new int[] { };
                if (UserContext.ChildCompanyId != null)
                    relationCompanyId = UserContext.ChildCompanyId;
                var noName = BLLCommodityAnalysis.Instance.GetCommoAnaItemByParentId(parentId, value, Type, UserContext.CompanyId, relationCompanyId, year);
                JsonDataResult.Data = noName;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Save(T_CommodityAnalysis noName)
        {
            ResponseBase responseResult;
            try
            {
                noName.CreatedUser = UserContext.UserID;
                noName.CompanyId = UserContext.CompanyId;
                if (noName.Id == 0)
                    noName.CreatedDate = DateTime.Now;
                else
                    noName.UpdatedDate = DateTime.Now;
                responseResult = BLLCommodityAnalysis.Instance.InsertOrUpdate(noName);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
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

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            ResponseBase result;
            try
            {
                result = BLLCommodityAnalysis.Instance.Delete(Id, UserContext.UserID);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }
        #endregion

        #region Commo Ana Phase
        [HttpPost]
        public JsonResult SavePhase(Commo_Ana_PhaseModel phase, List<Commo_Ana_Phase_TimePrepareModel> timePrepares)
        {
            ResponseBase responseResult;
            try
            {
                phase.ActionUser = UserContext.UserID;
                responseResult = BLLCommo_Ana_Phase.Instance.InsertOrUpdate(phase, timePrepares);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
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

        [HttpPost]
        public JsonResult GetPhases(string node, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                var phases = BLLCommo_Ana_Phase.Instance.GetListByNode(node, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = phases;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = phases.TotalItemCount;
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
                result = BLLCommo_Ana_Phase.Instance.Delete(Id, UserContext.UserID);
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
                result = BLLCommo_Ana_Phase.Instance.Copy(Id, UserContext.UserID);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult Copy_CommoAnaPhaseGroup(int CopyObjectId, int ObjectId)
        {
            ResponseBase result;
            try
            {
                result = BLLCommodityAnalysis.Instance.Copy_CommoAnaPhaseGroup(CopyObjectId, ObjectId, UserContext.UserID);
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
                throw ex;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]

        public async Task<JsonResult> UploadVideo( )
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
                return Json( returnName);
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult GetPhaseById(int phaseId)
        {
            try
            {
                var phase  = BLLCommo_Ana_Phase.Instance.GetPhase(phaseId);
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
                JsonDataResult.Records = BLLCommo_Ana_Phase.Instance.GetAllPhasesForSuggest( );
            }
            catch (Exception ex)
            { }
            return Json(JsonDataResult);
        }
        #region export excel
        public void export_PhaseManiVersion(int Id)
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
                    sheet.Cells[2, 2].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy");
                    sheet.Cells[2, 2, 2, 4].Merge = true;
                    sheet.Cells[2, 2, 2, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[3, 2].Value = "Khách hàng : ";
                    sheet.Cells[3, 2, 3, 4].Merge = true;
                    sheet.Cells[3, 2, 3, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[4, 2, 5, 4].Merge = true;
                    sheet.Cells[4, 2, 5, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[4, 2, 5, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[4, 2, 5, 4].Value = "Mã hàng : ";

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

                    sheet.Cells[4, 5].Value = "Định mức / 1h ";
                    sheet.Cells[4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[4, 8].Value = Math.Round(3600 / basicTime) + " SP";
                    sheet.Cells[4, 8, 4, 9].Merge = true;
                    sheet.Cells[4, 8, 4, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[5, 5].Value = "Định mức / 9h ";
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

                    //   sheet.Cells[6, 5].Value = result.VersionNumber;
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
                    sheet.Cells[7, 6].Value = "TMU thiết bị";

                    sheet.Cells[7, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[7, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[7, 7].Value = "TMU thao tác";

                    sheet.Cells[7, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[7, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[7, 8].Value = "TMU";

                    sheet.Cells[7, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[7, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells[7, 9].Value = "TG chuẩn (giây)";

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
                            tmu = ((item.TMUManipulation ?? 0 * item.Loop) + (item.TMUEquipment ?? 0 * item.Loop));
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
                    tmu = result.Details.Sum(x => (x.TMUManipulation ?? 0 * x.Loop) + (x.TMUEquipment ?? 0 * x.Loop));
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
                string fileName = "PTCĐ_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
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

        #endregion

        #region Techprocess
        [HttpPost]
        public JsonResult SaveTech(TechProcessVersionModel version)
        {
            ResponseBase responseResult;
            try
            {
                version.ActionUser = UserContext.UserID;
                responseResult = BLLTechProcessVersion.Instance.InsertOrUpdate(version);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                    JsonDataResult.Result = "OK";
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
                result = BLLTechProcessVersion.Instance.Delete(Id, UserContext.UserID);
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
                throw ex; ;
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetTech(int parentId, string node)
        {
            try
            {
                var details = BLLTechProcessVersion.Instance.Get(parentId, node);
                JsonDataResult.Data = details;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
            return Json(JsonDataResult);
        }

        #region Excel
        /// <summary>
        ///  Quy Trình Công Nghệ mẫu 1 thông tin phía trên
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_1(int parentId)
        {
            var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId);
            var excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "IED";
            excelPackage.Workbook.Properties.Title = "Quy trình công nghệ";
            var sheet = excelPackage.Workbook.Worksheets.Add("Quy trình công nghệ");
            sheet.Name = "Quy trình công nghệ";
            sheet.Cells.Style.Font.Size = 12;
            sheet.Cells.Style.Font.Name = "Times New Roman";

            sheet.Cells[1, 2].Value = ConfigurationManager.AppSettings["ComName"].ToString().ToUpper();
            sheet.Cells[1, 2].Style.Font.Size = 13;
            sheet.Cells[1, 2, 1, 10].Merge = true;
            sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
            sheet.Cells[1, 2].Style.WrapText = true;
            sheet.Cells[2, 2].Value = ConfigurationManager.AppSettings["ComAdd"].ToString();
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
                    sheet.Cells[rowIndex, 6].Value = Math.Round(group.ListTechProcessVerDetail.Sum(x => x.TimeByPercent));
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
            #endregion

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
            string fileName = "QTC_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/excel";
            Response.Flush();
            Response.End();
        }

        private static void AddCellBorder(ExcelWorksheet sheet, int rowIndex, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                sheet.Cells[rowIndex, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }

        /// <summary>
        /// Quy Trình Công Nghệ mẫu 2 thông tin phía dưới
        /// </summary>
        /// <param name="techProcessVersionId"></param>
        public void ExportToExcel_2(int parentId)
        {
            var techProcessInfo = BLLTechProcessVersion.Instance.GetInfoForExport(parentId);
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

                rowIndex++;
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
            string fileName = "QTC_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/excel";
            Response.Flush();
            Response.End();
        }


        #endregion
        #endregion

        #region thiết kế chuyền
        [HttpPost]
        public JsonResult Gets_TKC(int parentId, int jtStartIndex, int jtPageSize, string jtSorting)
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
        public JsonResult SaveTKC(string model)
        {
            ResponseBase rs;
            try
            {
                LabourDivisionModel obj = JsonConvert.DeserializeObject<LabourDivisionModel>(model);
                obj.ActionUser = UserContext.UserID;
                rs = BLLLabourDivision.Instance.Insert(obj);

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

                var tech = diagram.TechProcess;
                var linePos = diagram.LinePositions;// != null && diagram.LinePositions.Count > 0 ? SetEmployeeData(diagram.LinePositions) : new List<LinePositionModel>();
                #region
                sheet.Cells[1, 2].Value = ConfigurationManager.AppSettings["ComName"].ToString().ToUpper();
                sheet.Cells[1, 2].Style.Font.Size = 13;
                sheet.Cells[1, 2, 1, 10].Merge = true;
                sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[2, 2].Value = ConfigurationManager.AppSettings["ComAdd"].ToString();
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
                string fileName = "TKC_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();
            }

        }
        #endregion
        #endregion

    }
}