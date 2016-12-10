using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class AddressService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public AddressService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<Address> CreateAsync(Guid clientId, AddressCreated created)
        {
            var address = new LOCATION_CONTACT_INFO {
                REFERENCE_GUID = clientId,
                ADDRESS = created.Address
            };

            await this.context.AddAsync(address);
            await this.context.SaveChangesAsync();
            
            this.logger.LogInformation("Register new address. Name : {0}", created.Address);
            
            return new Address {
                Id = address.GUID_RECORD,
                Name = address.ADDRESS
            };
        }

        public async Task<IQueryable<Address>> GetAsync(Guid clientId)
        {
            await Task.FromResult(0);

            this.logger.LogInformation("Get address");

            return this.context.LocationContacts
                .Where(r => r.REFERENCE_GUID == clientId)
                .Select(r => new Address {
                    Id = r.GUID_RECORD,
                    Name = r.ADDRESS
                });
        }

        public async Task<Address> GetByIdAsync(Guid clientId, Guid id)
        {
            this.logger.LogInformation("Get address by Id {0}", id);

            return await this.context.LocationContacts
                .Where(r => r.REFERENCE_GUID == clientId)
                .Select(r => new Address {
                    Id = r.GUID_RECORD,
                    Name = r.ADDRESS
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Address> DeleteAsync(Guid clientId, Guid id)
        {
            this.logger.LogInformation("Get address by Id {0}", id);

            return await this.context.LocationContacts
                .Where(r => r.REFERENCE_GUID == clientId)
                .Select(r => new Address {
                    Id = r.GUID_RECORD,
                    Name = r.ADDRESS
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Address address)
        {
            var updated = new LOCATION_CONTACT_INFO 
            {
                GUID_RECORD = address.Id,
                ADDRESS = address.Name
            };

            this.context.Update(updated);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Address address)
        {
            var deleted = new LOCATION_CONTACT_INFO 
            {
                GUID_RECORD = address.Id
            };

            this.context.Remove(deleted);
            await this.context.SaveChangesAsync();
        }
    }
}
