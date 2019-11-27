using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Dac;

namespace DacShipper
{

    public interface IDacServicesFactory
    {
        DacServices Create(string serverName, string databaseName);
    }

    public class DacServicesFactory : IDacServicesFactory
    {
        private readonly DatabaseOptions _dbConfig;
        private readonly IUniversalAuthProvider _authProvider;

        public DacServicesFactory(IOptions<DatabaseOptions> dbConfig, IUniversalAuthProvider authProvider)
        {
            _dbConfig = dbConfig.Value;
            _authProvider = authProvider;
        }
        public DacServices Create(string serverName, string databaseName)
        {
            var connectionString = _dbConfig.BuildConnectionString(serverName, databaseName);

            if (_dbConfig.UseManagedIdentity)
            {
                return new DacServices(connectionString, _authProvider);
            }

            return new DacServices(connectionString);
        }
    }
}
