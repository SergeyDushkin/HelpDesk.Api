using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace servicedesk.api
{
    [Route("clients/{clientId}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService service;
        public UsersController(UserService service)
        {
            this.service = service;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            var query = await this.service.GetAsync(clientId);
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid clientId, Guid id)
        {
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid clientId, Guid id, [FromBody]User user)
        {
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            await service.UpdateAsync(clientId, user, User.Identity);

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(Guid clientId, [FromBody]UserCreated created)
        {
            var user = await service.CreateAsync(clientId, created, User.Identity);
            return Created(user.Id.ToString(), user);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid clientId, Guid id)
        {
            var deleted = await this.service.GetByIdAsync(clientId, id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(deleted);

            return NoContent();
        }
    }
}
