// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// A collection of ShardSets.
    /// </summary>
    /// <typeparam name="TShard">The type of the shardId index value.</typeparam>
    public class SqlShardSets<TShard> : ArgentSea.ShardSetsBase<TShard, SqlShardConnectionOptions<TShard>> where TShard : IComparable
	{
		public SqlShardSets(
			IOptions<SqlShardConnectionOptions<TShard>> configOptions,
			IOptions<SqlGlobalPropertiesOptions> globalOptions,
			ILogger<SqlShardSets<TShard>> logger
			) : base(configOptions, new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
