using System;
using KesselRunFramework.DataAccess.Ops;
using SimpleInjector;

namespace KesselRun.Web.Api.Infrastructure.Ioc
{
    public static class DataAccessRegistry
    {
        public static void RegisterDataAccessComponents(this Container container, string connectionString)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            if (connectionString.Trim().Equals(string.Empty)) throw new ArgumentException(nameof(connectionString) + " cannot be an empty string.");

            container.Register<ISimpleListRetriever, SimpleListRetriever>(Lifestyle.Singleton);
        }
    }
}
