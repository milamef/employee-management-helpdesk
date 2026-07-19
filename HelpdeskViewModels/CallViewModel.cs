/**
 * File name:	    CallViewModel.cs
 * Purpose: 		Provides a bridge between the user interface and the data access layer for call data.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private readonly CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProblemDescription { get; set; }
        public string? TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string? Notes { get; set; }
        public string? Timer { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }


        public async Task GetById()
        {
            try
            {
                Call call = await _dao.GetById((int)(Id!));
                Id = call.Id;
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                TechId = call.TechId;
                EmployeeName = call.Employee.LastName;
                ProblemDescription = call.Problem.Description;
                TechName = call.Tech.LastName;
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;
                Timer = Convert.ToBase64String(call.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                ProblemDescription = "not found";
            }
            catch (Exception ex)
            {
                ProblemDescription = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task<List<CallViewModel>> GetAll()
        {
            List<CallViewModel> allVms = new();
            try
            {
                List<Call> allCalls = await _dao.GetAll();

                foreach (Call call in allCalls)
                {
                    CallViewModel callVm = new()
                    {
                        Id = call.Id,
                        EmployeeId = call.EmployeeId,
                        ProblemId = call.ProblemId,
                        EmployeeName = call.Employee.LastName,
                        ProblemDescription = call.Problem.Description,
                        TechName = call.Tech.LastName,
                        TechId = call.TechId,
                        DateOpened = call.DateOpened,
                        DateClosed = call.DateClosed,
                        OpenStatus = call.OpenStatus,
                        Notes = call.Notes,
                        Timer = Convert.ToBase64String(call.Timer!)
                    };

                    allVms.Add(callVm);
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
                Call call = new()
                {
                    EmployeeId = EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes!
                };
                Id = await _dao.Add(call);
                Timer = Convert.ToBase64String(call.Timer!);
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
                Call call = new()
                {
                    Id = Id,
                    EmployeeId = EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes!,
                    Timer = string.IsNullOrEmpty(Timer) ? null : Convert.FromBase64String(Timer)
                };

                updateStatus = -1; // start out with a failed state
                updateStatus = Convert.ToInt16(await _dao.Update(call)); // overwrite status

                if (updateStatus == 1)
                {
                    Timer = Convert.ToBase64String(call.Timer!);
                }
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
    }
}
