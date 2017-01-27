using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using servicedesk.api.Storages;
using servicedesk.Services.Tickets.Shared.Commands;
using RawRabbit.Extensions.Client;
using servicedesk.api.Queries;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.api
{
    //[Route("clients/{clientId}/[controller]")]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService service;
        private readonly IAddressStorage storage;
        private readonly IBusClient bus;
        public AddressController(AddressService service, IAddressStorage storage, IBusClient busClient)
        {
            this.service = service;
            this.storage = storage;
            this.bus = busClient;
        }

        [HttpGet] //Authorize
        public async Task<IActionResult> GetByClientId() 
        { // Guid clientId
        //BrowseTickets query
            var result = await storage.BrowseAsync(null);
            //var query = await this.service.GetAsync(clientId);
            return Ok(result);
        }

/*
        [Route("{id}")]
        [HttpGet] // Authorize
        public async Task<IActionResult> GetById(Guid clientId, Guid id)
        {
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }*/

        [Route("{id}")]
        [HttpPut] // Authorize
        public async Task<IActionResult> Put(Guid clientId, Guid id, [FromBody]Address address)
        {
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            throw new NotImplementedException();
            //await service.UpdateAsync(clientId, address);
            //return NoContent();
        }

        [HttpPost] //, Authorize Guid clientId, 
        public async Task<IActionResult> Post([FromBody]AddressCreated created)
        {
            var commandId = Guid.NewGuid();
            
            var command = new CreateAddress 
            {
                Request  = servicedesk.Common.Commands.Request.Create<CreateAddress>(commandId, "servicedesk.Services.Tickets", "ru-ru"),
                Name = created.Name,
                Address = created.Address,
                UserId =  User.Identity.Name ?? "unauthenticated user"
            };

            await bus.PublishAsync(command, commandId, cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.Services.Tickets"))
                .WithRoutingKey("address.create"));

            return await Task.FromResult(Accepted(command));

            //var user = await service.CreateAsync(clientId, created);
            //return Created(user.Id.ToString(), user);
        }

        [Route("{id}")]
        [HttpDelete] // Authorize
        public async Task<IActionResult> Delete(Guid clientId, Guid id)
        {
            var deleted = await this.service.GetByIdAsync(clientId, id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(deleted);

            return NoContent();
        }
    }
}
