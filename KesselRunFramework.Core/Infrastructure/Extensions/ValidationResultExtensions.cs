using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IDictionary<string, IEnumerable<string>> ToDictionary(this ValidationResult source, string prefix = null)
        {
            return source.Errors
                .ToLookup(e => e.PropertyName, e => e.ErrorMessage, StringComparer.Ordinal)
                .ToDictionary(
                    lookupKey => string.IsNullOrEmpty(prefix)
                            ? lookupKey.Key
                            : string.IsNullOrEmpty(lookupKey.Key)
                                ? prefix
                                : prefix + "." + lookupKey.Key,
                    lookupCollection => lookupCollection.AsEnumerable()
                );
        }
    }
}
