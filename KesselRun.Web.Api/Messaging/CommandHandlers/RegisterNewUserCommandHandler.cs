using System.Threading;
using System.Threading.Tasks;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.Messaging.CommandHandlers
{
    public class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, ValidateableResponse<ApiResponse<int>>>
    {
        private readonly ILogger _logger;

        public RegisterNewUserCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(RegisterNewUserCommand request,
            CancellationToken cancellationToken)
        {
            // normally there would be code here which calls a service to persist the new user.
            // For demo purposes, I will just return the Id of 10 for the new imaginary user.

            return await Task.FromResult(
                new ValidateableResponse<ApiResponse<int>>(
                    new ApiResponse<int>
                    {
                        Data = 10,
                        Outcome = OperationOutcome.SuccessfulOutcome
                    }
                    )
                );
        }
    }
}
