using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.Reflection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Globalization;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ArgentSea.Sql
{
    public class TvpMapper
    {
        private static ConcurrentDictionary<Type, Func<object, ILogger, SqlDataRecord>> _setTvpParamCache = new ConcurrentDictionary<Type, Func<object, ILogger, SqlDataRecord>>();

        private static Func<object, ILogger, SqlDataRecord> BuildTableValuedParameterDelegate(Type modelT, ILogger logger)
        {
            var resultExpressions = new List<Expression>();
            var sqlMetaTypeExpressions = new List<NewExpression>();
            var setExpressions = new List<Expression>();

            ParameterExpression prmUntypedObj = Expression.Parameter(typeof(object), "objModel");
            ParameterExpression expLogger = Expression.Parameter(typeof(ILogger), "logger");
            var exprInPrms = new ParameterExpression[] { prmUntypedObj, expLogger };
            var prmModel = Expression.Variable(modelT, "model");

            ConstructorInfo ctorSqlDataRecord = typeof(SqlDataRecord).GetConstructor(new[] { typeof(SqlMetaData[]) });
            var expRecord = Expression.Variable(typeof(SqlDataRecord), "result");
            var noDupPrmNameList = new HashSet<string>();
            var ordinal = 0;

            resultExpressions.Add(Expression.Assign(prmModel, Expression.TypeAs(prmUntypedObj, modelT))); //var model = (modelT)object;

            using (logger.BuildTvpScope(modelT))
            {
                logger.SqlExpressionBlockStart(nameof(TvpMapper.ToTvpRecord), exprInPrms);
                IterateTvpProperties(modelT, resultExpressions, setExpressions, sqlMetaTypeExpressions, prmModel, expRecord, expLogger, noDupPrmNameList, ref ordinal, logger);
                logger.SqlExpressionBlockEnd(nameof(TvpMapper.ToTvpRecord));

                var expNewSqlRecord = Expression.New(ctorSqlDataRecord, Expression.NewArrayInit(typeof(SqlMetaData), sqlMetaTypeExpressions.ToArray()));
                var expAssign = Expression.Assign(expRecord, expNewSqlRecord);
                resultExpressions.Add(expAssign);
                resultExpressions.AddRange(setExpressions);

                resultExpressions.Add(expRecord); //return type
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.SqlExpressionNote($"Out-of-order expressions which should appear at the beginning of {nameof(TvpMapper.ToTvpRecord)}:");
                    foreach (var exp in resultExpressions)
                    {
                        logger.SqlExpressionLog(exp);
                    }
                }
            }
            var expBlock = Expression.Block(new[] { prmModel, expRecord }, resultExpressions);
            var lambda = Expression.Lambda<Func<object, ILogger, SqlDataRecord>>(expBlock, exprInPrms);
            return lambda.Compile();
        }

        private static void IterateTvpProperties(Type modelT, List<Expression> resultExpressions, List<Expression> setExpressions, List<NewExpression> sqlMetaTypeExpressions, Expression prmModel, ParameterExpression expRecord, ParameterExpression expLogger, HashSet<string> noDupPrmNameList, ref int ordinal, ILogger logger)
        {
            var unorderedProps = modelT.GetProperties();
            var props = unorderedProps.OrderBy(prop => prop.MetadataToken);
            var miLogTrace = typeof(SqlLoggingExtensions).GetMethod(nameof(SqlLoggingExtensions.TraceTvpMapperProperty));

            foreach (var prop in props)
            {
                if (prop.IsDefined(typeof(ParameterMapAttribute), true))
                {
                    var attrPMs = prop.GetCustomAttributes<SqlParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs)
                    {
                        var dataName = ExpressionHelpers.ToFieldName(attrPM.ParameterName);

                        var expTrace = Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name));
                        setExpressions.Add(expTrace);
                        logger.SqlExpressionLog(expTrace);

                        if (!attrPM.IsValidType(prop.PropertyType))
                        {
                            throw new ArgentSea.InvalidMapTypeException(prop, attrPM.SqlType);
                        }

                        //var tinfo = prop.PropertyType.GetTypeInfo();
                        MemberExpression expProperty = Expression.Property(prmModel, prop);

                        attrPM.AppendTvpExpressions(expRecord, expProperty, setExpressions, sqlMetaTypeExpressions, noDupPrmNameList, ref ordinal, prop.PropertyType, expLogger, logger);
                    }
                    ordinal++;
                }
                else if (prop.IsDefined(typeof(MapToModel)) && !prop.PropertyType.IsValueType)
                {
                    MemberExpression expProperty = Expression.Property(prmModel, prop);
                    IterateTvpProperties(prop.PropertyType, resultExpressions, setExpressions, sqlMetaTypeExpressions, expProperty, expRecord, expLogger, noDupPrmNameList, ref ordinal, logger);
                }
            }
        }
        /// Converts an object instance to a SqlMetaData instance.
        /// To convert an object list to an table-value input parameter, use: var prm = lst.ConvertAll(x => MapToTableParameterRecord(x));
        /// </summary>
        /// <typeparam name="T">The type of the model object. The "MapTo" attributes are used to create the Sql metadata and columns. The object property order become the column order.</typeparam>
        /// <param name="model">An object model instance. The property values are provided as table row values.</param>
        /// <param name="ignoreFields">A lists of colums that should not be created. Entries should not start with '@'.</param>
        /// <returns>A SqlMetaData object. A list of these can be used as a Sql table-valued parameter.</returns>
        public static SqlDataRecord ToTvpRecord<T>(T model, ILogger logger) where T : class
        {
            var modelT = typeof(T);
            if (!_setTvpParamCache.TryGetValue(modelT, out var SqlTblDelegate))
            {
                SqlLoggingExtensions.SqlTvpCacheMiss(logger, modelT);
                SqlTblDelegate = BuildTableValuedParameterDelegate(modelT, logger);
                if (!_setTvpParamCache.TryAdd(modelT, SqlTblDelegate))
                {
                    SqlTblDelegate = _setTvpParamCache[modelT];
                }
            }
            else
            {
                SqlLoggingExtensions.SqlTvpCacheHit(logger, modelT);
            }
            return SqlTblDelegate(model, logger);
        }
    }
}
