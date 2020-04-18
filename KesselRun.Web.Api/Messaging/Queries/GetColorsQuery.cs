using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Infrastructure.Validation;
using MediatR;
using System.Collections.Generic;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetColorsQuery : IRequest<Either<IEnumerable<ColorPayloadDto>, ValidationResult>>
    {
        // no properties necessary, as the query will return all Colors.
    }
}
