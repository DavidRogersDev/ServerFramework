using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace InControl.Framework.AspNet.Validation
{
    public class CompositeValidator<T> : IValidator<T>
    {
        private readonly IEnumerable<IValidator<T>> _validators;

        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators;
        }

        public CascadeMode CascadeMode { get; set; }

        public ValidationResult Validate(IValidationContext context)
        {
            var validationResults = new HashSet<ValidationResult>();

            foreach (var validator in _validators)
            {
                var res = validator.Validate(context);
                validationResults.Add(res);
            }

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }

        public async Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = new CancellationToken())
        {
            var validationTasks = new List<Task<ValidationResult>>();

            foreach (var validator in _validators)
            {
                var validationTask = validator.ValidateAsync(context, cancellation);
                validationTasks.Add(validationTask);
            }

            var validationResults = await Task.WhenAll(validationTasks);

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
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
            var validationTasks = new List<Task<ValidationResult>>();

            foreach (var validator in _validators)
            {
                var validationTask = validator.ValidateAsync(instance, cancellation);
                validationTasks.Add(validationTask);
            }

            var validationResults = await Task.WhenAll(validationTasks);

            return new ValidationResult(validationResults.SelectMany(vr => vr.Errors));
        }
    }
}
