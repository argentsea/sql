using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ArgentSea;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Immutable;
using Microsoft.Extensions.Options;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class represents is a (non-sharded) database connection.
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match as key in the DataResilienceOptions array.
    /// If the ResilienceKey is not defined, a default data resilience strategy will be used.
    /// </summary>
    public class SqlConnectionConfiguration : DataConnectionConfiguration
    {
         
        private readonly SqlConnectionStringBuilder _csb = new SqlConnectionStringBuilder();
        private string _connectionString = null;

        public override string ConnectionDescription
        {
            get => $"database {this._csb.InitialCatalog} on server {this._csb.DataSource}";
        }

        public override string GetConnectionString()
        {
            if (hasConnectionPropertyChanged && string.IsNullOrEmpty(_connectionString))
            {

                var security = base.GetSecurityConfiguration();
                if (!(security is null))
                {
                    if (security.WindowsAuth)
                    {
                        this._csb.IntegratedSecurity = true;
                    }
                    else
                    {
                        this._csb.UserID = security.UserName;
                        this._csb.Password = security.Password;
                        this._csb.IntegratedSecurity = false;
                    }
                }
                _connectionString = _csb.ToString();
            }
            return _connectionString;
        }

        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group.
        /// </summary>
        public ApplicationIntent ApplicationIntent
        {
            get => this._csb.ApplicationIntent;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ApplicationIntent = value;
            }
        }
        
        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get => this._csb.ApplicationName;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ApplicationName = value;
            }
        }
        /// <summary>
        /// The number of reconnections attempted after identifying that there was an idle connection failure. This must be an integer between 0 and 255. Default is 1. Set to 0 to disable reconnecting on idle connection failures.
        /// </summary>
        public int ConnectRetryCount
        {
            get => this._csb.ConnectRetryCount;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConnectRetryCount = value;
            }
        }
        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure. This must be an integer between 1 and 60. The default is 10 seconds.
        /// </summary>
        public int ConnectRetryInterval
        {
            get => this._csb.ConnectRetryInterval;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConnectRetryInterval = value;
            }
        }
        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectTimeout
        {
            get => this._csb.ConnectTimeout;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConnectTimeout = value;
            }
        }
        /// <summary>
        /// Gets or sets the SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage
        {
            get => this._csb.CurrentLanguage;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.CurrentLanguage = value;
            }
        }
        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource
        {
            get => this._csb.DataSource;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.DataSource = value;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool Encrypt
        {
            get => this._csb.Encrypt;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Encrypt = value;
            }
        }
        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get => this._csb.FailoverPartner;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.FailoverPartner = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog
        {
            get => this._csb.InitialCatalog;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.InitialCatalog = value;
            }
        }
        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int LoadBalanceTimeout
        {
            get => this._csb.LoadBalanceTimeout;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.LoadBalanceTimeout = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int MaxPoolSize
        {
            get => this._csb.MaxPoolSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MaxPoolSize = value;
            }
        }
        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int MinPoolSize
        {
            get => this._csb.MinPoolSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MinPoolSize = value;
            }
        }
        /// <summary>
        /// When true, an application can maintain multiple active result sets (MARS). When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection.
        /// </summary>
        public bool MultipleActiveResultSets
        {
            get => this._csb.MultipleActiveResultSets;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MultipleActiveResultSets = value;
            }
        }
        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        public bool MultiSubnetFailover
        {
            get => this._csb.MultiSubnetFailover;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MultiSubnetFailover = value;
            }
        }
        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
        /// </summary>
        public int PacketSize
        {
            get => this._csb.PacketSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.PacketSize = value;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool PersistSecurityInfo
        {
            get => this._csb.PersistSecurityInfo;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.PersistSecurityInfo = value;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
        /// </summary>
        public bool Pooling
        {
            get => this._csb.Pooling;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Pooling = value;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
        /// </summary>
        public bool Replication
        {
            get => this._csb.Replication;
            set => this._csb.Replication = value;
        }
        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
        /// </summary>
        public bool TrustServerCertificate
        {
            get => this._csb.TrustServerCertificate;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.TrustServerCertificate = value;
            }
        }
        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects.
        /// </summary>
        public string TypeSystemVersion
        {
            get => this._csb.TypeSystemVersion;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.TypeSystemVersion = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server.
        /// </summary>
        public string WorkstationID
        {
            get => this._csb.WorkstationID;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.WorkstationID = value;
            }
        }



		/* Commented out, as one these causes an Access Violation, and they are not otherwise used:
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
		*/
		/// <summary>
		/// Adds an item to the configuration
		/// </summary>
		/// <param name="item"></param>
		public void Add(KeyValuePair<string, object> item)
		{
			this._csb.Add(item.Key, item.Value);
		}
		/// <summary>
		/// Determines whether the configuration contains a specific key.
		/// </summary>
		public bool ContainsKey(string key)
		{
			return this._csb.ContainsKey(key);
		}

		/// <summary>
		/// Removes the entry from the configuration instance.
		/// </summary>
		public void Remove(KeyValuePair<string, object> item)
		{
			this._csb.Remove(item.Key);
		}

		/// <summary>
		/// Removes the entry from the configuration instance.
		/// </summary>
		public void Remove(string key)
		{
			this._csb.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return this._csb.TryGetValue(key, out value);
		}
	}
}
