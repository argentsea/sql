using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class manages the non-sharded SQL database connections.
    /// </summary>
	public class SqlDatabases : DbDataStores<SqlDbConnectionOptions>
	{
		public SqlDatabases(
			IOptions<SqlDbConnectionOptions> configOptions,
			IOptions<DataSecurityOptions> securityOptions,
			IOptions<DataResilienceOptions> resilienceStrategiesOptions,
			ILogger<SqlDatabases> logger
			) : base(configOptions, securityOptions, resilienceStrategiesOptions, (IDataProviderServiceFactory)new DataProviderServiceFactory(), logger)
		{

		}
	}
}
