using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cornerstone
{
    public static class FrameworkExtensions
    {


        public static FrameworkConstruction Configure(this FrameworkConstruction construction, Action<IConfigurationBuilder> configure = null)
        {

            var configurationBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{construction.Environment.Configuration}.json", optional: true, reloadOnChange: true);

            configure?.Invoke(configurationBuilder);

            var configuration = configurationBuilder.Build();
            construction.Services.AddSingleton<IConfiguration>(configuration);

            construction.Configuration = configuration;

            return construction;
        }
        public static FrameworkConstruction AddDefaultLogger(this FrameworkConstruction construction)
        {
            construction.Services.AddLogging(options =>
            {
                options.AddConfiguration(construction.Configuration.GetSection("Logging"));
                options.AddConsole();
                options.AddDebug();
            });

            construction.Services.AddTransient(provider => provider.GetService<ILoggerFactory>().CreateLogger("Cornerstone"));

            return construction;
        }
        public static FrameworkConstruction AddDefaultExceptionHandler(this FrameworkConstruction construction)
        {
            construction.Services.AddSingleton<IExceptionHandler>(new BaseExceptionHandler());

            return construction;
        }
        public static FrameworkConstruction UseDefaultServices(this FrameworkConstruction construction)
        {
            construction.AddDefaultExceptionHandler();

           

            construction.AddDefaultLogger();

            return construction;
        }

    }
}
