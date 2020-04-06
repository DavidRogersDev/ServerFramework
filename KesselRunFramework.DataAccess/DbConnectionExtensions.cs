using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess
{
    public static class DbConnectionExtensions
    {
        public static IDataReader ExecuteCommandQuery(this SqlConnection source, DbParameter[] parameters, string query)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentException(nameof(query));

            var command = new SqlCommand { Connection = source };
            command.Parameters.AddRange(parameters);
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            return command.ExecuteReader();
        }
    }
}
