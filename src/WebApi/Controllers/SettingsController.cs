using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SettingsDocument>> Get(string id)
        {
            var settings = await settingsService.GetOneAsync(id);
            if (settings is null)
            {
                return NotFound();
            }

            return settings;
        }

        [HttpGet]
        public async Task<ActionResult<SettingsDocument>> Get([FromQuery]string author, [FromQuery]string subject)
        {
            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(subject))
                return NotFound();

            author = author.ToLower();
            subject = subject.ToLower();

            var settings = await settingsService.GetOneAsync(author, subject);
            if (settings == null)
            {
                settings = await settingsService.GetOneAsync("default", "default");
                settings.Id = "";
                settings.Author = author;
                settings.Subject = subject;
                settings.Created = settings.Modified = DateTime.UtcNow;
                settings.Revision = 1;

                await settingsService.CreateAsync(settings);
                settings = await settingsService.GetOneAsync(author, subject);
            }

            return settings;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, SettingsDocument updatedSettings)
        {
            var settings = await settingsService.GetOneAsync(id);
            if (settings == null)
            {
                return NotFound();
            }

            updatedSettings.Id = settings.Id;
            await settingsService.UpdateAsync(id, updatedSettings);
            return NoContent();
        }

        private readonly SettingsService settingsService;
    }
}
