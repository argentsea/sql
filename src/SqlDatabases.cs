using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
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
