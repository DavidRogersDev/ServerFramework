using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using System.Collections.Generic;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetTimeZonesQuery : IQuery<ApiResponse<IReadOnlyList<string>>>
    {

    }
}
