using System;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.Globalization;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This static class provides the logging extension methods for handling table-valued parameter (TVP) mapping.
    /// </summary>
    public static class SqlLoggingExtensions
    {
		public enum SqlEventIdentifier
		{
			MapperTvpCacheStatus,
			MapperTvpTrace,
		}

		private static readonly Action<ILogger, Type, Exception> _sqlTvpCacheMiss;
		private static readonly Action<ILogger, Type, Exception> _sqlTvpCacheHit;
		private static readonly Action<ILogger, string, Exception> _sqlMapperTvpTrace;
		private static readonly Func<ILogger, Type, IDisposable> _buildTvpExpressionsScope;

		static SqlLoggingExtensions()
		{
			_sqlTvpCacheMiss = LoggerMessage.Define<Type>(LogLevel.Information, new EventId((int)SqlEventIdentifier.MapperTvpCacheStatus, nameof(SqlTvpCacheMiss)), "No cached delegate was found for mapping type {modelT} to Sql row metadata; this is normal for the first execution.");
			_sqlTvpCacheHit = LoggerMessage.Define<Type>(LogLevel.Debug, new EventId((int)SqlEventIdentifier.MapperTvpCacheStatus, nameof(SqlTvpCacheHit)), "A cached delegate for mapping type {modelT} to Sql row metadata was found.");
			_sqlMapperTvpTrace = LoggerMessage.Define<string>(LogLevel.Debug, new EventId((int)SqlEventIdentifier.MapperTvpTrace, nameof(TraceTvpMapperProperty)), "Tvp mapper is now processing property {name}.");
			_buildTvpExpressionsScope = LoggerMessage.DefineScope<Type>("Building SqlMetadata convertion logic for model {type}");
		}

		public static void SqlTvpCacheMiss(this ILogger logger, Type modelT)
		{
			_sqlTvpCacheMiss(logger, modelT, null);
		}
		public static void SqlTvpCacheHit(this ILogger logger, Type modelT)
		{
			_sqlTvpCacheHit(logger, modelT, null);
		}
		public static void TraceTvpMapperProperty(this ILogger logger, string propertyName)
		{
			_sqlMapperTvpTrace(logger, propertyName, null);
		}
		public static IDisposable BuildTvpScope(this ILogger logger, Type model)
		{
			return _buildTvpExpressionsScope(logger, model);
		}

	}
}
