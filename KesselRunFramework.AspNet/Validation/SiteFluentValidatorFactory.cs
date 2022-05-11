using FluentValidation;
using System;

namespace KesselRunFramework.AspNet.Validation
{
    public class SiteFluentValidatorFactory : IValidatorFactory
    {
        private IServiceProvider Provider { get; set; }

        public SiteFluentValidatorFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IValidator CreateInstance(Type validatorType)
        {
            return Provider.GetService(validatorType) as IValidator;
        }

        public IValidator<T> GetValidator<T>()
        {
            return Provider.GetService(typeof(IValidator<T>)) as IValidator<T>;
        }

        public IValidator GetValidator(Type type)
        {
            Type generic = typeof(IValidator<>);
            Type specific = generic.MakeGenericType(type);

            return Provider.GetService(specific) as IValidator;
        }
    }
}
