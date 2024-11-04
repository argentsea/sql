// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ArgentSea;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class represents a single database connection — a database connection or a shard instance read or write connection.
    /// </summary>
    public class SqlConnectionConfiguration : SqlConnectionPropertiesBase, IDataConnection
    {
        private string _connectionString = null;
        private SqlConnectionPropertiesBase _globalProperties = null;
        private SqlConnectionPropertiesBase _shardSetProperties = null;
        private SqlConnectionPropertiesBase _readWriteProperties = null;
        private SqlConnectionPropertiesBase _shardProperties = null;
        private readonly object _connectionLock = new object();
        private string _connectionDescription = null;

        private const int DefaultConnectTimeout = 5; //minimum recommended value per https://docs.microsoft.com/en-us/sql/database-engine/database-mirroring/connect-clients-to-a-database-mirroring-session-sql-server?view=sql-server-2017#RetryAlgorithm

        public SqlConnectionConfiguration()
        {
            //
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            _connectionString = null;
        }

        private void SetProperties(SqlConnectionStringBuilder csb, DataConnectionConfigurationBase properties)
        {
            if (!(properties.Password is null))
            {
                csb.Password = properties.Password;
            }
            if (!(properties.UserName is null))
            {
                csb.UserID = properties.UserName;
            }
            if (!(properties.WindowsAuth is null))
            {
                csb.IntegratedSecurity = properties.WindowsAuth.Value;
            }
            var props = (SqlConnectionPropertiesBase)properties;
            if (!(props.ApplicationIntent is null))
            {
                csb.ApplicationIntent = props.ApplicationIntent.Value;
            }
            if (!(props.ApplicationName is null))
            {
                csb.ApplicationName = props.ApplicationName;
            }
            //if (!(props.ConnectRetryCount is null))
            //{
            //    csb.ConnectRetryCount = props.ConnectRetryCount.Value;
            //}
            //if (!(props.ConnectRetryInterval is null))
            //{
            //    csb.ConnectRetryInterval = props.ConnectRetryInterval.Value;
            //}
            if (!(props.ConnectTimeout is null))
            {
                csb.ConnectTimeout = props.ConnectTimeout.Value;
            }
            if (!(props.CurrentLanguage is null))
            {
                csb.CurrentLanguage = props.CurrentLanguage;
            }
            if (!(props.DataSource is null))
            {
                csb.DataSource = props.DataSource;
            }
            if (!(props.Encrypt is null))
            {
                csb.Encrypt = props.Encrypt.Value;
            }
            if (!(props.FailoverPartner is null))
            {
                csb.FailoverPartner = props.FailoverPartner;
            }
            if (!(props.InitialCatalog is null))
            {
                csb.InitialCatalog = props.InitialCatalog;
            }
            if (!(props.LoadBalanceTimeout is null))
            {
                csb.LoadBalanceTimeout = props.LoadBalanceTimeout.Value;
            }
            if (!(props.MaxPoolSize is null))
            {
                csb.MaxPoolSize = props.MaxPoolSize.Value;
            }
            if (!(props.MinPoolSize is null))
            {
                csb.MinPoolSize = props.MinPoolSize.Value;
            }
            if (!(props.MultipleActiveResultSets is null))
            {
                csb.MultipleActiveResultSets = props.MultipleActiveResultSets.Value;
            }
            if (!(props.MultiSubnetFailover is null))
            {
                csb.MultiSubnetFailover = props.MultiSubnetFailover.Value;
            }
            if (!(props.PacketSize is null))
            {
                csb.PacketSize = props.PacketSize.Value;
            }
            if (!(props.PersistSecurityInfo is null))
            {
                csb.PersistSecurityInfo = props.PersistSecurityInfo.Value;
            }
            if (!(props.Pooling is null))
            {
                csb.Pooling = props.Pooling.Value;
            }
            if (!(props.Replication is null))
            {
                csb.Replication = props.Replication.Value;
            }
            if (!(props.TrustServerCertificate is null))
            {
                csb.TrustServerCertificate = props.TrustServerCertificate.Value;
            }
            if (!(props.TypeSystemVersion is null))
            {
                csb.TypeSystemVersion = props.TypeSystemVersion;
            }
            if (!(props.UserInstance is null))
            {
                csb.UserInstance = props.UserInstance.Value;
            }
            if (!(props.WorkstationID is null))
            {
                csb.WorkstationID = props.WorkstationID;
            }
        }

        public string GetConnectionString(ILogger logger)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                var csb = new SqlConnectionStringBuilder();
                csb.ConnectTimeout = DefaultConnectTimeout;
                csb.ConnectRetryCount = 0;
                if (!(_globalProperties is null))
                {
                    SetProperties(csb, _globalProperties);
                }
                if (!(_shardSetProperties is null))
                {
                    SetProperties(csb, _shardSetProperties);
                }
                if (!(_readWriteProperties is null))
                {
                    SetProperties(csb, _readWriteProperties);
                }
                if (!(_shardProperties is null))
                {
                    SetProperties(csb, _shardProperties);
                }
                SetProperties(csb, this);
                _connectionString = csb.ToString();
                _connectionDescription = $"database {csb.InitialCatalog} on server {csb.DataSource}";
                var logCS = _connectionString;
                var pwd = csb.Password;
                if (!string.IsNullOrEmpty(pwd))
                {
                    logCS = logCS.Replace(pwd, "********");
                }
                logger?.SqlConnectionStringBuilt(logCS);
            }
            return _connectionString;
        }

        public void SetAmbientConfiguration(DataConnectionConfigurationBase globalProperties, DataConnectionConfigurationBase shardSetProperties, DataConnectionConfigurationBase readWriteProperties, DataConnectionConfigurationBase shardProperties)
        {
            _globalProperties = globalProperties as SqlConnectionPropertiesBase;
            _shardSetProperties = shardSetProperties as SqlConnectionPropertiesBase;
            _readWriteProperties = readWriteProperties as SqlConnectionPropertiesBase;
            _shardProperties = shardProperties as SqlConnectionPropertiesBase;

            if (!(_globalProperties is null))
            {
                _globalProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardSetProperties is null))
            {
                _shardSetProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_readWriteProperties is null))
            {
                _readWriteProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardProperties is null))
            {
                _shardProperties.PropertyChanged += HandlePropertyChanged;
            }
        }

        public string ConnectionDescription
        {
            get
            {
                if (string.IsNullOrEmpty(this._connectionDescription))
                {
                    string initialCatalog = _globalProperties?.InitialCatalog;
                    string dataSource = _globalProperties?.DataSource;
                    if (!string.IsNullOrEmpty(_shardSetProperties?.InitialCatalog))
                    {
                        initialCatalog = _shardSetProperties.InitialCatalog;
                    }
                    if (!string.IsNullOrEmpty(_shardSetProperties?.DataSource))
                    {
                        dataSource = _shardSetProperties.DataSource;
                    }

                    if (!string.IsNullOrEmpty(_readWriteProperties?.InitialCatalog))
                    {
                        initialCatalog = _readWriteProperties.InitialCatalog;
                    }
                    if (!string.IsNullOrEmpty(_readWriteProperties?.DataSource))
                    {
                        dataSource = _readWriteProperties.DataSource;
                    }

                    if (!string.IsNullOrEmpty(_shardProperties?.InitialCatalog))
                    {
                        initialCatalog = _shardProperties.InitialCatalog;
                    }
                    if (!string.IsNullOrEmpty(_shardProperties?.DataSource))
                    {
                        dataSource = _shardProperties.DataSource;
                    }

                    if (!string.IsNullOrEmpty(this.InitialCatalog))
                    {
                        initialCatalog = this.InitialCatalog;
                    }
                    if (!string.IsNullOrEmpty(this.DataSource))
                    {
                        dataSource = this.DataSource;
                    }
                    _connectionDescription = $"database {initialCatalog} on server {dataSource}";
                }
                return this._connectionDescription;
            }
        }
    }
}
