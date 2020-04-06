using System.Collections.Generic;
using System.Data.Common;
using KesselRunFramework.DataAccess.Domain;

namespace KesselRunFramework.DataAccess.Ops
{
    public class SimpleListRetriever : ISimpleListRetriever
    {
        private readonly IDbFoundary _dbFoundary;

        public SimpleListRetriever(IDbFoundary dbFoundary)
        {
            _dbFoundary = dbFoundary;
        }

        public IEnumerable<ISimpleListItem> GetSimpleList(DbParameter [] parameters, string query)
        {
            var simpleList = new HashSet<ISimpleListItem>();
            
            using (var dataReader = _dbFoundary.GetDbConnectionManager().GetOpenConnection().ExecuteCommandQuery(parameters, query))
            {
                while (dataReader.Read())
                {
                    simpleList.Add(new SimpleListItemObject
                    {
                        Id = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1)
                    });
                }
            }

            return simpleList;
        }
    }
}
