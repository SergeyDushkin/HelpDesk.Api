using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicedesk.api
{
    public class UserService
    {
        protected ILogger logger { get; }
        private readonly HelpDeskDbContext context;
        public UserService(HelpDeskDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task<User> CreateAsync(Guid clientId, UserCreated created)
        {
            var user = new USER {
                LOCATION_GUID = clientId,
                FIRST_NAME = created.Name
            };

            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();
            
            this.logger.LogInformation("Register new user. Name : {0}", created.Name);
            
            return new User {
                Id = user.GUID_RECORD,
                Name = user.FIRST_NAME
            };
        }

        public async Task<IQueryable<User>> GetAsync(Guid clientId)
        {
            await Task.FromResult(0);

            this.logger.LogInformation("Get users");

            return this.context.Users
                .Where(r => r.LOCATION_GUID == clientId)
                .Select(r => new User {
                    Id = r.GUID_RECORD,
                    Name = r.FIRST_NAME
                });
        }

        public async Task<User> GetByIdAsync(Guid clientId, Guid id)
        {
            this.logger.LogInformation("Get user by Id {0}", id);

            return await this.context.Users
                .Where(r => r.LOCATION_GUID == clientId)
                .Where(r => r.GUID_RECORD == id)
                .Select(r => new User {
                    Id = r.GUID_RECORD,
                    Name = r.FIRST_NAME
                })
                .SingleOrDefaultAsync();
        }

        public async Task<User> DeleteAsync(Guid clientId, Guid id)
        {
            this.logger.LogInformation("Get user by Id {0}", id);

            return await this.context.Users
                .Where(r => r.LOCATION_GUID == clientId)
                .Select(r => new User {
                    Id = r.GUID_RECORD,
                    Name = r.FIRST_NAME
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var updated = new USER 
            {
                GUID_RECORD = user.Id,
                FIRST_NAME = user.Name
            };

            this.context.Users.Update(updated);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            var deleted = new USER 
            {
                GUID_RECORD = user.Id
            };

            this.context.Users.Remove(deleted);
            await this.context.SaveChangesAsync();
        }
    }
}
