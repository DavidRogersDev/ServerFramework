namespace KesselRunFramework.DataAccess
{
    public interface IDbFoundary
    {
        /// <summary>
        /// This method resolves the context (application-specific) and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetContext<T>() where T : class;

        ISqlDbConnectionManager GetDbConnectionManager();
    }
}
