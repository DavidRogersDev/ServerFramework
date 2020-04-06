using System.Collections.Generic;
using System.Linq;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static bool None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}
