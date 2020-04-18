using FluentValidation;
using KesselRun.Business.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace KesselRun.Business.Validation
{
    public class ColorCollectionValidator : AbstractValidator<IEnumerable<ColorPayloadDto>>
    {
        public ColorCollectionValidator()
        {
            // This is a bit of a silly validator. 
            // Normally a collection would be a property on a DTO.
            // But it interesting to see that you can do this with FluentValidation at all.
            RuleFor(c => c)
                .Must(c => c.Any())
                .WithMessage("The color collection must contain at least one color.");

        }
    }
}
