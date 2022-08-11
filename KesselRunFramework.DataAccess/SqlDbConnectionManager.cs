using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess
{
    public class SqlDbConnectionManager : ISqlDbConnectionManager
    {
        private string ConnectionString { get; }

        public SqlDbConnectionManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection GetOpenConnection()
        {
            var sqlDbConnection = new SqlConnection(ConnectionString);

            sqlDbConnection.Open();

            return sqlDbConnection;
        }
    }
}
