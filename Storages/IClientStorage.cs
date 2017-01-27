using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public interface IClientStorage
    {
        Task<Maybe<ClientDto>> GetAsync(Guid id);
        Task<IEnumerable<ClientDto>> BrowseAsync(BrowseAll query);
    }
}
