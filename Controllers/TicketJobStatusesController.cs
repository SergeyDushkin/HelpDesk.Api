using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RawRabbit;
using servicedesk.Common.Commands;
using RawRabbit.Configuration.Exchange;
using Microsoft.Extensions.Options;

namespace servicedesk.api
{
    [Route("tickets/{ticketId}/jobs/{jobId}/status")]
    public class TicketJobStatusesController : ControllerBase
    {
        private readonly StatusManagerConfiguration _configuration;
        private readonly IBusClient _busClient;
		private readonly ILogger<TicketJobStatusesController> _logger;
        public TicketJobStatusesController(IOptions<StatusManagerConfiguration> configuration, IBusClient busClient, ILoggerFactory loggerFactory)
        {
            _configuration = configuration.Value;
            _busClient = busClient;
            _logger = loggerFactory.CreateLogger<TicketJobStatusesController>();
        }

        [HttpGet] //Authorize
        public async Task<IActionResult> Get(Guid ticketId, Guid jobId)
        {
            var client = new StatusManagerClient(_configuration.Url);
            var result = await client.GetCurrentStatusAsync("job", jobId);

            return Ok(result);
        }

        [HttpGet, Route("next")] //Authorize
        public async Task<IActionResult> GetNext(Guid ticketId, Guid jobId)
        {
            var client = new StatusManagerClient(_configuration.Url);
            var result = await client.GetNextStatusAsync("job", jobId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid ticketId, Guid jobId, [FromBody]Status body)
        {
            var command = new SetStatus();

            command.Request = Coolector.Common.Commands.Request.New<SetStatus>();           
            command.ReferenceId = jobId;
            command.SourceId = new Guid("b29bdb1f-485d-4721-b905-e8b1f918739a");
            command.Message = "Manual change status";
            command.StatusId = body.StatusId;
            command.UserId = User.Identity.Name ?? "unauthenticated user";

            await _busClient.PublishAsync(command, Guid.NewGuid(), cfg => cfg
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
