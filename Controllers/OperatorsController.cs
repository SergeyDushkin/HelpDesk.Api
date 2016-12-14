using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace servicedesk.api
{
    [Route("[controller]")]
    public class OperatorsController : ControllerBase
    {
        private readonly UserService service;
        public OperatorsController(UserService service)
        {
            this.service = service;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            var clientId = Guid.Empty;
            var query = await this.service.GetAsync(clientId);
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var clientId = Guid.Empty;
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid id, [FromBody]User user)
        {
            var clientId = Guid.Empty;
            var record = await this.service.GetByIdAsync(clientId, id);

            if (record == null)
            {
                return NotFound();
            }

            await service.UpdateAsync(clientId, user, User.Identity);

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody]UserCreated created)
        {
            var clientId = Guid.Empty;
            var user = await service.CreateAsync(clientId, created, User.Identity);
            return Created(user.Id.ToString(), user);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clientId = Guid.Empty;
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
