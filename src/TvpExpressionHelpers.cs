using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;

namespace ArgentSea.Sql
{
    public static class TvpExpressionHelpers
    {
        public static void TvpStringExpressionBuilder(string parameterName, SqlDbType sqlType, int length, ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ToFieldName(parameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType), typeof(long) });
                var expPrmName = Expression.Constant(dataName, typeof(string));
                var expPrmType = Expression.Constant(sqlType, typeof(SqlDbType));
                var expPrmLength = Expression.Constant((long)length, typeof(long));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType, expPrmLength }));

                var miSet = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetString));
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));
                if (propertyType.IsEnum)
                {
                    var miEnumToString = typeof(Enum).GetMethod(nameof(Enum.ToString), new Type[] { });
                    var expGetString = Expression.Call(expProperty, miEnumToString);
                    setExpressions.Add(Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expGetString }));
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType).IsEnum) //Nullable Enum
                {
                    var miEnumToString = typeof(Enum).GetMethod(nameof(Enum.ToString), new Type[] { });
                    var piNullableHasValue = propertyType.GetProperty(nameof(Nullable<int>.HasValue));
                    var piNullableGetValue = propertyType.GetProperty(nameof(Nullable<int>.Value));

                    setExpressions.Add(Expression.Condition(
						Expression.Property(expProperty, piNullableHasValue),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Call(Expression.Property(expProperty, piNullableGetValue), miEnumToString) }),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
						));
                }
                else
                {
                    setExpressions.Add(Expression.Condition(
						Expression.Equal(expProperty, Expression.Constant(null, typeof(string))),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal }),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty })
						));
                }
            }
        }

        //int, short, byte, enum
        public static void TvpEnumXIntExpressionBuilder(string parameterName, SqlDbType sqlType, string methodName, Type baseType, ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ToFieldName(parameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType) });
                var expPrmName = Expression.Constant(dataName, typeof(string));
                var expPrmType = Expression.Constant(sqlType, typeof(SqlDbType));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType }));

                var miSet = typeof(SqlDataRecord).GetMethod(methodName);
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));
                if (propertyType.IsEnum)
                {
                    setExpressions.Add(Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Convert(expProperty, baseType) }));
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    //var baseType = Nullable.GetUnderlyingType(propertyType);
                    var piNullableHasValue = propertyType.GetProperty(nameof(Nullable<int>.HasValue));
                    var piNullableGetValue = propertyType.GetProperty(nameof(Nullable<int>.Value));

                    if (Nullable.GetUnderlyingType(propertyType).IsEnum)
                    {

                        setExpressions.Add(Expression.Condition(
							Expression.Property(expProperty, piNullableHasValue),
							Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Convert(Expression.Convert(Expression.Property(expProperty, piNullableGetValue), typeof(int)), baseType) }),
							Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
							));

                        //left off here: need to convert Nullable<Enum> and validate int
                    }
                    else //if (Nullable.GetUnderlyingType(propertyType) == typeof(int))
                    {
                        setExpressions.Add(Expression.Condition(
							Expression.Property(expProperty, piNullableHasValue),
							Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Convert(Expression.Property(expProperty, piNullableGetValue), baseType) }),
							Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
							));
                    }
                }
                else
                {
                    setExpressions.Add(Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty }));
                }
            }
        }

        //bool and long
        public static bool TvpSimpleValueExpressionBuilder(string parameterName, SqlDbType sqlType, string methodName, Type baseType, ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ToFieldName(parameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType) });
                var expPrmName = Expression.Constant(ToFieldName(dataName), typeof(string));
                var expPrmType = Expression.Constant(sqlType, typeof(SqlDbType));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType }));

                var miSet = typeof(SqlDataRecord).GetMethod(methodName);
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));

                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var piNullableHasValue = propertyType.GetProperty(nameof(Nullable<int>.HasValue));
                    var piNullableGetValue = propertyType.GetProperty(nameof(Nullable<int>.Value));

                    setExpressions.Add(Expression.Condition(
						Expression.Property(expProperty, piNullableHasValue),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Property(expProperty, piNullableGetValue) }),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
						));
                }
                else if (!propertyType.IsValueType)
                {
                    setExpressions.Add(Expression.Condition(
						Expression.Equal(expProperty, Expression.Constant(null, propertyType)),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal }),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty })
						));
                }
                else
                {
                    setExpressions.Add(Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty }));
                }
                return true;
            }
            return false;
        }

        public static void TvpGuidFloatingPointExpressionBuilder(string parameterName, SqlDbType sqlType, string methodName, Type baseType, ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ToFieldName(parameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType) });
                var expPrmName = Expression.Constant(dataName, typeof(string));
                var expPrmType = Expression.Constant(sqlType, typeof(SqlDbType));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType }));

                var miSet = typeof(SqlDataRecord).GetMethod(methodName);
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var piNullableHasValue = propertyType.GetProperty(nameof(Nullable<int>.HasValue));
                    var piNullableGetValue = propertyType.GetProperty(nameof(Nullable<int>.Value));

                    setExpressions.Add(Expression.Condition(
						Expression.Property(expProperty, piNullableHasValue),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Property(expProperty, piNullableGetValue) }),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
						));
                }
                else if (propertyType == typeof(Guid))
                {
                    setExpressions.Add(Expression.Condition(
						Expression.Equal(expProperty, Expression.Constant(Guid.Empty, propertyType)),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal }),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty })
						));
                }
                else
                {
                    var miIsNaN = propertyType.GetMethod(nameof(double.IsNaN));
                    setExpressions.Add(Expression.Condition(
						Expression.Call(miIsNaN, expProperty),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal }),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty })
						));
                }
            }
        }

        public static void TvpBinaryExpressionBuilder(string parameterName, SqlDbType sqlType, int length, ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ToFieldName(parameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType), typeof(int) });
                var expPrmName = Expression.Constant(dataName, typeof(string));
                var expPrmType = Expression.Constant(sqlType, typeof(SqlDbType));
                var expPrmLength = Expression.Constant((long)length, typeof(long));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType, expPrmLength }));

                var miSet = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetBytes));
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));
                var expFieldOffset = Expression.Constant(0L, typeof(long));
                var expBufOffset = Expression.Constant(0, typeof(int));
                var expLength = Expression.Property(expProperty, typeof(byte[]).GetProperty(nameof(Array.Length)));

                setExpressions.Add(Expression.Condition(
						Expression.Equal(expProperty, Expression.Constant(null, typeof(byte[]))),
						Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) }),
						Expression.Call(expRecord, miSet, new Expression[] { Expression.Constant(ordinal, typeof(int)), expFieldOffset, expProperty, expBufOffset, expLength })
						));
            }
        }
        internal static string ToParameterName(string parameterName)
        {
            if (!string.IsNullOrEmpty(parameterName) && !parameterName.StartsWith("@"))
            {
                parameterName = "@" + parameterName;
            }
            return parameterName;
        }
		internal static string ToFieldName(string parameterName)
		{
			if (!string.IsNullOrEmpty(parameterName) && parameterName.StartsWith("@"))
			{
				parameterName = parameterName.Substring(1);
			}
			return parameterName;
		}
	}
}
