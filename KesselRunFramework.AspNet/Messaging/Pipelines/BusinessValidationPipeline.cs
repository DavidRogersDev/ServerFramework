using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using KesselRunFramework.Core.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Messaging;
using MediatR;
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

        public BusinessValidationPipeline(IValidator<TRequest> compositeValidator, ILogger<TRequest> logger, ICurrentUser currentUser)
        {
            _compositeValidator = compositeValidator;
            _logger = logger;
            _currentUser = currentUser;
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
                    _currentUser?.UserName ?? "Anonymous"
                    );

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    var resultType = responseType.GetGenericArguments()[0];
                    var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);

                    var invalidResponse =
                        Activator.CreateInstance(invalidResponseType, null, result.Errors.Select(s => s.ErrorMessage).ToList()) as TResponse;

                    return invalidResponse;
                }
            }

            _logger.TraceMessageValidationPassed();
            var response = await next();

            return response;
        }
    }
}
