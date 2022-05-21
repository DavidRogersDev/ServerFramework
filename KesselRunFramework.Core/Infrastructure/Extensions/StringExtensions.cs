using System;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string FormatAs(this string source, params object[] paramObjects)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            if (ReferenceEquals(paramObjects, null)) throw new ArgumentNullException(nameof(paramObjects));

            return string.Format(source, paramObjects);
        }

        public static T ParseEnum<T>(this string source, bool? ignoreCase = null)
            where T : struct
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));

            return ignoreCase.HasValue
                ? (T)Enum.Parse(typeof(T), source, ignoreCase.Value)
                : (T)Enum.Parse(typeof(T), source);
        }
    }
}
