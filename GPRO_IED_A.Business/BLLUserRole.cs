using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business
{
    public class BLLUserRole
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLUserRole _Instance;
        public static BLLUserRole Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUserRole();

                return _Instance;
            }
        }
        private BLLUserRole() { }
        #endregion
        public List<ModelSelectItem> GetUserRolesModelByUserId(int userId, bool IsOwner, int companyId)
        {
            List<ModelSelectItem> roles = null;
            try
            {
                using (db = new IEDEntities())
                {
                    if (IsOwner)
                    {
                        roles = db.SRoLes.Where(x => !x.IsDeleted && x.CompanyId == companyId).Select(x => new ModelSelectItem()
                        {
                            Name = x.RoleName,
                            Value = x.Id
                        }).ToList();
                    }
                    else
                    {
                        roles = db.SUserRoles.Where(x => !x.IsDeleted && x.UserId == userId).Select(x => new ModelSelectItem()
                        {
                            Name = x.SRoLe.RoleName,
                            Value = x.Id
                        }).ToList();
                    } 
                    return roles;
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
