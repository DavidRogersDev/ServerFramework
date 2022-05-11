using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace KesselRunFramework.DataAccess.Infrastructure
{
    public interface IAdoNetUtilities
    {
        string AddParametersToCommand(
            SqlCommand sqlCommand,
            IEnumerable<int> ids,
            string paramPrefix = null
            );
    }
}
