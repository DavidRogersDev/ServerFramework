using AutoMapper;
using KesselRunFramework.DataAccess;

namespace KesselRunFramework.Core
{
    public interface IApplicationDataService
    {
        IDbResolver DbResolver { get; }
    }
}
