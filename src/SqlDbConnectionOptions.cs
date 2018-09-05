﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This configuration class defines an array of database <see cref="SqlConnectionConfiguration">connection configurations</see>. 
    /// <example>
    /// For example, you might configure your appsettings.json like this:
    /// <code>
    ///   "SqlDbConnections": [
    ///   {
    ///     "DatabaseKey": "MyDatabase",
    ///     "SecurityKey": "SecKey1",
    ///     "DataConnection": {
    ///       "DataSource": "localhost",
    ///       "InitialCatalog": "MyDb"
    ///       }
    ///     }
    ///   ]
    ///</code>
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a DataResilienceKey (if defined) must match a key in the DataResilienceOptions array.
    ///</example>
    /// </summary>
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
