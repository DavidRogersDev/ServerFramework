using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KesselRunFramework.DataAccess.Infrastructure
{
    public class AdoNetUtilities : IAdoNetUtilities
    {
        public string AddParametersToCommand(SqlCommand sqlCommand, IEnumerable<int> ids, string paramPrefix = null)
        {
            if (ReferenceEquals(sqlCommand, null)) throw new ArgumentNullException(nameof(sqlCommand));
            if (ReferenceEquals(ids, null)) throw new ArgumentNullException(nameof(ids));

            var idsArray = ids as int[] ?? ids.ToArray(); // small perf measure

            if (idsArray.Length == 0)
                return string.Empty;

            var inClause = new StringBuilder();

            var prefix = paramPrefix ?? "@p";

            for (var i = 0; i < idsArray.Length; i++)
            {
                var paramName = string.Concat(prefix, i.ToString());

                sqlCommand.Parameters.AddWithValue(paramName, idsArray[i]);

                inClause.Append(string.Concat(paramName, ","));
            }

            return inClause.ToString().TrimEnd(',');
        }
    }
}
