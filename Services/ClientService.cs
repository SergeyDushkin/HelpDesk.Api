using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class ClientService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public ClientService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<Client> RegisterAsync(ClientRegistered reg)
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
            
            return new Client {
                Id = client.GUID_RECORD,
                Name = client.LOCATION_NAME
            };
        }

        public async Task<IQueryable<Client>> GetAsync()
        {
            this.logger.LogTrace("Get clients");

            var typeId = await GetTypeIdAsync();
            
            return this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId).Select(r => new Client {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            });
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            this.logger.LogTrace("Get client by Id {0}", id);

            var typeId = await GetTypeIdAsync();
            
            return await this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId).Select(r => new Client {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            }).SingleOrDefaultAsync();
        }

        private async Task<Guid> GetTypeIdAsync()
        {
            var locationType = await this.context.LocationTypes.SingleOrDefaultAsync(r => r.LOCATION_TYPE_NAME == "Client");

            if (locationType == null) 
            {
                locationType = new LOCATION_TYPE { 
                    LOCATION_TYPE_NAME = "Client"
                };
                await this.context.LocationTypes.AddAsync(locationType);
                await this.context.SaveChangesAsync();
            }
            
            return locationType.GUID_RECORD;
        }
    }

    public class ClientRegistered 
    {
        public string Name { get; set; }
    }
}
