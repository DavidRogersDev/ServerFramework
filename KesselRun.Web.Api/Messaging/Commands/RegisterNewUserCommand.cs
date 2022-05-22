using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Commands;
using KesselRunFramework.Core.Infrastructure.Messaging;

namespace KesselRun.Web.Api.Messaging.Commands
{
    public class RegisterNewUserCommand : ICommand<ValidateableResponse<ApiResponse<int>>>
    {
        public RegisterUserPayloadDto Dto { get; set; }
    }
}
