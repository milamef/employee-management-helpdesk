/**
 * File name:	    DepartmentViewModel.cs
 * Purpose: 		Provides a bridge between the user interface and the data access layer for department data.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private readonly DepartmentDAO _dao;

        public int? Id { get; set; }
        public string? DepartmentName { get; set; }


        // constructor 
        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new();
            try
            {
                List<Department> allDepartments = await _dao.GetAll();
                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVm = new()
                    {
                        Id = dep.Id,
                        DepartmentName = dep.DepartmentName
                    };
                    allVms.Add(depVm);
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

    }
}
