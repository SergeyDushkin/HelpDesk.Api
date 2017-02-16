using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;


namespace servicedesk.api
{
   [Route("[controller]")]
   public class SettingsController : ControllerBase
   {
        private readonly SettingsService service;

        public SettingsController(SettingsService service)
        {
            this.service = service;
        }

        [Route("{code}")]
        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            var record = await this.service.GetByCodeAsync<SMTPSettings>(code);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("smtp")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]SMTPSettings value)
        {
            await this.service.SetByCodeAsync("smtp", value);
            return Ok();
        }

       
    }

}
