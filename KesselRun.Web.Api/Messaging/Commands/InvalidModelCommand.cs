using KesselRunFramework.Core.Cqrs.Queries;

namespace KesselRun.Web.Api.Messaging.Commands
{
    public class InvalidModelCommand : IQuery<string>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
