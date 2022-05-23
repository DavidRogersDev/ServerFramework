using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Infrastructure.Invariable;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Errors;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace KesselRun.Web.Api.Infrastructure.Bootstrapping
{
    public static class StartupConfigurer
    {
        public static IDictionary<string, Assembly> GetAssemblies()
        {
            var assemblies = new Dictionary<string, Assembly>(StringComparer.Ordinal)
            {
                {StartUpConfig.Domain, typeof(RegisterUserPayloadDto).GetTypeInfo().Assembly },
                {StartUpConfig.Executing, typeof(Startup).GetTypeInfo().Assembly},
                {StartUpConfig.FrameowrkCore, typeof(EventIDs).GetTypeInfo().Assembly},
            };

            // include any custom (domain) assemblies which will require scanning as part of the startup process.

            return assemblies;
        }

        public static AppConfiguration GetAppConfiguration(IConfiguration configuration)
        {
            var AppConfiguration = new AppConfiguration();
            configuration.Bind(Config.ApplicationConfigFromJson, AppConfiguration);

            return AppConfiguration;
        }
    }
}
