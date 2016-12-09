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
            var query = this.service.GetByKnipiId(knipiId).Restrict(r => r.knipi_id, User);
            return await Task.FromResult(Ok(query));
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid clientId, Guid id)
        {
            var record = await this.service.GetByIdAsync(id);
            record = record.Restrict(r => r.knipi_id, User);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}")]
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(Guid clientId, Guid id, [FromBody]directors director)
        {
            var record = await this.service.GetByIdAsync(id);
            record = record.Restrict(r => r.knipi_id, User);

            if (record == null)
            {
                return NotFound();
            }

            director.id = id;
            director.knipi_id = knipiId;

            await service.UpdateAsync(director);

            return Ok();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(Guid clientId, [FromBody]directors director)
        {
            director.knipi_id = knipiId;
            director = director.Restrict(r => r.knipi_id, User);

            if (director == null)
            {
                return NotFound();
            }

            var newDirector = await service.CreateAsync(director);

            return Created(newDirector.id.ToString(), newDirector);
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid knipiId, Guid id)
        {
            var director = await this.service.GetByIdAsync(id);
            director = director.Restrict(r => r.knipi_id, User);

            if (director == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(director);

            return Ok();
        }
    }
}
