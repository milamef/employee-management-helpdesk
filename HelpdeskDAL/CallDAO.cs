/**
 * File name:	    CallDAO.cs
 * Purpose: 		Handles data access operations related to Call records.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class CallDAO
    {
        readonly IRepository<Call> _repo;
        public CallDAO()
        {
            _repo = new HelpdeskRepository<Call>();
        }


        public async Task<Call> GetById(int id)
        {
            Call? selectedCall;
            try
            {
                selectedCall = await _repo.GetOne(call => call.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedCall!;
        }


        public async Task<List<Call>> GetAll()
        {
            List<Call> allCalls;
            try
            {
                allCalls = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allCalls;
        }


        public async Task<int> Add(Call newCall)
        {
            return (await _repo.Add(newCall)).Id;
        }


        public async Task<UpdateStatus> Update(Call updatedCall)
        {
            UpdateStatus status;
            try
            {
                status = await _repo.Update(updatedCall);
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
            int callsDeleted = -1;
            try
            {
                callsDeleted = await _repo.Delete((int)id!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return callsDeleted;
        }
    }
}
