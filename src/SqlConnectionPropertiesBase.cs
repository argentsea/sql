// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System.Data.SqlClient;

namespace ArgentSea.Sql
{

    public abstract class SqlConnectionPropertiesBase : DataConnectionConfigurationBase
    {
        //public event EventHandler<PropertyChangedEventArgs> RaisePropertyChangedEvent;

        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group.
        /// </summary>
        public ApplicationIntent? ApplicationIntent { get; set; }

        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// Defaults to 5 seconds (before retry) if not set.
        /// </summary>
        public int? ConnectTimeout { get; set; }

        /// <summary>
        /// Gets or sets the SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool? Encrypt { get; set; }

        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner { get; set; }

        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog { get; set; }

        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int? LoadBalanceTimeout { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MinPoolSize { get; set; }

        /// <summary>
        /// When true, an application can maintain multiple active result sets (MARS). When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection.
        /// </summary>
        public bool? MultipleActiveResultSets { get; set; }

        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        public bool? MultiSubnetFailover { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
        /// </summary>
        public int? PacketSize { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool? PersistSecurityInfo { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
        /// </summary>
        public bool? Pooling { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
        /// </summary>
        public bool? Replication { get; set; }

        /// <summary>
        /// Gets or sets a string value that indicates how the connection maintains its association with an enlisted System.Transactions transaction.
        /// </summary>
        public string TransactionBinding { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
        /// </summary>
        public bool? TrustServerCertificate { get; set; }

        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects.
        /// </summary>
        public string TypeSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to redirect the connection from the default SQL Server Express instance to a runtime-initiated instance running under the account of the caller.
        /// </summary>
        public bool? UserInstance { get; set; }

        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server.
        /// </summary>
        public string WorkstationID { get; set; }

    }
}
