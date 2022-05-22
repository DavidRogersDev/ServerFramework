using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetColorsQuery : IQuery<Either<IEnumerable<ColorPayloadDto>, ValidationResult>>
    {
        // no properties necessary, as the query will return all Colors.
    }
}