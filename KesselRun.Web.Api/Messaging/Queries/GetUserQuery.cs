using KesselRun.Business.DataTransferObjects;
using MediatR;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetUserQuery : IRequest<UserPayloadDto>
    {
        public int UserId { get; set; }
    }
}
