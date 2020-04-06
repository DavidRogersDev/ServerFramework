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
        public static void RegisterApplicationServices(this Container container, Assembly assembly, IConfiguration configuration, string applicationServicesFullNs)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (applicationServicesFullNs == null) throw new ArgumentNullException(nameof(applicationServicesFullNs));
            if (applicationServicesFullNs.Trim().Equals(string.Empty)) throw new ArgumentException(nameof(applicationServicesFullNs) + " cannot be an empty string.");

            const string applicationServicesInterface = nameof(IApplicationService);

            var applicationServices = assembly.GetExportedTypes()
                .Where(t => t.Namespace != null
                            && t.Namespace.StartsWith(applicationServicesFullNs, StringComparison.Ordinal))
                .Where(t => t.GetInterfaces().Any() && !t.IsInterface)
                .Select(type => new
                {
                    Service = type.GetInterfaces().First(i => i.Name != applicationServicesInterface),
                    Implementation = type
                });

            foreach (var applicationService in applicationServices)
            {
                container.Register(applicationService.Service, applicationService.Implementation, Lifestyle.Singleton);
            }
        }
    }
}
