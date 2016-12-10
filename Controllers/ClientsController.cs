using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace servicedesk.api
{
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService service;
        public ClientsController(ClientService service)
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ClientRegistered client)
        {
            var record = await this.service.RegisterAsync(client);
            return Ok(record);
        }
    }
}
