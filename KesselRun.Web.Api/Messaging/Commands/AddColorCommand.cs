using KesselRunFramework.Core.Cqrs.Commands;

namespace KesselRun.Web.Api.Messaging.Commands
{
    public class AddColorCommand : ICommand<int>
    {
        public string Color { get; set; }
        public bool IsKnownColor { get; set; }
    }
}
