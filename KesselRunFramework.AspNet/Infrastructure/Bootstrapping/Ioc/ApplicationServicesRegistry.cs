using System;
using System.Linq;
using System.Reflection;
using KesselRunFramework.Core;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc
{
    public static class ApplicationServicesRegistry
    {
        public static void RegisterApplicationServices(this Container container, Assembly assembly, IConfiguration configuration)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var applicationServicesInterface = typeof(IApplicationService);
            var applicationServicesInterfaceName = nameof(IApplicationService);
            var applicationDataServicesInterface = typeof(IApplicationDataService);
            var applicationDataServicesInterfaceName = nameof(IApplicationDataService);

            var applicationServices = assembly.GetExportedTypes()
                .Where(t => applicationDataServicesInterface.IsAssignableFrom(t)
                            || applicationServicesInterface.IsAssignableFrom(t)
                            )
                .Where(t => !t.IsInterface)
                .Select(t => new 
                    { 
                        Type = t, 
                        Interfaces = t.GetInterfaces() 
                    })
                .Where(t => t.Interfaces.Any())
                .Select(type => new
                {
                    Service = type.Interfaces.First(
                        i => !i.Name.Equals(applicationServicesInterfaceName, StringComparison.Ordinal)
                             && !i.Name.Equals(applicationDataServicesInterfaceName, StringComparison.Ordinal)
                             ),
                    Implementation = type.Type
                });

            foreach (var applicationService in applicationServices)
            {
                container.Register(applicationService.Service, applicationService.Implementation, Lifestyle.Singleton);
            }
        }
    }
}
