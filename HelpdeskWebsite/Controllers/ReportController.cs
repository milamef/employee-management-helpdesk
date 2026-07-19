using Microsoft.AspNetCore.Mvc;
using HelpdeskWebsite.Reports;
namespace HelpdeskWebsite.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public ReportController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Route("api/employeereport")]
        [HttpGet]
        public IActionResult GetEmployeeReport()
        {
            Reports.Reports emp = new();
            emp.GenerateEmployeeReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }


        [Route("api/callreport")]
        [HttpGet]
        public IActionResult GetCallReport()
        {
            Reports.Reports call = new();
            call.GenerateCallReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }
    }
}