using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using ArgentSea;
using ArgentSea.Sql;

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

			//services.Configure<SqlOptions>(config.GetSection(""));
			services.AddSqlDbConfiguration(config);

			//var someOptions = Options.Create<SampleOptions>(new SampleOptions());
			//var moreOptions = Options.Create(new SampleOptions());

			//Get option
			//var sqlOptions = ServiceProvider.GetServices(typeof(SqlConfig)).First();

		}
	}
}
