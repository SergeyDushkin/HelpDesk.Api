using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using servicedesk.Common.Commands;

namespace servicedesk.api
{
    [Route("tickets/{ticketId}/jobs/{jobId}/status")]
    public class TicketJobStatusesController : ControllerBase
    {
        private readonly IStatusManagerClient _client;
        private readonly IBusClient _busClient;
		private readonly ILogger<TicketJobStatusesController> _logger;
        public TicketJobStatusesController(IStatusManagerClient client, IBusClient busClient, ILoggerFactory loggerFactory)
        {
            _client = client;
            _busClient = busClient;
            _logger = loggerFactory.CreateLogger<TicketJobStatusesController>();
        }

        [HttpGet] //Authorize
        public async Task<IActionResult> Get(Guid ticketId, Guid jobId)
        {
            var result = await _client.GetCurrentStatusAsync("job", jobId);

            return Ok(result);
        }

        [HttpGet, Route("next")] //Authorize
        public async Task<IActionResult> GetNext(Guid ticketId, Guid jobId)
        {
            var result = await _client.GetNextStatusAsync("job", jobId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid ticketId, Guid jobId, [FromBody]Status body)
        {
            var command = new SetStatus();
            var commandId = Guid.NewGuid();

            command.Request = servicedesk.Common.Commands.Request.Create<SetStatus>(commandId, "servicedesk.api", "ru-ru");            
            command.ReferenceId = jobId;
            command.SourceId = new Guid("b29bdb1f-485d-4721-b905-e8b1f918739a");
            command.Message = "Manual change status";
            command.StatusId = body.StatusId;
            command.UserId = User.Identity.Name ?? "unauthenticated user";

            await _busClient.PublishAsync(command, commandId, cfg => cfg
                .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("servicedesk.statusmanagementsystem.commands"))
                .WithRoutingKey("setstatus.job"));

            return await Task.FromResult(Accepted(command));
        }

        public class Status
        {
            public Guid StatusId { get; set; }
        }
    }
}
