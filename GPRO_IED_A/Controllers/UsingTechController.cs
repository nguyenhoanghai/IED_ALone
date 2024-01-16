using GPRO_IED_A.Business;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class UsingTechController : BaseController
    {
        public ActionResult Index()
        { 
            //Export();
            return View();
        }

        public void Export(string from, string to)
        {
            if (isAuthenticate)
            {
                DateTime _from = DateTime.Now, _to = DateTime.Now;
                _from = DateTime.ParseExact(from + " 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to = DateTime.ParseExact(to + " 23:59:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to.AddHours(23);
                var report = BLLUsingTechLog.Instance.GetReport(_from, _to);

                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\using_tech_template.xlsx"));
                int rowIndex = 6;
                using (var package = new ExcelPackage(_file))
                {
                    var workbook = package.Workbook;
                    var sheet = workbook.Worksheets.First();

                    sheet.Cells[3, 2].Value = ("từ ngày: "+from + " đến: "+to).ToUpper();


                    sheet.Cells[rowIndex, 2].Value = ("tổng").ToUpper();
                    sheet.Cells[rowIndex, 3].Value = report.TotalProduct;
                    sheet.Cells[rowIndex, 4].Value = 0;
                    sheet.Cells[rowIndex, 5].Formula = "D4/C4";
                    sheet.Cells[rowIndex, 6].Value = report.TotalPhase;
                    sheet.Cells[rowIndex, 7].Value = report.TotalSubmitPhase;
                    sheet.Cells[rowIndex, 8].Value = report.TotalApprovePhase;
                    sheet.Cells[rowIndex, 9].Value = report.TotalViewPhase;
                    sheet.Cells[rowIndex, 10].Value = report.TotalDownloadPhase;
                    sheet.Cells[rowIndex, 11].Formula = "I4/$F$4";
                    sheet.Cells[rowIndex, 12].Value = report.TotalNewPhase;
                    sheet.Cells[rowIndex, 13].Formula = "(L4+I4)/F$4";
                    sheet.Cells[rowIndex, 14].Value = "";
                    AddCellBorder(sheet, rowIndex, 2, 14);
                    rowIndex++;

                    #region Thong tin QTCN
                    if (report != null && report.Details != null && report.Details.Count > 0)
                    {
                        foreach (var group in report.Details)
                        {
                            sheet.Cells[rowIndex, 2].Value = group.Name.ToUpper();
                            sheet.Cells[rowIndex, 3].Value = group.TotalProduct;
                            sheet.Cells[rowIndex, 4].Value = 0;
                            sheet.Cells[rowIndex, 5].Formula = "D" + rowIndex + "/C" + rowIndex;
                            sheet.Cells[rowIndex, 6].Value = group.TotalPhase;
                            sheet.Cells[rowIndex, 7].Value = group.TotalSubmitPhase;
                            sheet.Cells[rowIndex, 8].Value = group.TotalApprovePhase;
                            sheet.Cells[rowIndex, 9].Value = group.TotalViewPhase;
                            sheet.Cells[rowIndex, 10].Value = group.TotalDownloadPhase;
                            sheet.Cells[rowIndex, 11].Formula = "(I" + rowIndex + "+J" + rowIndex + ")/F$" + rowIndex;
                            sheet.Cells[rowIndex, 12].Value = group.TotalNewPhase;
                            sheet.Cells[rowIndex, 13].Formula = "(L" + rowIndex + "+I" + rowIndex + "+J" + rowIndex + ")/F$" + rowIndex;
                            sheet.Cells[rowIndex, 14].Value = "";
                            AddCellBorder(sheet, rowIndex, 2, 14);
                            rowIndex++;
                        }
                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "Báo cáo tương tác_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Flush();
                    Response.End();
                }
            }
        }

        private static void AddCellBorder(ExcelWorksheet sheet, int rowIndex, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                sheet.Cells[rowIndex, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[rowIndex, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        public JsonResult GetReportDetail(int userId, int workshopId, bool isView, DateTime from, DateTime to)
        {
            try
            {
                JsonDataResult.Records = BLLUsingTechLog.Instance.GetReportDetail(userId, workshopId, isView, from, to);
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        public JsonResult Gets(DateTime from, DateTime to)
        {
            try
            {
                JsonDataResult.Records = BLLUsingTechLog.Instance.GetReport(from, to);
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        public void ExportDetail(int userId, int workshopid, bool isView, string from, string to)
        {
            if (isAuthenticate)
            {
                DateTime _from = DateTime.Now, _to = DateTime.Now;
                _from = DateTime.ParseExact(from + " 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to = DateTime.ParseExact(to + " 23:59:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to.AddHours(23);
                var report = BLLUsingTechLog.Instance.GetReportDetail(userId, workshopid, isView, _from, _to);

                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\using_tech_detail_template.xlsx"));
                int rowIndex = 5;
                using (var package = new ExcelPackage(_file))
                {
                    var workbook = package.Workbook;
                    var sheet = workbook.Worksheets.First();
                    sheet.Cells[3, 2].Value = "Từ ngày: " + from + " đến: " + to;

                    #region Thong tin  
                    if (report != null && report.Count > 0)
                    {
                        int stt = 1;
                        foreach (var group in report)
                        {
                            sheet.Cells[rowIndex, 2].Value = stt;
                            sheet.Cells[rowIndex, 3].Value = group.WorkshopName;
                            sheet.Cells[rowIndex, 4].Value = group.UserName;
                            sheet.Cells[rowIndex, 5].Value = group.Note;
                            sheet.Cells[rowIndex, 6].Value = group.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                            AddCellBorder(sheet, rowIndex, 2, 7);
                            rowIndex++;
                            stt++;
                        }
                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "Báo cáo tương tác chi tiết_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public JsonResult GetUserPhases(int userId, DateTime from, DateTime to)
        {
            try
            {
                var phases = BLLCommo_Ana_Phase.Instance.GetReport(userId, from, to);
                phases.AddRange(BLLPhaseGroup_Phase.Instance.GetReport(userId, from, to));
                JsonDataResult.Records = phases;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(JsonDataResult);
        }

        public void ExportUserPhases(int userId, string from, string to)
        {
            if (isAuthenticate)
            {
                DateTime _from = DateTime.Now, _to = DateTime.Now;
                _from = DateTime.ParseExact(from + " 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to = DateTime.ParseExact(to + " 23:59:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _to.AddHours(23);
                var report = BLLCommo_Ana_Phase.Instance.GetReport(userId, _from, _to);
                report.AddRange(BLLPhaseGroup_Phase.Instance.GetReport(userId, _from, _to));

                var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\using_tech_user_phase_template.xlsx"));
                int rowIndex = 5;
                using (var package = new ExcelPackage(_file))
                {
                    var workbook = package.Workbook;
                    var sheet = workbook.Worksheets.First();
                    sheet.Cells[3, 2].Value = "Từ ngày: " + from + " đến: " + to;

                    #region Thong tin  
                    if (report != null && report.Count > 0)
                    {
                        int stt = 1;
                        foreach (var group in report)
                        {
                            sheet.Cells[rowIndex, 2].Value = stt;
                            sheet.Cells[rowIndex, 3].Value = group.Type;
                            sheet.Cells[rowIndex, 4].Value = group.ProductName;
                            sheet.Cells[rowIndex, 5].Value = group.PhaseGroupName;
                            sheet.Cells[rowIndex, 6].Value = group.PhaseName;
                            sheet.Cells[rowIndex, 7].Value = group.Status;
                            sheet.Cells[rowIndex, 8].Value = group.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                            AddCellBorder(sheet, rowIndex, 2, 9);
                            rowIndex++;
                            stt++;
                        }
                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(package.GetAsByteArray());
                    DateTime dateNow = DateTime.Now;
                    string fileName = "Báo cáo sl công đoạn phân tích của nv_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "application/excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Flush();
                    Response.End();
                }
            }
        }

    }
}