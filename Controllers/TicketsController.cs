using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace servicedesk.api
{
    [Route("api/v1/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService service;
        public TicketsController(TicketService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await this.service.GetAsync();
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = await this.service.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }
    }
}
