using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Cqrs.Queries;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetUserQuery : IQuery<UserPayloadDto>
    {
        public int UserId { get; set; }
    }
}
