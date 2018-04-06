using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Sql;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class SqlServiceBuilderExtensions
    {
		private const string cResilienceOptionName = "ResilienceConfig";
		private const string cSecurityOptionName = "SecurityConfig";
		private const string cDataOptionName = "SqlConfig";

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
			//services.Configure<DataResilienceOptions>(cResilienceOptionName, config);
			//services.Configure<DataSecurityOptions>(cSecurityOptionName, config);
			//services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			//services.Configure<SqlDbConnectionOptions>(cDataOptionName, config);
			services.Configure<DataResilienceOptions>(config);
			services.Configure<DataSecurityOptions>(config);
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<SqlDbConnectionOptions>(config);
			services.AddSingleton<DbDataStores<SqlDbConnectionOptions>, DbDataStores<SqlDbConnectionOptions>>();
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
			services.AddSingleton<ShardDataStores<TShard, SqlShardConnectionOptions<TShard>>, ShardDataStores<TShard, SqlShardConnectionOptions<TShard>>>();
			return services;
		}
	}
}
