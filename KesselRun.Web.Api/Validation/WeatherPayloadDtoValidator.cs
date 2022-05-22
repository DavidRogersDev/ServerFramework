using FluentValidation;
using KesselRun.Business.DataTransferObjects;

namespace KesselRun.Web.Api.Validation
{
    public class WeatherPayloadDtoValidator : AbstractValidator<WeatherPayloadDto>
    {
        public WeatherPayloadDtoValidator()
        {
            RuleFor(m => m.City).NotEmpty();
            RuleFor(m => m.Units).NotEmpty();
        }
    }
}
