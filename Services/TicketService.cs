using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class TicketService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public TicketService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
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
                    Address = new Address { 
                        Id = r.STORE_GUID, 
                        Name = r.STORE.LOCATION_NAME, 
                        Contact = new Contact { Address = r.STORE.CONTACT.ADDRESS } 
                    },
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
                    Address = new Address { 
                        Id = r.STORE_GUID, 
                        Name = r.STORE.LOCATION_NAME, 
                        Contact = new Contact { Address = r.STORE.CONTACT.ADDRESS } 
                    },
                    User = new User { Id = r.USER_GUID, Name = r.USER.FIRST_NAME },

                    RequestDate = r.RequestDate,
                    CompleteDate = r.CompleteDate
                })
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(TicketCreated created, ClaimsIdentity identity)
        {
            this.logger.LogTrace("Create new ticket by {0}", identity.FindFirst("name").Value);

            var ticket = new REQUEST {
                RequestDate = DateTime.Now,
                COMPANY_GUID = created.ClientId,
                STORE_GUID = created.AddressId,
                USER_GUID = created.UserId,
                Comments = created.Description,
                USERCREATE = identity.FindFirst("name").Value,
                USERUPDATE = identity.FindFirst("name").Value
            };

            await this.context.Requests.AddAsync(ticket);
            await this.context.SaveChangesAsync();
        }
    }
}
