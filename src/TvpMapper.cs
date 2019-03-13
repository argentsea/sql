// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

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
using System.Threading;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This class adds the ability to map model properties to SQL table-valued parameters.
    /// </summary>
    public class TvpMapper
    {
        private static ConcurrentDictionary<Type, Lazy<Delegate>> _setTvpParamCache = new ConcurrentDictionary<Type, Lazy<Delegate>>();

        private static Func<TModel, IList<string>, ILogger, SqlDataRecord> BuildTableValuedParameterDelegate<TModel>(ILogger logger) where TModel : class
        {
            var tModel = typeof(TModel);
            var resultExpressions = new List<Expression>();
            var sqlMetaTypeExpressions = new List<NewExpression>();
            var setExpressions = new List<Expression>();
            var variables = new List<ParameterExpression>();
            //ParameterExpression prmUntypedObj = Expression.Parameter(typeof(object), "objModel");
            var prmModel = Expression.Parameter(tModel, "model");
            ParameterExpression expLogger = Expression.Parameter(typeof(ILogger), "logger");
            ParameterExpression prmColumnList = Expression.Parameter(typeof(IList<string>), "columnList");
            var exprInPrms = new ParameterExpression[] { prmModel, prmColumnList, expLogger };

            ConstructorInfo ctorSqlDataRecord = typeof(SqlDataRecord).GetConstructor(new[] { typeof(SqlMetaData[]) });
            var expRecord = Expression.Variable(typeof(SqlDataRecord), "result");
            var noDupPrmNameList = new HashSet<string>();
            var ordinal = 0;

            variables.Add(expRecord);
            using (logger?.BuildTvpScope(tModel))
            {
                IterateTvpProperties(tModel, resultExpressions, setExpressions, sqlMetaTypeExpressions, variables, prmModel, expRecord, prmColumnList, expLogger, noDupPrmNameList, ref ordinal, logger);
                var expNewMetaArray = Expression.NewArrayInit(typeof(SqlMetaData), sqlMetaTypeExpressions.ToArray());
                var expGetDataRecordFields = Expression.Call(typeof(TvpMapper).GetMethod(nameof(TvpMapper.GetDataRecordFields), BindingFlags.NonPublic | BindingFlags.Static), expNewMetaArray, prmColumnList);
                var expNewSqlRecord = Expression.New(ctorSqlDataRecord, expGetDataRecordFields);
                var expAssign = Expression.Assign(expRecord, expNewSqlRecord);
                resultExpressions.Add(expAssign);
                resultExpressions.AddRange(setExpressions);

                resultExpressions.Add(expRecord); //return type

			}
            var expBlock = Expression.Block(variables, resultExpressions);
            logger?.CreatedExpressionTreeForModel(tModel, "SqlDataRecord", expBlock);
            var lambda = Expression.Lambda<Func<TModel, IList<string>, ILogger, SqlDataRecord>>(expBlock, exprInPrms);
            return lambda.Compile();
        }
        private static void IterateTvpProperties(Type tModel, List<Expression> resultExpressions, List<Expression> setExpressions, List<NewExpression> sqlMetaTypeExpressions, List<ParameterExpression> variables, Expression prmModel, ParameterExpression expRecord, ParameterExpression prmColumnList, ParameterExpression expLogger, HashSet<string> noDupPrmNameList, ref int ordinal, ILogger logger)
        {
            var unorderedProps = tModel.GetProperties();
            var props = unorderedProps.OrderBy(prop => prop.MetadataToken);
            var miLogTrace = typeof(SqlLoggingExtensions).GetMethod(nameof(SqlLoggingExtensions.TraceTvpMapperProperty));
            foreach (var prop in props)
            {
                MemberExpression expProperty = Expression.Property(prmModel, prop);
                MemberExpression expOriginalProperty = expProperty;
                var isShardKey = prop.IsDefined(typeof(MapShardKeyAttribute), true);
                var isShardChild = prop.IsDefined(typeof(MapShardChildAttribute), true);

                if ((isShardKey || isShardChild) && prop.IsDefined(typeof(SqlParameterMapAttribute), true))
                {
                    Type propType = prop.PropertyType;
                    var foundShardId = false;
                    var foundRecordId = false;
                    var foundChildId = false;
                    string shardIdPrm;
                    string recordIdPrm;
                    string childIdPrm;
                    setExpressions.Add(Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name)));

                    Expression expDetectNullOrEmpty;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        expProperty = Expression.Property(expProperty, propType.GetProperty(nameof(Nullable<int>.Value)));
                        propType = Nullable.GetUnderlyingType(propType);
                        expDetectNullOrEmpty = Expression.Property(expOriginalProperty, prop.PropertyType.GetProperty(nameof(Nullable<int>.HasValue)));
                    }
                    else
                    {
                        expDetectNullOrEmpty = Expression.NotEqual(expOriginalProperty, Expression.Property(null, propType.GetProperty(nameof(ShardKey<int, int>.Empty))));
                    }

                    if (isShardKey)
                    {
                        var shdData = prop.GetCustomAttribute<MapShardKeyAttribute>(true);
                        shardIdPrm = shdData.ShardIdName;
                        recordIdPrm = shdData.RecordIdName;
                        childIdPrm = null;
                    }
                    else
                    {
                        var shdData = prop.GetCustomAttribute<MapShardChildAttribute>(true);
                        shardIdPrm = shdData.ShardIdName;
                        recordIdPrm = shdData.RecordIdName;
                        childIdPrm = shdData.ChildIdName;
                    }
                    var attrPMs = prop.GetCustomAttributes<SqlParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs)
                    {
                        if (!string.IsNullOrEmpty(shardIdPrm) && attrPM.Name == shardIdPrm)
                        {
                            foundShardId = true;
                            var tDataShardId = propType.GetGenericArguments()[0];
                            if (!attrPM.IsValidType(tDataShardId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expShardProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int>.ShardId)));
                            ParameterExpression expNullableShardId;
                            if (tDataShardId.IsValueType)
                            {
                                expNullableShardId = Expression.Variable(typeof(Nullable<>).MakeGenericType(tDataShardId), prop.Name + "_NullableShardId");
                            }
                            else
                            {
                                expNullableShardId = Expression.Variable(tDataShardId, prop.Name + "_NullableShardId");
                            }
                            variables.Add(expNullableShardId);
                            setExpressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty,
                                Expression.Assign(expNullableShardId, Expression.Convert(expShardProperty, expNullableShardId.Type)),
                                Expression.Assign(expNullableShardId, Expression.Constant(null, expNullableShardId.Type))
                                ));
                            attrPM.AppendTvpExpressions(expRecord, expNullableShardId, setExpressions, sqlMetaTypeExpressions, noDupPrmNameList, ref ordinal, expNullableShardId.Type, prmColumnList, expLogger, logger);
                            ordinal++;
                        }
                        if (attrPM.Name == recordIdPrm)
                        {
                            foundRecordId = true;
                            var tDataRecordId = propType.GetGenericArguments()[1];
                            if (!attrPM.IsValidType(tDataRecordId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expRecordProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int>.RecordId)));
                            ParameterExpression expNullableRecordId;
                            if (tDataRecordId.IsValueType)
                            {
                                expNullableRecordId = Expression.Variable(typeof(Nullable<>).MakeGenericType(tDataRecordId), prop.Name + "_NullableRecordId");
                            }
                            else
                            {
                                expNullableRecordId = Expression.Variable(tDataRecordId, prop.Name + "_NullableRecordId");
                            }
                            variables.Add(expNullableRecordId);
                            setExpressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty,
                                Expression.Assign(expNullableRecordId, Expression.Convert(expRecordProperty, expNullableRecordId.Type)),
                                Expression.Assign(expNullableRecordId, Expression.Constant(null, expNullableRecordId.Type))
                                ));
                            attrPM.AppendTvpExpressions(expRecord, expNullableRecordId, setExpressions, sqlMetaTypeExpressions, noDupPrmNameList, ref ordinal, expNullableRecordId.Type, prmColumnList, expLogger, logger);
                            ordinal++;
                        }
                        if (isShardChild && attrPM.Name == childIdPrm)
                        {
                            foundChildId = true;
                            var tDataChildId = propType.GetGenericArguments()[2];
                            if (!attrPM.IsValidType(tDataChildId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expChildProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardChild<int, int, int>.ChildId)));
                            ParameterExpression expNullableChildId;
                            if (tDataChildId.IsValueType)
                            {
                                expNullableChildId = Expression.Variable(typeof(Nullable<>).MakeGenericType(tDataChildId), prop.Name + "_NullableChildId");
                            }
                            else
                            {
                                expNullableChildId = Expression.Variable(tDataChildId, prop.Name + "_NullableChildId");
                            }
                            variables.Add(expNullableChildId);
                            setExpressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty,
                                Expression.Assign(expNullableChildId, Expression.Convert(expChildProperty, expNullableChildId.Type)),
                                Expression.Assign(expNullableChildId, Expression.Constant(null, expNullableChildId.Type))
                                ));
                            attrPM.AppendTvpExpressions(expRecord, expNullableChildId, setExpressions, sqlMetaTypeExpressions, noDupPrmNameList, ref ordinal, expNullableChildId.Type, prmColumnList, expLogger, logger);
                            ordinal++;
                        }
                    }
                    if (!string.IsNullOrEmpty(shardIdPrm) && !foundShardId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.ShardId, shardIdPrm);
                    }
                    if (!foundRecordId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.RecordId, recordIdPrm);
                    }
                    if (isShardChild && !foundChildId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.ChildId, childIdPrm);
                    }
                }
                else if (prop.IsDefined(typeof(SqlParameterMapAttribute), true))
                {
                    bool alreadyFound = false;
                    var attrPMs = prop.GetCustomAttributes<SqlParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs)
                    {
                        if (alreadyFound)
                        {
                            throw new MultipleMapAttributesException(prop);
                        }
                        alreadyFound = true;
                        var dataName = attrPM.ColumnName;
                        setExpressions.Add(Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name)));
                        if (!attrPM.IsValidType(prop.PropertyType))
                        {
                            throw new ArgentSea.InvalidMapTypeException(prop, attrPM.SqlType);
                        }
                        attrPM.AppendTvpExpressions(expRecord, expProperty, setExpressions, sqlMetaTypeExpressions, noDupPrmNameList, ref ordinal, prop.PropertyType, prmColumnList, expLogger, logger);
                    }
                    ordinal++;
                }
                else if (prop.IsDefined(typeof(MapToModel)) && !prop.PropertyType.IsValueType)
                {
                    IterateTvpProperties(prop.PropertyType, resultExpressions, setExpressions, sqlMetaTypeExpressions, variables, expProperty, expRecord, prmColumnList, expLogger, noDupPrmNameList, ref ordinal, logger);
                }
            }
            if (ordinal == 0)
            {
                throw new NoMappingAttributesFoundException();
            }
        }

        /// <summary>
        /// Converts an object instance to a SqlMetaData instance.
        /// To convert an object list to an table-value input parameter, use: var prm = lst.ConvertAll(x => MapToTableParameterRecord(x));
        /// </summary>
        /// <typeparam name="TModel">The type of the model object. The "MapTo" attributes are used to create the Sql metadata and columns. The object property order become the column order.</typeparam>
        /// <param name="model">An object model instance. The property values are provided as table row values.</param>
        /// <param name="logger"></param>
        /// <returns>A SqlMetaData object. A list of these can be used as a Sql table-valued parameter.</returns>
        public static SqlDataRecord ToTvpRecord<TModel>(TModel model, ILogger logger) where TModel : class, new()
            => ToTvpRecord<TModel>(model, null, logger);

        /// <summary>
        /// Converts an object instance to a SqlMetaData instance.
        /// To convert an object list to an table-value input parameter, use: var prm = lst.ConvertAll(x => MapToTableParameterRecord(x));
        /// </summary>
        /// <typeparam name="TModel">The type of the model object. The "MapTo" attributes are used to create the Sql metadata and columns. The object property order become the column order.</typeparam>
        /// <param name="model">An object model instance. The property values are provided as table row values.</param>
        /// <param name="columnList">A list of column to include in the metadata result. These names must match precisely.</param>
        /// <param name="logger"></param>
        /// <returns>A SqlMetaData object. A list of these can be used as a Sql table-valued parameter.</returns>
        /// <summary>
        public static SqlDataRecord ToTvpRecord<TModel>(TModel model, IList<string> columnList, ILogger logger) where TModel : class, new()
        {
            var tModel = typeof(TModel);

            var lazyTvpParameters = _setTvpParamCache.GetOrAdd(tModel, new Lazy<Delegate>(() => BuildTableValuedParameterDelegate<TModel>(logger), LazyThreadSafetyMode.ExecutionAndPublication));
            if (lazyTvpParameters.IsValueCreated)
            {
                SqlLoggingExtensions.SqlTvpCacheHit(logger, tModel);
            }
            else
            {
                SqlLoggingExtensions.SqlTvpCacheMiss(logger, tModel);
            }
            return ((Func<TModel, IList<string>, ILogger, SqlDataRecord>)lazyTvpParameters.Value)(model, columnList, logger);
        }
        private static SqlMetaData[] GetDataRecordFields(SqlMetaData[] metadataColumns, IList<string> columnList)
        {
            if (columnList is null)
            {
                return metadataColumns;
            }
            var result = new SqlMetaData[columnList.Count];
            for (var i = 0; i < columnList.Count; i++)
            {
                var found = false;
                foreach (var meta in metadataColumns)
                {
                    if (meta.Name == columnList[i])
                    {
                        found = true;
                        result[i] = meta;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception($"Table-valued parameter column “{ columnList[i] }” was specified, but was not found in the model attributes.");
                }
            }
            return result;
        }
    }
}
