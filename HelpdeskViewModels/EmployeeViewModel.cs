/**
 * File name:	    EmployeeViewModel.cs
 * Purpose: 		Acts as an intermediary between the user interface and the data access layer for employee data.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;
        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Id { get; set; }
        public bool? IsTech { get; set; }
        public string? StaffPicture64 { get; set; }

        // constructor 
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }


        public async Task GetByEmail()
        {
            try
            {
                Employee emp = await _dao.GetByEmail(Email!);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "not found";
            }
            catch (Exception ex)
            {
                Email = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task GetById()
        {
            try
            {
                Employee emp = await _dao.GetById((int)(Id!));
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();
            try
            {
                List<Employee> allEmployees = await _dao.GetAll();

                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new()
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        DepartmentName = emp.Department.DepartmentName,
                        IsTech = emp.IsTech ?? false,
                        Timer = Convert.ToBase64String(emp.Timer!)
                    };

                    if (emp.StaffPicture != null)
                    {
                        empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }

                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }


        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null
                };
                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        
        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = (int)Id!,
                    DepartmentId = DepartmentId,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null,
                    Timer = Convert.FromBase64String(Timer!)
                };
                updateStatus = -1; // start out with a failed state
                updateStatus = Convert.ToInt16(await _dao.Update(emp)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }


        public async Task<int> Delete()
        {
            try
            {
                return await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task GetByPhoneNumber()
        {
            try
            {
                Employee emp = await _dao.GetByPhoneNumber(Phoneno!);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Phoneno = "not found";
            }
            catch (Exception ex)
            {
                Phoneno = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }



    }
}
