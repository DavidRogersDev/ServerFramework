﻿using FluentValidation;
using FluentValidation.AspNetCore;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.New
{
    public class BusinessValidationDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand: ICommand<TResponse>
        where TResponse: class
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

            var result = await _compositeValidator.ValidateAsync(command, cancellationToken);

            if (!result.IsValid)
            {
                _logger.TraceMessageValidationFailed(
                    result.Errors.Select(s => s.ErrorMessage).Aggregate(
                            (acc, current) => acc += string.Concat(GeneralPurpose.UniqueDelimiter, current)
                        ),
                    _currentUser?.UserName ?? GeneralPurpose.AnonymousUser
                    );

                
                var responseType = typeof(TResponse);

                if (responseType.BaseType.Name.Equals(nameof(ValidateableResponse), StringComparison.Ordinal))
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

            return await _decorator.ExecuteAsync(command, cancellationToken);
        }
    }
}