using GPRO_IED_A.Data;
using System.Web.Mvc;

namespace GPRO_IED_A.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Reset_SQLLog(string databaseName)
        {
            using (var db = new IEDEntities())
            {
                string query = string.Format(@" USE [{0}]; 
                                                ALTER DATABASE [{1}] SET RECOVERY SIMPLE;                                                      
                                                DBCC SHRINKFILE ([{2}_Log], 1);  
                                                ALTER DATABASE [{3}] SET RECOVERY FULL;", databaseName, databaseName, databaseName, databaseName);
                db.Database.ExecuteSqlCommand(query);
                db.SaveChanges();

            }
            return View();
        }
    }
}