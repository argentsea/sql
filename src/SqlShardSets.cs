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
    public class SqlShardSets : ArgentSea.ShardSetsBase<SqlShardConnectionOptions>
	{
		public SqlShardSets(
			IOptions<SqlShardConnectionOptions> configOptions,
			IOptions<SqlGlobalPropertiesOptions> globalOptions,
			ILogger<SqlShardSets> logger
			) : base(configOptions, new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
