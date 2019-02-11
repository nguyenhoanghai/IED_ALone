using GPRO.Core.Mvc;
using GPRO.Ultilities;
using GPRO_IED_A.Business.Model;
using GPRO_IED_A.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPRO_IED_A.Business
{
    public class BLLEmployee
    {
        #region constructor
        IEDEntities db;
        static object key = new object();
        private static volatile BLLEmployee _Instance;
        public static BLLEmployee Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLEmployee();

                return _Instance;
            }
        }
        private BLLEmployee() { }
        #endregion

        public ResponseBase Delete(int Id, int actionUserId, int companyId)
        {
            ResponseBase result;
            try
            {
                using (db = new IEDEntities())
                {
                    result = new ResponseBase();
                    var obj = db.HR_Employee.Where(c => c.CompanyId == companyId && !c.IsDeleted && c.Id == Id).FirstOrDefault();
                    if (obj == null)
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete", Message = "Dữ liệu đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                    }
                    else
                    {
                        obj.IsDeleted = true;
                        obj.DeletedUser = actionUserId;
                        obj.DeletedDate = DateTime.Now;
                        db.SaveChanges();
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase CreateOrUpdate(EmployeeModel model)
        {
            ResponseBase result = null;
            HR_Employee employee = null;
            bool flag = false;
            try
            {
                using (db = new IEDEntities())
                {
                    result = new ResponseBase();
                    if (!string.IsNullOrEmpty(model.Code))  // kiem tra ma nhan vien neu có
                    {
                        if (CheckExists(model.Code, model.CompanyId, model.Id, 1, db))
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "add new", Message = "Mã nhân viên này đã tồn tại. Vui lòng chọn lại Mã Khác" });
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        if (model.Id == 0)
                        {
                            employee = new HR_Employee();
                            Parse.CopyObject(model, ref employee);
                            employee.Image = employee.Image == "0" ? "" : employee.Image;
                            employee.CreatedUser = model.ActionUser;
                            employee.CreatedDate = DateTime.Now;
                            db.HR_Employee.Add(employee);
                        }
                        else
                        {
                            employee = db.HR_Employee.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id && x.CompanyId == model.CompanyId);
                            if (employee != null)
                            {
                                employee.FirstName = model.FirstName;
                                employee.LastName = model.LastName;
                                employee.Gender = model.Gender;
                                employee.Birthday = model.Birthday;
                                employee.Code = model.Code;
                                employee.CompanyId = model.CompanyId;
                                if (model.Image != null)
                                    employee.Image = model.Image; // hinh
                                employee.UpdatedUser = model.ActionUser;
                                employee.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "update employee", Message = "Không tìm thấy Nhân Viên này. \nCó thể nhân viên đã bị xóa hoặc không tồn tại. \nVui lòng kiểm tra lại dữ liệu." });
                            }
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckExists(string checkValue, int? companyId, int employeeId, int typeOfCheck, IEDEntities db)
        {
            try
            {
                HR_Employee employee = null;
                checkValue = checkValue.Trim().ToUpper();
                if (typeOfCheck == 1)
                    employee = db.HR_Employee.FirstOrDefault(x => !x.IsDeleted && x.CompanyId == companyId && x.Id != employeeId && x.Code.Trim().ToUpper().Equals(checkValue));

                if (employee == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedList<EmployeeModel> Gets(string keyWord, int companyId, int startIndexRecord, int pageSize, string sorting)
        {
            PagedList<EmployeeModel> pagelistReturn = null;
            List<EmployeeModel> employees = null;
            try
            {
                if (string.IsNullOrEmpty(sorting))
                    sorting = "CreatedDate DESC";
                employees = GetEmployees(keyWord, sorting, companyId);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                pagelistReturn = new PagedList<EmployeeModel>(employees, pageNumber, pageSize);
                return pagelistReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<EmployeeModel> GetEmployees(string keyWord, string sorting, int companyId)
        {
            try
            {
                using (db = new IEDEntities())
                {
                    IQueryable<EmployeeModel> objs;
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        keyWord = keyWord.Trim().ToUpper();
                        objs = db.HR_Employee.Where(x => !x.IsDeleted && x.CompanyId == companyId && (x.FirstName.Trim().ToUpper().Contains(keyWord) || x.LastName.Trim().ToUpper().Contains(keyWord) || x.Code.Trim().ToUpper().Contains(keyWord) || x.Mobile.Trim().ToUpper().Contains(keyWord) || x.Email.Trim().ToUpper().Contains(keyWord))).OrderByDescending(x => x.CreatedDate).Select(x => new EmployeeModel()
                      {
                          Gender = x.Gender,
                          Birthday = x.Birthday,
                          Id = x.Id,
                          Code = x.Code,
                          Email = x.Email,
                          FullName = x.LastName.Trim() + " " + x.FirstName.Trim(),
                          Mobile = x.Mobile
                      });
                    }
                    else
                    {
                        objs = db.HR_Employee.Where(x => !x.IsDeleted && x.CompanyId == companyId).OrderByDescending(x => x.CreatedDate).Select(x => new EmployeeModel()
                        {
                            Gender = x.Gender,
                            Birthday = x.Birthday,
                            Id = x.Id,
                            Code = x.Code,
                            Email = x.Email,
                            FullName = x.LastName.Trim() + " " + x.FirstName.Trim(),
                            Mobile = x.Mobile
                        });
                    }

                    if (objs != null && objs.Count() > 0)
                        return objs.ToList();
                }
            }
            catch (Exception)
            {
            }
            return new List<EmployeeModel>();
        }

        //public List<HR_Employee> GetEmployees(int companyId)
        //{
        //    return repEmployee.GetMany(x => !x.IsDeleted && x.CompanyId == companyId).ToList();
        //}

        public List<EmployeeWithSkillModel> GetEmployeeWithSkills(int companyId)
        {
            using (db = new IEDEntities())
            {
                return db.HR_Employee.Where(x => !x.IsDeleted && x.CompanyId == companyId).Select(x => new EmployeeWithSkillModel()
                           {
                               EmployeeId = x.Id,
                               EmployeeCode = x.Code,
                               EmployeeName = (x.FirstName + " " + x.LastName),
                               LastName = x.LastName
                           }).ToList();
            };
        }

    }
}
