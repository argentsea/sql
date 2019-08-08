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
			services.Configure<SqlShardConnectionOptions>(config);

			var serviceProvider = services.BuildServiceProvider();

			var globalOptions = serviceProvider.GetService<IOptions<SqlGlobalPropertiesOptions>>();
			var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
			var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions>>();

			var globalData = globalOptions.Value;
            globalData.RetryCount.Should().Be(15, "that is the first value set in the configurationsettings.json configuration file");

			var sqlDbData = sqlDbOptions.Value;
            sqlDbData.SqlDbConnections.Length.Should().Be(2, "two conections are defined in the configuration file.");
            sqlDbData.SqlDbConnections[0].ReadConnectionInternal.SetAmbientConfiguration(globalData, null, null, sqlDbData.SqlDbConnections[0]);
            sqlDbData.SqlDbConnections[0].WriteConnectionInternal.SetAmbientConfiguration(globalData, null, null, sqlDbData.SqlDbConnections[0]);
            sqlDbData.SqlDbConnections[1].ReadConnectionInternal.SetAmbientConfiguration(globalData, null, null, sqlDbData.SqlDbConnections[1]);
            sqlDbData.SqlDbConnections[1].WriteConnectionInternal.SetAmbientConfiguration(globalData, null, null, sqlDbData.SqlDbConnections[1]);

            sqlDbData.SqlDbConnections[0].ReadConnection.GetConnectionString(null).Should().Be("Data Source=10.10.25.1;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;ConnectRetryCount=0", "this is the value inherited from global configuration settings");
            sqlDbData.SqlDbConnections[0].WriteConnection.GetConnectionString(null).Should().Be("Data Source=10.10.25.5;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;ConnectRetryCount=0", "this is the value inherited from global configuration settings");
            sqlDbData.SqlDbConnections[1].ReadConnection.GetConnectionString(null).Should().Be("Data Source=MyOtherServer;Initial Catalog=OtherDb;Connect Timeout=20;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=English;ConnectRetryCount=0");
            sqlDbData.SqlDbConnections[1].WriteConnection.GetConnectionString(null).Should().Be("Data Source=MyOtherServer;Initial Catalog=OtherDb;Connect Timeout=20;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=English;ConnectRetryCount=0", "this is the value inherited from global configuration settings");

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
            services.AddSqlServices(config);

            var serviceProvider = services.BuildServiceProvider();
            var globalOptions = serviceProvider.GetService<IOptions<SqlGlobalPropertiesOptions>>();
			var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
			var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions>>();
			var dbLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<SqlDatabases>>();

			var dbService = new SqlDatabases(sqlDbOptions, globalOptions, dbLogger);
			dbService.Count.Should().Be(2, "two connections are defined in the configuration file");
            dbService["MainDb"].Read.ConnectionString.Should().Be("Data Source=10.10.25.1;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            dbService["MainDb"].Write.ConnectionString.Should().Be("Data Source=10.10.25.5;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            dbService["OtherDb"].Read.ConnectionString.Should().Be("Data Source=MyOtherServer;Initial Catalog=OtherDb;Connect Timeout=20;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=English;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Sql.SqlShardSets>>();

            var shardService = new ArgentSea.Sql.SqlShardSets(sqlShardOptions, globalOptions, shardLogger);

            shardService.Count.Should().Be(2, "two shard sets are defined in the configuration file");
			shardService["Inherit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set1");
			shardService["Explicit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set2");

            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Inherit"][0].Write.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadWrite;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Inherit"][1].Read.ConnectionString.Should().Be("Data Source=MyOtherServer;Initial Catalog=dbName4;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Inherit"][1].Write.ConnectionString.Should().Be("Data Source=MyOtherOtherServer;Initial Catalog=dbName3;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadWrite;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Explicit"][0].Read.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=;Initial Catalog=MyDb1;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp2;Current Language=english;Workstation ID=;ApplicationIntent=ReadWrite;MultiSubnetFailover=True;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Explicit"][0].Write.ConnectionString.Should().Be(shardService["Explicit"][0].Read.ConnectionString, "the read and write connections should be identical");
            shardService["Explicit"][1].Read.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=rorriMyM;Initial Catalog=MyDb2;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp3;Current Language=english;Workstation ID=;ApplicationIntent=ReadOnly;MultiSubnetFailover=True;ConnectRetryCount=0", "the configuration file builds this connection string");
            shardService["Explicit"][1].Write.ConnectionString.Should().Be("Data Source=localhost;Failover Partner=;Initial Catalog=MyDb3;Integrated Security=False;Persist Security Info=False;User ID=;Password=;Pooling=True;Min Pool Size=0;Max Pool Size=100;MultipleActiveResultSets=False;Replication=False;Connect Timeout=2;Encrypt=False;TrustServerCertificate=True;Load Balance Timeout=0;Packet Size=8000;Type System Version=Latest;Application Name=MyWebApp4;Current Language=english;Workstation ID=;ApplicationIntent=ReadWrite;MultiSubnetFailover=True;ConnectRetryCount=0", "the configuration file builds this connection string");
        }
        [Fact]
        public void TestConfigChange()
        {
            var bldr = new ConfigurationBuilder()
                .AddJsonFile("configurationsettings.json");

            var config = bldr.Build();
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddLogging();
            services.AddSqlServices(config);

            var serviceProvider = services.BuildServiceProvider();
            var globalOptions = serviceProvider.GetService<IOptions<SqlGlobalPropertiesOptions>>();
            var sqlDbOptions = serviceProvider.GetService<IOptions<SqlDbConnectionOptions>>();
            var sqlShardOptions = serviceProvider.GetService<IOptions<SqlShardConnectionOptions>>();
            var dbLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<SqlDatabases>>();

            var dbService = new SqlDatabases(sqlDbOptions, globalOptions, dbLogger);
            var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Sql.SqlShardSets>>();
            var shardService = new ArgentSea.Sql.SqlShardSets(sqlShardOptions, globalOptions, shardLogger);

            dbService["MainDb"].Read.ConnectionString.Should().Be("Data Source=10.10.25.1;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");

            globalOptions.Value.CurrentLanguage = "Pigeon";
            dbService["MainDb"].Read.ConnectionString.Should().Be("Data Source=10.10.25.1;Initial Catalog=MainDb;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyApp;Current Language=Pigeon;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=Pigeon;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");

            sqlDbOptions.Value.SqlDbConnections[0].PacketSize = 16384;
            sqlShardOptions.Value.SqlShardSets[0].PacketSize = 16384;
            dbService["MainDb"].Read.ConnectionString.Should().Be("Data Source=10.10.25.1;Initial Catalog=MainDb;Connect Timeout=5;Packet Size=16384;Type System Version=\"SQL Server 2012\";Application Name=MyApp;Current Language=Pigeon;ConnectRetryCount=0", "this is the value inherited from global configuratoin settings");
            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Packet Size=16384;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=Pigeon;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");
            sqlShardOptions.Value.SqlShardSets[0].PacketSize = 4096;
            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Data Source=10.10.23.20;Failover Partner=MyMirror;Initial Catalog=dbName2;Connect Timeout=5;Packet Size=4096;Type System Version=\"SQL Server 2012\";Application Name=MyOtherApp;Current Language=Pigeon;ApplicationIntent=ReadOnly;ConnectRetryCount=0", "the configuration file builds this connection string");

        }
    }
}
