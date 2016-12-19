using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class ContentService
    {
        protected ILogger logger { get; }
        private readonly ContentDbContext context;
        public ContentService(ContentDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<IQueryable<ContentDbContext.File>> GetAsync(Guid referenceId)
        {
            await Task.FromResult(0);

            this.logger.LogInformation("Get files");

            return this.context.Files
                .Where(r => r.ReferenceId == referenceId)
                .Select(r => new ContentDbContext.File {
                    Id = r.Id,
                    ReferenceId = r.ReferenceId,
                    Name = r.Name,
                    FileType = r.FileType,
                    ContentType = r.ContentType,
                    Size = r.Size
                });
        }

        public async Task<ContentDbContext.File> GetByIdAsync(Guid referenceId, Guid id)
        {
            this.logger.LogInformation("Get file by Id {0}", id);

            return await this.context.Files
                .Where(r => r.ReferenceId == referenceId)
                .Where(r => r.Id == id)
                .Select(r => new ContentDbContext.File {
                    Id = r.Id,
                    ReferenceId = r.ReferenceId,
                    Name = r.Name,
                    FileType = r.FileType,
                    ContentType = r.ContentType,
                    Size = r.Size
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ContentDbContext.File> GetByIdWithContentAsync(Guid referenceId, Guid id)
        {
            this.logger.LogInformation("Get file by Id {0}", id);

            return await this.context.Files
                .Where(r => r.ReferenceId == referenceId)
                .Where(r => r.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<ContentDbContext.File> CreateAsync(Guid referenceId, ContentDbContext.File created)
        {
            await this.context.AddAsync(created);
            await this.context.SaveChangesAsync();
            
            this.logger.LogInformation("Create new file. Name : {0}", created.Name);
            
            return created;
        }

        public async Task DeleteAsync(Guid referenceId, Guid id)
        {
            var deleted = new ContentDbContext.File
            {
                Id = id
            };

            this.context.Remove(deleted);
            await this.context.SaveChangesAsync();
        }
    }
}
