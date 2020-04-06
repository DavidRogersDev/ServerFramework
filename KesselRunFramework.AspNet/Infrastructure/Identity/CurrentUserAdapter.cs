using Microsoft.AspNetCore.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Identity
{
    public class CurrentUserAdapter : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public CurrentUserAdapter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
        }
        public bool IsAuthenticated => _httpContext.User.Identity.IsAuthenticated;
        public string UserName => _httpContext.User.Identity.Name;
    }
}
