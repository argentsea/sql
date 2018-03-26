using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ArgentSea;
using System.Collections;
using System.Data.SqlClient;


namespace ArgentSea.Sql
{
    public class SqlConnectionConfiguration : IConnectionConfiguration
    {

        private readonly SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

        public void SetSecurity(SecurityConfiguration security)
        {
            if (security.WindowsAuth)
            {
                this.csb.IntegratedSecurity = true;
            }
            else
            {
                this.csb.UserID = security.UserName;
                this.csb.Password = security.Password;
                this.csb.IntegratedSecurity = false;
            }
        }

        public string ConnectionDescription
        {
            get => $"database {this.csb.InitialCatalog} on server {this.csb.DataSource}";
        }

        public string GetConnectionString()
            => this.csb.ToString();





        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group.
        /// </summary>
        public ApplicationIntent ApplicationIntent
        {
            get => this.csb.ApplicationIntent;
            set => this.csb.ApplicationIntent = value;
        }
        
        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get => this.csb.ApplicationName;
            set => this.csb.ApplicationName = value;
        }
        /// <summary>
        /// The number of reconnections attempted after identifying that there was an idle connection failure. This must be an integer between 0 and 255. Default is 1. Set to 0 to disable reconnecting on idle connection failures.
        /// </summary>
        public int ConnectRetryCount
        {
            get => this.csb.ConnectRetryCount;
            set => this.csb.ConnectRetryCount = value;
        }
        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure. This must be an integer between 1 and 60. The default is 10 seconds.
        /// </summary>
        public int ConnectRetryInterval
        {
            get => this.csb.ConnectRetryInterval;
            set => this.csb.ConnectRetryInterval = value;
        }
        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectTimeout
        {
            get => this.csb.ConnectTimeout;
            set => this.csb.ConnectTimeout = value;
        }
        /// <summary>
        /// Gets or sets the SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage
        {
            get => this.csb.CurrentLanguage;
            set => this.csb.CurrentLanguage = value;
        }
        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource
        {
            get => this.csb.DataSource;
            set => this.csb.DataSource = value;
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool Encrypt
        {
            get => this.csb.Encrypt;
            set => this.csb.Encrypt = value;
        }
        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get => this.csb.FailoverPartner;
            set => this.csb.FailoverPartner = value;
        }
        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog
        {
            get => this.csb.InitialCatalog;
            set => this.csb.InitialCatalog = value;
        }
        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int LoadBalanceTimeout
        {
            get => this.csb.LoadBalanceTimeout;
            set => this.csb.LoadBalanceTimeout = value;
        }
        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int MaxPoolSize
        {
            get => this.csb.MaxPoolSize;
            set => this.csb.MaxPoolSize = value;
        }
        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int MinPoolSize
        {
            get => this.csb.MinPoolSize;
            set => this.csb.MinPoolSize = value;
        }
        /// <summary>
        /// When true, an application can maintain multiple active result sets (MARS). When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection.
        /// </summary>
        public bool MultipleActiveResultSets
        {
            get => this.csb.MultipleActiveResultSets;
            set => this.csb.MultipleActiveResultSets = value;
        }
        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        public bool MultiSubnetFailover
        {
            get => this.csb.MultiSubnetFailover;
            set => this.csb.MultiSubnetFailover = value;
        }
        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
        /// </summary>
        public int PacketSize
        {
            get => this.csb.PacketSize;
            set => this.csb.PacketSize = value;
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool PersistSecurityInfo
        {
            get => this.csb.PersistSecurityInfo;
            set => this.csb.PersistSecurityInfo = value;
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
        /// </summary>
        public bool Pooling
        {
            get => this.csb.Pooling;
            set => this.csb.Pooling = value;
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
        /// </summary>
        public bool Replication
        {
            get => this.csb.Replication;
            set => this.csb.Replication = value;
        }
        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
        /// </summary>
        public bool TrustServerCertificate
        {
            get => this.csb.TrustServerCertificate;
            set => this.csb.TrustServerCertificate = value;
        }
        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects.
        /// </summary>
        public string TypeSystemVersion
        {
            get => this.csb.TypeSystemVersion;
            set => this.csb.TypeSystemVersion = value;
        }
        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server.
        /// </summary>
        public string WorkstationID
        {
            get => this.csb.WorkstationID;
            set => this.csb.WorkstationID = value;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        public object this[string key]
        {
            get => this.csb[key];
            set => this.csb[key] = value;
        }

        /// <summary>
        /// Gets an ICollection{string} containing the keys of this configuration.
        /// </summary>
        public ICollection<string> Keys
        {
            get => this.Keys;
        }

        /// <summary>
        /// Gets an ICollection{string} containing the values in this configuration.
        /// </summary>
        public ICollection Values
        {
            get => this.csb.Values;
        }
        /// <summary>
        /// Adds an item to the configuration
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object> item)
        {
            this.csb.Add(item.Key, item.Value);
        }
        /// <summary>
        /// Determines whether the configuration contains a specific key.
        /// </summary>
        public bool ContainsKey(string key)
        {
            return this.csb.ContainsKey(key);
        }

        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(KeyValuePair<string, object> item)
        {
            this.csb.Remove(item.Key);
        }

        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(string key)
        {
            this.csb.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.csb.TryGetValue(key, out value);
        }
    }
}
