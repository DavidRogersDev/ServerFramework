using System;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NextDay(this DateTime source)
        {
            return source.AddDays(1);
        }
    }
}
