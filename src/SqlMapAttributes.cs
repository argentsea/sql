using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Logging;
using ArgentSea;

namespace ArgentSea.Sql
{

    public abstract class SqlParameterMapAttribute : ParameterMapAttribute
    {
        public SqlParameterMapAttribute(string parameterName, SqlDbType sqlType): base(parameterName, (int)sqlType)
        {
        }
        protected internal abstract void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger);
    }

    #region String parameters
    public class MapToSqlNVarCharAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Unicode database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The maximum length of the string. Set to -1 for NVarChar(max).</param>
        public MapToSqlNVarCharAttribute(string parameterName, int length) : base(parameterName, SqlDbType.NVarChar)
        {
            this.Length = length;
        }
        public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNVarCharInParameter), null, expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNVarCharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }

    public class MapToSqlNCharAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Unicode fixed-size database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The length of the fixed-size string.</param>
        public MapToSqlNCharAttribute(string parameterName, int length) : base(parameterName, SqlDbType.NChar)
        {
            this.Length = length;
        }
        public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNCharInParameter), null, expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNCharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }

    public class MapToSqlVarCharAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Ansi database column (note that because .NET is Unicode, NVarChar is recommended for most applications).
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The maximum length of the string. Set to -1 for VarChar(max).</param>
        /// <param name="localeId">The Ansi code-page to use for Unicode text conversion. For en-US use: 1033.</param>
        public MapToSqlVarCharAttribute(string parameterName, int length, int localeId) : base(parameterName, SqlDbType.VarChar)
        {
            this.Length = length;
            this.LocaleId = localeId;
        }
        public int Length { get; private set; }

        public int LocaleId { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarCharInParameter), Expression.Constant(this.LocaleId, typeof(int)), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarCharOutParameter), Expression.Constant(this.Length, typeof(int)), Expression.Constant(this.LocaleId, typeof(int)), parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlCharAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified fixed-size Ansi database column (note that because .NET is Unicode, NChar is recommended for most applications).
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The length of the fixed-size Ansi string.</param>
        /// <param name="localeId">The Ansi code-page to use for Unicode text conversion. For en-US use: 1033.</param>
        public MapToSqlCharAttribute(string parameterName, int length, int localeId) : base(parameterName, SqlDbType.Char)
        {
            this.Length = length;
            this.LocaleId = localeId;
        }
        public int Length { get; private set; }

        public int LocaleId { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlCharInParameter), Expression.Constant(this.LocaleId, typeof(int)), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlCharOutParameter), Expression.Constant(this.Length, typeof(int)), Expression.Constant(this.LocaleId, typeof(int)), parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    #endregion
    #region Number parameters
    public class MapToSqlBigIntAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified BigInt (64-bit) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlBigIntAttribute(string parameterName) : base(parameterName, SqlDbType.BigInt)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(long) 
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(long));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter), typeof(long?), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);
		//=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter), null, null, parameterNames, expLogger, logger);

		protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt64), typeof(long), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);
		//=> TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt64), typeof(long), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetLong), nameof(DbParameterCollectionExtensions.GetNullableLong), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);
		//=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetLong), nameof(DbParameterCollectionExtensions.GetNullableLong), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

		protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(long), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
		//=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
	}
	public class MapToSqlIntAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Int (32-bit) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlIntAttribute(string parameterName) : base(parameterName, SqlDbType.Int)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(int)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(int) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), typeof(int?), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt32), typeof(int), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetInteger), nameof(DbParameterCollectionExtensions.GetNullableInteger), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(int), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlSmallIntAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified SmallInt (16-bit) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlSmallIntAttribute(string parameterName) : base(parameterName, SqlDbType.SmallInt)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(short)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(short) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter), typeof(short?), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt16), typeof(short), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetShort), nameof(DbParameterCollectionExtensions.GetNullableShort), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(short), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlTinyIntAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified TinyInt (unsigned 8-bit) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlTinyIntAttribute(string parameterName) : base(parameterName, SqlDbType.TinyInt)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(byte) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), typeof(byte?), expressions, prms, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetByte), typeof(byte), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetByte), nameof(DbParameterCollectionExtensions.GetNullableByte), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(byte), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlBitAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Bit (boolean) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlBitAttribute(string parameterName) : base(parameterName, SqlDbType.Bit)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(bool) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(bool));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBitInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetBoolean), typeof(bool), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBitOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBoolean), nameof(DbParameterCollectionExtensions.GetNullableBoolean), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);


    }
    public class MapToSqlDecimalAttribute : SqlParameterMapAttribute
    {
        // <summary>
        // Map this property to the specified decimal database column.
        // </summary>
        // <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        // <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>

        /// <summary>
        /// Map this property to the specified decimal database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="precision">The maximum number of digits in the database value.</param>
        /// <param name="scale">The number of digits to the right of the decimal point.</param>
        public MapToSqlDecimalAttribute(string parameterName, byte precision, byte scale) : base(parameterName, SqlDbType.Decimal)
        {
            Precision = precision;
            Scale = scale;
        }

        public byte Scale { get; private set; }

        public byte Precision { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDecimalInParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = ExpressionHelpers.ToFieldName(this.ParameterName);
            if (parameterNames.Add(dataName))
            {
                var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType), typeof(byte), typeof(byte) });
                var expPrmName = Expression.Constant(dataName, typeof(string));
				var expPrmType = Expression.Constant((SqlDbType)this.SqlType, typeof(SqlDbType));
                var expPrecision = Expression.Constant(this.Precision, typeof(byte));
                var expScale = Expression.Constant(this.Scale, typeof(byte));
                sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { expPrmName, expPrmType, expPrecision, expScale }));

                var miSet = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDecimal));
                var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
                var expOrdinal = Expression.Constant(ordinal, typeof(int));
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var piNullableHasValue = propertyType.GetProperty(nameof(Nullable<int>.HasValue));
                    var piNullableGetValue = propertyType.GetProperty(nameof(Nullable<int>.Value));

                    var expIf = Expression.IfThenElse(
                        Expression.Property(expProperty, piNullableHasValue),
                        Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Property(expProperty, piNullableGetValue) }),
                        Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
                        );
                    setExpressions.Add(expIf);
                    logger.SqlExpressionLog(expIf);
                }
                else
                {
                    var expCall = Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty });
                    setExpressions.Add(expCall);
                    logger.SqlExpressionLog(expCall);
                }
            }
        }

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDecimalOutParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expIgnoreParameters, logger);


        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlMoneyAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Money database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlMoneyAttribute(string parameterName) : base(parameterName, SqlDbType.Money)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlMoneyInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDecimal), typeof(decimal), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlMoneyOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlSmallMoneyAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified SmallMoney database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlSmallMoneyAttribute(string parameterName) : base(parameterName, SqlDbType.SmallMoney)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallMoneyInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDecimal), typeof(decimal), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallMoneyOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlFloatAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Float (64-bit floating point or .NET double) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlFloatAttribute(string parameterName) : base(parameterName, SqlDbType.Float)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(double) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(double));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlFloatInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDouble), typeof(double), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlFloatOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDouble), nameof(DbParameterCollectionExtensions.GetNullableDouble), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(double.NaN, typeof(double)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlRealAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Real (32-bit floating point or .NET float) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlRealAttribute(string parameterName) : base(parameterName, SqlDbType.Real)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(float) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(float));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlRealInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetFloat), typeof(float), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlRealOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetFloat), nameof(DbParameterCollectionExtensions.GetNullableFloat), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(float.NaN, typeof(float)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    #endregion
    #region Temporal parameters
    public class MapToSqlDateTimeAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified DateTime database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlDateTimeAttribute(string parameterName) : base(parameterName, SqlDbType.DateTime)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlDateTime2Attribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified DateTime2 database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlDateTime2Attribute(string parameterName) : base(parameterName, SqlDbType.DateTime2)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTime2InParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTime2OutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlDateAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Date database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlDateAttribute(string parameterName) : base(parameterName, SqlDbType.Date)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlTimeAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified Time database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlTimeAttribute(string parameterName) : base(parameterName, SqlDbType.Time)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(TimeSpan) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(TimeSpan));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTimeInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetTimeSpan), typeof(TimeSpan), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetTimeSpan), nameof(DbParameterCollectionExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
	public class MapToSqlDateTimeOffsetAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified DateTimeOffset database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlDateTimeOffsetAttribute(string parameterName) : base(parameterName, SqlDbType.DateTimeOffset)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTimeOffset) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTimeOffset));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOffsetInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTimeOffset), typeof(DateTimeOffset), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOffsetOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTimeOffset), nameof(DbParameterCollectionExtensions.GetNullableDateTimeOffset), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    #endregion
    #region Other parameters
    public class MapToSqlVarBinaryAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified VarBinary database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The maximum length of the binary value or blob. Set to -1 for VarBinary(max).</param>
        public MapToSqlVarBinaryAttribute(string parameterName, int length) : base(parameterName, SqlDbType.VarBinary)
        {
            this.Length = length;
        }
        public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte[]);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarBinaryInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        => TvpExpressionHelpers.TvpBinaryExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarBinaryOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    /// <summary>
    /// Map this property to the specified fixed-size Binary database column.
    /// </summary>
    /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
    /// <param name="length">The size of the binary value.</param>
    public class MapToSqlBinaryAttribute : SqlParameterMapAttribute
    {
        public MapToSqlBinaryAttribute(string parameterName, int length) : base(parameterName, SqlDbType.Binary)
        {
            this.Length = length;
        }
        public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte[]);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        => TvpExpressionHelpers.TvpBinaryExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    public class MapToSqlUniqueIdentifierAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified UniqueIdentifier (Guid) database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        public MapToSqlUniqueIdentifierAttribute(string parameterName) : base(parameterName, SqlDbType.UniqueIdentifier)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(Guid) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(Guid));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, prms, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlUniqueIdentifierInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ParameterName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetGuid), typeof(Guid), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlUniqueIdentifierOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetGuid), nameof(DbParameterCollectionExtensions.GetNullableGuid), expProperty, expressions, expPrms, expPrm, propertyInfo, expLogger, logger);

        protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(Guid.Empty, typeof(Guid)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyInfo.PropertyType, expLogger, logger);
    }
    #endregion
//    #region ShardKey/ShardChild

 //   public class MapToSqlShardKeyAttribute : SqlParameterMapAttribute
 //   {
 //       public DataOrigin Origin { get; set; }
 //       public string ShardParameterName { get; set; }
 //       public string ConcurrencyStampParameterName { get; set; }
 //       public bool IncludeShardNumberInTvp { get; set; }

 //       public SqlDbType ShardNumberSqlType { get; set; }
 //       public SqlDbType RecordIdSqlType { get; set; }
 //       public SqlDbType ConcurrencyStampSqlType { get; set; }

 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardKeyAttribute(char dataOriginValue, string shardParameterName, string parameterName) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardKeyAttribute(char dataOriginValue, string shardParameterName, string parameterName, string concurrencyStampParameterName) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardKeyAttribute(DataOrigin dataOrigin, string shardParameterName, string parameterName) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardKeyAttribute(DataOrigin dataOrigin, string shardParameterName, string parameterName, string concurrencyStampParameterName) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardKeyAttribute(char dataOriginValue, string shardParameterName, string parameterName, bool includeShardNumberInTvp) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardKeyAttribute(char dataOriginValue, string shardParameterName, string parameterName, string concurrencyStampParameterName, bool includeShardNumberInTvp) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardKeyAttribute(DataOrigin dataOrigin, string shardParameterName, string parameterName, bool includeShardNumberInTvp) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardKey properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="parameterName">The name of the parameter or column that contains the record identifier (usually an identity column). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardKeyAttribute(DataOrigin dataOrigin, string shardParameterName, string parameterName, string concurrencyStampParameterName, bool includeShardNumberInTvp) : base(parameterName, SqlDbType.Int)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }

 //       public override bool IsValidType(Type candidateType)
 //           => candidateType == typeof(ShardKey<,>) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(ShardKey<,>));

 //       private (Type shardNumber, Type recordId) GetShardKeyTypes(Type propertyType)
 //       {
 //           if (propertyType == typeof(Nullable<>))
 //           {
 //               propertyType = Nullable.GetUnderlyingType(propertyType);
 //           }
 //           var types = propertyType.GetGenericArguments();
 //           if (types is null || types.Length != 2)
 //           {
 //               throw new Exception("The property is decorated with a ShardKey attribute, but is not a ShardKey type.");
 //           }
 //           return (types[0], types[1]);
 //       }
 //       private (Type shardNumber, Type recordId, Type childId) GetShardChildTypes(Type propertyType)
 //       {
 //           if (propertyType == typeof(Nullable<>))
 //           {
 //               propertyType = Nullable.GetUnderlyingType(propertyType);
 //           }
 //           var types = propertyType.GetGenericArguments();
 //           if (types is null || types.Length != 3)
 //           {
 //               throw new Exception("The property is decorated with a ShardChild attribute, but is not a ShardChild type.");
 //           }
 //           return (types[0], types[1], types[2]);
 //       }
 //       protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {

 //       }

 //       private static string GetCommandByType(Type fieldType)
 //       {
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlFloatInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlRealInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlDecimalInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions. AddTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //           if (fieldType == typeof(int))
 //           {
 //               return nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter);
 //           }
 //       }

 //       private static Expression GetGenericInParameter(Type baseType, string dataShdName, ParameterExpression prms, ParameterExpression expIgnoreParameters)
 //       {
 //           /*
 //* byte/byte?
 //* char/char?
 //* DateTime/DateTime?
 //* DateTimeOffset/DateTimeOffset?
 //* decimal/decimal?
 //* double/double?
 //* float/float?
 //* int/int?
 //* long/long?
 //* sbyte/sbyte?
 //* short/short?
 //* string
 //* TimeSpan/TimeSpan?
 //* uint/uint?
 //* ulong/ulong?
 //* ushort/ushort?
 //            */

 //           var p = new SqlParameter;
 //           p.sq

 //           if (baseType == typeof(byte))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(byte?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(char))
 //           {
 //               //TODO: char or nchar?
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(char?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlCharInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(DateTime))
 //           {
 //               //TODO: Date, DateTime, DateTime2, or Time?
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(DateTime?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(DateTimeOffset))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(DateTimeOffset?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOffsetInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(decimal))
 //           {
 //               //TODO: scale precision?
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(decimal?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDecimalInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(double))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(double?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlFloatInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(float))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(float?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlRealInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(Guid))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(float?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlUniqueIdentifierInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(int))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(int?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(long))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(long?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter), null, null, expIgnoreParameters);
 //           }
 //           //if (baseType == typeof(sbyte))
 //           //{
 //           //    return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(sbyte?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           //}
 //           if (baseType == typeof(short))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(short?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(string))
 //           {
 //               //TODO: nvarchar, varchar, char or nchar? Length?
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(string)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNVarCharInParameter), null, null, expIgnoreParameters);
 //           }
 //           if (baseType == typeof(TimeSpan))
 //           {
 //               return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(TimeSpan?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTimeInParameter), null, null, expIgnoreParameters);
 //           }
 //           //if (baseType == typeof(uint))
 //           //{
 //           //    return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(uint?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           //}
 //           //if (baseType == typeof(ulong))
 //           //{
 //           //    return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(ulong?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           //}
 //           //if (baseType == typeof(ushort))
 //           //{
 //           //    return ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(ushort?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //           //}
 //           throw new Exception("This type is not supported.");
 //       }


 //       private void ShardKeyInParameterExpressions<TShard, TRecord>(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //           where TShard: IComparable where TRecord: IComparable
 //       {
 //           var dataName = ExpressionHelpers.ToFieldName(this.ParameterName);
 //           var dataShdName = ExpressionHelpers.ToParameterName(this.ShardParameterName);
 //           var dataRecName = ExpressionHelpers.ToParameterName(this.ParameterName);
 //           var dataTsName = ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName);
 //           var addShdPrm = parameterNames.Add(dataShdName);
 //           var addRecPrm = parameterNames.Add(dataRecName);
 //           var expSetToNullLabel = Expression.Label($"NullSection{dataName}");
 //           var expNextLabel = Expression.Label($"Exit{dataName}");
 //           MemberExpression expPropValue;


 //           // if property is not null (i.e. Shard.Empty or Shard? = null)
 //           if (propertyType == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(ShardKey<TShard, TRecord>))
 //           {
 //               var expIf = Expression.IfThen(
 //                      Expression.IsFalse(Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.HasValue)))),
 //                      Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expIf);
 //               logger.SqlExpressionLog(expIf);
 //               expPropValue = Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.Value)));
 //           }
 //           else
 //           {
 //               var expIf = Expression.IfThen(
 //                       Expression.Equal(expProperty, Expression.Constant(ShardKey<TShard,TRecord>.Empty, propertyType)),
 //                       Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expIf);
 //               logger.SqlExpressionLog(expIf);
 //               expPropValue = expProperty;
 //           }

 //           //then set values
 //           if (addShdPrm)
 //           {
 //               var expShdId = Expression.Property(expPropValue, typeof(ShardKey<TShard, TRecord>).GetProperty(nameof(ShardKey<TShard, TRecord>.ShardNumber)));
 //               var expShdPrm = ExpressionHelpers.InParmHelper(dataShdName, prms, expShdId, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expShdPrm);
 //               logger.SqlExpressionLog(expShdPrm);
 //           }
 //           if (addRecPrm)
 //           {
 //               var expRecId = Expression.Property(expPropValue, typeof(ShardKey<TShard, TRecord>).GetProperty(nameof(ShardKey<TShard, TRecord>.RecordID)));
 //               var expPrm = ExpressionHelpers.InParmHelper(dataRecName, prms, expRecId, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expPrm);
 //               logger.SqlExpressionLog(expPrm);
 //           }
 //           //if (addTsPrm)
 //           //{
 //           //    var expTS = Expression.Property(expPropValue, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<,>.ConcurrencyStamp)));
 //           //    var expTsPrm = ExpressionHelpers.InParmHelper(dataTsName, prms, expTS, nameof(QueryParameter.AddBinaryInParameter), Expression.Constant(8, typeof(int)), null);
 //           //    expressions.Add(expTsPrm);
 //           //    logger.SqlExpressionLog(expTsPrm);
 //           //}
 //           var expNext = Expression.Goto(expNextLabel);
 //           expressions.Add(expNext);
 //           logger.SqlExpressionLog(expNext);
 //           // else set to null
 //           var expNull = Expression.Label(expSetToNullLabel);
 //           expressions.Add(expNull);
 //           logger.SqlExpressionLog(expNull);
 //           if (addShdPrm)
 //           {
 //               //>TODO vvvvvvvvvvvvvvvv
 //               var expNullShd = ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(byte?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expNullShd);
 //               logger.SqlExpressionLog(expNullShd);
 //           }
 //           if (addRecPrm)
 //           {
 //               //>TODO vvvvvvvvvvvvvvvv
 //               var expNullRec = ExpressionHelpers.InParmHelper(dataRecName, prms, Expression.Constant(null, typeof(int?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expNullRec);
 //               logger.SqlExpressionLog(expNullRec);
 //           }
 //           //if (addTsPrm)
 //           //{
 //           //    var expNullTs = ExpressionHelpers.InParmHelper(dataTsName, prms, Expression.Constant(null, typeof(byte[])), nameof(QueryParameter.AddBinaryInParameter), Expression.Constant(8, typeof(int)), null);
 //           //    expressions.Add(expNullTs);
 //           //    logger.SqlExpressionLog(expNullTs);
 //           //}

 //           //exit
 //           var expExit = Expression.Label(expNextLabel);
 //           expressions.Add(expExit);
 //           logger.SqlExpressionLog(expExit);
 //       }
 //       protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var dataRecName = ExpressionHelpers.ToFieldName(this.ParameterName);
 //           var expSetToNullLabel = Expression.Label($"NullSection{dataRecName}");
 //           var expNextLabel = Expression.Label($"Exit{dataRecName}");
 //           MemberExpression expPropValue;
 //           var startOrdinal = ordinal;

 //           // if property is not null (i.e. Shard.Empty or Shard? = null)
 //           if (propertyType == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(ShardKey<,>))
 //           {
 //               var expIf = Expression.IfThen(
 //                      Expression.IsFalse(Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.HasValue)))),
 //                      Expression.Goto(expSetToNullLabel));
 //               setExpressions.Add(expIf);
 //               logger.SqlExpressionLog(expIf);
 //               expPropValue = Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.Value)));
 //           }
 //           else
 //           {
 //               var expIf = Expression.IfThen(
 //                       Expression.Equal(expProperty, Expression.Constant(ShardKey<int,int>.Empty, propertyType)),
 //                       Expression.Goto(expSetToNullLabel));
 //               setExpressions.Add(expIf);
 //               logger.SqlExpressionLog(expIf);
 //               expPropValue = expProperty;
 //           }
 //           //then set values
 //           if (this.IncludeShardNumberInTvp)
 //           {
 //               var expShdId = Expression.Property(expPropValue, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.ShardNumber)));
 //               if (TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ShardParameterName, SqlDbType.TinyInt, nameof(SqlDataRecord.SetByte), typeof(byte), expRecord, expShdId, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, typeof(byte), expLogger, logger))
 //               {
 //                   ordinal++;
 //               }
 //           }
 //           var expRecId = Expression.Property(expPropValue, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.RecordID)));
 //           if (TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ParameterName, SqlDbType.Int, nameof(SqlDataRecord.SetInt32), typeof(int), expRecord, expRecId, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, typeof(int), expLogger, logger))
 //           {
 //               ordinal++;
 //           }
 //           //if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           //{
 //           //    var expTS = Expression.Property(expPropValue, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<,>.ConcurrencyStamp)));
 //           //    TvpExpressionHelpers.TvpBinaryExpressionBuilder(this.ConcurrencyStampParameterName, SqlDbType.Binary, 8, expRecord, expTS, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, typeof(byte[]), expLogger, logger);
 //           //    ordinal++;
 //           //}
 //           ordinal--;
 //           var expNext = Expression.Goto(expNextLabel);
 //           setExpressions.Add(expNext);
 //           logger.SqlExpressionLog(expNext);
 //           // else set to null
 //           var expSetNull = Expression.Label(expSetToNullLabel);
 //           setExpressions.Add(expSetNull);
 //           logger.SqlExpressionLog(expSetNull);
 //           var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
 //           ordinal = startOrdinal;

 //           if (this.IncludeShardNumberInTvp)
 //           {
 //               var expNullShd = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //               setExpressions.Add(expNullShd);
 //               logger.SqlExpressionLog(expNullShd);
 //               ordinal++;
 //           }
 //           var expNullRec = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //           setExpressions.Add(expNullRec);
 //           logger.SqlExpressionLog(expNullRec);
 //           //if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           //{
 //           //    ordinal++;
 //           //    var expNullTs = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //           //    setExpressions.Add(expNullTs);
 //           //    logger.SqlExpressionLog(expNullTs);
 //           //}

 //           //exit
 //           var expExit = Expression.Label(expNextLabel);
 //           setExpressions.Add(expExit);
 //           logger.SqlExpressionLog(expExit);
 //       }

 //       protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {
 //           ExpressionHelpers.OutParameterBuilder(this.ShardParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);
 //           ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);
 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryOutParameter), Expression.Constant(8, typeof(int)), null, parameterNames, expIgnoreParameters, logger);
 //           }
 //       }

 //       protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
 //       {

 //           var typeShard = typeof(int);
 //           var typeRecord = typeof(int);

 //           var blkExpressions = new List<Expression>();
 //           var expVarShardNumber = Expression.Variable(typeof(byte?), "dataShardNumber");
 //           var expVarRecordId = Expression.Variable(typeof(int?), "recordId");
 //           ParameterExpression expVarConcurrencyStamp = null;

 //           var expExit = Expression.Label($"exit{propertyInfo.Name}");
 //           var miGetParameter = typeof(Mapper).GetMethod(nameof(ExpressionHelpers.GetParameter), BindingFlags.Static | BindingFlags.NonPublic);

 //           var expGetShdPrm = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ShardParameterName), typeof(string))));
 //           blkExpressions.Add(expGetShdPrm);
 //           logger.SqlExpressionLog(expGetShdPrm);
 //           //var expShdLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ShardParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //           //var expShdGotoNotFound = Expression.Goto(expExit);
 //           //var expShdNull = Expression.IfThen(
 //           //    Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //           //        Expression.Block(new Expression[] {
 //           //            expShdLogNotFound,
 //           //            expShdGotoNotFound
 //           //        })
 //           //    );
 //           //blkExpressions.Add(expShdNull);
 //           //logger.SqlExpressionLog(expShdNull);
 //           //logger.SqlExpressionLog(expShdLogNotFound);
 //           //logger.SqlExpressionLog(expShdGotoNotFound);
 //           //logger.SqlExpressionNote("End if");

 //           var expByteGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetNullableByte)), expPrm);
 //           var expUseShardNo = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expShardNumber });
 //           var x = new Nullable<byte>(0);
 //           var expShdNo = Expression.IfThenElse(
 //               Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //               Expression.Assign(expVarShardNumber, expUseShardNo),
 //               Expression.Assign(expVarShardNumber, expByteGet)
 //               );
 //           blkExpressions.Add(expShdNo);
 //           logger.SqlExpressionLog(expShdNo);

 //           var expGetRec = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ParameterName), typeof(string))));
 //           blkExpressions.Add(expGetRec);
 //           logger.SqlExpressionLog(expGetRec);
 //           var expRecLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //           var expRecGotoNotFound = Expression.Goto(expExit);
 //           var expRecNull = Expression.IfThen(
 //               Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //                   Expression.Block(new Expression[] {
 //                       expRecLogNotFound,
 //                       expRecGotoNotFound
 //                   })
 //               );
 //           blkExpressions.Add(expRecNull);
 //           logger.SqlExpressionLog(expRecNull);
 //           logger.SqlExpressionLog(expRecLogNotFound);
 //           logger.SqlExpressionLog(expRecGotoNotFound);
 //           logger.SqlExpressionNote("End if");
 //           var expIntGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetNullableInteger)), expPrm);
 //           var expAssignRec = Expression.Assign(expVarRecordId, expIntGet);
 //           blkExpressions.Add(expAssignRec);
 //           logger.SqlExpressionLog(expAssignRec);

 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               expVarConcurrencyStamp = Expression.Variable(typeof(byte[]), "concurrencyStamp");
 //               var expGetTs = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName), typeof(string))));
 //               blkExpressions.Add(expGetTs);
 //               logger.SqlExpressionLog(expGetTs);
 //               var expTsLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //               var expTsGotoNotFound = Expression.Goto(expExit);
 //               var expTsNull = Expression.IfThen(
 //                   Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //                       Expression.Block(new Expression[] {
 //                           expTsLogNotFound,
 //                           expTsGotoNotFound
 //                       })
 //                   );
 //               blkExpressions.Add(expTsNull);
 //               logger.SqlExpressionLog(expTsNull);
 //               logger.SqlExpressionLog(expTsLogNotFound);
 //               logger.SqlExpressionLog(expTsGotoNotFound);
 //               logger.SqlExpressionNote("End if");
 //               var expBinGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetBytes)), expPrm);
 //               var expAssignTs = Expression.Assign(expVarConcurrencyStamp, expBinGet);
 //               blkExpressions.Add(expAssignTs);
 //               logger.SqlExpressionLog(expAssignTs);
 //           }

 //           var expDataOrigin = Expression.New(typeof(DataOrigin).GetConstructor(new[] { typeof(char) }), new[] { Expression.Constant(this.Origin.SourceIndicator, typeof(char)) });


 //           //var expMkShd = ExpressionHelpers.MakeShardKeyExpressions(propertyInfo.Name, expProperty, expVarShardNumber, expVarRecordId, expVarConcurrencyStamp, expExit, blkExpressions, this.Origin.SourceIndicator, propertyInfo.PropertyType, expLogger, logger);
 //           MethodInfo method = typeof(ExpressionHelpers).GetMethod(nameof(ExpressionHelpers.MakeShardKeyExpressions));
 //           method = method.MakeGenericMethod(new Type[] { typeShard, typeRecord });
 //           var expMkShd = (BlockExpression)method.Invoke(null, new object[] { propertyInfo.Name, expProperty, expVarShardNumber, expVarRecordId, expVarConcurrencyStamp, expExit, blkExpressions, this.Origin.SourceIndicator, propertyInfo.PropertyType, expLogger, logger });

 //           expressions.Add(expMkShd);
 //       }

 //       protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var blkExpressions = new List<Expression>();
 //           var expVarShardNumber = Expression.Variable(typeof(byte?), "prmShardNumber");
 //           var expVarRecordId = Expression.Variable(typeof(int?), "recordId");
 //           ParameterExpression expVarConcurrencyStamp = null;
 //           var expExit = Expression.Label($"exit{propIndex.ToString()}");

 //           //ExpressionHelpers.ReaderSimpleValueExpressions(this.ShardParameterName, expVarShardNumber, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(byte?), expLogger, logger);
 //           //set shard variable
 //           var miGetByteFieldValue = typeof(SqlDataReader).GetMethod(nameof(SqlDataReader.GetFieldValue)).MakeGenericMethod(typeof(byte));
 //           var expGetShdField = Expression.Call(prmSqlRdr, miGetByteFieldValue, new[] { expOrdinal });
 //           var miGetFieldOrdinal = typeof(Mapper).GetMethod(nameof(ExpressionHelpers.GetFieldOrdinal), BindingFlags.NonPublic | BindingFlags.Static);
 //           columnLookupExpressions.Add(Expression.Call(miGetFieldOrdinal, new Expression[] { prmSqlRdr, Expression.Constant(this.ShardParameterName, typeof(string)) }));

 //           var expAssign = Expression.Assign(expOrdinal, Expression.ArrayAccess(expOrdinals, new[] { Expression.Constant(propIndex, typeof(int)) }));
 //           expressions.Add(expAssign);
 //           logger.SqlExpressionLog(expAssign);

 //           var expUseShdPrm = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expGetShdField });
 //           var expUseShardNo = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expShardNumber });
 //           var miIsDbNull = typeof(SqlDataReader).GetMethod(nameof(SqlDataReader.IsDBNull));
 //           var expIf = Expression.IfThenElse(
 //               Expression.Equal(expOrdinal, Expression.Constant(-1, typeof(int))),
 //               Expression.Assign(expVarShardNumber, expUseShardNo),
 //               Expression.IfThenElse(
 //                      Expression.Call(prmSqlRdr, miIsDbNull, new[] { expOrdinal }),
 //                      Expression.Assign(expVarShardNumber, Expression.Constant(null, typeof(byte?))),
 //                      Expression.Assign(expVarShardNumber, expUseShdPrm)
 //                      )
 //               );
 //           blkExpressions.Add(expIf);
 //           logger.SqlExpressionLog(expIf);
 //           propIndex++;

 //           ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expVarRecordId, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(int?), expLogger, logger);

 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               expVarConcurrencyStamp = Expression.Variable(typeof(byte[]), "concurrencyStamp");
 //               propIndex++;
 //               ExpressionHelpers.ReaderSimpleValueExpressions(this.ConcurrencyStampParameterName, expVarConcurrencyStamp, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(byte[]), expLogger, logger);
 //           }

 //           var expDataOrigin = Expression.New(typeof(DataOrigin).GetConstructor(new[] { typeof(char) }), new[] { Expression.Constant(this.Origin.SourceIndicator, typeof(char)) });
 //           var expMkShd = ExpressionHelpers.MakeShardKeyExpressions(propertyInfo.Name, expProperty, expVarShardNumber, expVarRecordId, expVarConcurrencyStamp, expExit, blkExpressions, this.Origin.SourceIndicator, propertyInfo.PropertyType, expLogger, logger);
 //           expressions.Add(expMkShd);
 //           logger.SqlExpressionLog(expMkShd);
 //       }
 //   }

 //   public class MapToSqlShardChildAttribute : SqlParameterMapAttribute
 //   {
 //       public DataOrigin Origin { get; set; }
 //       public string ShardParameterName { get; set; }
 //       public string RecordParameterName { get; set; }
 //       public string ConcurrencyStampParameterName { get; set; }
 //       public bool IncludeShardNumberInTvp { get; set; }

 //       /// <summary>
 //       /// Map the ShardChild properties maps to the specified compound key columns.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardChildAttribute(char dataOriginValue, string shardParameterName, string recordParameterName, string childParameterName) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardChildAttribute(char dataOriginValue, string shardParameterName, string recordParameterName, string childParameterName, string concurrencyStampParameterName) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardChildAttribute(DataOrigin dataOrigin, string shardParameterName, string recordParameterName, string childParameterName) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = false;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns and concurrency stamp column.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       public MapToSqlShardChildAttribute(DataOrigin dataOrigin, string shardParameterName, string recordParameterName, string childParameterName, string concurrencyStampParameterName) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = false;
 //       }

 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns. Optionally includes the ShardNumber column in generated SqlDataRecord objects.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardChildAttribute(char dataOriginValue, string shardParameterName, string recordParameterName, string childParameterName, bool includeShardNumberInTvp) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns. Optionally includes the ShardNumber column in generated SqlDataRecord objects.
 //       /// </summary>
 //       /// <param name="dataOriginValue">A char used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardChildAttribute(char dataOriginValue, string shardParameterName, string recordParameterName, string childParameterName, string concurrencyStampParameterName, bool includeShardNumberInTvp) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = new DataOrigin(dataOriginValue);
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns. Optionally includes the ShardNumber column in generated SqlDataRecord objects.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardChildAttribute(DataOrigin dataOrigin, string shardParameterName, string recordParameterName, string childParameterName, bool includeShardNumberInTvp) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = null;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }
 //       /// <summary>
 //       /// Map the ShardChild properties to the specified compound key columns. Optionally includes the ShardNumber column in generated SqlDataRecord objects.
 //       /// </summary>
 //       /// <param name="dataOrigin">Am object used to distinguish the data origin. For example, customer data could use a 'c' and inventory data might be represented by 'i'. In this way, you can ensure that if inventory IDs are accidentally compared to customer IDs, they do not evaluate as equal.</param>
 //       /// <param name="shardParameterName">The name of the parameter or column that contains the shardNumber. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="recordParameterName">The name of the parameter or column that contains the record Id of a compound key (typically the identity column of the parent record). The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="childParameterName">The name of the parameter or column that contains the second element of a compound key. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="concurrencyStampParameterName">The name of the parameter or column that contains the record concurrency stamp. The system will automatically add or remove the prefix '@' as needed.</param>
 //       /// <param name="includeShardNumberInTvp">Automatically created SqlDataRecord objects (for table valued parameters) do include a ShardNumber field by default. Set this to true to change this behavior.</param>
 //       public MapToSqlShardChildAttribute(DataOrigin dataOrigin, string shardParameterName, string recordParameterName, string childParameterName, string concurrencyStampParameterName, bool includeShardNumberInTvp) : base(childParameterName, SqlDbType.SmallInt)
 //       {
 //           this.ShardParameterName = shardParameterName;
 //           this.RecordParameterName = recordParameterName;
 //           this.ConcurrencyStampParameterName = concurrencyStampParameterName;
 //           this.Origin = dataOrigin;
 //           this.IncludeShardNumberInTvp = includeShardNumberInTvp;
 //       }

 //       public override bool IsValidType(Type candidateType)
 //           => candidateType == typeof(ShardChild<,,>) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(ShardChild<,,>));


 //       protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, MemberExpression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var dataName = ExpressionHelpers.ToFieldName(this.ParameterName);
 //           var dataShdName = ExpressionHelpers.ToParameterName(this.ShardParameterName);
 //           var dataRecName = ExpressionHelpers.ToParameterName(this.RecordParameterName);
 //           var dataChdName = ExpressionHelpers.ToParameterName(this.ParameterName);
 //           var dataTsName = ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName);
 //           var addShdPrm = parameterNames.Add(dataShdName);
 //           var addRecPrm = parameterNames.Add(dataRecName);
 //           var addChdPrm = parameterNames.Add(dataChdName);
 //           var addTsPrm = !string.IsNullOrEmpty(dataTsName) && parameterNames.Add(dataTsName);
 //           var expSetToNullLabel = Expression.Label($"NullSection{dataName}");
 //           var expNextLabel = Expression.Label($"Exit{dataName}");
 //           MemberExpression expPropValue;

 //           // if property is not null (i.e. Shard.Empty or Shard? = null)
 //           if (propertyType == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(ShardChild<,,>))
 //           {
 //               var expShdNull = Expression.IfThen(
 //                      Expression.IsFalse(Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.HasValue)))),
 //                      Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expShdNull);
 //               logger.SqlExpressionLog(expShdNull);
 //               expPropValue = Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.Value)));
 //           }
 //           else
 //           {
 //               var expShdNull = Expression.IfThen(
 //                       Expression.Equal(expProperty, Expression.Constant(ShardChild<int,int,int>.Empty, propertyType)),
 //                       Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expShdNull);
 //               logger.SqlExpressionLog(expShdNull);
 //               expPropValue = expProperty;
 //           }


 //           //then set values
 //           var expShdKey = Expression.Property(expPropValue, typeof(ShardChild<,,>).GetProperty(nameof(ShardChild<int,int,int>.Key)));
 //           if (addShdPrm)
 //           {
 //               var expShdId = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.ShardNumber)));
 //               var expGetShd = ExpressionHelpers.InParmHelper(dataShdName, prms, expShdId, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expGetShd);
 //               logger.SqlExpressionLog(expGetShd);
 //           }
 //           if (addRecPrm)
 //           {
 //               var expRecId = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.RecordID)));
 //               var expGetRec = ExpressionHelpers.InParmHelper(dataRecName, prms, expRecId, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expGetRec);
 //               logger.SqlExpressionLog(expGetRec);
 //           }
 //           if (addChdPrm)
 //           {
 //               var expChdId = Expression.Property(expPropValue, typeof(ShardChild<,,>).GetProperty(nameof(ShardChild<int,int,int>.ChildRecordId)));
 //               var expGetChd = ExpressionHelpers.InParmHelper(dataChdName, prms, expChdId, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expGetChd);
 //               logger.SqlExpressionLog(expGetChd);
 //           }
 //           if (addTsPrm)
 //           {
 //               var expTS = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.ConcurrencyStamp)));
 //               var expGetTs = ExpressionHelpers.InParmHelper(dataTsName, prms, expTS, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryInParameter), Expression.Constant(8, typeof(int)), null, expIgnoreParameters);
 //               expressions.Add(expGetTs);
 //               logger.SqlExpressionLog(expGetTs);
 //           }
 //           var expGotoNext = Expression.Goto(expNextLabel);
 //           expressions.Add(expGotoNext);
 //           logger.SqlExpressionLog(expGotoNext);
 //           // else set to null
 //           var expDoNull = Expression.Label(expSetToNullLabel);
 //           expressions.Add(expDoNull);
 //           logger.SqlExpressionLog(expDoNull);
 //           if (addShdPrm)
 //           {
 //               var expNullShd = ExpressionHelpers.InParmHelper(dataShdName, prms, Expression.Constant(null, typeof(byte?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expNullShd);
 //               logger.SqlExpressionLog(expNullShd);
 //           }
 //           if (addRecPrm)
 //           {
 //               var expNullRec = ExpressionHelpers.InParmHelper(dataRecName, prms, Expression.Constant(null, typeof(int?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), null, null, expIgnoreParameters);
 //               expressions.Add(expNullRec);
 //               logger.SqlExpressionLog(expNullRec);
 //           }
 //           if (addChdPrm)
 //           {
 //               var expNullChd = ExpressionHelpers.InParmHelper(dataChdName, prms, Expression.Constant(null, typeof(short?)), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter), null, null, expIgnoreParameters);

 //               expressions.Add(expNullChd);
 //               logger.SqlExpressionLog(expNullChd);
 //           }
 //           if (addTsPrm)
 //           {
 //               var expNullTs = ExpressionHelpers.InParmHelper(dataTsName, prms, Expression.Constant(null, typeof(byte[])), typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryInParameter), Expression.Constant(8, typeof(int)), null, expIgnoreParameters);
 //               expressions.Add(expNullTs);
 //               logger.SqlExpressionLog(expNullTs);
 //           }
 //           //exit
 //           var expNext = Expression.Label(expNextLabel);
 //           expressions.Add(expNext);
 //           logger.SqlExpressionLog(expNext);
 //       }

 //       protected internal override void AppendTvpExpressions(ParameterExpression expRecord, MemberExpression expProperty, IList<Expression> expressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var dataShdName = ExpressionHelpers.ToFieldName(this.ShardParameterName);
 //           var dataRecName = ExpressionHelpers.ToFieldName(this.RecordParameterName);
 //           var dataChdName = ExpressionHelpers.ToFieldName(this.ParameterName);
 //           var dataTsName = ExpressionHelpers.ToFieldName(this.ConcurrencyStampParameterName);
 //           var expSetToNullLabel = Expression.Label($"NullSection{dataChdName}");
 //           var expNextLabel = Expression.Label($"Exit{dataChdName}");
 //           MemberExpression expPropValue;
 //           var startOrdinal = ordinal;

 //           // if property is not null (i.e. Shard.Empty or Shard? = null)
 //           if (propertyType == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(ShardChild<,,>))
 //           {
 //               var expIfNull = Expression.IfThen(
 //                      Expression.IsFalse(Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.HasValue)))),
 //                      Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expIfNull);
 //               logger.SqlExpressionLog(expIfNull);
 //               expPropValue = Expression.Property(expProperty, propertyType.GetProperty(nameof(Nullable<int>.Value)));
 //           }
 //           else
 //           {
 //               var expIfNull = Expression.IfThen(
 //                       Expression.Equal(expProperty, Expression.Constant(ShardChild<int,int,int>.Empty, propertyType)),
 //                       Expression.Goto(expSetToNullLabel));
 //               expressions.Add(expIfNull);
 //               logger.SqlExpressionLog(expIfNull);
 //               expPropValue = expProperty;
 //           }

 //           //then set values
 //           var expShdKey = Expression.Property(expPropValue, typeof(ShardChild<,,>).GetProperty(nameof(ShardChild<int,int,int>.Key)));
 //           var ctor = typeof(SqlMetaData).GetConstructor(new[] { typeof(string), typeof(SqlDbType) });

 //           if (this.IncludeShardNumberInTvp && parameterNames.Add(dataShdName))
 //           {
 //               var expShdId = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.ShardNumber)));
 //               sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { Expression.Constant(dataShdName, typeof(string)), Expression.Constant(SqlDbType.TinyInt, typeof(SqlDbType)) }));
 //               var expSetShd = Expression.Call(expRecord, typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetByte)), new Expression[] { Expression.Constant(ordinal, typeof(int)), expShdId });
 //               expressions.Add(expSetShd);
 //               logger.SqlExpressionLog(expSetShd);
 //               ordinal++;
 //           }
 //           if (parameterNames.Add(dataRecName))
 //           {
 //               var expRecId = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.RecordID)));
 //               sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { Expression.Constant(dataRecName, typeof(string)), Expression.Constant(SqlDbType.Int, typeof(SqlDbType)) }));
 //               var expSetRec = Expression.Call(expRecord, typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetInt32)), new Expression[] { Expression.Constant(ordinal, typeof(int)), expRecId });
 //               expressions.Add(expSetRec);
 //               logger.SqlExpressionLog(expSetRec);
 //               ordinal++;
 //           }
 //           if (parameterNames.Add(dataChdName))
 //           {
 //               var expChdId = Expression.Property(expPropValue, typeof(ShardChild<,,>).GetProperty(nameof(ShardChild<int,int,int>.ChildRecordId)));
 //               sqlMetaDataTypeExpressions.Add(Expression.New(ctor, new[] { Expression.Constant(dataChdName, typeof(string)), Expression.Constant(SqlDbType.SmallInt, typeof(SqlDbType)) }));
 //               var expSetChd = Expression.Call(expRecord, typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetInt16)), new Expression[] { Expression.Constant(ordinal, typeof(int)), expChdId });
 //               expressions.Add(expSetChd);
 //               logger.SqlExpressionLog(expSetChd);
 //           }
 //           if (!string.IsNullOrEmpty(dataTsName) && parameterNames.Add(dataTsName))
 //           {
 //               parameterNames.Remove(dataTsName);
 //               ordinal++;
 //               var expTS = Expression.Property(expShdKey, typeof(ShardKey<,>).GetProperty(nameof(ShardKey<int,int>.ConcurrencyStamp)));
 //               TvpExpressionHelpers.TvpBinaryExpressionBuilder(dataTsName, SqlDbType.Binary, 8, expRecord, expTS, expressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, typeof(byte[]), expLogger, logger);
 //           }
 //           var expGotoExit = Expression.Goto(expNextLabel);
 //           expressions.Add(expGotoExit);
 //           logger.SqlExpressionLog(expGotoExit);

 //           // else set to null
 //           var expDoNull = Expression.Label(expSetToNullLabel);
 //           expressions.Add(expDoNull);
 //           logger.SqlExpressionLog(expDoNull);
 //           var miDbNull = typeof(SqlDataRecord).GetMethod(nameof(SqlDataRecord.SetDBNull));
 //           ordinal = startOrdinal;

 //           if (this.IncludeShardNumberInTvp)
 //           {
 //               var expNullShd = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //               expressions.Add(expNullShd);
 //               logger.SqlExpressionLog(expNullShd);
 //               ordinal++;
 //           }
 //           var expNullRec = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //           expressions.Add(expNullRec);
 //           logger.SqlExpressionLog(expNullRec);
 //           ordinal++;
 //           var expNullChd = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //           expressions.Add(expNullChd);
 //           logger.SqlExpressionLog(expNullChd);
 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               ordinal++;
 //               var expNullTs = Expression.Call(expRecord, miDbNull, new Expression[] { Expression.Constant(ordinal, typeof(int)) });
 //               expressions.Add(expNullTs);
 //               logger.SqlExpressionLog(expNullTs);
 //           }

 //           var expNext = Expression.Label(expNextLabel);
 //           expressions.Add(expNext);
 //           logger.SqlExpressionLog(expNext);
 //       }

 //       protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression prms, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Type propertyType, ParameterExpression expLogger, ILogger logger)
 //       {
 //           ExpressionHelpers.OutParameterBuilder(this.ShardParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);
 //           ExpressionHelpers.OutParameterBuilder(this.RecordParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);
 //           ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);
 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               ExpressionHelpers.OutParameterBuilder(this.ParameterName, prms, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryOutParameter), Expression.Constant(8, typeof(int)), null, parameterNames, expIgnoreParameters, logger);
 //           }
 //       }

 //       protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var blkExpressions = new List<Expression>();
 //           var expVarShardNumber = Expression.Variable(typeof(byte?), "shardNumber");
 //           var expVarRecordId = Expression.Variable(typeof(int?), "recordId");
 //           var expVarChildId = Expression.Variable(typeof(short?), "childId");
 //           ParameterExpression expVarConcurrencyStamp = null;
 //           var expExit = Expression.Label($"exit{propertyInfo.Name}");
 //           var miGetParameter = typeof(Mapper).GetMethod(nameof(ExpressionHelpers.GetParameter), BindingFlags.Static | BindingFlags.NonPublic);

 //           var expGetShdPrm = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ShardParameterName), typeof(string))));
 //           blkExpressions.Add(expGetShdPrm);
 //           logger.SqlExpressionLog(expGetShdPrm);
 //           //var expShdLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ShardParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //           //var expShdGotoNotFound = Expression.Goto(expExit);
 //           //var expShdPrm = Expression.IfThen(
 //           //    Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //           //        Expression.Block(new Expression[] {
 //           //            expShdLogNotFound,
 //           //            expShdGotoNotFound
 //           //        })
 //           //    );
 //           //blkExpressions.Add(expShdPrm);
 //           //logger.SqlExpressionLog(expShdPrm);
 //           //logger.SqlExpressionLog(expShdLogNotFound);
 //           //logger.SqlExpressionLog(expShdGotoNotFound);
 //           //logger.SqlExpressionNote("End if");

 //           var expByteGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetNullableByte)), expPrm);
 //           var expUseShardNo = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expShardNumber });
 //           var expShdNo = Expression.IfThenElse(
 //               Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //               Expression.Assign(expVarShardNumber, expUseShardNo),
 //               Expression.Assign(expVarShardNumber, expByteGet)
 //               );
 //           blkExpressions.Add(expShdNo);
 //           logger.SqlExpressionLog(expShdNo);

 //           var expGetRec = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.RecordParameterName), typeof(string))));
 //           blkExpressions.Add(expGetRec);
 //           logger.SqlExpressionLog(expGetRec);
 //           var expRecLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.RecordParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //           var expRecGotoNotFound = Expression.Goto(expExit);
 //           var expRecPrm = Expression.IfThen(
 //               Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //                   Expression.Block(new Expression[] {
 //                       expRecLogNotFound,
 //                       expRecGotoNotFound
 //                   })
 //               );
 //           blkExpressions.Add(expRecPrm);
 //           logger.SqlExpressionLog(expRecPrm);
 //           logger.SqlExpressionLog(expRecLogNotFound);
 //           logger.SqlExpressionLog(expRecGotoNotFound);
 //           logger.SqlExpressionNote("End if");

 //           var expIntGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetNullableInteger)), expPrm);
 //           var expAssignRec = Expression.Assign(expVarRecordId, expIntGet);
 //           blkExpressions.Add(expAssignRec);
 //           logger.SqlExpressionLog(expAssignRec);

 //           var expGetChd = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ParameterName), typeof(string))));
 //           blkExpressions.Add(expGetChd);
 //           logger.SqlExpressionLog(expGetChd);
 //           var expChdLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //           var expChdGotoNotFound = Expression.Goto(expExit);
 //           var expChdPrm = Expression.IfThen(
 //               Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //                   Expression.Block(new Expression[] {
 //                       expChdLogNotFound,
 //                       expChdGotoNotFound
 //                   })
 //               );
 //           blkExpressions.Add(expChdPrm);
 //           logger.SqlExpressionLog(expChdPrm);
 //           logger.SqlExpressionLog(expChdLogNotFound);
 //           logger.SqlExpressionLog(expChdGotoNotFound);
 //           logger.SqlExpressionNote("End if");

 //           var expShortGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetNullableShort)), expPrm);
 //           var expAssignChd = Expression.Assign(expVarChildId, expShortGet);
 //           blkExpressions.Add(expAssignChd);
 //           logger.SqlExpressionLog(expAssignChd);

 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               expVarConcurrencyStamp = Expression.Variable(typeof(byte[]), "concurrencyStamp");

 //               var expGetTs = Expression.Assign(expPrm, Expression.Call(miGetParameter, expPrms, Expression.Constant(ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName), typeof(string))));
 //               blkExpressions.Add(expGetTs);
 //               logger.SqlExpressionLog(expGetTs);

 //               var expTsLogNotFound = Expression.Call(typeof(SqlLoggerExtensions).GetMethod(nameof(SqlLoggerExtensions.SqlParameterNotFound)), new Expression[] { expLogger, Expression.Constant(ExpressionHelpers.ToParameterName(this.ConcurrencyStampParameterName), typeof(string)), Expression.Constant(propertyInfo, typeof(PropertyInfo)) });
 //               var expTsGotoNotFound = Expression.Goto(expExit);
 //               var expTsNull = Expression.IfThen(
 //                   Expression.Equal(expPrm, Expression.Constant(null, typeof(SqlParameter))),
 //                       Expression.Block(new Expression[] {
 //                           expTsLogNotFound,
 //                           expTsGotoNotFound
 //                       })
 //                   );
 //               blkExpressions.Add(expTsNull);
 //               logger.SqlExpressionLog(expTsNull);
 //               logger.SqlExpressionLog(expTsLogNotFound);
 //               logger.SqlExpressionLog(expTsGotoNotFound);
 //               logger.SqlExpressionNote("End if");

 //               var expBinGet = Expression.Call(typeof(SqlParameterCollectionExtensions).GetMethod(nameof(SqlParameterCollectionExtensions.GetBytes)), expPrm);
 //               var expAssignTs = Expression.Assign(expVarConcurrencyStamp, expBinGet);
 //               blkExpressions.Add(expAssignTs);
 //               logger.SqlExpressionLog(expAssignTs);
 //           }

 //           var expDataOrigin = Expression.New(typeof(DataOrigin).GetConstructor(new[] { typeof(char) }), new[] { Expression.Constant(this.Origin.SourceIndicator, typeof(char)) });
 //           var expMkChd = ExpressionHelpers.MakeShardChildExpressions(propertyInfo.Name, expProperty, expVarShardNumber, expVarRecordId, expVarChildId, expVarConcurrencyStamp, expExit, blkExpressions, this.Origin.SourceIndicator, propertyInfo.PropertyType, expLogger, logger);
 //           expressions.Add(expMkChd);
 //       }

 //       protected override void AppendReaderExpressions(MemberExpression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, PropertyInfo propertyInfo, ParameterExpression expLogger, ILogger logger)
 //       {
 //           var blkExpressions = new List<Expression>();
 //           var expVarShardNumber = Expression.Variable(typeof(byte?), "prmShardNumber");
 //           var expVarRecordId = Expression.Variable(typeof(int?), "recordId");
 //           var expVarChildId = Expression.Variable(typeof(short?), "childId");
 //           ParameterExpression expVarConcurrencyStamp = null;
 //           var expPrm = Expression.Variable(typeof(SqlParameter), "prm"); //redeclare at block scope
 //           var expExit = Expression.Label($"exit{propIndex.ToString()}");

 //           //set shard variable
 //           var miGetByteFieldValue = typeof(SqlDataReader).GetMethod(nameof(SqlDataReader.GetFieldValue)).MakeGenericMethod(typeof(byte));
 //           var expGetShdField = Expression.Call(prmSqlRdr, miGetByteFieldValue, new[] { expOrdinal });
 //           var miGetFieldOrdinal = typeof(Mapper).GetMethod(nameof(ExpressionHelpers.GetFieldOrdinal), BindingFlags.NonPublic | BindingFlags.Static);
 //           columnLookupExpressions.Add(Expression.Call(miGetFieldOrdinal, new Expression[] { prmSqlRdr, Expression.Constant(this.ShardParameterName, typeof(string)) }));

 //           var expAssign = Expression.Assign(expOrdinal, Expression.ArrayAccess(expOrdinals, new[] { Expression.Constant(propIndex, typeof(int)) }));
 //           expressions.Add(expAssign);
 //           logger.SqlExpressionLog(expAssign);

 //           var expUseShdPrm = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expGetShdField });
 //           var expUseShardNo = Expression.New(typeof(byte?).GetConstructor(new[] { typeof(byte) }), new[] { expShardNumber });
 //           var miIsDbNull = typeof(SqlDataReader).GetMethod(nameof(SqlDataReader.IsDBNull));
 //           var expIf = Expression.IfThenElse(
 //               Expression.Equal(expOrdinal, Expression.Constant(-1, typeof(int))),
 //               Expression.Assign(expVarShardNumber, expUseShardNo),
 //               Expression.IfThenElse(
 //                      Expression.Call(prmSqlRdr, miIsDbNull, new[] { expOrdinal }),
 //                      Expression.Assign(expVarShardNumber, Expression.Constant(null, typeof(byte?))),
 //                      Expression.Assign(expVarShardNumber, expUseShdPrm)
 //                      )
 //               );
 //           blkExpressions.Add(expIf);
 //           logger.SqlExpressionLog(expIf);
 //           //ExpressionHelpers.ReaderSimpleValueExpressions(this.ShardParameterName, expVarShardNumber, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(byte?), expLogger, logger);
 //           propIndex++;

 //           ExpressionHelpers.ReaderSimpleValueExpressions(this.RecordParameterName, expVarRecordId, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(int?), expLogger, logger);
 //           propIndex++;
 //           ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expVarChildId, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(short?), expLogger, logger);

 //           if (!string.IsNullOrEmpty(this.ConcurrencyStampParameterName))
 //           {
 //               expVarConcurrencyStamp = Expression.Variable(typeof(byte[]), "concurrencyStamp");
 //               propIndex++;
 //               ExpressionHelpers.ReaderSimpleValueExpressions(this.ConcurrencyStampParameterName, expVarConcurrencyStamp, columnLookupExpressions, blkExpressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, typeof(byte[]), expLogger, logger);
 //           }

 //           var expDataOrigin = Expression.New(typeof(DataOrigin).GetConstructor(new[] { typeof(char) }), new[] { Expression.Constant(this.Origin.SourceIndicator, typeof(char)) });
 //           var expMkChd = ExpressionHelpers.MakeShardChildExpressions(propertyInfo.Name, expProperty, expVarShardNumber, expVarRecordId, expVarChildId, expVarConcurrencyStamp, expExit, blkExpressions, this.Origin.SourceIndicator, propertyInfo.PropertyType, expLogger, logger);
 //           expressions.Add(expMkChd);
 //           logger.SqlExpressionLog(expMkChd);
 //       }
 //   }
 //   #endregion
}
