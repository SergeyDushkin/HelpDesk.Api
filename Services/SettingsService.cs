using System;
//using System.Linq;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Linq;
using Consul;
using System.Text;

namespace servicedesk.api
{
    public class SettingsService
    {
        protected ILogger _logger { get; }
        private readonly HttpClient _client;
        private ConsulClientConfiguration _consuleconfiguration;

        public SettingsService(IOptions<SettingsConfiguration> configuration, ILoggerFactory loggerFactory)
        {
           _logger = loggerFactory.CreateLogger(GetType().Namespace);
           
           /* 
           _client = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });

            _client.BaseAddress = new Uri(configuration.Value.Url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            */

            _consuleconfiguration = new ConsulClientConfiguration {
                Address = new Uri(configuration.Value.Url)
        };
    }

        /*
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
        */
        public async Task<string> GetByCodeAsync(string code)
        {

            _logger.LogInformation("Get setting by code {0}", code);

            /*
         var response = await _client.GetAsync("/v1/kv/"+code);

        _logger.LogInformation($"/v1/kv/{0}", code);

         if (!response.IsSuccessStatusCode)
             throw new Exception(await response.Content.ReadAsStringAsync());

         var json = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<Setting[]>(json).SingleOrDefault();

         return result;

        


            using (var client = new ConsulClient(_consuleconfiguration))
            {

                var getPair = await client.KV.Get(code);

                return new Setting
                {
                    Key = code,
                    Value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length)
                };
            }

             */
            using (var client = new ConsulClient(_consuleconfiguration))
            {

                var getPair = await client.KV.Get(code);

                return Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
            }

        }

        /*
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
                  */
    }

  
}
