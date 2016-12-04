using System;
using System.Collections.Generic;
using System.Linq;
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
                    StoreName = r.STORE.LOCATION_NAME,
                    Comments = r.Comments,
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
                    StoreName = r.STORE.LOCATION_NAME,
                    Comments = r.Comments,
                    RequestDate = r.RequestDate,
                    CompleteDate = r.CompleteDate
                })
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public  async Task CreateAsync(TicketCreated created)
        {
            var ticket = new REQUEST {
                RequestDate = DateTime.Now,
                Comments = created.Description
            };

            await this.context.Requests.AddAsync(ticket);
            await this.context.SaveChangesAsync();
        }
    }
}
