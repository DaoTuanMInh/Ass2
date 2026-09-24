using Assignment2.DTOs;
using Assignment2.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Assignment2.Controllers
{
    [Route("api")]
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
                    status = "503",
                    message = "thất bại",
                    data = new { status = "ok", dbConnected = false }
                });

            }
            return Ok(new
            {
                traceId = HttpContext.TraceIdentifier,
                status = "200",
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
                return CreatedAtAction(nameof(GetItemDetails), new { id = newWorkItem}, newWorkItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
