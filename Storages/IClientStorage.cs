using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.Common.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public interface IClientStorage
    {
        Task<ClientDto> GetAsync(Guid id);
        Task<IEnumerable<ClientDto>> BrowseAsync(GetByReferenceId query);
    }
}
