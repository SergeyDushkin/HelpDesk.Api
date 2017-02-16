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

        private ConsulClient _consuleclient;
        private SettingsConfiguration configuration;

        public SettingsService(IOptions<ConsulClient> consuleclient, ILoggerFactory loggerFactory)
        {
           _logger = loggerFactory.CreateLogger(GetType().Namespace);

            this._consuleclient = consuleclient;

        }
   

        // ћетод извлекает значение их хранилища по коду
        public async Task<T> GetByCodeAsync<T>(string code)
        {

            _logger.LogInformation("Get setting by code {0}", code);
                       
            using (var client = new ConsulClient(x => { x.Address = new Uri(configuration.Url); } ))
            {

                var getPair = await client.KV.Get(code);

                if (getPair.Response != null)
                {
                    var json = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default(T);
            }

        }

        // ћетод устанавливает значение в хранилище по коду
        public async Task SetByCodeAsync<T>(string code, T value)
        {
            _logger.LogInformation("Set setting by code {0}", code);

         

                var putPair = new KVPair(code)
                {
                    Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))
                };

                var putAttempt = await client.KV.Put(putPair);

                if (!putAttempt.Response)
                    throw new Exception("Response result error");
       

}

       
    }

  
}
