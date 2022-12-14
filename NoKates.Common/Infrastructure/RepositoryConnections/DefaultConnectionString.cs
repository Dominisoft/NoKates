using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.Common.Infrastructure.RepositoryConnections
{
    public interface IConnectionString
    {
        string GetConnectionString();

    }

    public interface IDefaultConnectionString : IConnectionString
    {

    }
    public class DefaultConnectionString : IDefaultConnectionString
    {
        public string GetConnectionString()
            => ConfigurationValues.Values["DefaultConnectionString"];
    }
}
