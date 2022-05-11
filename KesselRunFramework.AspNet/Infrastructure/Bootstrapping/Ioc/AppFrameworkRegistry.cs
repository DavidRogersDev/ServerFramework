using System;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using InControl.Framework.AspNet.Validation;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc
{
    public static class AppFrameworkRegistry
    {
        public static void RegisterAspNetCoreAbstractions(this Container container)
        {

        }

        public static void RegisterValidationAbstractions(this Container container, IEnumerable<Assembly> validationAssemblies)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (validationAssemblies == null) throw new ArgumentNullException(nameof(validationAssemblies));

            container.Collection.Register(typeof(IValidator<>), validationAssemblies, Lifestyle.Singleton);

            container.Register(typeof(IValidator<>), typeof(CompositeValidator<>), Lifestyle.Singleton);
        }
    }
}
