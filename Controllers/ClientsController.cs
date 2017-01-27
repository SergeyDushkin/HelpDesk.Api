using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using RawRabbit.Extensions.Client;
using RawRabbit.Configuration.Exchange;
using servicedesk.api.Storages;
using servicedesk.Services.Tickets.Shared.Commands;
using servicedesk.Common.Queries;

namespace servicedesk.api
{
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientStorage storage;
        private readonly IBusClient bus;
        public ClientsController(IClientStorage storage, IBusClient busClient)
        {
            this.storage = storage;
            this.bus = busClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetByReferenceId query)
        {
            var result = await storage.BrowseAsync(query);
            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = await this.storage.GetAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ClientRegistered client)
        {            
            var commandId = Guid.NewGuid();
            
            var command = new CreateClient 
            {
                Request  = servicedesk.Common.Commands.Request.Create<CreateAddress>(commandId, "servicedesk.Services.Tickets", "ru-ru"),
                Name = client.Name,
                UserId =  User.Identity.Name ?? "unauthenticated user"
            };

            await bus.PublishAsync(command, commandId, cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.Services.Tickets"))
                .WithRoutingKey("client.create"));

            return await Task.FromResult(Accepted(command));
        }
    }
}
