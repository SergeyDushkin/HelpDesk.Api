using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class UserService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public UserService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<User> CreateAsync(UserCreated reg)
        {
            var typeId = await GetTypeIdAsync();

            if (await this.context.Locations.AnyAsync(r => r.LOCATION_TYPE_GUID == typeId && r.LOCATION_NAME == reg.Name))
            {
                throw new Exception(String.Format("Client {0} already exists", reg.Name));
            }

            var client = new LOCATION {
                LOCATION_NAME = reg.Name,
                LOCATION_TYPE_GUID = typeId
            };

            await this.context.Locations.AddAsync(client);
            await this.context.SaveChangesAsync();
            
            this.logger.LogTrace("Register new client. Name : {0}", reg.Name);
            
            return new User {
                Id = client.GUID_RECORD,
                Name = client.LOCATION_NAME
            };
        }

        public async Task<IQueryable<User>> GetAsync()
        {
            this.logger.LogTrace("Get clients");

            var typeId = await GetTypeIdAsync();
            
            return this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId).Select(r => new User {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            });
        }

        public async Task<User> GetByIdAsync(Guid clientId, Guid id)
        {
            this.logger.LogTrace("Get client by Id {0}", id);

            return await this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId).Select(r => new User {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            }).SingleOrDefaultAsync();
        }
    }
}
