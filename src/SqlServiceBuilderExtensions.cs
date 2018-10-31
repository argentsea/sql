// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This static class adds the injectable SQL data services into the services collection.
    /// </summary>
    public static class SqlServiceBuilderExtensions
    {
		/// <summary>
		/// Loads configuration into injectable Options and the DbDataStores service. This overload does not load ShardSets.  ILogger service should have already be created.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IServiceCollection AddSqlServices(
            this IServiceCollection services,
            IConfiguration config
            ) 
        {
			// even if these configs are already set by another provider, this will just overwrite with the same values.
			services.Configure<DataResilienceOptions>(config);
			services.Configure<DataSecurityOptions>(config);
			services.Configure<SqlDbConnectionOptions>(config);
			services.AddSingleton<SqlDatabases>();
			return services;
        }

		/// <summary>
		/// Loads configuration into injectable Options and the DbDataStores and ShardDataStores services. ILogger service should have already be created.
		/// </summary>
		/// <typeparam name="TShard"></typeparam>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IServiceCollection AddSqlServices<TShard>(
			this IServiceCollection services,
			IConfiguration config
			) where TShard: IComparable
		{
			services.AddSqlServices(config);
			//services.Configure<SqlShardConnectionOptions<TShard>>(cDataOptionName, config);
			services.Configure<SqlShardConnectionOptions<TShard>>(config);
			services.AddSingleton<ShardSetsBase<TShard, SqlShardConnectionOptions<TShard>>, SqlShardSets<TShard>>();
			return services;
		}
	}
}
