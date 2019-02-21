using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPRO_IED_A.Business
{
    public class BLLConfig
    {
        #region constructor 
        static object key = new object();
        private static volatile BLLConfig _Instance;
        public static BLLConfig Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLConfig();

                return _Instance;
            }
        }
        private BLLConfig() { }
        #endregion
        private void CheckDefault(bool isdefault, string tableName, IEDEntities db)
        {
            try
            {
                if (isdefault == true)
                {
                    var listConfig = (from c in db.SConfigs where !c.IsDeleted && c.TableName == tableName && c.IsDefault select c).ToList();
                    if (listConfig.Count > 0)
                    {
                        foreach (var config in listConfig)
                        {
                            config.IsDefault = false;
                            db.Entry<SConfig>(config).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Create(SConfig model)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    CheckDefault(model.IsDefault, model.TableName, db);
                    model.CreatedDate = DateTime.Now;
                    db.SConfigs.Add(model);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(SConfig modelConfig)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    CheckDefault(modelConfig.IsDefault, modelConfig.TableName, db);
                    var config = db.SConfigs.FirstOrDefault(c => c.TableName == modelConfig.TableName && !c.IsDeleted && c.ObjectId == modelConfig.ObjectId && c.ConpanyId == modelConfig.ConpanyId);
                    if (config != null)
                    {
                        config.IsDefault = modelConfig.IsDefault;
                        config.IsHidden = modelConfig.IsHidden;
                        config.UpdatedDate = modelConfig.UpdatedDate;
                        config.UpdatedUser = modelConfig.UpdatedUser;
                        db.Entry<SConfig>(config).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelConfig> GetListModelConfigByTableNameAndCompanyId(string tableName, int companyId)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    List<ModelConfig> listConfig = null;
                    listConfig = (from c in db.SConfigs
                                  where !c.IsDeleted && c.TableName == tableName && c.ConpanyId == companyId
                                  select new ModelConfig()
                                  {
                                      Id = c.Id,
                                      TableName = c.TableName,
                                      ObjectId = c.ObjectId,
                                      IsHidden = c.IsHidden,
                                      IsDefault = c.IsDefault
                                  }).ToList();
                    return listConfig;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ChangeIsDefaultSatusOfListConfigByTableNameAndCompanyId(string tableName, int companyId, int actionUser)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    var listConfig = (from x in db.SConfigs
                                      where x.ConpanyId == companyId && x.TableName.Equals(tableName) && x.IsDefault && !x.IsDeleted
                                      select x);
                    if (listConfig != null)
                    {
                        foreach (var config in listConfig)
                        {
                            config.IsDefault = false;
                            config.UpdatedUser = actionUser;
                            config.UpdatedDate = DateTime.Now;
                            db.Entry<SConfig>(config).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CreateConfig(string tableName, int ObjectId, bool IsDefault, bool IsShow, int companyId, int actionUser)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    if (IsDefault)
                    {
                        var listConfig = (from x in db.SConfigs
                                          where x.ConpanyId == companyId && x.TableName.Equals(tableName) && x.IsDefault && !x.IsDeleted
                                          select x);
                        if (listConfig != null)
                        {
                            foreach (var config in listConfig)
                            {
                                config.IsDefault = false;
                                config.UpdatedUser = actionUser;
                                config.UpdatedDate = DateTime.Now;
                                db.Entry<SConfig>(config).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                    var confi = new SConfig();
                    confi.ConpanyId = companyId;
                    confi.TableName = tableName;
                    confi.ObjectId = ObjectId;
                    confi.IsHidden = IsShow;
                    confi.IsDefault = IsDefault;
                    confi.CreatedUser = actionUser;
                    confi.CreatedDate = DateTime.Now;
                    db.SConfigs.Add(confi);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void UpdateConfigStatusByObjectId(int ObjectId, string tableName, bool IsDefault, bool IsShow, int actionUser)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    if (IsDefault)
                    {
                        var objs = (from x in db.SConfigs
                                    where !x.IsDeleted && x.TableName.Equals(tableName) && x.IsDefault
                                    select x);
                        if (objs != null && objs.Count() > 0)
                        {
                            foreach (var item in objs)
                            {
                                item.IsDefault = false;
                                item.UpdatedUser = actionUser;
                                item.UpdatedDate = DateTime.Now;
                                db.Entry<SConfig>(item).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                    var cf = (from x in db.SConfigs
                              where !x.IsDeleted && x.TableName.Equals(tableName) && x.ObjectId == ObjectId
                              select x).FirstOrDefault();
                    if (cf != null)
                    {
                        cf.IsHidden = IsShow;
                        cf.IsDefault = IsDefault;
                        cf.UpdatedUser = actionUser;
                        cf.UpdatedDate = DateTime.Now;
                        db.Entry<SConfig>(cf).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ChangeIsDefaultSatusOfListConfigByTableNameAndCompanyIdNotObjectId(string tableName, int companyId, int objectId, int actionUser)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    var listConfig = (from x in db.SConfigs
                                      where x.ConpanyId == companyId && x.TableName.Equals(tableName) && x.IsDefault && !x.IsDeleted && x.ObjectId != objectId
                                      select x);
                    if (listConfig != null)
                    {
                        foreach (var config in listConfig)
                        {
                            config.IsDefault = false;
                            config.UpdatedUser = actionUser;
                            config.UpdatedDate = DateTime.Now;
                            db.Entry<SConfig>(config).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void DeleteConfigByObjectId(int ObjectId, int actionUser, string tableName)
        {
            using (var db = new IEDEntities())
            {
                try
                {
                    var configOld = (from x in db.SConfigs
                                     where x.ObjectId == ObjectId && x.TableName.Equals(tableName) && !x.IsDeleted
                                     select x).FirstOrDefault();
                    if (configOld != null)
                    {
                        configOld.DeletedUser = actionUser;
                        configOld.DeletedDate = DateTime.Now;
                        db.Entry<SConfig>(configOld).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int GetDefaultValueByTableNameAndCompanyId(string tableName, int companyId)
        {
            using (var db = new IEDEntities())
            {
                int defaultValue = 0;
                try
                {
                    var config = (from x in db.SConfigs
                                  where x.TableName.Equals(tableName) && x.ConpanyId == companyId && x.IsDefault && !x.IsDeleted
                                  select x).FirstOrDefault();
                    if (config != null)
                        defaultValue = config.ObjectId ?? 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return defaultValue;
            }
        }

        public ModelListHiddenObjectIdAndDefaultValue GetListModelConfigAndDefaultValueByTableNameAndCompanyId(string tableName, int companyId,IEDEntities db)
        { 
                ModelListHiddenObjectIdAndDefaultValue model;
                List<int> listHiddenObjectId = null;
                int defaultValue = 0;
                try
                {
                    model = new ModelListHiddenObjectIdAndDefaultValue();
                    var listConfig = (from c in db.SConfigs
                                      where !c.IsDeleted && c.TableName == tableName && c.ConpanyId == companyId
                                      select new ModelConfig()
                                      {
                                          Id = c.Id,
                                          TableName = c.TableName,
                                          ObjectId = c.ObjectId,
                                          IsHidden = c.IsHidden,
                                          IsDefault = c.IsDefault
                                      });
                    if (listConfig != null)
                    {
                        var objDefault = listConfig.Where(x => x.IsDefault).FirstOrDefault();
                        listHiddenObjectId = listConfig.Where(x => x.IsHidden).Select(x => x.ObjectId ?? 0).ToList();
                        defaultValue = objDefault != null ? objDefault.ObjectId ?? 0 : 0;
                    }
                    model.listHiddenObjectId = listHiddenObjectId;
                    model.defaultValue = defaultValue;
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                } 
        }
    }
}
