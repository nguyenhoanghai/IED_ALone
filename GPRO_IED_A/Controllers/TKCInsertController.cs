using GPRO.Core.Mvc;
using GPRO_IED_A.Business;
using GPRO_IED_A.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class TKCInsertController : BaseController
    {
        #region nhập năng xuất công đoạn
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTKCLineSelectList(int parentId)
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = BLLLabourDivision.Instance.GetLineModelSelectItems(parentId); ;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult GetLastLabourDevisionVer(int labourId)
        {
            try
            {
                JsonDataResult.Result = "OK";
                var diagram = BLLLabourDivision.Instance.GetLastLabourDevisionVer(labourId);
                JsonDataResult.Records = diagram;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult GetLinePositions(int laDevisionId, string date)
        {
            try
            {
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = BLLLinePosition.Instance.GetLinePositionWithLastVersion(laDevisionId, date);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        public JsonResult InsertProduction(T_LinePoDailyQuantities model)
        {
            try
            {
                model.CreatedUser = UserContext.UserID;
                model.CreatedDate = DateTime.Now;
                var response = BLLLinePoDailyQuantities.Instance.Insert(model);

                if (response.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(response.Errors);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }
        #endregion

        #region nhập năng xuất cụm công đoạn 
        public ActionResult InsertPhaseGroup()
        {
            return View();
        }

        public JsonResult InsertPhaseGroupProduction(T_PhaseGroupDailyProduction model)
        {
            try
            {
                model.CreatedUser = UserContext.UserID;
                model.CreatedDate = DateTime.Now;
                var response = BLLPhaseGroupDailyProduction.Instance.Insert(model);

                if (response.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(response.Errors);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetPhaseGroupProductionInDay(string date, int commoId, int phaseGroupId, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    var objs = BLLPhaseGroupDailyProduction.Instance.GetList(date, commoId, phaseGroupId, jtStartIndex, jtPageSize, jtSorting);
                    JsonDataResult.Records = objs;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = objs.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        #endregion

        #region báo cáo năng xuất công nhân trên từng công đoạn
        public ActionResult ReportEmployeeProduction()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReportEmployeeProduction(int labourId, int employId, int jtStartIndex = 0, int jtPageSize = 1000, string jtSorting = "")
        {
            try
            {
                if (isAuthenticate)
                {
                    var objs = BLLLinePoDailyQuantities.Instance.Gets(labourId, employId, jtStartIndex, jtPageSize, jtSorting);
                    JsonDataResult.Records = objs;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = objs.TotalItemCount;
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        #region export excel
        public void ExportEmployeeProduction(int labourId, int employId, string product, string workshop, string line, string employee)
        {
            if (isAuthenticate)
            {
                try
                {
                    var _phases = BLLLinePoDailyQuantities.Instance.Gets(labourId, employId);
                    var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\ProductionReport_Template.xlsx"));
                    using (var package = new ExcelPackage(_file))
                    {
                        var workbook = package.Workbook;
                        var sheet = workbook.Worksheets.First();


                        sheet.Cells[3, 7].Value = DateTime.Now.ToString("dd/MM/yyy");
                        sheet.Cells[4, 3].Value = product;
                        sheet.Cells[5, 3].Value = workshop;
                        sheet.Cells[6, 3].Value = line;
                        sheet.Cells[7, 3].Value = employee;

                        if (_phases.Count > 0)
                        {
                            int _row = 10;
                            for (int i = 0; i < _phases.Count; i++)
                            {
                                sheet.Cells[_row, 2].Value = i + 1;
                                sheet.Cells[_row, 3].Value = _phases[i].Name;
                                sheet.Cells[_row, 4].Value = _phases[i].Total;
                                sheet.Cells[_row, 5].Value = _phases[i].Price;
                                sheet.Cells[_row, 6].Value = _phases[i].Coefficient;
                                sheet.Cells[_row, 7].Value = _phases[i].Total * _phases[i].Price * _phases[i].Coefficient;
                                AddCellBorder(sheet, _row);
                            }
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(package.GetAsByteArray());
                        DateTime dateNow = DateTime.Now;
                        string fileName = "EmployeeProductionReport_" + employee + "_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.ContentType = "application/excel";
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.Flush();
                        Response.End();
                    }
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
        }

        #endregion
        #endregion

        #region bang can doi chuyền 
        public ActionResult ReportWorkerBalance()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReportWorkerBalance(int labourId)
        {
            try
            {
                if (isAuthenticate)
                {
                    var objs = BLLLabourDivision.Instance.GetLastLabourDevisionVer(labourId);
                    JsonDataResult.Records = objs;
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public void ExportReportWorkerBalance(int labourId, string product)
        {
            if (isAuthenticate)
            {
                try
                {
                    var diagram = BLLLabourDivision.Instance.GetLastLabourDevisionVer(labourId);
                    var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\BangCanDoi_Template.xlsx"));
                    using (var package = new ExcelPackage(_file))
                    {
                        var workbook = package.Workbook;
                        var sheet = workbook.Worksheets.First();

                        sheet.Cells[3, 9].Value = "Ngày: " + DateTime.Now.ToString("dd/MM/yyy");
                        sheet.Cells[4, 2].Value = ("mã hàng: " + product).ToUpper();
                        sheet.Cells[4, 5].Value = diagram.TechProcess.NumberOfWorkers;
                        sheet.Cells[5, 5].Value = diagram.TechProcess.TimeCompletePerCommo;
                        sheet.Cells[6, 5].Value = diagram.TechProcess.PacedProduction;

                        sheet.Cells[4, 8].Value = diagram.TechProcess.ProOfGroupPerHour;
                        sheet.Cells[5, 8].Value = diagram.TechProcess.ProOfGroupPerDay;

                        if (diagram.Positions.Count > 0)
                        {
                            int _row = 9;
                            var _positions = diagram.Positions.OrderBy(x => x.OrderIndex).ToList();
                            for (int i = 0; i < _positions.Count; i++)
                            {
                                sheet.Cells[_row, 1].Value = _positions[i].OrderIndex;
                                sheet.Cells[_row, 2].Value = (_positions[i].EmployeeId.HasValue ? _positions[i].EmployeeName : "").ToUpper();
                                _AddCellBorder(sheet, _row);
                                if (_positions[i].Details.Count > 0)
                                {
                                    sheet.Cells[_row, 1, _row + _positions[i].Details.Count - 1, 1].Merge = true;
                                    sheet.Cells[_row, 2, _row + _positions[i].Details.Count - 1, 2].Merge = true;
                                    for (int ii = 0; ii < _positions[i].Details.Count; ii++)
                                    {
                                        int _totalTMU = (int)Math.Round(_positions[i].Details[ii].TotalTMU);//,
                                                                                                            //_totalTMU_15HP = _totalTMU + (_totalTMU > 0 ? (int)Math.Round(_totalTMU * 15 / 100) : 0);



                                        sheet.Cells[_row, 3].Value = _positions[i].Details[ii].PhaseCode;
                                        sheet.Cells[_row, 4].Value = _positions[i].Details[ii].PhaseName;
                                        sheet.Cells[_row, 5].Value = _positions[i].Details[ii].TotalTMU;
                                        sheet.Cells[_row, 6].Value = 0;// _positions[i].Details[ii].TotalTMU;
                                        sheet.Cells[_row, 7].Value = 0;// _positions[i].Details[ii].TotalTMU; //DM
                                        sheet.Cells[_row, 8].Value = 0;// _positions[i].Details[ii].TotalTMU; //TG can
                                        sheet.Cells[_row, 9].Value = 0;// _positions[i].Details[ii].TotalTMU; //TG lech
                                        _AddCellBorder(sheet, _row);
                                        _row++;
                                    }
                                }
                                else
                                    _row++;
                            }
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(package.GetAsByteArray());
                        DateTime dateNow = DateTime.Now;
                        string fileName = "WorkerBalance_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.ContentType = "application/excel";
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.Flush();
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void _AddCellBorder(ExcelWorksheet sheet, int rowIndex)
        {
            sheet.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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

        #region bang can doi chuyền ns thực tế - định mức
        public ActionResult ReportWorkerBalance_Realtime()
        {
            return View();
        }

        //[HttpPost]
        //public JsonResult GetReportWorkerBalance(int labourId)
        //{
        //    try
        //    {
        //        if (isAuthenticate)
        //        {
        //            var objs = BLLLabourDivision.Instance.GetLastLabourDevisionVer(labourId);
        //            JsonDataResult.Records = objs;
        //            JsonDataResult.Result = "OK";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonDataResult.Result = "ERROR";
        //        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
        //    }
        //    return Json(JsonDataResult);
        //}

        public void ExportReportWorkerBalance_Realtime(int labourId, string product)
        {
            if (isAuthenticate)
            {
                try
                {
                    var diagram = BLLLabourDivision.Instance.GetLastLabourDevisionVer(labourId);
                    var _file = new FileInfo(Server.MapPath(@"~\ReportTemplates\BangCanDoi_realtime_Template.xlsx"));
                    using (var package = new ExcelPackage(_file))
                    {
                        var workbook = package.Workbook;
                        var sheet = workbook.Worksheets.First();

                        sheet.Cells[3, 9].Value = "Ngày: " + DateTime.Now.ToString("dd/MM/yyy");
                        sheet.Cells[4, 2].Value = ("mã hàng: " + product).ToUpper();
                        sheet.Cells[4, 5].Value = diagram.TechProcess.NumberOfWorkers;
                        sheet.Cells[5, 5].Value = diagram.TechProcess.TimeCompletePerCommo;
                        sheet.Cells[6, 5].Value = diagram.TechProcess.PacedProduction;

                        sheet.Cells[4, 8].Value = diagram.TechProcess.ProOfGroupPerHour;
                        sheet.Cells[5, 8].Value = diagram.TechProcess.ProOfGroupPerDay;

                        if (diagram.Positions.Count > 0)
                        {
                            int _row = 9;
                            var _positions = diagram.Positions.OrderBy(x => x.OrderIndex).ToList();
                            for (int i = 0; i < _positions.Count; i++)
                            {
                                sheet.Cells[_row, 1].Value = _positions[i].OrderIndex;
                                AddBorder(sheet.Cells[_row, 1]);
                                if (_positions[i].Details.Count == 0)
                                {
                                    sheet.Cells[_row, 2, _row, 3].Merge = true;
                                    AddBorder(sheet.Cells[_row, 2, _row, 3]);
                                }
                                sheet.Cells[_row, 2].Value = (_positions[i].EmployeeId.HasValue ? _positions[i].EmployeeName : "").ToUpper();
                                //  _AddCellBorder(sheet, _row);
                                if (_positions[i].Details.Count > 0)
                                {
                                    sheet.Cells[_row, 1, _row + _positions[i].Details.Count - 1, 1].Merge = true;
                                    sheet.Cells[_row, 2, _row + _positions[i].Details.Count - 1, 3].Merge = true;
                                    AddBorder(sheet.Cells[_row, 2, _row + _positions[i].Details.Count - 1, 3]);
                                    for (int ii = 0; ii < _positions[i].Details.Count; ii++)
                                    {
                                        int _totalTMU = (int)Math.Round(_positions[i].Details[ii].TotalTMU);//,
                                                                                                            //_totalTMU_15HP = _totalTMU + (_totalTMU > 0 ? (int)Math.Round(_totalTMU * 15 / 100) : 0);


                                        sheet.Cells[_row, 4].Value = _positions[i].Details[ii].PhaseCode;
                                        AddBorder(sheet.Cells[_row, 4]);
                                        sheet.Cells[_row, 5, _row, 7].Merge = true;
                                        sheet.Cells[_row, 5].Value = _positions[i].Details[ii].PhaseName;
                                        AddBorder(sheet.Cells[_row, 5, _row, 7]);

                                        sheet.Cells[_row, 8].Value = 0;// _positions[i].Details[ii].TotalTMU; //TG can
                                        AddBorder(sheet.Cells[_row, 8]);
                                        sheet.Cells[_row, 9].Value = 0;// _positions[i].Details[ii].TotalTMU; //TG lech
                                        AddBorder(sheet.Cells[_row, 9]);
                                     
                                        _row++;
                                    }
                                }
                                else
                                {
                                    AddBorder(sheet.Cells[_row, 4]);
                                    sheet.Cells[_row, 5, _row, 7].Merge = true;
                                    AddBorder(sheet.Cells[_row, 5, _row, 7]);
                                    AddBorder(sheet.Cells[_row, 8]);
                                    AddBorder(sheet.Cells[_row, 9]); 
                                    _row++;
                                }
                                   
                            }
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(package.GetAsByteArray());
                        DateTime dateNow = DateTime.Now;
                        string fileName = "WorkerBalance_" + dateNow.ToString("yyMMddhhmmss") + ".xlsx";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.ContentType = "application/excel";
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.Flush();
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void AddBorder(ExcelRange cell)
        {
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }


        #endregion
    }
}