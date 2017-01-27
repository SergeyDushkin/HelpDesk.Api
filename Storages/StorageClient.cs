using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        
        public async Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class
        {
            logger.LogDebug($"Get data from storage, endpoint: {endpoint}");
            var response = await GetResponseAsync(endpoint);
            if(response.HasNoValue)
                return new Maybe<T>();

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        /*
        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string endpoint) where T : class
        {
            logger.LogDebug($"Get data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}");
            var response = await GetResponseAsync(endpoint);
            if (response.HasNoValue)
                return new Maybe<PagedResult<T>>();

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(content);

            return data.ToPagedResult(response.Value.Headers);
        }*/

        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint) where T : class
        {
            logger.LogDebug($"Get data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}");
            var response = await GetResponseAsync(endpoint);
            if (response.HasNoValue)
                return default(IEnumerable<T>);

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(content);

            return data;
        }

        public async Task<Maybe<Stream>> GetStreamAsync(string endpoint)
        {
            logger.LogDebug($"Get stream from endpoint: {endpoint}");
            var response = await GetResponseAsync(endpoint);
            if (response.HasNoValue)
                return new Maybe<Stream>();

            return await response.Value.Content.ReadAsStreamAsync();
        }

        /*
        async Task<Maybe<PagedResult<TResult>>> IStorageClient.GetFilteredCollectionAsync<TResult, TQuery>(TQuery query, string endpoint)
        {
            logger.LogDebug($"Get filtered data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            var results = await GetCollectionAsync<TResult>(queryString);
            if (results.HasNoValue || results.Value.IsEmpty)
                return PagedResult<TResult>.Empty;
            return results.Value;
        }*/
        
        async Task<IEnumerable<TResult>> IStorageClient.GetFilteredCollectionAsync<TResult, TQuery>(TQuery query, string endpoint)
        {
            logger.LogDebug($"Get filtered data from storage, endpoint: {String.Concat(BaseAddress, endpoint)}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            var results = await GetCollectionAsync<TResult>(queryString);
            
            return results;
        }

        private async Task<Maybe<HttpResponseMessage>> GetResponseAsync(string endpoint)
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
