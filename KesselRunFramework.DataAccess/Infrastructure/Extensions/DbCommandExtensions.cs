using Microsoft.Data.SqlClient;
using System.Data;

namespace KesselRunFramework.DataAccess.Infrastructure.Extensions
{
    public static class DbCommandExtensions
    {
        public static void AddIdParameter(this SqlCommand command, string paramName, int id)
        {
            command.Parameters.Add(paramName, SqlDbType.Int).Value = id;
        }
    }
}
