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
    public class ProblemViewModel
    {
        private readonly ProblemDAO _dao;
        public int? Id { get; set; }
        public string? Description { get; set; }

        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        public async Task GetByDescription()
        {
            try
            {
                Problem prob = await _dao.GetByDescription(Description!);
                Id = prob.Id;
                Description = prob.Description;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

        }


        public async Task<List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> allVms = new();
            try
            {
                List<Problem> allProblems = await _dao.GetAll();
                foreach (Problem prob in allProblems)
                {
                    ProblemViewModel probVm = new()
                    {
                        Id = prob.Id,
                        Description = prob.Description
                    };
                    allVms.Add(probVm);
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
