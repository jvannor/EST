using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using WebApi.Models;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public ReportController(ReportService service) =>
            reportService = service;

        [HttpGet]
        public async Task<List<Report>> Get() => await reportService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Report>> Get(string id)
        {
            var report = await reportService.GetAsync(id);
            if (report is null)
            {
                return NotFound();
            }

            return report;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Report newReport)
        {
            await reportService.CreateAsync(newReport);
            return CreatedAtAction(nameof(Get), new { id = newReport.Id }, newReport);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Report updatedReport)
        {
            var report = await reportService.GetAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            updatedReport.Id = report.Id;
            await reportService.UpdateAsync(id, updatedReport);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var report = await reportService.GetAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            await reportService.RemoveAsync(report.Id);
            return NoContent();
        }

        private readonly ReportService reportService;
    }
}
