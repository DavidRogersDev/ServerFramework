using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Messaging;

namespace KesselRun.Web.Api.New
{
    public class RegisterNewUserCommand : ICommand<ValidateableResponse<ApiResponse<int>>>
    {
        public RegisterUserPayloadDto Dto { get; set; }
    }
}
