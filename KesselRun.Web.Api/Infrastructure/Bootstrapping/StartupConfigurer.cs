using KesselRun.Web.Api.Infrastructure.Invariable;
using Microsoft.Extensions.Configuration;

namespace KesselRun.Web.Api.Infrastructure.Bootstrapping
{
    public static class StartupConfigurer
    {
        public static AppConfiguration GetAppConfiguration(IConfiguration configuration)
        {
            var AppConfiguration = new AppConfiguration();
            configuration.Bind(Config.ApplicationConfigFromJson, AppConfiguration);

            return AppConfiguration;
        }
    }
}
