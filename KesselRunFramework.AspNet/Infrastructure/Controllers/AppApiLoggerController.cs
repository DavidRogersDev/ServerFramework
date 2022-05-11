using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.Controllers
{
    public class AppApiLoggerController : AppApiController
    {
        protected readonly ILogger _logger;

        public AppApiLoggerController(ICurrentUser currentUser, ILogger logger)
            : base(currentUser)
        {
            _logger = logger;
        }
    }
}
