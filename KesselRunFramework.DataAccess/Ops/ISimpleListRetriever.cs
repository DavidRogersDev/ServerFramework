using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.DataAccess.Domain;
using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess.Ops
{
    public interface ISimpleListRetriever
    {
        IEnumerable<ISimpleListItem> GetSimpleList(
            string query,
            SqlParameter[] parameters = null,
            bool? withNullCheck = null
            );
        Task<IEnumerable<ISimpleListItem>> GetSimpleListAsync(
            string query, 
            CancellationToken cancellationToken, 
            SqlParameter[] parameters = null,
            bool? withNullCheck = null
            );
    }
}
