using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace KesselRunFramework.AspNet.Validation
{
    public class NullValidator<T> : IValidator<T>
    {
        public ValidationResult Validate(T instance) => new ValidationResult();

        public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public CascadeMode CascadeMode { get; set; }

        public ValidationResult Validate(object instance)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
