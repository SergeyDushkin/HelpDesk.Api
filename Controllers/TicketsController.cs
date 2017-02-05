using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using RawRabbit.Extensions.Client;
using RawRabbit.Configuration.Exchange;
using servicedesk.api.Storages;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Commands;

namespace servicedesk.api
{
    [Route("[controller]"), Authorize]
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
        public async Task<IActionResult> Post([FromBody]CreateTicket create) 
        {
            var commandId = Guid.NewGuid();
            
            var command = new CreateTicket 
            {
                Request  = servicedesk.Common.Commands.Request.Create<CreateTicket>(commandId, "servicedesk.Services.Tickets", "ru-ru"),

                ClientId = create.ClientId,
                AddressId = create.AddressId,
                ContractId = create.ContractId,
                Description = create.Description,
                BusinessUnitId = create.BusinessUnitId,
                OperatorId = create.OperatorId,
                PriorityId = create.PriorityId,
                ServiceId = create.ServiceId,
                StartDate = create.StartDate,
                StatusId = create.StatusId,
                UserId = User.Identity.Name ?? "unauthenticated user"
            };

            await bus.PublishAsync(command, commandId, cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.Services.Tickets"))
                .WithRoutingKey("ticket.create"));
                
            return await Task.FromResult(Accepted(command));
        }
    }
}
