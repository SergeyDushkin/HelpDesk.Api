using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

public class SecureLinkMiddleware
    {
        private Dictionary<string, Request> urls = new Dictionary<string, Request>();

        private readonly RequestDelegate _next;
        private readonly double _timeout;

        public SecureLinkMiddleware(RequestDelegate next)
        {
            _next = next;
            _timeout = 5000;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("securityKey")) 
            {
                StringValues key;
                context.Request.Query.TryGetValue("securityKey", out key);

                if (!urls.ContainsKey(key))
                {
                    throw new Exception($"Security key {key} is not valid");
                }

                var request = urls[key];
                
                if (request.Date.AddMilliseconds(this._timeout) < DateTime.Now)
                {
                    urls.Remove(key);
                    throw new Exception($"Security key was expired");
                }

                foreach(var header in request.Headers) {
                    if (context.Request.Headers.ContainsKey(header.Key))
                        context.Request.Headers.Remove(header.Key);

                    context.Request.Headers.Add(header.Key, header.Value);
                }

                await _next.Invoke(context);
                return;
            }

            if (context.Request.Query.ContainsKey("postback")) 
            {
                var key = Guid.NewGuid().ToString("N");

                var headers = context.Request.Headers.Select(r => new Header { Key = r.Key, Value = r.Value }).ToList();
                this.urls.Add(key, new Request(headers));

                var url = context.Request.Path.Add(new QueryString($"?securityKey={key}"));

                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new { url = url }));
                return;
            }
            
            await _next.Invoke(context);
        }

        class Request
        {
            readonly public DateTime Date;
            readonly public IEnumerable<Header> Headers;

            public Request(IEnumerable<Header> headers) 
            {
                this.Date = DateTime.Now;
                this.Headers = headers;
            }
        }

        class Header {
            public string Key { get; set; }
            public StringValues Value { get; set; }
        }
    }

    public static class SecureLinkMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecureLinkMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecureLinkMiddleware>();
        }
    }