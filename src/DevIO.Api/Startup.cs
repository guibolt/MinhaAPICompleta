using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DevIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
        
            services.AddIdentityConfig(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();
            services.AddLoggingConfig(Configuration);
            services.AddSwaggerConfig();
            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
           
            else
                app.UseHsts();
            
            app.UseAuthentication();
            app.UseMvcConfiguration();
            app.UseLoggingConfiguration();
            app.UseSwaggerConfig(provider);
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHealthChecks("/api/hc", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }
            ); 

            app.UseHealthChecksUI(opt => { opt.UIPath = "/api/hc-ui"; });

        }
    }
}
