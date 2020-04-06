using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Messaging;
using MediatR;

namespace KesselRun.Web.Api.Messaging.Commands
{
    public class RegisterNewUserCommand : IRequest<ValidateableResponse<ApiResponse<int>>>, IValidateable, IMembershipCommand
    {
        public RegisterUserPayloadDto Dto { get; set; }
    }
}
