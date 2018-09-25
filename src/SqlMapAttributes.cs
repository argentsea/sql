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
    /// <summary>
    /// This abstract class is a SQL-specific implementation of the ParameterMapAttribute class.
    /// </summary>
	public abstract class SqlParameterMapAttribute : ParameterMapAttributeBase
    {
        //public SqlParameterMapAttribute(string parameterName, SqlDbType sqlType): base(TvpExpressionHelpers.ToParameterName(parameterName), (int)sqlType)
        public SqlParameterMapAttribute(string parameterName, SqlDbType sqlType) : base(parameterName, (int)sqlType)
        {
        }
		public SqlParameterMapAttribute(string parameterName, SqlDbType sqlType, bool isRequired) : base(parameterName, (int)sqlType, isRequired)
		{
		}
		protected internal abstract void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger);

    }

    #region String parameters
    /// <summary>
    /// This attribute maps a model property to/from a SQL NVarChar parameter or column.
    /// </summary>
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
		public MapToSqlNVarCharAttribute(string parameterName, int length, bool isRequired) : base(parameterName, SqlDbType.NVarChar, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNVarCharInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNVarCharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL NChar parameter or column.
    /// </summary>
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
		public MapToSqlNCharAttribute(string parameterName, int length, bool isRequired) : base(parameterName, SqlDbType.NChar, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNCharInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlNCharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attributes maps a model property to/from a SQL VarChar parameter or column.
    /// </summary>
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
		public MapToSqlVarCharAttribute(string parameterName, int length, int localeId, bool isRequired) : base(parameterName, SqlDbType.VarChar, isRequired)
		{
			this.Length = length;
			this.LocaleId = localeId;
		}
		public int Length { get; private set; }

        public int LocaleId { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarCharInParameter), Expression.Constant(this.LocaleId, typeof(int)), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarCharOutParameter), Expression.Constant(this.Length, typeof(int)), Expression.Constant(this.LocaleId, typeof(int)), parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Char parameter or column.
    /// </summary>
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
		public MapToSqlCharAttribute(string parameterName, int length, int localeId, bool isRequired) : base(parameterName, SqlDbType.Char, isRequired)
		{
			this.Length = length;
			this.LocaleId = localeId;
		}
		public int Length { get; private set; }

        public int LocaleId { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlCharInParameter), Expression.Constant(this.LocaleId, typeof(int)), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpStringExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlCharOutParameter), Expression.Constant(this.Length, typeof(int)), Expression.Constant(this.LocaleId, typeof(int)), parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderStringExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }
    #endregion
    #region Number expSprocParameters

    /// <summary>
    /// This attribute maps a model property to/from a SQL BigInt parameter or column.
    /// </summary>
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
		public MapToSqlBigIntAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.BigInt, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(long) 
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(long));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter), typeof(long?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);
		//=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntInParameter), null, null, parameterNames, expLogger, logger);

		protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt64), typeof(long), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);
		//=> TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt64), typeof(long), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBigIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetLong), nameof(DbParameterCollectionExtensions.GetNullableLong), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);
		//=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetLong), nameof(DbParameterCollectionExtensions.GetNullableLong), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ColumnName, expProperty, typeof(long), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
        //=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Int parameter or column.
    /// </summary>
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
		public MapToSqlIntAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Int, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(int)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(int) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntInParameter), typeof(int?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt32), typeof(int), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetInteger), nameof(DbParameterCollectionExtensions.GetNullableInteger), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ColumnName, expProperty, typeof(int), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL SmallInt parameter or column.
    /// </summary>
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
		public MapToSqlSmallIntAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.SmallInt, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(short)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(short) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntInParameter), typeof(short?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetInt16), typeof(short), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetShort), nameof(DbParameterCollectionExtensions.GetNullableShort), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ColumnName, expProperty, typeof(short), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL TinyInt parameter or column.
    /// </summary>
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
		public MapToSqlTinyIntAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.TinyInt, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte)
            || candidateType.IsEnum
            || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(byte) || Nullable.GetUnderlyingType(candidateType).IsEnum));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntInParameter), typeof(byte?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpEnumXIntExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetByte), typeof(byte), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTinyIntOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetByte), nameof(DbParameterCollectionExtensions.GetNullableByte), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderEnumXIntExpressions(this.ColumnName, expProperty, typeof(byte), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Bit parameter or column.
    /// </summary>
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
		public MapToSqlBitAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Bit, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(bool) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(bool));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBitInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetBoolean), typeof(bool), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBitOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBoolean), nameof(DbParameterCollectionExtensions.GetNullableBoolean), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }

    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Decimal parameter or column.
    /// </summary>
    public class MapToSqlDecimalAttribute : SqlParameterMapAttribute
    {
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
		public MapToSqlDecimalAttribute(string parameterName, byte precision, byte scale, bool isRequired) : base(parameterName, SqlDbType.Decimal, isRequired)
		{
			Precision = precision;
			Scale = scale;
		}

		public byte Scale { get; private set; }

        public byte Precision { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDecimalInParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        {
            var dataName = TvpExpressionHelpers.ToFieldName(this.ParameterName);
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

                    setExpressions.Add(Expression.IfThenElse(
						Expression.Property(expProperty, piNullableHasValue),
						Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, Expression.Property(expProperty, piNullableGetValue) }),
						Expression.Call(expRecord, miDbNull, new Expression[] { expOrdinal })
						));
                }
                else
                {
                    setExpressions.Add(Expression.Call(expRecord, miSet, new Expression[] { expOrdinal, expProperty }));
                }
            }
        }

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDecimalOutParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expIgnoreParameters, logger);


        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Money parameter or column.
    /// </summary>
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
		public MapToSqlMoneyAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Money, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlMoneyInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDecimal), typeof(decimal), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlMoneyOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL SmallMoney parameter or column.
    /// </summary>
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
		public MapToSqlSmallMoneyAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.SmallMoney, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallMoneyInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDecimal), typeof(decimal), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlSmallMoneyOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Float parameter or column.
    /// </summary>
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
		public MapToSqlFloatAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Float, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(double) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(double));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlFloatInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDouble), typeof(double), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlFloatOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDouble), nameof(DbParameterCollectionExtensions.GetNullableDouble), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ColumnName, expProperty, Expression.Constant(double.NaN, typeof(double)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Real parameter or column.
    /// </summary>
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
		public MapToSqlRealAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Real, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(float) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(float));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlRealInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetFloat), typeof(float), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlRealOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetFloat), nameof(DbParameterCollectionExtensions.GetNullableFloat), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ColumnName, expProperty, Expression.Constant(float.NaN, typeof(float)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }
    #endregion
    #region Temporal parameters
    /// <summary>
    /// This attribute maps a model property to/from a SQL DateTime parameter or column.
    /// </summary>
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
		public MapToSqlDateTimeAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.DateTime, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL DateTime2 parameter or column.
    /// </summary>
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
		public MapToSqlDateTime2Attribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.DateTime2, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTime2InParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTime2OutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Date parameter or column.
    /// </summary>
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
		public MapToSqlDateAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Date, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTime) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTime), typeof(DateTime), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Time parameter or column.
    /// </summary>
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
		public MapToSqlTimeAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.Time, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(TimeSpan) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(TimeSpan));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTimeInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetTimeSpan), typeof(TimeSpan), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetTimeSpan), nameof(DbParameterCollectionExtensions.GetNullableTimeSpan), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL DateTimeOffset parameter or column.
    /// </summary>
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
		public MapToSqlDateTimeOffsetAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.DateTimeOffset, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
            => candidateType == typeof(DateTimeOffset) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTimeOffset));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOffsetInParameter), null, null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpSimpleValueExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetDateTimeOffset), typeof(DateTimeOffset), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlDateTimeOffsetOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTimeOffset), nameof(DbParameterCollectionExtensions.GetNullableDateTimeOffset), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }
    #endregion
    #region Other parameters
    /// <summary>
    /// This attribute maps a model property to/from a SQL VarBinary parameter or column.
    /// </summary>
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
		public MapToSqlVarBinaryAttribute(string parameterName, int length, bool isRequired) : base(parameterName, SqlDbType.VarBinary, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte[]);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarBinaryInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        => TvpExpressionHelpers.TvpBinaryExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlVarBinaryOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL Binary parameter or column.
    /// </summary>
    public class MapToSqlBinaryAttribute : SqlParameterMapAttribute
    {
        /// <summary>
        /// Map this property to the specified fixed-size Binary database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The size of the binary value.</param>
        public MapToSqlBinaryAttribute(string parameterName, int length) : base(parameterName, SqlDbType.Binary)
        {
            this.Length = length;
        }
        /// <summary>
        /// Map this property to the specified fixed-size Binary database column.
        /// </summary>
        /// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
        /// <param name="length">The size of the binary value.</param>
        /// <param name="isRequired">When true, set the entire model instance to null if the parameter or column is db null.</param>
        public MapToSqlBinaryAttribute(string parameterName, int length, bool isRequired) : base(parameterName, SqlDbType.Binary, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(byte[]);

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
        => TvpExpressionHelpers.TvpBinaryExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, this.Length, expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlBinaryOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderSimpleValueExpressions(this.ColumnName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }

    /// <summary>
    /// This attribute maps a model property to/from a SQL UniqueIdentifier parameter or column.
    /// </summary>
    public class MapToSqlUniqueIdentifierAttribute : SqlParameterMapAttribute
    {
		/// <summary>
		/// Map this property to the specified UniqueIdentifier (Guid) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToSqlUniqueIdentifierAttribute(string parameterName) : base(parameterName, SqlDbType.UniqueIdentifier, false)
		{
			//
		}
		public MapToSqlUniqueIdentifierAttribute(string parameterName, bool isRequired) : base(parameterName, SqlDbType.UniqueIdentifier, isRequired)
        {
            //
        }

        public override bool IsValidType(Type candidateType)
            => candidateType == typeof(Guid) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(Guid));

        protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlUniqueIdentifierInParameter), null, null, parameterNames, expLogger, logger);


        protected internal override void AppendTvpExpressions(ParameterExpression expRecord, Expression expProperty, IList<Expression> setExpressions, IList<NewExpression> sqlMetaDataTypeExpressions, HashSet<string> parameterNames, ref int ordinal, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => TvpExpressionHelpers.TvpGuidFloatingPointExpressionBuilder(this.ColumnName, (SqlDbType)this.SqlType, nameof(SqlDataRecord.SetGuid), typeof(Guid), expRecord, expProperty, setExpressions, sqlMetaDataTypeExpressions, parameterNames, ref ordinal, propertyType, expLogger, logger);

        protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(SqlParameterCollectionExtensions), nameof(SqlParameterCollectionExtensions.AddSqlUniqueIdentifierOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

        protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetGuid), nameof(DbParameterCollectionExtensions.GetNullableGuid), expProperty, expressions, expSprocParameters, expPrm, propertyType, expLogger, logger);

        protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
            => ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ColumnName, expProperty, Expression.Constant(Guid.Empty, typeof(Guid)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => TvpExpressionHelpers.ToParameterName(base.Name); }

        public override string ColumnName { get => TvpExpressionHelpers.ToFieldName(base.Name); }
    }
    #endregion
}
