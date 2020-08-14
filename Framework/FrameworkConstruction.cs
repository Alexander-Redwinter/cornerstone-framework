using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone
{
    public class FrameworkConstruction
    {
        public IServiceCollection Services { get; set; }

        public FrameworkEnvironment Environment { get; set; }

        public IConfiguration Configuration { get; set; }

        public FrameworkConstruction()
        {
            Services = new ServiceCollection();

            Environment = new FrameworkEnvironment();

            Services.AddSingleton(Environment);

        }
    }
}
