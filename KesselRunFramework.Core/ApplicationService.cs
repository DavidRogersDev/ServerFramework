using AutoMapper;

namespace KesselRunFramework.Core
{
    public abstract class ApplicationService : IApplicationService
    {
        protected ApplicationService(IMapper mapper)
        {
            Mapper = mapper;
        }

        public IMapper Mapper { get; }
    }
}
