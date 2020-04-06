using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace KesselRunFramework.Core.Infrastructure.Mapping
{
    public class ProfileBase : Profile
    {
        protected ProfileBase(string profileName)
            : base(profileName)
        {
            ConfigureProfile();
        }

        protected void ConfigureProfile()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            ShouldMapField = field => false;
        }

        public void InitializeMappings(IEnumerable<Assembly> assemblies)
        {
            var assemblyTypes = assemblies.SelectMany(a => a.GetExportedTypes());

            LoadStandardMappings(assemblyTypes);
            LoadCustomMappings(assemblyTypes);
        }
        
        private void LoadStandardMappings(IEnumerable<Type> coreAssemblyTypes)
        {
            var maps = (from t in coreAssemblyTypes
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select new
                {
                    Source = i.GetGenericArguments()[0],
                    Destination = t
                }).ToArray();

            foreach (var map in maps)
            {
                CreateMap(map.Source, map.Destination);
            }
        }

        private void LoadCustomMappings(IEnumerable<Type> coreAssemblyTypes)
        {
            var maps = (from t in coreAssemblyTypes
                from i in t.GetInterfaces()
                where typeof(ICustomMapSomeClasses).IsAssignableFrom(t) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select (ICustomMapSomeClasses)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(this);
            }
        }
    }
}
