using System;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string FormatAs(this string source, params object[] paramObjects)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return string.Format(source, paramObjects);
        }
    }
}
