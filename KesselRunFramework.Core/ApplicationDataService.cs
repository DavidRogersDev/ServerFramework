using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public abstract class ApplicationDataService : IApplicationDataService
    {
        protected ApplicationDataService(IDbResolver dbResolver, IMapper mapper)
        {
            DbResolver = dbResolver;
            Mapper = mapper;
        }

        public IDbResolver DbResolver { get; }
        public IMapper Mapper { get; }
    }
}
