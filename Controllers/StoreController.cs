using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace servicedesk.api
{
    [Route("api/v1/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly HelpDeskDbContext context;
        public StoreController(HelpDeskDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = this.context.Locations                
                .Include(s => s.LOCATION_OWNER)
                .Include(s => s.LOCATION_TYPE)
                .Include(s => s.STORE_ATTRS)
                .Include(s => s.SYNC)
                .Include(s => s.CONTACT)
                .Where(s => s.LOCATION_TYPE_GUID == LOCATION_TYPES.Store 
                    || s.LOCATION_TYPE_GUID == LOCATION_TYPES.FuelTerminal
                    || s.LOCATION_TYPE_GUID == LOCATION_TYPES.Land 
                    || s.LOCATION_TYPE_GUID == LOCATION_TYPES.Warehouse 
                    || s.LOCATION_TYPE_GUID == LOCATION_TYPES.Office)
                .AsQueryable();

            return await Task.FromResult(Ok(query));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = this.context.Locations.SingleOrDefault(r => r.GUID_RECORD == id);

            if (record == null)
            {
                return NotFound();
            }

            return await Task.FromResult(Ok(record));
        }
    }
}
