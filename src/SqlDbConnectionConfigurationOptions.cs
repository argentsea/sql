using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
	public class SqlDbConnectionConfigurationOptions : IDbDataConfigurationOptions
	{
		public SqlDbConnectionConfiguration[] DbConnections { get; set; }
		public IDbConnectionConfiguration[] DbConnectionsInternal { get => DbConnections; }

	}
	public class SqlDbConnectionConfiguration : IDbConnectionConfiguration
	{
		public string DatabaseKey { get; set; }
		public string SecurityKey { get; set; }
		public string DataResilienceKey { get; set; }
		
		public IConnectionConfiguration DataConnectionInternal { get => DataConnection; }
		public SqlConnectionConfiguration DataConnection { get; set; }
	}
}
