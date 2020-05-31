using MediatR;

namespace KesselRun.Web.Api.Messaging.Commands
{
    public class InvalidModelCommand : IRequest<string>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
