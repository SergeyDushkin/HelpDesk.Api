using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RawRabbit;
using servicedesk.Common.Commands;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.api
{
    [Route("tickets/{ticketId}/jobs/{jobId}/status")]
    public class TicketJobStatusesController : ControllerBase
    {
        private readonly IBusClient _busClient;
		private readonly ILogger<TicketJobStatusesController> _logger;
        public TicketJobStatusesController(IBusClient busClient, ILoggerFactory loggerFactory)
        {
            _busClient = busClient;
            _logger = loggerFactory.CreateLogger<TicketJobStatusesController>();
        }

        /*
        [HttpGet, Authorize]
        public async Task<IActionResult> Get(Guid ticketId)
        {
            var query = await this.service.GetAsync(ticketId);
            return Ok(query);
        }
        */

        [HttpPost]
        public async Task<IActionResult> Post(Guid ticketId, Guid jobId, [FromBody]SetStatus command)
        {
            command.Request = Coolector.Common.Commands.Request.New<SetStatus>();           
            command.ReferenceId = jobId;

            await _busClient.PublishAsync(command, Guid.NewGuid(), cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.statusmanagementsystem.commands"))
                .WithRoutingKey("setstatus.job"));

            return await Task.FromResult(Accepted(command));
        }
    }
}
