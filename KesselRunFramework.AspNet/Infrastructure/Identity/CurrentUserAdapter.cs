using Microsoft.AspNetCore.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Identity
{
    public class CurrentUserAdapter : ICurrentUser
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly HttpContext _httpContext;

        public CurrentUserAdapter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
        }

        public virtual bool IsAuthenticated => _httpContext.User.Identity.IsAuthenticated;
        public virtual string UserName => _httpContext.User.Identity.Name;
    }
}
