using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel.Resolution;
using ArgentSea;
using ArgentSea.Sql;
using FluentAssertions;

namespace ArgentSea.Sql.Test
{
    public class ConfigurationTests
    {
        [Fact]
        public void TestConfigurationOptions()
        {
			var bldr = new ConfigurationBuilder()
				.AddJsonFile("configurationsettings.json");

			var config = bldr.Build();
			var services = new ServiceCollection();
			services.AddOptions();

			services.Configure<SqlGlobalPropertiesOptions>(config.GetSection("SqlGlobalSettings"));
			services.Configure<SqlDbConnectionOptions>(config);
			services.Configure<SqlShardConnectionOptions<int>>(config);

			var serviceProvider = services.BuildServiceProvider();

			var globalOptions = serviceProvider.GetService<IOptions<SqlGlobalPropertiesOptions>>();
			var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
			var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions<int>>>();

			var globalData = globalOptions.Value;
            globalData.RetryCount.Should().Be(15, "that is the first value set in the configurationsettings.json configuration file");

			var sqlDbData = sqlDbOptions.Value;
			sqlDbData.SqlDbConnections.Length.Should().Be(2, "two conections are defined in the configuration file.");
            sqlDbData.SqlDbConnections[0].DataConnection.GetConnectionString().Should().Contain("MyApp", "the the application name should be inherited from a the global value");
            sqlDbData.SqlDbConnections[1].DataConnection.GetConnectionString().Should().Contain("MyOtherApp", "the the application name should be inherited from a the global value");

			var sqlShardData = sqlShardOptions.Value;
			sqlShardData.SqlShardSets.Length.Should().Be(2, "there are two shard sets defined");
		}
		[Fact]
		public void TestServiceBuilder()
		{
			var bldr = new ConfigurationBuilder()
				.AddJsonFile("configurationsettings.json");

			var config = bldr.Build();
			var services = new ServiceCollection();
			services.AddOptions();

			services.AddLogging();
            services.AddSqlServices<byte>(config);

            var serviceProvider = services.BuildServiceProvider();
            var globalOptions = serviceProvider.GetService<IOptions<SqlGlobalPropertiesOptions>>();
			var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
			var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions<byte>>>();
			var dbLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<SqlDatabases>>();

			var dbService = new SqlDatabases(sqlDbOptions, globalOptions, dbLogger);
			dbService.Count.Should().Be(2, "two connections are defined in the configuration file");

			var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Sql.SqlShardSets<byte>>>();

            //var shardService = new ShardSetsBase<byte, SqlShardConnectionOptions<byte>>(sqlShardOptions, securityOptions, resilienceOptions, new DataProviderServiceFactory(), shardLogger);
            var shardService = new ArgentSea.Sql.SqlShardSets<byte>(sqlShardOptions, globalOptions, shardLogger);

            shardService.Count.Should().Be(2, "two shard sets are defined in the configuration file");
			shardService["Inherited"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set1");
			shardService["Explicit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set2");
			shardService["Inherited"][0].Read.ConnectionString.Should().Contain("MyMirror", "the global configuration metadata specifies this failover partner");
			shardService["Explicit"][0].Read.ConnectionString.Should().Contain("rorriMyM", "the configuration file specifies this failover partner");
		}
	}
}
