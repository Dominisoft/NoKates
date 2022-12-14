using System.Linq;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Repositories;
using NoKates.Common.Models;

namespace NoKates.Common.Infrastructure.Helpers
{
    public static class RepositoryHelper
    {
        public static ISqlRepository<TEntity> CreateRepository<TEntity>() where TEntity:Entity,new()
        {
            var defaultConnectionName = GetDefaultConnectionName<TEntity>();
            var variableName = $"{defaultConnectionName}ConnectionString";
            var hasConnection = ConfigurationValues.TryGetValue(out var connectionString, variableName);
            if (!hasConnection)
                throw new System.Exception($"No Connection String Defined: {variableName}");

            return new SqlRepository<TEntity>(connectionString);
        }

        private static string GetDefaultConnectionName<TEntity>() where TEntity : Entity
        {
            var type = typeof(TEntity);
            var attributes = type.GetCustomAttributes(true);
            var defaultConnectionAttribute = (DefaultConnectionString) attributes.FirstOrDefault(a => a.GetType() == typeof(DefaultConnectionString));
            return defaultConnectionAttribute?.Name ?? "Default";
        }
    }
}
