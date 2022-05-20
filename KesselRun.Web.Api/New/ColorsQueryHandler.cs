using FluentValidation.Results;
using KesselRun.Business.ApplicationServices;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.New
{
    public class ColorsQueryHandler : IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidationResult>>
    {
        private readonly IColorsService _colorsService;

        public ColorsQueryHandler(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        public async Task<Either<IEnumerable<ColorPayloadDto>, ValidationResult>> HandleAsync(GetColorsQuery query, CancellationToken cancellationToken)
        {
            return await _colorsService.GetColorsAsync();
        }
    }
}
