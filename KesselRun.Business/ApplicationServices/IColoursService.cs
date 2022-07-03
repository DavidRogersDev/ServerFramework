using BusinessValidation;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KesselRun.Business.ApplicationServices
{
    public interface IColorsService
    {
        Task<Either<IEnumerable<ColorPayloadDto>, Validator>> GetColorsAsync();
    }
}
