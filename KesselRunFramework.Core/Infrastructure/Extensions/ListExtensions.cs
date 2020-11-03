using System.Collections.Generic;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static void Add<T>(this List<T> source, IEnumerable<T> items)
        {
            source.AddRange(items);
        }
    }
}
