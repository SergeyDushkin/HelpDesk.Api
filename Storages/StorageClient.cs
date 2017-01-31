using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using servicedesk.Common.Extensions;
using servicedesk.Common.Security;

namespace servicedesk.api.Storages
{
    public class StorageClient<TType> : IStorageClient
    {
        private readonly ILogger logger;
        private readonly ServiceSettings settings;
        private readonly HttpClient httpClient;
        private string BaseAddress
            => settings.Url.EndsWith("/", StringComparison.CurrentCulture) ? settings.Url : $"{settings.Url}/";

        public StorageClient(ApplicationServiceSettings<TType> configuration, ILogger<StorageClient<TType>> logger)
        {
            this.logger = logger;
            this.settings = configuration;
            this.httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };
            this.httpClient.DefaultRequestHeaders.Remove("Accept");
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        
        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            logger.LogDebug($"Get data from storage, endpoint: {endpoint}");
            var response = await GetResponseAsync(endpoint);

            if (response == null)
                return default(T);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint) where T : class
        {
            logger.LogDebug($"Get data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}");
            var response = await GetResponseAsync(endpoint);

            if (response == null)
                return default(IEnumerable<T>);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(content);

            return data;
        }

        public async Task<Stream> GetStreamAsync(string endpoint)
        {
            logger.LogDebug($"Get stream from endpoint: {endpoint}");
            var response = await GetResponseAsync(endpoint);

            if (response == null)
                return default(Stream);

            return await response.Content.ReadAsStreamAsync();
        }

        async Task<IEnumerable<TResult>> IStorageClient.GetFilteredCollectionAsync<TResult, TQuery>(TQuery query, string endpoint)
        {
            logger.LogDebug($"Get filtered data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            var results = await GetCollectionAsync<TResult>(queryString);
            
            return results;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(string endpoint)
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException("Endpoint can not be empty.");
            }

            var retryNumber = 0;
            while (retryNumber < settings.RetryCount)
            {
                try
                {
                    logger.LogDebug($"Fetch data from http endpoint: {String.Concat(BaseAddress, endpoint)}");
                    var response = await httpClient.GetAsync(endpoint);
                    if (response.StatusCode != HttpStatusCode.NotFound)
                        return response;

                    await Task.Delay(settings.RetryDelayMilliseconds);
                    retryNumber++;
                }
                catch (Exception ex)
                {
                    logger.LogError(new EventId(10010, "Try GetResponseAsync in StorageClient"), ex, $"Exception occured while fetching data from endpoint: {String.Concat(BaseAddress, endpoint)}");
                }
            }

            return null;
        }
    } 
}