using System;
using System.Collections.Generic;
using System.Linq;

namespace KesselRun.Business.ApplicationServices
{
    public class TemporalService : ITemporalService
    {
        public IReadOnlyList<string> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(tz => tz.DisplayName)
                .ToList();
        }
    }
}
