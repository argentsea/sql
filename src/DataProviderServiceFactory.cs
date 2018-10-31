// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Data.SqlClient;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class is a provider-specific resouce to enable provider-neutral code to execute. It is unlikely that you would reference this in consumer code.
    /// </summary>
    public class DataProviderServiceFactory: IDataProviderServiceFactory
    {
        public bool GetIsErrorTransient(Exception exception)
        {
            if (exception is SqlException)
            {
                switch (((SqlException)exception).Number)
                {
					// Attempted to update a row that was updated in a different transaction since the start of the present transaction.
					case 41302:
					// Repeatable read validation failure. A row read from a memory-optimized table this transaction has been updated by another transaction that has committed before the commit of this transaction.
					case 41305:
					// Serializable validation failure.A new row was inserted into a range that was scanned earlier by the present transaction.We call this a phantom row.
					case 41325:
					// Dependency failure: a dependency was taken on another transaction that later failed to commit.
					case 41301:
					// Quota for user data in memory-optimized tables and table variables was reached.
					case 41823:
					case 41840:
					// Transaction exceeded the maximum number of commit dependencies.
					case 41839:
					// Login to read-secondary failed due to long wait on 'HADR_DATABASE_WAIT_FOR_TRANSITION_TO_VERSIONING'. 
					// The replica is not available for login because row versions are missing for transactions that were in-flight when the replica 
					// was recycled. The issue can be resolved by rolling back or committing the active transactions on the primary replica. 
					// Occurrences of this condition can be minimized by avoiding long write transactions on the primary.
					case 4221:
                    // Cannot process request. Too many operations in progress for subscription "%ld".
                    // The service is busy processing multiple requests for this subscription. Requests are currently blocked for 
                    // resource optimization. Query sys.dm_operation_status for operation status. Wait until pending requests are 
                    // complete or delete one of your pending requests and retry your request later.
                    case 49920:
                    // Cannot process create or update request. Too many create or update operations in progress for subscription "%ld".
                    // The service is busy processing multiple create or update requests for your subscription or server. 
                    // Requests are currently blocked for resource optimization. Query sys.dm_operation_status for pending operations. 
                    // Wait till pending create or update requests are complete or delete one of your pending requests and retry your request later.
                    case 49919:
                    // Cannot process request. Not enough resources to process request.
                    // The service is currently busy. Please retry the request later.
                    case 49918:
                    // Cannot open database "%.*ls" requested by the login. The login failed.
                    case 4060:
                    // Database XXXX on server YYYY is not currently available. Please retry the connection later.
                    // If the problem persists, contact customer support, and provide them the session tracing ID of ZZZZZ.
                    case 40613:
                    // The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be decoded).
                    case 40501:
                    // The service has encountered an error processing your request. Please try again.
                    case 40197:
                    // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d and the current usage for the database is %d.
                    // However, the server is currently too busy to support requests greater than %d for this database.
                    // For more information, see http://go.microsoft.com/fwlink/?LinkId=267637. Otherwise, please try again.
                    case 10929:
                    // Resource ID: %d. The %s limit for the database is %d and has been reached. For more information,
                    // see http://go.microsoft.com/fwlink/?LinkId=267637.
                    case 10928:
                    // A network-related or instance-specific error occurred while establishing a connection to SQL Server. 
                    // The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server 
                    // is configured to allow remote connections. (provider: TCP Provider, error: 0 - A connection attempt failed 
                    // because the connected party did not properly respond after a period of time, or established connection failed 
                    // because connected host has failed to respond.)"}
                    case 10060:
                    // A transport-level error has occurred when sending the request to the server. 
                    // (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
                    case 10054:
                    // A transport-level error has occurred when receiving results from the server.
                    // An established connection was aborted by the software in your host machine.
                    case 10053:
                    // The client was unable to establish a connection because of an error during connection initialization process before login. 
                    // Possible causes include the following: the client tried to connect to an unsupported version of SQL Server;
                    // the server was too busy to accept new connections; or there was a resource limitation (insufficient memory or maximum
                    // allowed connections) on the server. (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by
                    // the remote host.)
                    case 233:
                    // A connection was successfully established with the server, but then an error occurred during the login process. 
                    // (provider: TCP Provider, error: 0 - The specified network name is no longer available.) 
                    case 64:
                    // The instance of SQL Server you attempted to connect to does not support encryption.
                    case 20:
                    //Deadlock victum
                    case 1205:
                        return true;
                    default:
                        return false;
                }
            }
			else
			{
				return false;
			}
        }

        public DbCommand NewCommand(string storedProcedureName, DbConnection connection)
        {
            if (connection is SqlConnection)
            {
                return new SqlCommand(storedProcedureName, (SqlConnection)connection);
            }
            throw new ArgumentException(nameof(connection));
        }

        public DbConnection NewConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        //public string NormalizeFieldName(string fieldName)
        //{
        //    if (fieldName.StartsWith("@"))
        //    {
        //        return fieldName.Substring(1);
        //    }
        //    return fieldName;
        //}

        //public string NormalizeParameterName(string parameterName)
        //{
        //    if (!parameterName.StartsWith("@"))
        //    {
        //        return "@" + parameterName;
        //    }
        //    return parameterName;
        //}

        public void SetParameters(DbCommand cmd, DbParameterCollection parameters, Dictionary<string, object> parameterValues)
        {
            for (var i = 0; i < parameters.Count; i++)
            {
                var prmSource = (SqlParameter)parameters[i];

                var prmTarget = new SqlParameter()
                {
                    CompareInfo = prmSource.CompareInfo,
                    DbType = prmSource.DbType,
                    Direction = prmSource.Direction,
                    IsNullable = prmSource.IsNullable,
                    LocaleId = prmSource.LocaleId,
                    Offset = prmSource.Offset,
                    ParameterName = prmSource.ParameterName,
                    Precision = prmSource.Precision,
                    Scale = prmSource.Scale,
                    Size = prmSource.Size,
                    SourceColumn = prmSource.SourceColumn,
                    SourceColumnNullMapping = prmSource.SourceColumnNullMapping,
                    SqlDbType = prmSource.SqlDbType,
                    SqlValue = prmSource.SqlValue,
                    TypeName = prmSource.TypeName,
                    Value = prmSource.Value
                };
                if (!(parameterValues is null))
                {
                    if (parameterValues.TryGetValue(prmTarget.ParameterName, out var prmValue))
                    {
                        prmTarget.Value = prmValue;
                    }
                }
                cmd.Parameters.Add(prmTarget);
            }
        }

    }
}
