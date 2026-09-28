using Assignment2.DTOs;
using Assignment2.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportItemController : ControllerBase
    {
        public readonly IReportProjectService _reportProjectService;

        public ReportItemController(IReportProjectService? reportProjectService)
        {
            _reportProjectService = reportProjectService;
        }
        [HttpGet("project-summary")]
        public async Task<IActionResult> ReportProject(int minItems = 0)
        {
            var workItems = await _reportProjectService.ReportProject(minItems);
            return Ok(workItems);
        }
    }
}
