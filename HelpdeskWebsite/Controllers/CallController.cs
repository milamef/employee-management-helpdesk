/**
 * File name:	    CallController.cs
 * Purpose: 		Handles HTTP requests for call data in the Helpdesk web application.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                CallViewModel viewmodel = new() { Id = id };
                await viewmodel.GetById();
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                CallViewModel viewmodel = new();
                List<CallViewModel> allCalls = await viewmodel.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post(CallViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Call " + viewmodel.Id + " added!" })
                : Ok(new { msg = "Call " + viewmodel.Id + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                  MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut]
        public async Task<ActionResult> Put(CallViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Call " + viewmodel.Id + " updated!" }),
                    -1 => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Id + ", Call not updated!" }),
                    _ => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                CallViewModel viewmodel = new() { Id = id };
                return await viewmodel.Delete() == 1
                    ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                  MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
