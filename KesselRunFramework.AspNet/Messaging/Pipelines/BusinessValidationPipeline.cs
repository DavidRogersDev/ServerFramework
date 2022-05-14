using System;
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
        where TRequest : IValidateable
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
            
            var result = await _compositeValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                _logger.TraceMessageValidationFailed(
                    result.Errors.Select(s => s.ErrorMessage).Aggregate(
                            (acc, current) => acc += string.Concat(GeneralPurpose.UniqueDelimiter, current)
                        ),
                    _currentUser?.UserName ?? GeneralPurpose. AnonymousUser
                    );

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    // Add validation fail to ModelState to make it available there. Just in case it is needed.
                    result.AddToModelState(_actionContextAccessor.ActionContext.ModelState, string.Empty);                    

                    var resultType = responseType.GetGenericArguments()[0];
                    var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);

                    var invalidResponse =
                        Activator.CreateInstance(
                            invalidResponseType, 
                            null, 
                            result.ToDictionary()
                            ) as TResponse;

                    return invalidResponse;
                }
                
                throw new Exception("IValidateable implementation must be a generic type.");
            }

            _logger.TraceMessageValidationPassed();

            var response = await next();

            return response;
        }
    }
}
