using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Microsoft.Extensions.Logging;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public class TicketStorage : ITicketStorage
    {
        private readonly ILogger logger;
        private readonly IStorageClient client;

        public TicketStorage(IStorageClient storageClient, ILogger<TicketStorage> logger)
        {
            this.logger = logger;
            this.client = storageClient;

            this.logger.LogDebug("Init TicketStorage");
        }

        public async Task<Maybe<TicketDto>> GetAsync(Guid id)
            => await client.GetAsync<TicketDto>($"tickets/{id}");

        public async Task<IEnumerable<TicketDto>> BrowseAsync(BrowseTickets query)
            => await client.GetFilteredCollectionAsync<TicketDto, BrowseTickets>(query, "tickets");
    }
}
