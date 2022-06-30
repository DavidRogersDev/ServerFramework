using BusinessValidation;
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
    public class GetColorsQueryHandler : IRequestHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, Validator>>
    {
        private readonly IColorsService _colorsService;

        public GetColorsQueryHandler(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        public async Task<Either<IEnumerable<ColorPayloadDto>, Validator>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
        {
            return await _colorsService.GetColorsAsync();            
        }
    }
}
