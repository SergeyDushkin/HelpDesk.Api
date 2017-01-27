using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly ILogger logger;
        private readonly IStorageClient client;

        public ClientStorage(IStorageClient storageClient, ILogger<ClientStorage> logger)
        {
            this.logger = logger;
            this.client = storageClient;

            this.logger.LogDebug("Init ClientsStorage");
        }

        public async Task<ClientDto> GetAsync(Guid id)
            => await client.GetAsync<ClientDto>($"clients/{id}");

        public async Task<IEnumerable<ClientDto>> BrowseAsync(BrowseAll query)
            => await client.GetFilteredCollectionAsync<ClientDto, BrowseAll>(query, "clients");
    }
}
