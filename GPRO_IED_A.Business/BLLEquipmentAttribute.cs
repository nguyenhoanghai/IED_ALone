using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business
{
   public class BLLEquipmentAttribute
    {
       #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLEquipmentAttribute _Instance;
        public static BLLEquipmentAttribute Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEquipmentAttribute();

                return _Instance;
            }
        }
        private BLLEquipmentAttribute() { }
        #endregion
    }
}
