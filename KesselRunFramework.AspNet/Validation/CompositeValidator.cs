using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace KesselRunFramework.AspNet.Validation
{
    public class CompositeValidator<T> : IValidator<T>
    {
        private readonly IEnumerable<IValidator<T>> _validators;

        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators;
        }

        public ValidationResult Validate(T instance)
        {
            var failures = _validators
                .Select(v => v.Validate(instance))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(f => f != null)
                .ToList();

            return new ValidationResult(failures);
        }

        public async Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new CancellationToken())
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            foreach (var validator in _validators)
            {
                var res = await validator.ValidateAsync(instance, cancellation);
                validationResults.Add(res);
            }

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }

        public CascadeMode CascadeMode { get; set; }

        public ValidationResult Validate(object instance)
        {
            return ValidatePrivate(instance);
        }

        public async Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = new CancellationToken())
        {
            return await ValidateAsyncPrivate(instance, cancellation);
        }

        public ValidationResult Validate(ValidationContext context)
        {
            return ValidatePrivate(context.InstanceToValidate);
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = new CancellationToken())
        {
            return await ValidateAsyncPrivate(context.InstanceToValidate, cancellation);
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
        }

        private ValidationResult ValidatePrivate(object instance)
        {
            var validationResults = new HashSet<ValidationResult>();

            foreach (var validator in _validators)
            {
                var res = validator.Validate(instance);
                validationResults.Add(res);
            }

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }

        private async Task<ValidationResult> ValidateAsyncPrivate(object instance, CancellationToken cancellation = new CancellationToken())
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            foreach (var validator in _validators)
            {
                var res = await validator.ValidateAsync(instance, cancellation);
                validationResults.Add(res);
            }

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }
    }
}
