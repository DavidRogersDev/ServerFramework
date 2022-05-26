using FluentValidation;
using FluentValidation.AspNetCore;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using KesselRunFramework.Core.Cqrs.Commands;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRunFramework.AspNet.Messaging.CommandDecorators
{
    public class BusinessValidationDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : class
    {
        private readonly ICommandHandler<TCommand, TResponse> _decorator;
        private readonly IValidator<TCommand> _compositeValidator;
        private readonly ILogger<TCommand> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IActionContextAccessor _actionContextAccessor;

        public BusinessValidationDecorator(
            ICommandHandler<TCommand, TResponse> decorator,
            IValidator<TCommand> compositeValidator,
            ILogger<TCommand> logger,
            ICurrentUser currentUser,
            IActionContextAccessor actionContextAccessor
            )
        {
            _decorator = decorator;
            _compositeValidator = compositeValidator;
            _logger = logger;
            _currentUser = currentUser;
            _actionContextAccessor = actionContextAccessor;
        }


        public async Task<TResponse> ExecuteAsync(TCommand command, CancellationToken cancellationToken)
        {
            _logger.TraceBeforeValidatingMessage();

            var responseType = typeof(TResponse);

            if (responseType.Name.StartsWith(nameof(ValidateableResponse), StringComparison.Ordinal))
            {
                var result = await _compositeValidator.ValidateAsync(command, cancellationToken);

                if (!result.IsValid)
                {
                    var errorsCollated = result.ToDictionary();

                    _logger.TraceMessageValidationFailed(errorsCollated, _currentUser?.UserName ?? GeneralPurpose.AnonymousUser);

                    // Add validation fail to ModelState to make it available there. Just in case it is needed.
                    result.AddToModelState(_actionContextAccessor.ActionContext.ModelState, string.Empty);

                    // Deal with type depending on whether it is the generic version of ValidateableResponse or not.
                    var resultType = responseType.GetGenericArguments().FirstOrDefault();

                    if (ReferenceEquals(resultType, null))
                    {
                        var nonGenericInvalidResponse =
                            Activator.CreateInstance(
                                responseType,
                                errorsCollated
                                ) as TResponse;

                        return nonGenericInvalidResponse;
                    }

                    var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);

                    var invalidResponse =
                        Activator.CreateInstance(
                            invalidResponseType,
                            null,
                            errorsCollated
                            ) as TResponse;

                    return invalidResponse;
                }

                _logger.TraceMessageValidationPassed();

                return await _decorator.ExecuteAsync(command, cancellationToken);
            }

            _logger.TraceMessageValidationPassed();

            return await _decorator.ExecuteAsync(command, cancellationToken);
        }
    }
}
