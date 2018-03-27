using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Sql;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class SqlDataServiceBuilderExtensions
    {
		private const string cResilienceOptionName = "ResilienceConfig";
		private const string cSecurityOptionName = "SecurityConfig";
		private const string cDataOptionName = "SqlConfig";

		public static IServiceCollection AddSqlDbConfiguration(
            this IServiceCollection services,
            IConfiguration config
            ) 
        {
			services.Configure<DataResilienceOptions>(cResilienceOptionName, config);
			services.Configure<DataSecurityOptions>(cSecurityOptionName, config);
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<SqlDbConnectionOptions>(cDataOptionName, config);
			//(Logger)
			services.AddSingleton<DbDataStores, DbDataStores>();
			return services;
        }
		public static IServiceCollection AddSqlDbShardConfiguration<TShard>(
			this IServiceCollection services,
			IConfiguration config
			) where TShard: IComparable
		{
			services.Configure<DataResilienceOptions>(cResilienceOptionName, config);
			services.Configure<DataSecurityOptions>(cSecurityOptionName, config);
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<SqlDbConnectionOptions>(cDataOptionName, config);
			services.Configure<SqlShardConnectionOptions<TShard>>(cDataOptionName, config);
			//(Logger)
			services.AddSingleton<ShardDataStores<TShard>, ShardDataStores<TShard>>();
			return services;
		}
	}
}
