// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This configuration class defines an array of database <see cref="ArgentSea.Sql.SqlConnectionConfiguration" />. 
    /// <example>
    /// For example, you might configure your appsettings.json like this:
    /// <code>
    ///   "SqlDbConnections": [
    ///   {
    ///     "DatabaseKey": "MyDatabase",
    ///     "DataConnection": {
    ///       "UserName": "webUser",
    ///       "Password": "pwd1234",
    ///       "DataSource": "localhost",
    ///       "InitialCatalog": "MyDb"
    ///       }
    ///     }
    ///   ]
    ///</code>
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match a key in the DataResilienceOptions array.
    ///</example>
    /// </summary>
    public class SqlDbConnectionOptions : IDatabaseConfigurationOptions
	{
		public SqlDbConnectionConfiguration[] SqlDbConnections { get; set; }

        public IDatabaseConnectionConfiguration[] DbConnectionsInternal { get => SqlDbConnections; }

	}
	public class SqlDbConnectionConfiguration : SqlConnectionPropertiesBase, IDatabaseConnectionConfiguration
    {
		public string DatabaseKey { get; set; }

        public IDataConnection ReadConnectionInternal { get => ReadConnection; }
        public IDataConnection WriteConnectionInternal { get => WriteConnection; }
        public SqlConnectionConfiguration ReadConnection { get; set; } = new SqlConnectionConfiguration();
        public SqlConnectionConfiguration WriteConnection { get; set; } = new SqlConnectionConfiguration();
    }
}
