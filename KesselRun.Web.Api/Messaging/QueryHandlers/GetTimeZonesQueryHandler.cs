using KesselRun.Business.ApplicationServices;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetTimeZonesQueryHandler : IQueryHandler<GetTimeZonesQuery, ApiResponse<IReadOnlyList<string>>>
    {
        private readonly ITemporalService _temporalService;

        public GetTimeZonesQueryHandler(ITemporalService temporalService)
        {
            _temporalService = temporalService;
        }

        public async Task<ApiResponse<IReadOnlyList<string>>> HandleAsync(GetTimeZonesQuery query, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                new ApiResponse<IReadOnlyList<string>>() { Data = _temporalService.GetTimeZones() }
                ); ;
        }
    }
}
