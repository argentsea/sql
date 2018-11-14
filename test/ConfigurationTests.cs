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
            sqlDbData.SqlDbConnections[0].DataConnectionInternal.SetAmbientConfiguration(globalData, null, null);
            sqlDbData.SqlDbConnections[1].DataConnectionInternal.SetAmbientConfiguration(globalData, null, null);

            sqlDbData.SqlDbConnections[0].DataConnection.GetConnectionString().Should().Be("Data Source=10.10.25.2;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp", "this is the value inherited from global configuration settings");
            sqlDbData.SqlDbConnections[1].DataConnection.GetConnectionString().Should().Be("Data Source=MyOtherServer;Initial Catalog=OtherDb;Connect Timeout=20;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=English", "this is the value inherited from global configuratoin settings");

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
            dbService["MainDb"].ConnectionString.Should().Be("Data Source=10.10.25.2;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp", "this is the value inherited from global configuratoin settings");
            dbService["OtherDb"].ConnectionString.Should().Be("Data Source=MyOtherServer;Initial Catalog=OtherDb;Connect Timeout=20;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=English", "this is the value inherited from global configuratoin settings");
            var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Sql.SqlShardSets<byte>>>();

            var shardService = new ArgentSea.Sql.SqlShardSets<byte>(sqlShardOptions, globalOptions, shardLogger);

            shardService.Count.Should().Be(2, "two shard sets are defined in the configuration file");
			shardService["Inherit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set1");
			shardService["Explicit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set2");

            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp", "the configuration file builds this connection string");
            shardService["Inherit"][0].Write.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp", "the configuration file builds this connection string");
            shardService["Inherit"][1].Read.ConnectionString.Should().Be("Data Source=MyOtherServer;Initial Catalog=dbName4;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadOnly", "the configuration file builds this connection string");
            shardService["Inherit"][1].Write.ConnectionString.Should().Be("Data Source=MyOtherOtherServer;Initial Catalog=dbName3;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadWrite", "the configuration file builds this connection string");
            shardService["Explicit"][0].Read.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=;Initial Catalog=MyDb1;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp2;Current Language=english;Workstation ID=;ApplicationIntent=ReadWrite;MultiSubnetFailover=True", "the configuration file builds this connection string");
            shardService["Explicit"][0].Write.ConnectionString.Should().Be(shardService["Explicit"][0].Read.ConnectionString, "the read and write connections should be identical");
            shardService["Explicit"][1].Read.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=rorriMyM;Initial Catalog=MyDb2;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp3;Current Language=english;Workstation ID=;ApplicationIntent=ReadOnly;MultiSubnetFailover=True", "the configuration file builds this connection string");
            shardService["Explicit"][1].Write.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=;Initial Catalog=MyDb3;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp4;Current Language=english;Workstation ID=;ApplicationIntent=ReadWrite;MultiSubnetFailover=True", "the configuration file builds this connection string");
        }
    }
}
