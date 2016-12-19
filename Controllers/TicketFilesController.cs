using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace servicedesk.api
{
    [Route("tickets/{ticketId}/files")]
    public class TicketFilesController : ControllerBase
    {
        private readonly ContentService service;
        public TicketFilesController(ContentService service)
        {
            this.service = service;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get(Guid ticketId)
        {
            var query = await this.service.GetAsync(ticketId);
            return Ok(query);
        }

        [Route("{id}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetById(Guid ticketId, Guid id)
        {
            var record = await this.service.GetByIdAsync(ticketId, id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [Route("{id}/download")]
        [HttpGet, Authorize]
        public async Task<IActionResult> Download(Guid ticketId, Guid id)
        {
            var record = await this.service.GetByIdWithContentAsync(ticketId, id);

            if (record == null)
            {
                return NotFound();
            }

            return File(record.Content, record.ContentType, record.Name);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(Guid ticketId, ICollection<IFormFile> files)
        {
            foreach(var f in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await f.CopyToAsync(memoryStream);

                    var file = new ContentDbContext.File 
                    {
                        ReferenceId = ticketId,
                        Name  = f.FileName,
                        FileType  = f.ContentType,
                        ContentType = f.ContentType,
                        Size = f.Length,
                        Content = memoryStream.ToArray()
                    };

                    await service.CreateAsync(ticketId, file);
                }
            }
            
            return Accepted();
        }

        [Route("{id}")]
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(Guid ticketId, Guid id)
        {
            var deleted = await this.service.GetByIdAsync(ticketId, id);

            if (deleted == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(ticketId, id);

            return NoContent();
        }
    }
}
