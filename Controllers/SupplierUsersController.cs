using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace servicedesk.api
{
    [Route("suppliers/{supplierId}/users")]
    public class SupplierUsersController : ControllerBase
    {
        private readonly UserService service;
        public SupplierUsersController(UserService service)
        {
            this.service = service;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get(Guid supplierId)
        {
            var query = await this.service.GetAsync(supplierId);
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid supplierId, Guid id)
        {
            var record = await this.service.GetByIdAsync(supplierId, id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid supplierId, Guid id, [FromBody]User user)
        {
            var record = await this.service.GetByIdAsync(supplierId, id);

            if (record == null)
            {
                return NotFound();
            }

            await service.UpdateAsync(supplierId, user);

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(Guid supplierId, [FromBody]UserCreated created)
        {
            var user = await service.CreateAsync(supplierId, created);
            return Created(user.Id.ToString(), user);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid supplierId, Guid id)
        {
            var deleted = await this.service.GetByIdAsync(supplierId, id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(deleted);

            return NoContent();
        }
    }
}
