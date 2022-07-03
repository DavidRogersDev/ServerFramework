using KesselRun.Business.ApplicationServices;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Messaging;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetColorsQueryHandler : IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidateableResponse>>
    {
        private readonly IColorsService _colorsService;

        public GetColorsQueryHandler(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        public async Task<Either<IEnumerable<ColorPayloadDto>, ValidateableResponse>> HandleAsync(GetColorsQuery query, CancellationToken cancellationToken)
        {
            var result = await _colorsService.GetColorsAsync();

            if (result.RightOrDefault() is null)
                return result.LeftOrDefault().ToList(); // call ToList() because for implicit casting to work, the type must be concrete i.e. can't just be IEnumerable<ColorPayloadDto>

            return new ValidateableResponse(result.RightOrDefault().ValidationFailures);
        }
    }
}
