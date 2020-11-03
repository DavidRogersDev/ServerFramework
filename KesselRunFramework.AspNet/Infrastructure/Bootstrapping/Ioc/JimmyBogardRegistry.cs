using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using KesselRunFramework.Core.Infrastructure.Mapping;
using MediatR;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc
{
    public static class JimmyBogardRegistry
    {
        public static void RegisterMediatRAbstractions(this Container container, Assembly[] mediatrAssemblies, Type[] pipelineBehaviors)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (mediatrAssemblies == null) throw new ArgumentNullException(nameof(mediatrAssemblies));
            if (pipelineBehaviors == null) throw new ArgumentNullException(nameof(pipelineBehaviors));

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), mediatrAssemblies, Lifestyle.Scoped);

            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            var notificationHandlerTypes = container.GetTypesToRegister(typeof(INotificationHandler<>), mediatrAssemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });

            container.Collection.Register(typeof(INotificationHandler<>), notificationHandlerTypes);

            // Register Pipelines here
            container.Collection.Register(typeof(IPipelineBehavior<,>), pipelineBehaviors);

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

        }

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
