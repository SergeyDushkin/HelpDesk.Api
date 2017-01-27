using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using servicedesk.api.Storages;
using servicedesk.api.Queries;
using RawRabbit.Extensions.Client;
using servicedesk.Services.Tickets.Shared.Commands;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.api
{
    [Route("[controller]")] //Authorize
    public class TicketsController : ControllerBase
    {
        private readonly ITicketStorage storage;
        private readonly IBusClient bus;
        public TicketsController(ITicketStorage storage, IBusClient busClient)
        {
            this.storage = storage;
            this.bus = busClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(BrowseTickets query) => Ok(await storage.BrowseAsync(query));

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await storage.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TicketCreated created) 
        {
            var commandId = Guid.NewGuid();
            
            var command = new CreateTicket 
            {
                Request  = servicedesk.Common.Commands.Request.Create<CreateTicket>(commandId, "servicedesk.Services.Tickets", "ru-ru"),
                AddressId = created.AddressId,
                ClientId = created.ClientId,
                Description = created.Description,
                RequestDate = created.RequestDate,
                UserId =  User.Identity.Name ?? "unauthenticated user"
            };

            await bus.PublishAsync(command, commandId, cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.Services.Tickets"))
                .WithRoutingKey("ticket.create"));

            return await Task.FromResult(Accepted(command));
        }
    }
}
