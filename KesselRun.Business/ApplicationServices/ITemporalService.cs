using System.Collections.Generic;

namespace KesselRun.Business.ApplicationServices
{
    public interface ITemporalService
    {
        IReadOnlyList<string> GetTimeZones();
    }
}
