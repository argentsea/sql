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
			services.Configure<DataResilienceConfigurationOptions>(cResilienceOptionName, config.GetSection("Resilience"));
			services.Configure<DataSecurityOptions>(cSecurityOptionName, config.GetSection("DataSecurity"));
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<SqlDbConnectionConfigurationOptions>(cDataOptionName, config.GetSection("DbConnections"));
			//(Logger)
			services.AddSingleton<DbDataStores, DbDataStores>();
			return services;
        }
		public static IServiceCollection AddSqlShardConfiguration<TShard>(
			this IServiceCollection services,
			IConfiguration config
			) where TShard: IComparable
		{
			services.Configure<DataResilienceConfigurationOptions>(cResilienceOptionName, config.GetSection("Resilience"));
			services.Configure<DataSecurityOptions>(cSecurityOptionName, config.GetSection("DataSecurity"));
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<SqlShardConnectionConfigurationOptions<TShard>>(cDataOptionName, config.GetSection("ShardConnections"));
			//(Logger)
			services.AddSingleton<ShardDataStores<TShard>, ShardDataStores<TShard>>();
			return services;
		}
	}
}
