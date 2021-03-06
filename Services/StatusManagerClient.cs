﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace servicedesk.api
{
    public interface IStatusManagerClient
    {
        Task<StatusEventDto> GetCurrentStatusAsync(string sourceName, Guid referenceId);
        Task<IEnumerable<StatusDto>> GetNextStatusAsync(string sourceName, Guid referenceId);
    }

    public class StatusManagerClient : IStatusManagerClient
    {
        private readonly HttpClient _client;
        public StatusManagerClient(IOptions<StatusManagerConfiguration> configuration) 
        {
            _client = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });

            _client.BaseAddress = new Uri(configuration.Value.Url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<StatusEventDto> GetCurrentStatusAsync(string sourceName, Guid referenceId)
        {
            var response = await _client.GetAsync($"/sources/{sourceName}/{referenceId}/current");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
                
            //return default(StatusEventDto);
            // response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StatusEventDto>(json);

            return result;
        }

        public async Task<IEnumerable<StatusDto>> GetNextStatusAsync(string sourceName, Guid referenceId)
        {
            var response = await _client.GetAsync($"/sources/{sourceName}/{referenceId}/next");

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception(await response.Content.ReadAsStringAsync());
                
            //return default(IEnumerable<StatusDto>);
            // response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<StatusDto>>(json);

            return result;
        }
    }
    
    public class StatusEventDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string State { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public bool IsApproved { get; set; }
        public bool IsUndo { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Success => State == "completed" && Code == "success";
        public StatusDto Status { get; set; }

    }
    
    public class StatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Step { get; set; }
        public bool IsFinal { get; set; }
    }
}
