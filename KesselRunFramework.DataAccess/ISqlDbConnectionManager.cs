using System;
using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess
{
    public interface ISqlDbConnectionManager : IDisposable
    {
        SqlConnection GetOpenConnection();
    }
}
