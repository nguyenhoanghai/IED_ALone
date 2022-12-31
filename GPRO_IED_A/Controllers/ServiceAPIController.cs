using GPRO_IED_A.App_Global;
using GPRO_IED_A.Business;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using GPRO_IED_A.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GPRO_IED_A.Controllers
{
    public class ServiceAPIController : ApiController
    {
        [HttpGet]
        public APIResultModel Login(string userName, string password)
        {
            var rs = BLLUser.Instance.Login(userName, password);
            var result = new APIResultModel();
            if (!rs.IsSuccess)
            {
                result.Status = "ERROR";
                result.ResultInfo = rs.Errors[0].Message;
            }
            else
            {
                result.Status = "OK";
                int userId = (int)rs.Data;
                result.ResultInfo = JsonConvert.SerializeObject(BLLUser.Instance.GetUserService(userId, AppGlobal.MODULE_NAME));
            }
            return result;
        }


        //public APIResultModel GetItems(int type, int parentid)
        //{
        //    var result = new APIResultModel();

        //    result.Status = "OK";
        //    result.ResultInfo = JsonConvert.SerializeObject(BLLCommodityAnalysis.Instance.GetCommoAna(type, parentid));

        //    return result;
        //}
        //public APIResultModel GetLines(int parentid)
        //{
        //    var result = new APIResultModel();

        //    result.Status = "OK";
        //    result.ResultInfo = JsonConvert.SerializeObject(BLLLabourDivision.Instance.GetItems(parentid));

        //    return result;
        //}
        public APIResultModel GetTKCs(int lineid)
        {
            var result = new APIResultModel();
            result.Status = "OK";
            result.ResultInfo = JsonConvert.SerializeObject(BLLLabourDivision.Instance.GetTKCs(lineid));
            return result;
        }

        public APIResultModel GetTKCInfo(int labourVerid, int userId, string date)
        {
            var result = new APIResultModel();
            result.Status = "OK";
            result.ResultInfo = JsonConvert.SerializeObject(BLLLabourDivision.Instance.GetTKCInfoById(labourVerid, userId, date));

            return result;
        }

        //public APIResultModel GetEmployees(int labourId)
        //{
        //    var result = new APIResultModel();

        //    result.Status = "OK";
        //    var lb = BLLLabourDivision.Instance.Get(labourId);
        //    int _lineid = 0;
        //    if (lb != null)
        //        _lineid = lb.LineId;
        //    result.ResultInfo = JsonConvert.SerializeObject(BLLEmployee.Instance.GetSelectItem(_lineid));

        //    return result;
        //}

        [HttpGet]
        public APIResultModel InsertLinePositonQuantities(string _model)
        {
            var result = new APIResultModel();
            result.Status = "OK";
            var model = JsonConvert.DeserializeObject<APILinePositionModel>(_model);
            var rs = BLLLinePoDailyQuantities.Instance.Insert(model);
            result.ResultInfo = rs.IsSuccess ? "OK" : rs.Errors.FirstOrDefault().Message;
            return result;
        }
    }
}
