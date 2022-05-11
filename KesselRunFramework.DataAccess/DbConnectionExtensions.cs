using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess
{
    public static class DbConnectionExtensions
    {
        public static IDataReader ExecuteCommandQuery(this SqlConnection source, string query, SqlParameter[] parameters = null)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentException(nameof(query));

            var command = new SqlCommand { Connection = source };

            if (!ReferenceEquals(parameters, null))
                command.Parameters.AddRange(parameters);

            command.CommandText = query;
            command.CommandType = CommandType.Text;

            return command.ExecuteReader();
        }

        public static async Task<IDataReader> ExecuteCommandQueryAsync(this SqlConnection source, string query, CancellationToken cancellationToken, SqlParameter[] parameters = null)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentException(nameof(query));

            var command = new SqlCommand { Connection = source };

            if (!ReferenceEquals(parameters, null))
                command.Parameters.AddRange(parameters);

            command.CommandText = query;
            command.CommandType = CommandType.Text;

            return await command.ExecuteReaderAsync(cancellationToken);
        }
    }
}
