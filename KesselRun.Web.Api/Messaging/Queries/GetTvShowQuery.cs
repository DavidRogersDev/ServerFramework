using MediatR;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetTvShowQuery : IRequest<string>
    {
        public int Season { get; set; }
        public string Title { get; set; }
    }
}
