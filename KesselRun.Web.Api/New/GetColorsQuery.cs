using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;

namespace KesselRun.Web.Api.New
{
    public class GetColorsQuery : IQuery<Either<IEnumerable<ColorPayloadDto>, ValidationResult>>
    {
        // no properties necessary, as the query will return all Colors.
    }
}