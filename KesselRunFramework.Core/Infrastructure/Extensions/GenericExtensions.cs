using System.Collections.Generic;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class GenericExtensions
    {
        public static T[] InArray<T>(this T item)
        {
            return new[] { item };
        }

        public static IList<T> InList<T>(this T item)
        {
            return new List<T> { item };
        }
    }
}
