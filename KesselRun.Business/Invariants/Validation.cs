using FrmwkInvariants = KesselRunFramework.Core.Infrastructure.Invariants;

namespace KesselRun.Business.Invariants
{
    public sealed class Validation
    {
        public sealed class PartMessages
        {
            public const string UserAlreadyExists = "already exists as a user.";

        }

        public sealed class Messages
        {
            public const string UserNameMustNotBeEmpty = "UserName " + FrmwkInvariants.Validation.PartMessages.MustNotBeEmpty;
            public const string UserRoleMustNotBeEmpty = "User Role " + FrmwkInvariants.Validation.PartMessages.MustNotBeEmpty;
        }
    }
}
