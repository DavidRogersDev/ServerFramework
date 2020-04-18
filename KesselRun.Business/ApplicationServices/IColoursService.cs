using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KesselRun.Business.ApplicationServices
{
    public interface IColorsService
    {
        Task<(ValidationResult ValidationResult, List<ColorPayloadDto> DTOs)> GetColorsAsync();
    }
}
