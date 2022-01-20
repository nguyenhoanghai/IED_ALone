using GPRO_IED_A.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public JsonResult Single()
        {
            try
            {
                string returnPath = string.Empty, path = string.Empty, last = string.Empty;
                Guid guid;

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        var filename = Path.GetFileName(file.FileName);
                        guid = Guid.NewGuid();
                        returnPath = "~/UploadFile/Files/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                        string directoryPath = Server.MapPath(returnPath);
                        if (!System.IO.Directory.Exists(directoryPath))
                            System.IO.Directory.CreateDirectory(directoryPath);

                        last = (guid.ToString() + "_" + filename);
                        path = Path.Combine(Server.MapPath(returnPath), last);
                        file.SaveAs(path);
                    }
                }
                return Json((returnPath + last).Replace('~', ' '));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ProductFile()
        {
            try
            {
                string returnPath = string.Empty, path = string.Empty, last = string.Empty;
                Guid guid;
                var listFiles = new List<T_ProductFile>();
                
                if ( Request.Files != null && Request.Files.Count > 0)
                {
                   
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        var filename = Path.GetFileName(file.FileName);
                        guid = Guid.NewGuid();
                        returnPath = "~/UploadFile/Files/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                        string directoryPath = Server.MapPath(returnPath);
                        if (!System.IO.Directory.Exists(directoryPath))
                            System.IO.Directory.CreateDirectory(directoryPath);

                        last = (guid.ToString() + "_" + filename);
                        path = Path.Combine(Server.MapPath(returnPath), last);
                       file.SaveAs(path);
                        listFiles.Add(new T_ProductFile()
                        {
                            FakeName = last,
                            FileName = filename,
                            Path = Path.Combine(returnPath.Replace("~", ""), last)
                        });
                    }
                }
                 var str = JsonConvert.SerializeObject(listFiles);
                  return Json(str); 
            }
            catch (Exception ex)
            {
                return Json("catch ex:" + ex.Message);
            }
        }

    }
}