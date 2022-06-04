using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Messaging;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetColorsQuery : IQuery<Either<IEnumerable<ColorPayloadDto>, ValidateableResponse>>
    {
        // no properties necessary, as the query will return all Colors.
    }
}