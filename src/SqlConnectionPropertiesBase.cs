// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Microsoft.Data.SqlClient;

namespace ArgentSea.Sql
{

    public abstract class SqlConnectionPropertiesBase : DataConnectionConfigurationBase
    {
        private ApplicationIntent? _applicationIntent = null;
        private string _applicationName = null;
        private int? _connectTimeout = null;
        private string _currentLanguage = null;
        private string _dataSource = null;
        private bool? _encrypt = null;
        private string _failoverPartner = null;
        private string _initialCatalog = null;
        private int? _loadBalanceTimeout = null;
        private int? _maxPoolSize = null;
        private int? _minPoolSize = null;
        private bool? _multipleActiveResultSets = null;
        private bool? _multiSubnetFailover = null;
        private int? _packetSize = null;
        private bool? _persistSecurityInfo = null;
        private bool? _pooling = null;
        private bool? _replication = null;
        private string _transactionBinding = null;
        private bool? _trustServerCertificate = null;
        private string _typeSystemVersion = null;
        private bool? _userInstance = null;
        private string _workstationID = null;

        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group.
        /// </summary>
        public ApplicationIntent? ApplicationIntent
        {
            get { return _applicationIntent; }
            set
            {
                _applicationIntent = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (_applicationName != value)
                {
                    _applicationName = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// Defaults to 5 seconds (before retry) if not set.
        /// </summary>
        public int? ConnectTimeout
        {
            get { return _connectTimeout; }
            set
            {
                if (_connectTimeout != value)
                {
                    _connectTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                if (_dataSource != value)
                {
                    _dataSource = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool? Encrypt
        {
            get { return _encrypt; }
            set
            {
                if (_encrypt != value)
                {
                    _encrypt = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get { return _failoverPartner; }
            set
            {
                if (_failoverPartner != value)
                {
                    _failoverPartner = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog
        {
            get { return _initialCatalog; }
            set
            {
                if (_initialCatalog != value)
                {
                    _initialCatalog = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int? LoadBalanceTimeout
        {
            get { return _loadBalanceTimeout; }
            set
            {
                if (_loadBalanceTimeout != value)
                {
                    _loadBalanceTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MaxPoolSize
        {
            get { return _maxPoolSize; }
            set
            {
                if (_maxPoolSize != value)
                {
                    _maxPoolSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MinPoolSize
        {
            get { return _minPoolSize; }
            set
            {
                if (_minPoolSize != value)
                {
                    _minPoolSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// When true, an application can maintain multiple active result sets (MARS). When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection.
        /// </summary>
        public bool? MultipleActiveResultSets
        {
            get { return _multipleActiveResultSets; }
            set
            {
                if (_multipleActiveResultSets != value)
                {
                    _multipleActiveResultSets = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        public bool? MultiSubnetFailover
        {
            get { return _multiSubnetFailover; }
            set
            {
                if (_multiSubnetFailover != value)
                {
                    _multiSubnetFailover = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
        /// </summary>
        public int? PacketSize
        {
            get { return _packetSize; }
            set
            {
                if (_packetSize != value)
                {
                    _packetSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool? PersistSecurityInfo
        {
            get { return _persistSecurityInfo; }
            set
            {
                if (_persistSecurityInfo != value)
                {
                    _persistSecurityInfo = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
        /// </summary>
        public bool? Pooling
        {
            get { return _pooling; }
            set
            {
                if (_pooling != value)
                {
                    _pooling = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
        /// </summary>
        public bool? Replication
        {
            get { return _replication; }
            set
            {
                if (_replication != value)
                {
                    _replication = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a string value that indicates how the connection maintains its association with an enlisted System.Transactions transaction.
        /// </summary>
        public string TransactionBinding
        {
            get { return _transactionBinding; }
            set
            {
                if(_transactionBinding != value)
                {
                    _transactionBinding = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
        /// </summary>
        public bool? TrustServerCertificate
        {
            get { return _trustServerCertificate; }
            set
            {
                if (_trustServerCertificate != value)
                {
                    _trustServerCertificate = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects.
        /// </summary>
        public string TypeSystemVersion
        {
            get { return _typeSystemVersion; }
            set
            {
                if (_typeSystemVersion != value)
                {
                    _typeSystemVersion = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to redirect the connection from the default SQL Server Express instance to a runtime-initiated instance running under the account of the caller.
        /// </summary>
        public bool? UserInstance
        {
            get { return _userInstance; }
            set
            {
                if (_userInstance != value)
                {
                    _userInstance = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server.
        /// </summary>
        public string WorkstationID
        {
            get { return _workstationID; }
            set
            {
                if (_workstationID != value)
                {
                    _workstationID = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}
