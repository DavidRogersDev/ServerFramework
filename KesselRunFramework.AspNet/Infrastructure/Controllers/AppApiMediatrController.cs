using MediatR;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.Controllers
{
    public class AppApiMediatrController : AppApiLoggerController
    {
        protected readonly IMediator _mediator;

        public AppApiMediatrController(ICurrentUser currentUser, ILogger logger, IMediator mediator)
            : base(currentUser, logger)
        {
            _mediator = mediator;
        }
    }
}
