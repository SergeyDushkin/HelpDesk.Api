﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.api.Queries;
using servicedesk.Services.Tickets.Shared.Dto;

namespace servicedesk.api.Storages
{
    public interface ITicketStorage
    {
        Task<Maybe<TicketDto>> GetAsync(Guid id);
        Task<IEnumerable<TicketDto>> BrowseAsync(BrowseTickets query);
    }
}
