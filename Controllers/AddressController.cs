using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using servicedesk.api.Storages;
using servicedesk.Services.Tickets.Shared.Commands;
using RawRabbit.Extensions.Client;
using RawRabbit.Configuration.Exchange;
using servicedesk.Common.Queries;

namespace servicedesk.api
{
    [Route("clients/{referenceId}/[controller]"), Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressStorage storage;
        private readonly IBusClient bus;
        public AddressController(IAddressStorage storage, IBusClient busClient)
        {
            this.storage = storage;
            this.bus = busClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetByClientId(Guid referenceId) 
        { 
            var result = await storage.BrowseAsync(new GetByReferenceId { ReferenceId = referenceId });
            return Ok(result);
        }


        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid referenceId, Guid id)
        {
            var record = await storage.GetAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Put(Guid referenceId, Guid id, [FromBody]Address address)
        {
            var record = await storage.GetAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            throw new NotImplementedException();
            //await service.UpdateAsync(clientId, address);
            //return NoContent();
        }

        [HttpPost]
        public Task<IActionResult> Post(Guid referenceId, [FromBody]AddressCreated created)
        {
            throw new NotImplementedException();
            
            //var commandId = Guid.NewGuid();
            
            //var command = new CreateAddress 
            //{
            //    Request  = servicedesk.Common.Commands.Request.Create<CreateAddress>(commandId, "servicedesk.Services.Tickets", "ru-ru"),
            //    ReferenceId = referenceId,
            //    Name = created.Name,
            //    Address = created.Address,
            //    UserId =  User.Identity.Name ?? "unauthenticated user"
            //};

            //await bus.PublishAsync(command, commandId, cfg => cfg
            //    .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.Services.Tickets"))
            //    .WithRoutingKey("address.create"));

            //return await Task.FromResult(Accepted(command));
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid referenceId, Guid id)
        {
            var record = await storage.GetAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            throw new NotImplementedException();

            //await service.DeleteAsync(deleted);
            //return NoContent();
        }
    }
}
