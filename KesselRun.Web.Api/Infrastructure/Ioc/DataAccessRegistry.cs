using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using KesselRun.Business.ApplicationServices;
using KesselRun.Web.Api.Infrastructure.Mapping;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Messaging.Decorators;
using KesselRunFramework.AspNet.Messaging.QueryDecorators;
using KesselRunFramework.Core.Cqrs.Commands;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Http;
using KesselRunFramework.DataAccess.Ops;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace KesselRun.Web.Api.Infrastructure.Ioc
{
    public static class DataAccessRegistry
    {
        public static void RegisterDataAccessComponents(this Container container, string connectionString)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            if (connectionString.Trim().Equals(string.Empty)) throw new ArgumentException(nameof(connectionString) + " cannot be an empty string.");

            container.Register<ISimpleListRetriever, SimpleListRetriever>(Lifestyle.Singleton);
        }

        public static void RegisterApplicationServices(this Container container, IConfiguration configuration, IDictionary<string, Assembly> assemblies)
        {
            container.RegisterSingleton<ITypedClientResolver, TypedClientResolver>();

            container.RegisterValidationAbstractions(new[] { assemblies[StartUpConfig.Executing], assemblies[StartUpConfig.Domain] });
            container.RegisterAutomapperAbstractions(GetAutoMapperProfiles(assemblies));
            container.RegisterApplicationServices(assemblies[StartUpConfig.Domain], configuration);

            container.RegisterSingleton<ITemporalService, TemporalService>();
        }

        public static void RegisterAspectParts(this Container container, IDictionary<string, Assembly> assemblies)
        {
            // commands and command decorators
            container.Register(typeof(ICommandHandler<,>), assemblies[StartUpConfig.Executing]);
            // command "pipelines"
            container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(BusinessValidationDecorator<,>));
            container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(LogContextDecorator<,>));

            // queries and query decorators
            container.Register(typeof(IQueryHandler<,>), assemblies[StartUpConfig.Executing]);
            // query "pipelines"
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(RequestProfilerDecorator<,>));

            // These are the equivalents of mediators in Mediatr. You can create as many as you like e.g. perhaps an IAdoNetQueryProcessor for legacy access.
            container.RegisterSingleton<ICommandProcessor, CommandProcessor>();
            container.RegisterSingleton<IQueryProcessor, QueryProcessor>();
        }

        private static Profile[] GetAutoMapperProfiles(IDictionary<string, Assembly> configurationAssemblies)
        {
            var kesselRunApiProfile = new KesselRunApiProfile("KesselRunApiProfile");
            kesselRunApiProfile.InitializeMappings(configurationAssemblies[StartUpConfig.Domain].InArray());

            return kesselRunApiProfile.InArray();
        }
    }
}
