using Microsoft.Extensions.Configuration;
using Serilog;
using AndcultureCode.CSharp.Core.Providers.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using Serilog.Sinks.AwsCloudWatch;

namespace DylanJustice.Demo.Business.Core.Providers.Logging
{
    public class LoggingProvider
    {

        #region Constants

        private const string APP_NAME = "GravityBootsApi";
        private const string OUTPUT_TEMPLATE = "{MachineName} {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Application}] [{SourceContext}] {Message:lj}{NewLine}{Exception}{NewLine}";
        private const string CLOUDWATCH_LOG_GROUP = "dylanjustice-demo/api";

        #endregion Constants

        #region Private Properties

        private readonly IConfiguration _config;
        private readonly IHostEnvironment _environment;
        private readonly IServiceCollection _services;

        #endregion Private Properties

        #region Public Methods

        public LoggingProvider(IHostEnvironment environment, IServiceCollection services, IConfiguration config = null)
        {
            _config = config ?? new LocalConfigurationProvider().GetConfiguration();
            _environment = environment;
            _services = services;
        }

        public Serilog.ILogger GetLogger()
        {
            if (_environment.IsDevelopment())
            {
                return new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console(
                         restrictedToMinimumLevel: LogEventLevel.Debug,
                         outputTemplate: OUTPUT_TEMPLATE,
                         theme: SystemConsoleTheme.Colored)
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty("Application", APP_NAME)
                     .CreateLogger();
            }

            return new LoggerConfiguration()
                .WriteTo.Console(
                         restrictedToMinimumLevel: LogEventLevel.Debug,
                         outputTemplate: OUTPUT_TEMPLATE,
                         theme: SystemConsoleTheme.Colored)
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", APP_NAME)
                .CreateLogger();
        }

        #endregion Public Methods
    }
}