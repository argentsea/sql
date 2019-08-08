// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Sql;
using System.IO;

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
            var global = config.GetSection("SqlGlobalSettings");
            services.Configure<SqlGlobalPropertiesOptions>(global);
			services.Configure<SqlDbConnectionOptions>(config);
            services.AddSingleton<SqlDatabases>();
            services.AddSqlServices(config);
            services.Configure<SqlShardConnectionOptions>(config);
            services.AddSingleton<ShardSetsBase<SqlShardConnectionOptions>, SqlShardSets>();
            return services;
        }
	}
}
