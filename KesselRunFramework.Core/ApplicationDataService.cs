using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public abstract class ApplicationDataService : ApplicationService, IApplicationDataService
    {
        protected ApplicationDataService(IDbResolver dbResolver, IMapper mapper)
            : base(mapper)
        {
            DbResolver = dbResolver;
        }

        public IDbResolver DbResolver { get; }
    }
}
