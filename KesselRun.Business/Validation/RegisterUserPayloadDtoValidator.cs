using FluentValidation;
using KesselRun.Business.DataTransferObjects;

namespace KesselRun.Business.Validation
{
    public class RegisterUserPayloadDtoValidator : AbstractValidator<RegisterUserPayloadDto>
    {
        public RegisterUserPayloadDtoValidator()
        {
            RuleFor(p => p.Password).NotEmpty(); // defer password strength validation to Identity library
            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.Password);
            RuleFor(p => p.UserName).NotEmpty();
        }
    }
}
