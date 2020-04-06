using System;
using System.Reflection;
using FluentValidation;
using KesselRunFramework.AspNet.Validation;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc
{
    public static class AppFrameworkRegistry
    {
        public static void RegisterAspNetCoreAbstractions(this Container container)
        {

        }

        public static void RegisterValidationAbstractions(this Container container, Assembly[] validationAssemblies)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (validationAssemblies == null) throw new ArgumentNullException(nameof(validationAssemblies));

            container.Collection.Register(typeof(IValidator<>), validationAssemblies);

            container.Register(typeof(IValidator<>), typeof(CompositeValidator<>), Lifestyle.Singleton);
        }
    }
}
