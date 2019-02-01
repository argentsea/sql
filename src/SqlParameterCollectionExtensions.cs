// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

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
    /// <summary>
    /// This class adds extension methods which simplify setting SQL parameter values from .NET types.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        internal static string NormalizeSqlParameterName(string parameterName)
        {
            if (!parameterName.StartsWith("@"))
            {
                return $"@{parameterName}";
            }
            return parameterName;
        }
        internal static string NormalizeSqlColumnName(string columnName)
        {
            if (!string.IsNullOrEmpty(columnName) && columnName.StartsWith("@"))
            {
                columnName = columnName.Substring(1);
            }
            return columnName;
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
        public static DbParameterCollection AddSqlNVarCharInputParameter(this DbParameterCollection prms, string parameterName, string value, int maxLength)
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
        public static DbParameterCollection AddSqlNVarCharOutputParameter(this DbParameterCollection prms, string parameterName, int maxLength)
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
        public static DbParameterCollection AddSqlNCharInputParameter(this DbParameterCollection prms, string parameterName, string value, int length)
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
        public static DbParameterCollection AddSqlNCharOutputParameter(this DbParameterCollection prms, string parameterName, int length)
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
        public static DbParameterCollection AddSqlVarCharInputParameter(this DbParameterCollection prms, string parameterName, string value, int maxLength, int localeId)
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
        public static DbParameterCollection AddSqlVarCharOutputParameter(this DbParameterCollection prms, string parameterName, int maxLength, int localeId)
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
        public static DbParameterCollection AddSqlCharInputParameter(this DbParameterCollection prms, string parameterName, string value, int length, int localeId)
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
        public static DbParameterCollection AddSqlCharOutputParameter(this DbParameterCollection prms, string parameterName, int length, int localeId)
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
        public static DbParameterCollection AddSqlBigIntInputParameter(this DbParameterCollection prms, string parameterName, long value)
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
        public static DbParameterCollection AddSqlBigIntInputParameter(this DbParameterCollection prms, string parameterName, long? value)
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
        public static DbParameterCollection AddSqlBigIntOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlIntInputParameter(this DbParameterCollection prms, string parameterName, int value)
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
        public static DbParameterCollection AddSqlIntInputParameter(this DbParameterCollection prms, string parameterName, int? value)
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
        public static DbParameterCollection AddSqlIntOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlSmallIntInputParameter(this DbParameterCollection prms, string parameterName, short value)
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
        public static DbParameterCollection AddSqlSmallIntInputParameter(this DbParameterCollection prms, string parameterName, short? value)
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
        public static DbParameterCollection AddSqlSmallIntOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlTinyIntInputParameter(this DbParameterCollection prms, string parameterName, byte value)
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
        public static DbParameterCollection AddSqlTinyIntInputParameter(this DbParameterCollection prms, string parameterName, byte? value)
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
        public static DbParameterCollection AddSqlTinyIntOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlFloatInputParameter(this DbParameterCollection prms, string parameterName, double value)
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
        public static DbParameterCollection AddSqlFloatInputParameter(this DbParameterCollection prms, string parameterName, double? value)
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
        public static DbParameterCollection AddSqlFloatOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlRealInputParameter(this DbParameterCollection prms, string parameterName, float value)
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
        public static DbParameterCollection AddSqlRealInputParameter(this DbParameterCollection prms, string parameterName, float? value)
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
        public static DbParameterCollection AddSqlRealOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlBitInputParameter(this DbParameterCollection prms, string parameterName, bool value)
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
        public static DbParameterCollection AddSqlBitInputParameter(this DbParameterCollection prms, string parameterName, bool? value)
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
        public static DbParameterCollection AddSqlBitOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlDecimalInputParameter(this DbParameterCollection prms, string parameterName, decimal value, byte precision, byte scale)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Value = value,
                Direction = ParameterDirection.Input,
				Precision = precision,
				Scale = scale
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
        public static DbParameterCollection AddSqlDecimalInputParameter(this DbParameterCollection prms, string parameterName, decimal? value, byte precision, byte scale)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Decimal)
            {
                Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
                Direction = ParameterDirection.Input,
				Precision = precision,
				Scale = scale
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
        public static DbParameterCollection AddSqlDecimalOutputParameter(this DbParameterCollection prms, string parameterName, byte precision, byte scale)
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
        public static DbParameterCollection AddSqlMoneyInputParameter(this DbParameterCollection prms, string parameterName, decimal value)
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
        public static DbParameterCollection AddSqlMoneyInputParameter(this DbParameterCollection prms, string parameterName, decimal? value)
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
        public static DbParameterCollection AddSqlMoneyOutputParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Money)
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
        public static DbParameterCollection AddSqlSmallMoneyInputParameter(this DbParameterCollection prms, string parameterName, decimal value)
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
        public static DbParameterCollection AddSqlSmallMoneyInputParameter(this DbParameterCollection prms, string parameterName, decimal? value)
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
        public static DbParameterCollection AddSqlSmallMoneyOutputParameter(this DbParameterCollection prms, string parameterName)
        {
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.SmallMoney)
            {
                Direction = ParameterDirection.Output
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
        public static DbParameterCollection AddSqlDateInputParameter(this DbParameterCollection prms, string parameterName, DateTime value)
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
        public static DbParameterCollection AddSqlDateInputParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
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
        public static DbParameterCollection AddSqlDateOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlDateTimeInputParameter(this DbParameterCollection prms, string parameterName, DateTime value)
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
        public static DbParameterCollection AddSqlDateTimeInputParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
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
        public static DbParameterCollection AddSqlDateTimeOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlDateTime2InputParameter(this DbParameterCollection prms, string parameterName, DateTime value)
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
        public static DbParameterCollection AddSqlDateTime2InputParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
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
        public static DbParameterCollection AddSqlDateTime2OutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlTimeInputParameter(this DbParameterCollection prms, string parameterName, TimeSpan value)
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
        public static DbParameterCollection AddSqlTimeInputParameter(this DbParameterCollection prms, string parameterName, TimeSpan? value)
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
        public static DbParameterCollection AddSqlTimeOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlDateTimeOffsetInputParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset value)
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
        public static DbParameterCollection AddSqlDateTimeOffsetInputParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset? value)
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
        public static DbParameterCollection AddSqlDateTimeOffsetOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlUniqueIdentifierInputParameter(this DbParameterCollection prms, string parameterName, Guid value)
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
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlUniqueIdentifierInputParameter(this DbParameterCollection prms, string parameterName, Guid? value)
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
        public static DbParameterCollection AddSqlUniqueIdentifierOutputParameter(this DbParameterCollection prms, string parameterName)
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
        public static DbParameterCollection AddSqlVarBinaryInputParameter(this DbParameterCollection prms, string parameterName, byte[] value, int length)
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
        public static DbParameterCollection AddSqlVarBinaryOutputParameter(this DbParameterCollection prms, string parameterName, int length)
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
        public static DbParameterCollection AddSqlBinaryInputParameter(this DbParameterCollection prms, string parameterName, byte[] value, int length)
		{
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Binary, length)
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
        public static DbParameterCollection AddSqlBinaryOutputParameter(this DbParameterCollection prms, string parameterName, int length)
		{
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Binary, length)
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
        /// <summary>
        /// Creates a parameter for providing a user-defined table to a stored procedure.
        /// </summary>
        /// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
        /// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
        /// <param name="value">A list of SqlDataRecord objects containing the table contents.</param>
        /// <param name="length">The fixed number of bytes in the database column.</param>
        /// <returns>The DbParameterCollection to which the parameter was appended.</returns>
        public static DbParameterCollection AddSqlTableValuedParameter<TModel>(this DbParameterCollection prms, string parameterName, IEnumerable<TModel> values, ILogger logger) where TModel: class, new()
        {
            var tvp = new List<SqlDataRecord>();
            foreach (var val in values)
            {
                tvp.Add(TvpMapper.ToTvpRecord<TModel>(val, logger));
            }
            var prm = new SqlParameter(NormalizeSqlParameterName(parameterName), SqlDbType.Structured)
            {
                Value = tvp,
                Direction = ParameterDirection.Input
            };
            prms.Add(prm);
            return prms;
        }

        #endregion
    }
}
