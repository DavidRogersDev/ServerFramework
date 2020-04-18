using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public abstract class ApplicationDataService : IApplicationDataService
    {
        protected ApplicationDataService(IDbFoundary dbFoundary, IMapper mapper)
        {
            DbFoundary = dbFoundary;
            Mapper = mapper;
        }

        public IDbFoundary DbFoundary { get; }
        public IMapper Mapper { get; }
    }
}
