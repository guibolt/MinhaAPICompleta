using System;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        const string chaveApiLog = "64382751abb34cfd8ec13b644fd7bd22";
        const string chaveLog = "e1efec4f-c395-49fc-ad8e-808ac154f0f5";
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddElmahIo(o =>
            {
                o.ApiKey = chaveApiLog;
                o.LogId = new Guid(chaveLog);
            });

            services.AddHealthChecks()
           .AddCheck("Produtos", new SqlServerHealthCheck(connection))
           .AddSqlServer(connection);
            services.AddHealthChecksUI();

            //services.AddHealthChecks()
            //    .AddElmahIoPublisher(options =>
            //    {
            //        options.ApiKey = "388dd3a277cb44c4aa128b5c899a3106";
            //        options.LogId = new Guid("c468b2b8-b35d-4f1a-849d-f47b60eef096");
            //        options.HeartbeatId = "API Fornecedores";

            //    })
            //    .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            //    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}