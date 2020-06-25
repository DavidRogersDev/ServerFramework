using System;
using FluentValidation.AspNetCore;
using KesselRunFramework.AspNet.Validation;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class FluentValidationConfigurer
    {
        public static Action<FluentValidationMvcConfiguration> ConfigureFluentValidation(Container container)
        {
            return fluentValidationMvcConfiguration =>
            {
                fluentValidationMvcConfiguration.ValidatorFactory = new SiteFluentValidatorFactory(container);
            };
        }
    }
}
