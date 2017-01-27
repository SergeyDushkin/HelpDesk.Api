using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public interface IAddressStorage
    {
        Task<AddressDto> GetAsync(Guid id);
        Task<IEnumerable<AddressDto>> BrowseAsync(BrowseAll query);
    }
}
