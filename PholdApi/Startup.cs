using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag.Generation.Processors.Security;
using PholdApi.Filters;
using PholdApi.Interfaces;
using PholdApi.Services;

namespace PholdApi
{
    public class Startup
    {

        private const string _docName = "PholdAPI";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<CredentialsFilter>();
            services.AddScoped<IPholdStorageService, PholdStorageService>();
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<ICredentialsService, CredentialsService>();
            
            services.AddOpenApiDocument(c =>
            {
                c.DocumentProcessors.Add(new SecurityDefinitionAppender("api-key", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "api-key",
                    Description = "api-key",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Query
                }));

                c.OperationProcessors.Add(new OperationSecurityScopeProcessor("api-key"));

                c.Title = _docName;
                c.Version = "V1";

            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseOpenApi();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerUi3();
        }
    }
}
