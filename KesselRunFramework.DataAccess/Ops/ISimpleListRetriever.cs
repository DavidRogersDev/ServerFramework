using System.Collections.Generic;
using System.Data.Common;
using KesselRunFramework.DataAccess.Domain;

namespace KesselRunFramework.DataAccess.Ops
{
    public interface ISimpleListRetriever
    {
        IEnumerable<ISimpleListItem> GetSimpleList(DbParameter[] parameters, string query);
    }
}
