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
            var typeId = await GetTypeIdAsync();

            if (await this.context.Locations.AnyAsync(r => r.LOCATION_TYPE_GUID == typeId && r.LOCATION_OWNER_GUID == clientId && r.LOCATION_NAME == created.Name))
            {
                throw new Exception(String.Format("Address {0} already exists", created.Name));
            }

            var transaction = this.context.Database.BeginTransaction();

            try
            {         
                var address = new LOCATION {
                    LOCATION_NAME = created.Name,
                    LOCATION_TYPE_GUID = typeId,
                    LOCATION_OWNER_GUID = clientId,
                    CONTACT = new LOCATION_CONTACT_INFO {
                        ADDRESS = created.Address
                    }
                };

                await this.context.AddAsync(address);
                await this.context.SaveChangesAsync();

                var contact = new LOCATION_CONTACT_INFO {
                    REFERENCE_GUID = address.GUID_RECORD,
                    ADDRESS = created.Address, 
                };

                await this.context.AddAsync(contact);
                await this.context.SaveChangesAsync();

                transaction.Commit();

                this.logger.LogInformation("Register new address. Name : {0}", created.Name);
                
                return new Address {
                    Id = address.GUID_RECORD,
                    Name = address.LOCATION_NAME,
                    Contact = new Contact {
                        Address = address.CONTACT.ADDRESS
                    }
                };

            } catch(Exception ex) 
            {
                transaction.Rollback();
                throw new Exception("DbException", ex);
            }
        }

        public async Task<IQueryable<Address>> GetAsync(Guid clientId)
        {
            var typeId = await GetTypeIdAsync();

            this.logger.LogInformation("Get address");

            return this.context.Locations
                .Where(r => r.LOCATION_TYPE_GUID == typeId)
                .Where(r => r.LOCATION_OWNER_GUID == clientId)
                .Select(r => new Address {
                    Id = r.GUID_RECORD,
                    Name = r.LOCATION_NAME,
                    Contact = new Contact {
                        Address = r.CONTACT.ADDRESS
                    }
                });
        }

        public async Task<Address> GetByIdAsync(Guid clientId, Guid id)
        {
            var typeId = await GetTypeIdAsync();

            this.logger.LogInformation("Get address by Id {0}", id);

            return await this.context.Locations
                .Where(r => r.LOCATION_TYPE_GUID == typeId)
                .Where(r => r.LOCATION_OWNER_GUID == clientId)
                .Where(r => r.GUID_RECORD == id)
                .Select(r => new Address {
                    Id = r.GUID_RECORD,
                    Name = r.LOCATION_NAME,
                    Contact = new Contact {
                        Address = r.CONTACT.ADDRESS
                    }
                })
                .SingleOrDefaultAsync();
        }

/*
        public async Task UpdateAsync(Guid clientId, Address address)
        {
            var updated = new LOCATION_CONTACT_INFO 
            {
                GUID_RECORD = address.Id,
                REFERENCE_GUID = clientId,
                ADDRESS = address.Name
            };

            this.context.Update(updated);
            await this.context.SaveChangesAsync();
        }
*/
        public async Task DeleteAsync(Address address)
        {
            var deleted = new LOCATION 
            {
                GUID_RECORD = address.Id
            };

            this.context.Remove(deleted);
            await this.context.SaveChangesAsync();
        }

        private async Task<Guid> GetTypeIdAsync()
        {
            var locationType = await this.context.LocationTypes.SingleOrDefaultAsync(r => r.LOCATION_TYPE_NAME == "Address");

            if (locationType == null) 
            {
                locationType = new LOCATION_TYPE { 
                    LOCATION_TYPE_NAME = "Address"
                };
                await this.context.LocationTypes.AddAsync(locationType);
                await this.context.SaveChangesAsync();
            }
            
            return locationType.GUID_RECORD;
        }
    }
}
