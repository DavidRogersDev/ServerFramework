using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Messaging.Pipelines
{
    public class BusinessValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest<TResponse>, IValidateable
    {
        private readonly IValidator<TRequest> _compositeValidator;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IActionContextAccessor _actionContextAccessor;

        public BusinessValidationPipeline(
            IValidator<TRequest> compositeValidator,
            ILogger<TRequest> logger,
            ICurrentUser currentUser,
            IActionContextAccessor actionContextAccessor)
        {
            _compositeValidator = compositeValidator;
            _logger = logger;
            _currentUser = currentUser;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.TraceBeforeValidatingMessage();

            var responseType = typeof(TResponse);

            if (responseType.Name.StartsWith(nameof(ValidateableResponse), StringComparison.Ordinal))
            {
                var result = await _compositeValidator.ValidateAsync(request, cancellationToken);

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
                        var nonGenericInvalidResponse = new ValidateableResponse(errorsCollated) as TResponse;

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

                return await next();
            }

            throw new Exception($"IValidateable implementation must be a {nameof(ValidateableResponse)}.");
        }
    }
}
