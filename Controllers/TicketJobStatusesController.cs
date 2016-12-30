using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Attributes;

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
            command.UserId = "sadushkin";
            command.SourceId = Guid.Empty;
            command.ReferenceId = jobId;

            await _busClient.PublishAsync(command, Guid.NewGuid());
                //(configuration) => configuration.WithExchange((exchange) => exchange.WithName("servicedesk.statusmanagementsystem.commands")));
            
            return await Task.FromResult(Accepted(command));
        }
    }
    
    [Exchange(Type = RawRabbit.Configuration.Exchange.ExchangeType.Topic, Name = "servicedesk.statusmanagementsystem.commands")]
	[Queue(Name = "servicedesk.StatusManagementSystem/SetStatus_helpdesk_statusmanagementsystem", Durable = false)]
	[Routing(RoutingKey = "setstatus.job")]
    public class SetStatus
    {
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid StatusId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
