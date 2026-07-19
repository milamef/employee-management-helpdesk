/**
 * File name:	    DepartmentDAO.cs
 * Purpose: 		Handles data access operations related to Department records.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        readonly IRepository<Department> _repo;
        public DepartmentDAO()
        {
            _repo = new HelpdeskRepository<Department>();
        }


        public async Task<List<Department>> GetAll()
        {
            List<Department> allDepartments;
            try
            {
                allDepartments = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allDepartments;
        }
    }
}
