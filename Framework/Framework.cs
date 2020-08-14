using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Cornerstone
{
    public static class Framework
    {
        public static IServiceProvider Provider { get; private set; }

        public static IConfiguration Configuration => Provider.GetService<IConfiguration>();
        public static ILogger Logger => Provider.GetService<ILogger>();

        public static FrameworkEnvironment Environment => Provider.GetService<FrameworkEnvironment>();

        public static IExceptionHandler ExceptionHandler => Provider.GetService<IExceptionHandler>();
        public static void Build(this FrameworkConstruction construction)
        {
            Provider = construction.Services.BuildServiceProvider();

            Logger.LogCriticalSource($"Framework started in {Environment.Configuration} mode...");
        }
    }
}
