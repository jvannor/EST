using Microsoft.AspNetCore.Mvc;
using System.Web;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        // GET: api/<ReportController>/subject
        [HttpGet("{subject}")]
        public IEnumerable<Report> Get(
            string subject, 
            [FromQuery] DateTime? begin, 
            [FromQuery] DateTime? end)
        {
            return new Report[] 
            {
                new Report
                {
                    Id = new Guid("352e7bfe-ca8d-43cf-aced-1a421df06b24"),
                    ReportType = "SeizureReport",
                    Subject = HttpUtility.UrlDecode(subject),
                    Author = HttpUtility.UrlDecode(subject),
                    Created = begin != null ? (DateTime)begin : DateTime.UtcNow,
                    Modified = begin != null ? (DateTime)begin : DateTime.UtcNow,
                    Revision = 1,
                    Description = "Seizure Report"
                },
                new Report
                {
                    Id = new Guid("d8e488c1-df39-4795-a647-db1b0cffd273"),
                    ReportType = "SeizureReport",
                    Subject = HttpUtility.UrlDecode(subject),
                    Author = HttpUtility.UrlDecode(subject),
                    Created = end != null ? (DateTime)end : DateTime.UtcNow,
                    Modified = end != null ? (DateTime)end : DateTime.UtcNow,
                    Revision = 1,
                    Description = "Seizure Report"
                }
            };
        }

        // GET api/<ReportController>/subject/id
        [HttpGet("{subject}/{id}")]
        public Report Get(string subject, string id)
        {
            return new Report
            {
                Id = new Guid("92240664-6f84-46b9-affa-6f22e5b76951"),
                ReportType = "SeizureReport",
                Subject = HttpUtility.UrlDecode(subject),
                Author = HttpUtility.UrlDecode(subject),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Revision = 1,
                Description = "Seizure Report"
            };
        }

        // POST api/<ReportController>/subject
        [HttpPost("{subject}")]
        public void Post(string subject, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/<ReportController>/subject
        [HttpPut("{subject}")]
        public void Put(string subject, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<ReportController>/subject/id
        [HttpDelete("{subject}/{id}")]
        public void Delete(string subject, string id)
        {
            throw new NotImplementedException();
        }
    }
}
