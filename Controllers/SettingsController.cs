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
/*
        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            var query = await this.service.GetAsync();
            return Ok(query);
        }
*/
        [Route("{code}")]
        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            var record = await this.service.GetByCodeAsync(code);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }
/*
        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid id, [FromBody]Supplier supplier)
        {
            var record = await this.service.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            await service.UpdateNameAsync(id, supplier.Name);

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody]ServiceCreated created)
        {
            var supplier = await service.CreateAsync(created);
            return Created(supplier.Id.ToString(), supplier);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await this.service.GetByIdAsync(id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(id);

            return NoContent();
        }
        */
    }
    
}
