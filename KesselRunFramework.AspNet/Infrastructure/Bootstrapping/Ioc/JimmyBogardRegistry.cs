using System;
using System.Collections.Generic;
using AutoMapper;
using KesselRunFramework.Core.Infrastructure.Mapping;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc
{
    public static class JimmyBogardRegistry
    {       
        public static void RegisterAutomapperAbstractions(this Container container, Profile[] automapperProfiles)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (automapperProfiles == null) throw new ArgumentNullException(nameof(automapperProfiles));

            container.Register<MapperProvider>(Lifestyle.Singleton);
            container.RegisterSingleton(() => GetMapper(container, automapperProfiles));
        }

        private static IMapper GetMapper(Container container, IEnumerable<Profile> profiles)
        {
            var mapperProvider = container.GetInstance<MapperProvider>();
            return mapperProvider.GetMapper(profiles); 
        }

    }
}
