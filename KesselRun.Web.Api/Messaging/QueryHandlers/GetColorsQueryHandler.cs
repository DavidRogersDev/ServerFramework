using FluentValidation.Results;
using KesselRun.Business.ApplicationServices;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Infrastructure.Validation;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetColorsQueryHandler : IRequestHandler<GetColorsQuery, Either<List<ColorPayloadDto>, ValidationResult>>
    {
        private readonly IColorsService _colorsService;

        public GetColorsQueryHandler(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        public async Task<Either<List<ColorPayloadDto>, ValidationResult>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
        {
            var result = await _colorsService.GetColorsAsync();

            if (!result.ValidationResult.IsValid)
                return result.ValidationResult;

            return result.DTOs;
        }
    }
}
