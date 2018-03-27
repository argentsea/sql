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
        public void TestMethod1()
        {
			//var dtn = new Dictionary<string, string>();
			//dtn.Add("", "");
			//dtn.Add("", "");
			//dtn.Add("", "");
			//dtn.Add("", "");
			//bldr.AddInMemoryCollection(dtn);

			var bldr = new ConfigurationBuilder()
				.AddJsonFile("configurationsettings.json");

			var config = bldr.Build();
			var services = new ServiceCollection();
			services.AddOptions();
			//services.AddSqlDbConfiguration(config);
			services.Configure<DataResilienceOptions>(config);
			services.Configure<DataSecurityOptions>(config);
			services.Configure<SqlDbConnectionOptions>(config);
			services.Configure<SqlShardConnectionOptions<int>>(config);

			var serviceProvider = services.BuildServiceProvider();

			var resilienceOptions = serviceProvider.GetService<IOptions<DataResilienceOptions>>();
			var securityOptions = serviceProvider.GetService<IOptions<DataSecurityOptions>>();
			var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
			var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions<int>>>();

			var securityData = securityOptions.Value;
			securityData.Credentials[0].SecurityKey.Should().Be("One", "that is the first value set in the configurationsettings.json configuration file");
			securityData.Credentials[1].SecurityKey.Should().Be("Two", "that is the second value set in the configurationsettings.json configuration file");

			var resilienceData = resilienceOptions.Value;
			resilienceData.DataResilienceStrategies.Length.Should().Be(2, "there are two strategies defined in configuration file");
			resilienceData.DataResilienceStrategies[0].DataResilienceKey.Should().Be("local", "that is the first key in the configuration file");
			resilienceData.DataResilienceStrategies[1].DataResilienceKey.Should().Be("remote", "that is the second key in the configuration file");

			var sqlDbData = sqlDbOptions.Value;
			sqlDbData.SqlDbConnections.Length.Should().Be(2, "two conections are defined in the configuration file.");

			sqlDbData.SqlDbConnections.Length.Should().Be(2, "two Db conections are defined in the configuration file.");
			sqlDbData.SqlDbConnections[0].DataConnection.GetConnectionString().Should().Contain("MyServer", "the servername should be in the connection string.");
			sqlDbData.SqlDbConnections[1].DataConnection.GetConnectionString().Should().Contain("dbName2", "the database name should be in the connection string.");

			var sqlShardData = sqlShardOptions.Value;
			sqlShardData.SqlShardSets.Length.Should().Be(2, "there are two shard sets defined");

			//var securityOptions = serviceProvider.GetService<IOptions<DataSecurityOptions>>();
			//var connectionOptions = serviceProvider.GetService<IOptions<SqlDbConnectionConfigurationOptions>>();
			//var dataStores = serviceProvider.GetService<DbDataStores>();

		}
	}
}
