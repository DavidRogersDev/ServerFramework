using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.DataAccess.Domain;
using Microsoft.Data.SqlClient;

namespace KesselRunFramework.DataAccess.Ops
{
    public class SimpleListRetriever : ISimpleListRetriever
    {
        private readonly IDbResolver _dbResolver;

        public SimpleListRetriever(IDbResolver dbResolver)
        {
            _dbResolver = dbResolver;
        }

        public IEnumerable<ISimpleListItem> GetSimpleList(string query, SqlParameter[] parameters = null, bool? withNullCheck = null)
        {
            var simpleList = new HashSet<ISimpleListItem>();
            using (var conn = _dbResolver.GetDbConnectionManager().GetOpenConnection())
            using (var dataReader = conn.ExecuteCommandQuery(query, parameters))
            {
                if (withNullCheck.HasValue && withNullCheck.Value)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader.IsDBNull(0) || dataReader.IsDBNull(1))
                            continue;

                        simpleList.Add(new SimpleListItemObject
                        {
                            Id = dataReader.GetInt32(0), 
                            Name = dataReader.GetString(1)
                        });
                    }
                }
                else
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
            }

            return simpleList;
        }


        public async Task<IEnumerable<ISimpleListItem>> GetSimpleListAsync(string query, CancellationToken cancellationToken, SqlParameter[] parameters = null, bool? withNullCheck = null)
        {
            var simpleList = new HashSet<ISimpleListItem>();
            using (var conn = _dbResolver.GetDbConnectionManager().GetOpenConnection())
            using (var dataReader = await conn.ExecuteCommandQueryAsync(query, cancellationToken, parameters))
            {
                if (withNullCheck.HasValue && withNullCheck.Value)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader.IsDBNull(0) || dataReader.IsDBNull(1))
                            continue;

                        simpleList.Add(new SimpleListItemObject
                        {
                            Id = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1)
                        });
                    }
                }
                else
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
            }

            return simpleList;
        }
    }
}
