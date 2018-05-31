using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MapShardChildAttribute : MapShardChildAttributeBase
	{

		public MapShardChildAttribute(char originValue, string shardIdParameterName, string recordIdParameterName, string childIdParameterName)
			: base(new DataOrigin(originValue), shardIdParameterName, recordIdParameterName, childIdParameterName)
		{

		}
		public MapShardChildAttribute(DataOrigin origin, string shardIdParameterName, string recordIdParameterName, string childIdParameterName)
			: base(origin, shardIdParameterName, recordIdParameterName, childIdParameterName)
		{

		}
		public MapShardChildAttribute(char originValue, string recordIdParameterName, string childIdParameterName)
			: base(new DataOrigin(originValue), null, recordIdParameterName, childIdParameterName)
		{

		}
		public MapShardChildAttribute(DataOrigin origin, string recordIdParameterName, string childIdParameterName)
			: base(origin, null, recordIdParameterName, childIdParameterName)
		{

		}

		public override string ShardIdParameterName
		{
			get
			{
				return base.ShardIdFieldName;
			}
			set
			{
				base.ShardIdFieldName = TvpExpressionHelpers.ToParameterName(value);
			}
		}

		public override string RecordIdParameterName
		{
			get
			{
				return base.RecordIdFieldName;
			}
			set
			{
				base.RecordIdFieldName = TvpExpressionHelpers.ToParameterName(value);
			}
		}
		public override string ChildIdParameterName
		{
			get
			{
				return base.ChildIdFieldName;
			}
			set
			{
				base.ChildIdFieldName = TvpExpressionHelpers.ToParameterName(value);
			}
		}

		public override string ShardIdFieldName
		{
			get
			{
				return base.ShardIdParameterName;
			}
			set
			{
				base.ShardIdParameterName = TvpExpressionHelpers.ToParameterName(value);
			}
		}

		public override string RecordIdFieldName
		{
			get
			{
				return base.RecordIdParameterName;
			}
			set
			{
				base.RecordIdParameterName = TvpExpressionHelpers.ToParameterName(value);
			}
		}

		public override string ChildIdFieldName
		{
			get
			{
				return base.ChildIdParameterName;
			}
			set
			{
				base.ChildIdParameterName = TvpExpressionHelpers.ToParameterName(value);
			}
		}
	}
}
