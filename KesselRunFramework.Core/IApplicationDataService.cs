using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public interface IApplicationDataService
    {
        IDbFoundary DbFoundary { get; }
    }
}
