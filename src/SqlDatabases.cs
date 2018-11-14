// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class manages the non-sharded SQL database connections.
    /// </summary>
	public class SqlDatabases : DatabasesBase<SqlDbConnectionOptions>
	{
		public SqlDatabases(
			IOptions<SqlDbConnectionOptions> configOptions,
            IOptions<SqlGlobalPropertiesOptions> globalOptions,
            ILogger<SqlDatabases> logger
			) : base(configOptions, (IDataProviderServiceFactory)new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
