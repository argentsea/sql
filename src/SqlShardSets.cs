using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This configuration class contains the configuration information for a shard set.
    /// </summary>
    /// <typeparam name="TShard"></typeparam>
    public class SqlShardSets<TShard> : ArgentSea.ShardDataStores<TShard, SqlShardConnectionOptions<TShard>> where TShard : IComparable
	{
		public SqlShardSets(
			IOptions<SqlShardConnectionOptions<TShard>> configOptions,
			IOptions<DataSecurityOptions> securityOptions,
			IOptions<DataResilienceOptions> resilienceStrategiesOptions,
			ILogger<SqlShardSets<TShard>> logger
			) : base(configOptions, securityOptions, resilienceStrategiesOptions, new DataProviderServiceFactory(), logger)
		{

		}
	}
}
