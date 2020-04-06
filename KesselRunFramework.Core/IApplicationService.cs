using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public interface IApplicationService
    {
        IDbFoundary DbFoundary { get; }
        IMapper Mapper { get; }
    }
}
