using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
	public class SqlDbConnectionOptions : IDbDataConfigurationOptions
	{
		public SqlDbConnectionConfiguration[] SqlDbConnections { get; set; }
		public IDbConnectionConfiguration[] DbConnectionsInternal { get => SqlDbConnections; }

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
