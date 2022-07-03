using AutoMapper;
using BusinessValidation;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace KesselRun.Business.ApplicationServices
{
    public class ColorsService : ApplicationService, IColorsService
    {
        public ColorsService(IMapper mapper)
            : base(mapper)
        {
        }

        public async Task<Either<IEnumerable<ColorPayloadDto>, Validator>> GetColorsAsync()
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

            var validator = new Validator();

            validator.Validate("NoColors", "The color collection must contain at least one color.", colorPayloadDtos, c => c.Any());

            if (validator)
                return await Task.FromResult(colorPayloadDtos);

            return await Task.FromResult(validator);
        }
    }
}
