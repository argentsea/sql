// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System.Collections.Generic;
using System.Data.SqlClient;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class represents a single database connection — a database connection or a shard instance read or write connection.
    /// </summary>
    public class SqlConnectionConfiguration : SqlConnectionPropertiesBase, IDataConnection
    {
        private readonly SqlConnectionStringBuilder _csb;
        private string _connectionString = null;
        private SqlConnectionPropertiesBase _globalProperties = null;
        private SqlConnectionPropertiesBase _shardSetProperties = null;
        private SqlConnectionPropertiesBase _shardProperties = null;

        private const int DefaultConnectTimeout = 5;

        public SqlConnectionConfiguration()
        {
            _csb = new SqlConnectionStringBuilder();
            _csb.ConnectTimeout = DefaultConnectTimeout;

       }

        private void SetProperties(DataConnectionConfigurationBase properties)
        {
            if (!(properties.Password is null))
            {
                _csb.Password = properties.Password;
            }
            if (!(properties.UserName is null))
            {
                _csb.UserID = properties.UserName;
            }
            if (!(properties.WindowsAuth is null))
            {
                _csb.IntegratedSecurity = properties.WindowsAuth.Value;
            }
            var props = (SqlConnectionPropertiesBase)properties;
            if (!(props.ApplicationIntent is null))
            {
                _csb.ApplicationIntent = props.ApplicationIntent.Value;
            }
            if (!(props.ApplicationName is null))
            {
                _csb.ApplicationName = props.ApplicationName;
            }
            //if (!(props.ConnectRetryCount is null))
            //{
            //    _csb.ConnectRetryCount = props.ConnectRetryCount.Value;
            //}
            //if (!(props.ConnectRetryInterval is null))
            //{
            //    _csb.ConnectRetryInterval = props.ConnectRetryInterval.Value;
            //}
            if (!(props.ConnectTimeout is null))
            {
                _csb.ConnectTimeout = props.ConnectTimeout.Value;
            }
            if (!(props.CurrentLanguage is null))
            {
                _csb.CurrentLanguage = props.CurrentLanguage;
            }
            if (!(props.DataSource is null))
            {
                _csb.DataSource = props.DataSource;
            }
            if (!(props.Encrypt is null))
            {
                _csb.Encrypt = props.Encrypt.Value;
            }
            if (!(props.FailoverPartner is null))
            {
                _csb.FailoverPartner = props.FailoverPartner;
            }
            if (!(props.InitialCatalog is null))
            {
                _csb.InitialCatalog = props.InitialCatalog;
            }
            if (!(props.LoadBalanceTimeout is null))
            {
                _csb.LoadBalanceTimeout = props.LoadBalanceTimeout.Value;
            }
            if (!(props.MaxPoolSize is null))
            {
                _csb.MaxPoolSize = props.MaxPoolSize.Value;
            }
            if (!(props.MinPoolSize is null))
            {
                _csb.MinPoolSize = props.MinPoolSize.Value;
            }
            if (!(props.MultipleActiveResultSets is null))
            {
                _csb.MultipleActiveResultSets = props.MultipleActiveResultSets.Value;
            }
            if (!(props.MultiSubnetFailover is null))
            {
                _csb.MultiSubnetFailover = props.MultiSubnetFailover.Value;
            }
            if (!(props.PacketSize is null))
            {
                _csb.PacketSize = props.PacketSize.Value;
            }
            if (!(props.PersistSecurityInfo is null))
            {
                _csb.PersistSecurityInfo = props.PersistSecurityInfo.Value;
            }
            if (!(props.Pooling is null))
            {
                _csb.Pooling = props.Pooling.Value;
            }
            if (!(props.Replication is null))
            {
                _csb.Replication = props.Replication.Value;
            }
            if (!(props.TrustServerCertificate is null))
            {
                _csb.TrustServerCertificate = props.TrustServerCertificate.Value;
            }
            if (!(props.TypeSystemVersion is null))
            {
                _csb.TypeSystemVersion = props.TypeSystemVersion;
            }
            if (!(props.UserInstance is null))
            {
                _csb.UserInstance = props.UserInstance.Value;
            }
            if (!(props.WorkstationID is null))
            {
                _csb.WorkstationID = props.WorkstationID;
            }
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!(_globalProperties is null))
                {
                    SetProperties(_globalProperties);
                }
                if (!(_shardSetProperties is null))
                {
                    SetProperties(_shardSetProperties);
                }
                if (!(_shardProperties is null))
                {
                    SetProperties(_shardProperties);
                }
                SetProperties(this);
                _connectionString = _csb.ToString();
            }
            return _connectionString;
        }

        public void SetAmbientConfiguration(DataConnectionConfigurationBase globalProperties, DataConnectionConfigurationBase shardSetProperties, DataConnectionConfigurationBase shardProperties)
        {
            _globalProperties = globalProperties as SqlConnectionPropertiesBase;
            _shardSetProperties = shardSetProperties as SqlConnectionPropertiesBase;
            _shardProperties = shardProperties as SqlConnectionPropertiesBase;
        }

        public string ConnectionDescription
        {
            get => $"database {this._csb.InitialCatalog} on server {this._csb.DataSource}";
        }

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
