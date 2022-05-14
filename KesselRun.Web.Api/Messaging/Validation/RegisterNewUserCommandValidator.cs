using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using KesselRun.Web.Api.Messaging.Commands;
using Constants = KesselRun.Business.Invariants;

namespace KesselRun.Web.Api.Messaging.Validation
{
    public class RegisterNewUserCommandValidator : AbstractValidator<RegisterNewUserCommand>
    {

        public RegisterNewUserCommandValidator()
        {
            RuleFor(u => u.Dto.UserName)
                .MustAsync(NotContainUserAlready)
                .WithMessage((u,userName) => $"{userName} {Constants.Validation.PartMessages.UserAlreadyExists}");
        }

        async Task<bool> NotContainUserAlready(RegisterNewUserCommand dto, string userName, CancellationToken cancellation = new CancellationToken())
        {
            // **************************************************************************************************** /
            //
            // Normally we would hit a database and retrieve the User here.
            // For demo purposes, I will enforce an assumption that a user with UserName "DaveRogers" already exists.
            //
            // **************************************************************************************************** /

            return await Task.FromResult(!userName.Equals("DaveRogers"));
        }
    }
}
