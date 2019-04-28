using JetBrains.Annotations;
using Lykke.Logs.Loggers.LykkeSlack;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;

namespace Lykke.Service.EasyBuy
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "EasyBuy API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.Extend = (serviceCollection, settings) =>
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfiles(typeof(PostgresRepositories.AutoMapperProfile));
                        cfg.AddProfiles(typeof(AutoMapperProfile));
                    });

                    Mapper.AssertConfigurationIsValid();
                };

                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "EasyBuyLog";
                    logs.AzureTableConnectionStringResolver =
                        settings => settings.EasyBuyService.Db.LogsConnectionString;
                    
                    logs.Extended = extendedLogs =>
                    {
                        extendedLogs.AddAdditionalSlackChannel("liquidity-market-maker-errors",
                            channelOptions => { channelOptions.MinLogLevel = LogLevel.Warning; });
                    };
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options => { options.SwaggerOptions = _swaggerOptions; });
        }
    }
}
