// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// A collection of ShardSets (with a byte shardId type).
    /// </summary>
    public class SqlShardSets : ArgentSea.ShardSetsBase<byte, SqlShardConnectionOptions<byte>>
    {
        public SqlShardSets(
            IOptions<SqlShardConnectionOptions<byte>> configOptions,
            IOptions<DataSecurityOptions> securityOptions,
            IOptions<DataResilienceOptions> resilienceStrategiesOptions,
            ILogger<SqlShardSets<byte>> logger
            ) : base(configOptions, securityOptions, resilienceStrategiesOptions, new DataProviderServiceFactory(), logger)
        {

        }
    }

    /// <summary>
    /// A collection of ShardSets.
    /// </summary>
    /// <typeparam name="TShard">The type of the shardId index value.</typeparam>
    public class SqlShardSets<TShard> : ArgentSea.ShardSetsBase<TShard, SqlShardConnectionOptions<TShard>> where TShard : IComparable
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
