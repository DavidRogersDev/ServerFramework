using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IReadOnlyDictionary<string, IReadOnlyList<string>> ToReadOnlyDictionary(this ValidationResult source, string prefix = null)
        {
            return new ReadOnlyDictionary<string, IReadOnlyList<string>>(source.Errors
                .ToLookup(e => e.PropertyName, e => e.ErrorMessage, StringComparer.Ordinal)
                .ToDictionary(
                    lookupKey => string.IsNullOrEmpty(prefix)
                            ? lookupKey.Key
                            : string.IsNullOrEmpty(lookupKey.Key)
                                ? prefix
                                : prefix + "." + lookupKey.Key,
                    lookupCollection => (IReadOnlyList<string>)lookupCollection.ToList()
                    ));
        }
    }
}
