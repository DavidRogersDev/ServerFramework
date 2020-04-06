using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public abstract class ApplicationService : IApplicationService
    {
        protected ApplicationService(IMapper mapper, IDbFoundary dbFoundary)
        {
            Mapper = mapper;
            DbFoundary = dbFoundary;
        }

        public IDbFoundary DbFoundary { get; }

        public IMapper Mapper { get; }
    }
}
