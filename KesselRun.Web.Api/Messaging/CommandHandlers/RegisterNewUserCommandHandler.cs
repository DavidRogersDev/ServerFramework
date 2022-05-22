using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Commands;
using KesselRunFramework.Core.Infrastructure.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Messaging.CommandHandlers
{
    public class RegisterNewUserCommandHandler : ICommandHandler<RegisterNewUserCommand, ValidateableResponse<ApiResponse<int>>>
    {


        public async Task<ValidateableResponse<ApiResponse<int>>> ExecuteAsync(RegisterNewUserCommand command, CancellationToken cancellationToken)
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
