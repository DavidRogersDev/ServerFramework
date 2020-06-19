using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using KesselRunFramework.Core.Infrastructure.Validation;

namespace KesselRun.Business.ApplicationServices
{
    public class ColorsService : ApplicationService, IColorsService
    {
        private readonly IValidator<IEnumerable<ColorPayloadDto>> _colorValidator;

        public ColorsService(IValidator<IEnumerable<ColorPayloadDto>> colorValidator, IMapper mapper)
            : base(mapper)
        {
            _colorValidator = colorValidator;
        }

        public async Task<Either<IEnumerable<ColorPayloadDto>, ValidationResult>> GetColorsAsync()
        {
            // This example is a bit silly.
            // Here, for the purposes of this example, the business rule is that the collection of colors that
            // is returned must not be empty.
            // To exercise the code, comment out option 1 if you want validation to pass and option 2 if you want validation to fail.

            /****************************** OPTION 1 ******************************/
            var colorPayloadDtos = new List<ColorPayloadDto>
            {
                new ColorPayloadDto{ Id = Color.Gainsboro.ToArgb(), IsKnownColor = Color.Gainsboro.IsKnownColor, Color = Color.Gainsboro.Name},
                new ColorPayloadDto{ Id = Color.SaddleBrown.ToArgb(), IsKnownColor = Color.SaddleBrown.IsKnownColor  ,Color =Color.SaddleBrown.Name},
                new ColorPayloadDto{ Id = Color.PaleGreen.ToArgb(), IsKnownColor = Color.PaleGreen.IsKnownColor,Color= Color.PaleGreen.Name},
                new ColorPayloadDto{ Id = Color.DarkBlue.ToArgb(), IsKnownColor = Color.DarkBlue.IsKnownColor,Color= Color.DarkBlue.Name}
            };

            /****************************** OPTION 2 ******************************/
            //var colorPayloadDtos = new List<ColorPayloadDto>(0);



            var validationResult = await _colorValidator.ValidateAsync(colorPayloadDtos);

            if (validationResult.IsValid)
                return colorPayloadDtos;

            return validationResult;
        }
    }
}
