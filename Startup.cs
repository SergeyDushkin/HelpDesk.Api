using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using RawRabbit.Attributes;
using RawRabbit.Common;
using RawRabbit.Extensions.Client;
using RawRabbit.vNext.Logging;
using Serilog;
using servicedesk.api.Storages;
using Consul;

namespace servicedesk.api
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfigurationRoot _configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HelpDeskDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("HelpDeskDatabase")));
            services.AddDbContext<ContentDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("ContentDatabase")));
                
            services.Configure<StatusManagerConfiguration>(_configuration.GetSection("StatusService"));
            services.AddSingleton<IStatusManagerClient, StatusManagerClient>();

            //добавляем единственный экземпляр (Singleton) consul сервиса IConsulClient при помощи  инициации конкретного объекта ConsulClient
            //в конструкторе которого задаем анон. функцию, ConsulClient в части Url из настроечного файла. 
            services.AddSingleton<IConsulClient>(
                new ConsulClient (
                    x => { x.Address = new Uri(_configuration.GetSection("SettingsService").Get<SettingsConfiguration>().Url); }
                    ));
            services.AddSingleton<SettingsService>();


            services.AddConfiguration(_configuration.GetSection("TicketService"), () => new ApplicationServiceSettings<TicketStorage>());
            services.AddConfiguration(_configuration.GetSection("AddressService"), () => new ApplicationServiceSettings<AddressStorage>());


            services.AddSingleton<IStatusManagerClient, StatusManagerClient>();

            services.AddSingleton<IStorageClient, StorageClient<TicketStorage>>();
            services.AddSingleton<IStorageClient, StorageClient<AddressStorage>>();

            services.AddSingleton<ITicketStorage, TicketStorage>();
            services.AddSingleton<IAddressStorage, AddressStorage>();


            services.AddScoped<TicketService>();
            services.AddScoped<ClientService>();
            services.AddScoped<UserService>();
            services.AddScoped<AddressService>();
            services.AddScoped<SupplierService>();
            services.AddScoped<JobService>();
            services.AddScoped<ContentService>();
            services.AddScoped<ServiceService>();

            services.AddCors(x => x.AddPolicy("corsGlobalPolicy", policy => {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            }));
            /* 
            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireClaim("role", "OPERATOR"));
            });*/

            
			services
				.AddRawRabbit(
					_configuration.GetSection("RawRabbit"),
					ioc => ioc
						.AddSingleton(LoggingFactory.ApplicationLogger))
						.AddSingleton<IConfigurationEvaluator, AttributeConfigEvaluator>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCors("corsGlobalPolicy");

            var serilogLogger = new LoggerConfiguration()
                .Enrich.WithProperty("Application","ServiceDesk.Services.Tickets")
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();

            loggerFactory.AddSerilog(serilogLogger);
            loggerFactory.AddConsole(Microsoft.Extensions.Logging.LogLevel.Debug);
            loggerFactory.AddDebug();
            
            if (env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
            }
              /*
            var cert = new X509Certificate2(Path.Combine(_environment.ContentRootPath, 
                _configuration.GetSection("Authentication:Certificate").Value), _configuration.GetSection("Authentication:CertificatePassword").Value);

            var options = new JwtBearerOptions
            {
                Authority = _configuration.GetSection("Authentication:Authority").Value,
                Audience = _configuration.GetSection("Authentication:Audience").Value,

                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,

                TokenValidationParameters = { 
                   IssuerSigningKey = new X509SecurityKey(cert), 
                   ValidateIssuerSigningKey = true, 
                   ValidateLifetime = true, 
                   ClockSkew = TimeSpan.Zero,
                   ValidateIssuer = false,
                   ValidateAudience = false
                }
            };
            app.UseJwtBearerAuthentication(options);

  */
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseSecureLinkMiddleware();

            app.UseMvc();
        }
    }
}