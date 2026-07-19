/**
 * File name:	    EmployeeDAO.cs
 * Purpose: 		Provides data access methods for performing CRUD operations on Employee records.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employee> _repo;
        public EmployeeDAO()
        {
            _repo = new HelpdeskRepository<Employee>();
        }


        public async Task<Employee> GetByEmail(string email)
        {
            Employee? selectedEmployee;
            try
            {
                selectedEmployee = await _repo.GetOne(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee!;
        }


        public async Task<Employee> GetById(int id)
        {
            Employee? selectedEmployee;
            try
            {
                selectedEmployee = await _repo.GetOne(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee!;
        }


        public async Task<List<Employee>> GetAll()
        {
            List<Employee> allEmployees;
            try
            {
                allEmployees = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allEmployees;
        }


        public async Task<int> Add(Employee newEmployee)
        {
            return (await _repo.Add(newEmployee)).Id;
        }


        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            UpdateStatus status;
            try
            {
                status = await _repo.Update(updatedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return status;
        }


        public async Task<int> Delete(int? id)
        {
            int employeesDeleted = -1;
            try
            {
                employeesDeleted = await _repo.Delete((int)id!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return employeesDeleted;
        }


        public async Task<Employee> GetByPhoneNumber(string phoneNum)
        {
            Employee? selectedEmployee;
            try
            {
                selectedEmployee = await _repo.GetOne(emp => emp.PhoneNo == phoneNum);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee!;
        }

    }
}
