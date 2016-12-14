using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class JobService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public JobService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<IQueryable<Job>> GetAsync(Guid ticketId)
        {
            await Task.FromResult(0);

            this.logger.LogInformation("Get jobs");

            return this.context.RequestJobs
                .Where(r => r.REQUEST_GUID == ticketId)
                .Select(r => new Job {
                    Id = r.GUID_RECORD,
                    JobNumber = r.JOB_NUMBER,
                    SupplierId = r.SUPPLIER_GUID.Value,
                    UserId = r.USER_GUID,
                    Description = r.COMMENT,
                    StartDate = r.STARTDATE,
                    CompleteDate = r.ENDDATE,
                    Client = new Client { Id = r.SUPPLIER_GUID.Value, Name = r.SUPPLIER.LOCATION_NAME },
                    User = r.USER != null ? new User { Id = r.USER_GUID.Value, Name = r.USER.FIRST_NAME } : default(User)
                });
        }

        public async Task<Job> GetByIdAsync(Guid id)
        {
            this.logger.LogInformation("Get job by Id {0}", id);

            return await this.context.RequestJobs
                .Where(r => r.GUID_RECORD == id)
                .Select(r => new Job {
                    Id = r.GUID_RECORD,
                    JobNumber = r.JOB_NUMBER,
                    SupplierId = r.SUPPLIER_GUID.Value,
                    UserId = r.USER_GUID,
                    Description = r.COMMENT,
                    StartDate = r.STARTDATE,
                    CompleteDate = r.ENDDATE,
                    Client = new Client { Id = r.SUPPLIER_GUID.Value, Name = r.SUPPLIER.LOCATION_NAME },
                    User = r.USER != null ? new User { Id = r.USER_GUID.Value, Name = r.USER.FIRST_NAME } : default(User)
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Job> CreateAsync(Guid ticketId, JobCreated created)
        {
            var job = new REQUEST_JOB {
                REQUEST_GUID = ticketId,
                SUPPLIER_GUID = created.SupplierId,
                USER_GUID = created.UserId,
                COMMENT = created.Description,
                STARTDATE = DateTime.Now
            };

            await this.context.AddAsync(job);
            await this.context.SaveChangesAsync();
            
            this.logger.LogInformation("Create new job");
            
            return await GetByIdAsync(job.GUID_RECORD);
        }

        public async Task DeleteAsync(Guid id)
        {
            var deleted = new REQUEST_JOB 
            {
                GUID_RECORD = id
            };

            this.context.Remove(deleted);
            await this.context.SaveChangesAsync();
        }
    }
}
