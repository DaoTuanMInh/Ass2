using Assignment2.Common;
using Assignment2.DTOs;
using Assignment2.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Assignment2.Controllers
{
    [Route("api/work-items")]
    [ApiController]
    public class Controller : ControllerBase
    {
        public readonly IWorkItemService _workItemService;
        public Controller(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }
        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            var connection = await _workItemService.HealChecK();

            if (!connection)
            {
                return StatusCode(503, new
                {
                    traceId = HttpContext.TraceIdentifier,
                    message = "thất bại",
                    data = new { status = "ok", dbConnected = false }
                });

            }
            return Ok(new
            {
                traceId = HttpContext.TraceIdentifier,
                message = "Thành công",
                data = new { status = "available", dbConnected = true }
            });
        }
        [HttpDelete("work-items/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await _workItemService.Delete(id);
            if (!res)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("work-items/{id}")]
        public async Task<IActionResult> GetItemDetails(long id)
        {
            try
            {
                var itemDetails = await _workItemService.GetItemDetails(id);
                return Ok(itemDetails);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPost("work-items")]
        public async Task<IActionResult> AddWorkItem([FromBody] AddWorkItem workItem)
        {
            try
            {
                var newWorkItem = await _workItemService.AddWorkItem(workItem);
                return Ok(newWorkItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("work-items/{id}/assignee")]
        public async Task<IActionResult> AssignWorkItem(long id, AssigneeItem assigneeItem)
        {
            try
            {
                var result = await _workItemService.AssigItem(id, assigneeItem);
                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] WorkItemFilterDto filter)
        {
            var result = await _workItemService.GetWorkItemsList(filter);
            return Ok(new
            {
                traceId = HttpContext.TraceIdentifier,
                status = 200,
                message = "Thành công",
                data = result
            });
        }
        [HttpGet("filter-history")]
        public async Task<IActionResult> FilterHistory([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var filter = await _workItemService.FileterHistory(startDate, endDate);
                return Ok(filter);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}/history")]
        public async Task<IActionResult> HistoryDetails(long id)
        {
            try
            {
                var his = await _workItemService.HistoryDetails(id);
                return Ok(his);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("{id}/notes")]
        public async Task<IActionResult> Note(long id,[FromBody] string note)
        {
            try
            {
                var res = await _workItemService.Note(id,note);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
