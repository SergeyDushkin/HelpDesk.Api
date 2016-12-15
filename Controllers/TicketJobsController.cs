using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace servicedesk.api
{
    [Route("tickets/{ticketId}/jobs")]
    public class TicketJobsController : ControllerBase
    {
        private readonly JobService service;
        public TicketJobsController(JobService service)
        {
            this.service = service;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get(Guid ticketId)
        {
            var query = await this.service.GetAsync(ticketId);
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid ticketId, Guid id)
        {
            var record = await this.service.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid ticketId, Guid id, [FromBody]Job job)
        {
            var record = await this.service.GetByIdAsync( id);

            if (record == null)
            {
                return NotFound();
            }

            //await service.UpdateAsync(ticketId, job);

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(Guid ticketId, [FromBody]JobCreated created)
        {
            var job = await service.CreateAsync(ticketId, created);
            return Created(job.Id.ToString(), job);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid ticketId, Guid id)
        {
            var deleted = await this.service.GetByIdAsync(id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(id);

            return NoContent();
        }
    }
}
