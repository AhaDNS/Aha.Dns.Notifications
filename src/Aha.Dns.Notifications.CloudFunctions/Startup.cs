using Aha.Dns.Notifications.CloudFunctions.ApiClients;
using Aha.Dns.Notifications.CloudFunctions.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using System.IO;

[assembly: FunctionsStartup(typeof(Aha.Dns.Notifications.CloudFunctions.Startup))]
namespace Aha.Dns.Notifications.CloudFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "host.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.ConfigurationBuilder.Build())
               .Enrich.FromLogContext()
               .Enrich.WithExceptionDetails()
               .Enrich.WithProperty("Proc", "Aha.Dns.Notifications.CloudFunctions")
               .CreateLogger();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Settings
            builder.Services.AddOptions<SummarizedStatisticsApiSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(SummarizedStatisticsApiSettings.ConfigSectionName).Bind(settings);
            });

            // Http client
            builder.Services.AddHttpClient<ISummarizedStatisticsApiClient, SummarizedStatisticsApiClient>();
        }
    }
}
