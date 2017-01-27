using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public class AddressStorage : IAddressStorage
    {
        private readonly ILogger logger;
        private readonly IStorageClient client;

        public AddressStorage(IStorageClient storageClient, ILogger<AddressStorage> logger)
        {
            this.logger = logger;
            this.client = storageClient;

            this.logger.LogDebug("Init AddressStorage");
        }

        public async Task<AddressDto> GetAsync(Guid id)
            => await client.GetAsync<AddressDto>($"addresses/{id}");

        public async Task<IEnumerable<AddressDto>> BrowseAsync(BrowseAll query)
            => await client.GetFilteredCollectionAsync<AddressDto, BrowseAll>(query, "addresses");
    }
}
