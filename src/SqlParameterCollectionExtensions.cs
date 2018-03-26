 using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Logging;

namespace ArgentSea.Sql
{
    public static class SqlParameterCollectionExtensions
    {

        private static string NormalizeSqlParameterName(string parameterName)
        {
            if (!parameterName.StartsWith("@"))
            {
                return "@" + parameterName;
            }
            return parameterName;
        }
		public static DbParameterCollection MapInputParameters<TShard, TModel>(this DbParameterCollection prms, TModel model, ILogger logger) where TModel: class where TShard: IComparable
		{
			ShardMapper<TShard>.MapToInParameters(prms, model, logger);
			return prms;
		}
		public static DbParameterCollection MapOutputParameters<TShard, TModel>(this DbParameterCollection prms, TModel model, ILogger logger) where TModel : class where TShard : IComparable
		{
			ShardMapper<TShard>.MapToOutParameters(prms, typeof(TModel), logger);
			return prms;
		}

		#region String types
		//NVARCHAR
		/// <summary>
		/// Creates parameter for providing a string or a DBNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
		/// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
		/// <param name="maxLength">This should match the size of the parameter, not the size of the input string (and certainly not the number of bytes).
		/// For nvarchar(max) parameters, specify -1. 
		/// Setting the value correctly will help avoid plan cache pollution (when not using stored procedures) and minimize memory buffer allocations.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddSqlNVarCharInParameter(this DbParameterCollection prms, string parameterName, string value, int maxLength)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.NVarChar, maxLength)
            {
                Value = value != null ? value : (dynamic)System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates parameter for obtaining a string from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlNVarCharOutParameter(this DbParameterCollection prms, string parameterName, int maxLength)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.NVarChar, maxLength)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //NCHAR
        /// <summary>
        /// Creates parameter for providing a fixed-length string or a DBNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
        /// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlNCharInParameter(this DbParameterCollection prms, string parameterName, string value, int length)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.NChar, length)
            {
                Value = value != null ? value : (dynamic)System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates parameter for obtaining a fixed-length string from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlNCharOutParameter(this DbParameterCollection prms, string parameterName, int length)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.NChar, length)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //VARCHAR
        /// <summary>
        /// Creates parameter for providing a string or a DBNull value to a stored procedure, which is converted to the target ANSI code page (if possible).
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
        /// <param name="maxLength">This should match the size of the parameter, not the size of the input string (and certainly not the number of bytes).
        /// For nvarchar(max) parameters, specify -1. 
        /// Setting the value correctly will help avoid plan cache pollution (when not using stored procedures) and minimize memory buffer allocations.</param>
        /// <param name="localeId">Specify the code page for ANSI conversions. For example, the value 1033 is U.S. English.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlVarCharInParameter(this DbParameterCollection prms, string parameterName, string value, int maxLength, int localeId)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.VarChar, maxLength)
            {
                Value = value != null ? value : (dynamic)System.DBNull.Value,
                LocaleId = localeId,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates parameter for obtaining a string from a stored procedure, which has been converted from the source ANSI code page.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="maxLength">This should match the size of the parameter, not the size of the input string (and certainly not the number of bytes).
        /// For nvarchar(max) parameters, specify -1. 
        /// Setting the value correctly will help avoid plan cache pollution (when not using stored procedures) and minimize memory buffer allocations.</param>
        /// <param name="localeId">Specify the code page for ANSI conversions. For example, the value 1033 is U.S. English.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlVarCharOutParameter(this DbParameterCollection prms, string parameterName, int maxLength, int localeId)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.VarChar, maxLength)
            {
                Direction = ParameterDirection.Output,
                LocaleId = localeId
            };
            prms.Add(prm);
            return prms;
        }
        //CHAR
        /// <summary>
        /// Creates parameter for providing a fixed-length string or a DBNull value to a stored procedure, which is converted to the target ANSI code page (if possible).
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
        /// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
        /// <param name="localeId">Specify the code page for ANSI conversions. For example, the value 1033 is U.S. English.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlCharInParameter(this DbParameterCollection prms, string parameterName, string value, int length, int localeId)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Char, length)
            {
                Value = value != null ? value : (dynamic)System.DBNull.Value,
                LocaleId = localeId,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates parameter for obtaining a fixed-length string from a stored procedure, which has been converted from the source ANSI code page.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
        /// <param name="localeId">Specify the code page for ANSI conversions. For example, the value 1033 is U.S. English.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlCharOutParameter(this DbParameterCollection prms, string parameterName, int length, int localeId)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Char, length)
            {
                Direction = ParameterDirection.Output,
                LocaleId = localeId
            };
            prms.Add(prm);
            return prms;
        }

        #endregion
        #region Numeric types
        //LONG
        /// <summary>
        /// Creates a parameter for providing a 64-bit signed integer (long) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 64-bit signed integer value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBigIntInParameter(this DbParameterCollection prms, string parameterName, long value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.BigInt)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 64-bit signed integer (long) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 64-bit signed integer value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBigIntInParameter(this DbParameterCollection prms, string parameterName, long? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.BigInt)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 64-bit signed integer (long) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBigIntOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //INT
        /// <summary>
        /// Creates a parameter for providing a 32-bit signed integer (int) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 32-bit signed integer value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlIntInParameter(this DbParameterCollection prms, string parameterName, int value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Int)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 32-bit signed integer (int) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 32-bit signed integer value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlIntInParameter(this DbParameterCollection prms, string parameterName, int? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Int)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 32-bit signed integer (int) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlIntOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
         }

        //SHORT
        /// <summary>
        /// Creates a parameter for providing a 16-bit signed integer (short) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 16-bit signed integer value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallIntInParameter(this DbParameterCollection prms, string parameterName, short value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallInt)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 16-bit signed integer (short) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 16-bit signed integer value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallIntInParameter(this DbParameterCollection prms, string parameterName, short? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallInt)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 32-bit signed integer (short) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallIntOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallInt)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //BYTE
        /// <summary>
        /// Creates a parameter for providing a 8-bit unsigned integer (byte) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An unsigned 8-bit integer value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTinyIntInParameter(this DbParameterCollection prms, string parameterName, byte value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.TinyInt)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 8-bit unsigned integer (byte) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An unsigned 8-bit integer value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlIntInParameter(this DbParameterCollection prms, string parameterName, byte? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.TinyInt)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 32-bit signed integer (byte) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTinyIntOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.TinyInt)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }

        //DOUBLE
        /// <summary>
        /// Creates a parameter for providing a 64-bit floating-point value (double) to a stored procedure. NaN will be converted to DbNull.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 64-bit floating-point value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlFloatInParameter(this DbParameterCollection prms, string parameterName, double value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Float)
            {
                Value = double.IsNaN(value) ? (dynamic)System.DBNull.Value : value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 64-bit floating-point value (double) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 64-bit floating-point value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlFloatInParameter(this DbParameterCollection prms, string parameterName, double? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Float)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 64-bit floating-point value (double) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlFloatOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Float)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //SINGLE
        /// <summary>
        /// Creates a parameter for providing a 32-bit floating-point value (float) to a stored procedure. NaN will be converted to DbNull.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 32-bit floating point value (float).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlRealInParameter(this DbParameterCollection prms, string parameterName, float value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Real)
            {
                Value = float.IsNaN(value) ? (dynamic)System.DBNull.Value : value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a 32-bit floating-point value (float) or DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 32-bit floating point value (float) or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlRealInParameter(this DbParameterCollection prms, string parameterName, float? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Real)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a 32-bit floating-point value (float) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlRealOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Real)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //BOOL
        /// <summary>
        /// Creates a parameter for providing a boolean value (bool) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A boolean value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBitInParameter(this DbParameterCollection prms, string parameterName, bool value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Bit)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a boolean value (bool) or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A boolean value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBitInParameter(this DbParameterCollection prms, string parameterName, bool? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Bit)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a boolean value (bool) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBitOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //DECIMAL
        /// <summary>
        /// Creates a parameter for providing a decmial value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDecimalInParameter(this DbParameterCollection prms, string parameterName, decimal value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a decmial value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDecimalInParameter(this DbParameterCollection prms, string parameterName, decimal? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a decmial value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="precision">Specifies the maximum number of digits used to store the number (inclusive of both sides of the decimal point).</param>
        /// <param name="scale">Specifies the number of digits used in the fractional portion of the number (i.e. digits to the right of the decimal point).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDecimalOutParameter(this DbParameterCollection prms, string parameterName, byte precision, byte scale)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output,
                Precision = precision,
                Scale = scale
            };
            prms.Add(prm);
            return prms;
        }
        //MONEY
        /// <summary>
        /// Creates a parameter for providing a decmial value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Money)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a decmial value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Money)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a decmial value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="precision">Specifies the maximum number of digits used to store the number (inclusive of both sides of the decimal point).</param>
        /// <param name="scale">Specifies the number of digits used in the fractional portion of the number (i.e. digits to the right of the decimal point).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlMoneyOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //SMALLMONEY
        /// <summary>
        /// Creates a parameter for providing a decmial value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallMoney)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a decmial value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A decmial value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallMoney)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a decmial value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="precision">Specifies the maximum number of digits used to store the number (inclusive of both sides of the decimal point).</param>
        /// <param name="scale">Specifies the number of digits used in the fractional portion of the number (i.e. digits to the right of the decimal point).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlSmallMoneyOutParameter(this DbParameterCollection prms, string parameterName, byte precision, byte scale)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallMoney)
            {
                Direction = ParameterDirection.Output,
                Precision = precision,
                Scale = scale
            };
            prms.Add(prm);
            return prms;
        }
        #endregion
        #region Temporal types

        //DATE
        /// <summary>
        /// Creates a parameter for providing a date (sans time) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A DateTime value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateInParameter(this DbParameterCollection prms, string parameterName, DateTime value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Date)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a date (sans time) or DbNull to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A DateTime value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateInParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Date)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a date (sans time) from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Date)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //DATETIME
        /// <summary>
        /// Creates a parameter for providing a date and time value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A date and time value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeInParameter(this DbParameterCollection prms, string parameterName, DateTime value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a date and time value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A date and time value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeInParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a date and time value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //DATETIME2
        /// <summary>
        /// Creates a parameter for providing a date and time value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A date and time value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTime2InParameter(this DbParameterCollection prms, string parameterName, DateTime value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime2)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a date and time value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A date and time value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTime2InParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime2)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving date and time value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTime2OutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTime2)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //TIME
        /// <summary>
        /// Creates a parameter for providing a time value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A time value .</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTimeInParameter(this DbParameterCollection prms, string parameterName, TimeSpan value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Time)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a time value or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A time value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTimeInParameter(this DbParameterCollection prms, string parameterName, TimeSpan? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Time)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a time value from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTimeOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Time)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //DATETIMEOFFSET
        /// <summary>
        /// Creates a parameter for providing a DateTimeOffset value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A DateTimeOffset value.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeOffsetInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTimeOffset)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a DateTimeOffset or a DbNull value to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A DateTimeOffset value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeOffsetInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTimeOffset)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a DateTimeOffset from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlDateTimeOffsetOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.DateTimeOffset)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        #endregion
        #region Other types
        //GUID
        /// <summary>
        /// Creates a parameter for providing a Guid or DBNull (via Guid.Empty) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A Guid value. Will convert Guild.Empty to DBNull.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlUniqueIdentifierInParameter(this DbParameterCollection prms, string parameterName, Guid value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.UniqueIdentifier)
            {
                Value = Guid.Empty.Equals(value) ? (dynamic)System.DBNull.Value : value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for providing a Guid or DBNull (via null value) to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A 64-bit signed integer value or null.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlUniqueIdentifierInParameter(this DbParameterCollection prms, string parameterName, Guid? value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.UniqueIdentifier)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates an output parameter for retrieving a Guid from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlUniqueIdentifierOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //VARBINARY
        /// <summary>
        /// Creates a parameter for providing a variable-sized byte array to a stored procedure. A null reference will save DBNull.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An array of bytes, or null.</param>
        /// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlVarBinaryInParameter(this DbParameterCollection prms, string parameterName, byte[] value, int length)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.VarBinary, length)
            {
                Value = value == null ? (dynamic)System.DBNull.Value : value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for obtaining a variable-sized byte array from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlVarBinaryOutParameter(this DbParameterCollection prms, string parameterName, int length)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.VarBinary, length)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }
        //BINARY
        /// <summary>
        /// Creates a parameter for providing a fixed-sized byte array to a stored procedure. A null reference will save DBNull.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">An array of bytes, or null.</param>
        /// <param name="length">The fixed number of bytes in the database column.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBinaryInParameter(this DbParameterCollection prms, string parameterName, byte[] value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Binary)
            {
                Value = value == null ? (dynamic)System.DBNull.Value : value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }
        /// <summary>
        /// Creates a parameter for obtaining a fixed-sized byte array from a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="length">The fixed number of bytes in the database column.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlBinaryOutParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Binary)
            {
                Direction = ParameterDirection.Output
            };
            prms.Add(prm);
            return prms;
        }

        //TVP
        /// <summary>
        /// Creates a parameter for providing a user-defined table to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A list of SqlDataRecord objects containing the table contents.</param>
        /// <param name="length">The fixed number of bytes in the database column.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTableValuedParameter(this DbParameterCollection prms, string parameterName, IEnumerable<SqlDataRecord> value)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Structured)
            {
                Value = value,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }

        #endregion
        #region Casting
        /// <summary>
        /// Returns a string, or null if the parameter value is DbNull.
        /// </summary>
        /// <returns>Parameter value as a string.</returns>
        public static string GetString(this SqlParameter prm) => prm.Value as string;
        /// <summary>
        /// Returns a byte array, or null if the parameter value is DbNull.
        /// </summary>
        /// <returns>Parameter value as a byte[].</returns>
        public static byte[] GetBytes(this SqlParameter prm) => prm.Value as byte[];
        /// <summary>
        /// Returns a Char value from the parameter, or NUL (char 0) if the value is DbNull.
        /// </summary>
        /// <returns>Parameter value as Char.</returns>
        //public static char GetChar(this SqlParameter prm)
        //{
        //    if (System.DBNull.Value.Equals(prm.Value))
        //    {
        //        return (char)0;
        //    }
        //    else
        //    {
        //        return (char)prm.Value;
        //    }
        //}
        public static long GetLong(this SqlParameter prm) => (long)prm.Value;
        public static long? GetNullableLong(this SqlParameter prm) => prm.Value as long?;
        public static int GetInteger(this SqlParameter prm) => (int)prm.Value;
        public static int? GetNullableInteger(this SqlParameter prm) => prm.Value as int?;
        public static short GetShort(this SqlParameter prm) => (short)prm.Value;
        public static short? GetNullableShort(this SqlParameter prm) => prm.Value as short?;
        public static byte GetByte(this SqlParameter prm) => (byte)prm.Value;
        public static byte? GetNullableByte(this SqlParameter prm) => prm.Value as byte?;
        public static bool GetBoolean(this SqlParameter prm) => (bool)prm.Value;
        public static bool? GetNullableBoolean(this SqlParameter prm) => prm.Value as bool?;
        public static decimal GetDecimal(this SqlParameter prm) => (decimal)prm.Value;
        public static decimal? GetNullableDecimal(this SqlParameter prm) => prm.Value as decimal?;
        /// <summary>
        /// Returns a double (64-bit floating point) value from the parameter, or NaN (Not a Number) if the value is DbNull.
        /// </summary>
        /// <returns>Parameter value as double.</returns>
        public static double GetDouble(this SqlParameter prm)
        {
            if (System.DBNull.Value.Equals(prm.Value))
            {
                return double.NaN;
            }
            else
            {
                return (double)prm.Value;
            }
        }
        public static double? GetNullableDouble(this SqlParameter prm)
            => prm.Value as double?;
        /// <summary>
        /// Returns a double (32-bit floating point) value from the parameter, or NaN (Not a Number) if the value is DbNull.
        /// </summary>
        /// <returns>Parameter value as float.</returns>
        public static float GetFloat(this SqlParameter prm)
        {
            if (System.DBNull.Value.Equals(prm.Value))
            {
                return float.NaN;
            }
            else
            {
                return (float)prm.Value;
            }
        }
        public static float? GetNullableFloat(this SqlParameter prm)
            => prm.Value as float?;

        /// <summary>
        /// Returns a Guid value from the parameter, or Guid.Emtpy if the value is DbNull.
        /// </summary>
        /// <returns>Parameter value as Guid.</returns>
        public static Guid GetGuid(this SqlParameter prm)
        {
            if (System.DBNull.Value.Equals(prm.Value))
            {
                return Guid.Empty;
            }
            else
            {
                return (Guid)prm.Value;
            }
        }
        public static Guid? GetNullableGuid(this SqlParameter prm)
        {
            if (System.DBNull.Value.Equals(prm.Value))
            {
                return null;
            }
            else
            {
                return (Guid?)prm.Value;
            }
        }
        public static DateTime GetDateTime(this SqlParameter prm) => (DateTime)prm.Value;
        public static DateTime? GetNullableDateTime(this SqlParameter prm) => prm.Value as DateTime?;
        public static DateTimeOffset GetDateTimeOffset(this SqlParameter prm) => (DateTimeOffset)prm.Value;
        public static DateTimeOffset? GetNullableDateTimeOffset(this SqlParameter prm) => prm.Value as DateTimeOffset?;
        public static TimeSpan GetTimeSpan(this SqlParameter prm) => (TimeSpan)prm.Value;
        public static TimeSpan? GetNullableTimeSpan(this SqlParameter prm) => prm.Value as TimeSpan?;
        #endregion
    }
}
