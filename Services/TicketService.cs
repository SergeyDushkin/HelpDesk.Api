using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace servicedesk.api
{
    public class TicketService
    {
        private readonly HelpDeskDbContext context;
        public TicketService(HelpDeskDbContext context)
        {
            this.context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<IEnumerable<Ticket>> GetAsync()
        {
            return await this.context.Requests
                .Select(r => new Ticket {
                    Id = r.GUID_RECORD,
                    TicketNumber = r.REQUEST_ID.ToString(),
                    Description = r.Comments,

                    ClientId = r.COMPANY_GUID,
                    AddressId = r.STORE_GUID,
                    UserId = r.USER_GUID,

                    Client = new Client { Id = r.COMPANY_GUID, Name = r.COMPANY.LOCATION_NAME },
                    Address = new Address { Id = r.STORE_GUID, Name = r.STORE.LOCATION_NAME },
                    User = new User { Id = r.USER_GUID, Name = r.USER.FIRST_NAME },

                    RequestDate = r.RequestDate,
                    CompleteDate = r.CompleteDate
                }).ToListAsync();
        } 

        public async Task<Ticket> GetByIdAsync(Guid id)
        {
            return await this.context.Requests
                .Select(r => new Ticket {
                    Id = r.GUID_RECORD,
                    TicketNumber = r.REQUEST_ID.ToString(),
                    Description = r.Comments,

                    ClientId = r.COMPANY_GUID,
                    AddressId = r.STORE_GUID,
                    UserId = r.USER_GUID,

                    Client = new Client { Id = r.COMPANY_GUID, Name = r.COMPANY.LOCATION_NAME },
                    Address = new Address { Id = r.STORE_GUID, Name = r.STORE.LOCATION_NAME },
                    User = new User { Id = r.USER_GUID, Name = r.USER.FIRST_NAME },

                    RequestDate = r.RequestDate,
                    CompleteDate = r.CompleteDate
                })
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(TicketCreated created, IIdentity identity)
        {
            var ticket = new REQUEST {
                RequestDate = DateTime.Now,
                COMPANY_GUID = created.ClientId,
                STORE_GUID = created.AddressId,
                USER_GUID = created.UserId,
                Comments = created.Description,
                USERCREATE = identity.Name
            };

            await this.context.Requests.AddAsync(ticket);
            await this.context.SaveChangesAsync();
        }
    }
}
