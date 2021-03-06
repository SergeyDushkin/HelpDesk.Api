using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class SupplierService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        private const string LocationTypeName = "Supplier";
        public SupplierService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        
        public async Task<Supplier> CreateAsync(SupplierCreated created)
        {
            var typeId = await GetTypeIdAsync();

            if (await this.context.Locations.AnyAsync(r => r.LOCATION_TYPE_GUID == typeId && r.LOCATION_NAME == created.Name))
            {
                throw new Exception(String.Format("Supplier {0} already exists", created.Name));
            }

            var supplier = new LOCATION {
                LOCATION_NAME = created.Name,
                LOCATION_TYPE_GUID = typeId
            };

            await this.context.Locations.AddAsync(supplier);
            await this.context.SaveChangesAsync();

            this.logger.LogTrace("Create supplier. Name : {0}", created.Name);

            return await GetByIdAsync(supplier.GUID_RECORD);
        }

        public async Task<IQueryable<Supplier>> GetAsync()
        {
            this.logger.LogTrace("Get suppliers");

            var typeId = await GetTypeIdAsync();
            
            return this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId).Select(r => new Supplier {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            });
        }

        public async Task<Supplier> GetByIdAsync(Guid id)
        {
            this.logger.LogTrace("Get supplier by Id {0}", id);

            var typeId = await GetTypeIdAsync();
            
            return await this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId && r.GUID_RECORD == id).Select(r => new Supplier {
                Id = r.GUID_RECORD,
                Name = r.LOCATION_NAME
            }).SingleOrDefaultAsync();
        }
        public async Task UpdateNameAsync(Guid id, string name)
        {
            var typeId = await GetTypeIdAsync();
            var updated = await this.context.Locations.Where(r => r.LOCATION_TYPE_GUID == typeId && r.GUID_RECORD == id).SingleOrDefaultAsync();
            
            updated.LOCATION_NAME = name;

            this.context.Update(updated);
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleted = new LOCATION 
            {
                GUID_RECORD = id
            };

            this.context.Remove(deleted);
            await this.context.SaveChangesAsync();
        }

        private async Task<Guid> GetTypeIdAsync()
        {
            var locationType = await this.context.LocationTypes.SingleOrDefaultAsync(r => r.LOCATION_TYPE_NAME == LocationTypeName);

            if (locationType == null) 
            {
                locationType = new LOCATION_TYPE { 
                    LOCATION_TYPE_NAME = LocationTypeName
                };

                await this.context.LocationTypes.AddAsync(locationType);
                await this.context.SaveChangesAsync();
            }
            
            return locationType.GUID_RECORD;
        }
    }
}
