//using System;
//using System.Collections.Generic;
//using System.Text;
//using ArgentSea;

//namespace ArgentSea.Sql
//{
//	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
//	public class MapShardKeyAttribute : MapShardKeyAttributeBase
//	{

//		public MapShardKeyAttribute(char originValue, string shardIdParameterName, string recordIdParameterName)
//			: base(new DataOrigin(originValue), shardIdParameterName, recordIdParameterName)
//		{

//		}
//		public MapShardKeyAttribute(DataOrigin origin, string shardIdParameterName, string recordIdParameterName)
//			: base(origin, shardIdParameterName, recordIdParameterName)
//		{

//		}
//		public MapShardKeyAttribute(char originValue, string recordIdParameterName)
//			: base(new DataOrigin(originValue), null, recordIdParameterName)
//		{

//		}
//		public MapShardKeyAttribute(DataOrigin origin, string recordIdParameterName)
//			: base(origin, null, recordIdParameterName)
//		{

//		}

//		public override string ShardIdParameterName
//		{
//			get
//			{
//				return base.ShardIdFieldName;
//			}
//			set
//			{
//				base.ShardIdFieldName = TvpExpressionHelpers.ToParameterName(value);
//			}
//		}

//		public override string RecordIdParameterName
//		{
//			get
//			{
//				return base.RecordIdFieldName;
//			}
//			set
//			{
//				base.RecordIdFieldName = TvpExpressionHelpers.ToParameterName(value);
//			}
//		}

//		public override string ShardIdFieldName
//		{
//			get
//			{
//				return base.ShardIdParameterName;
//			}
//			set
//			{
//				base.ShardIdParameterName = TvpExpressionHelpers.ToParameterName(value);
//			}
//		}

//		public override string RecordIdFieldName
//		{
//			get
//			{
//				return base.RecordIdName;
//			}
//			set
//			{
//				base.RecordIdName = TvpExpressionHelpers.ToParameterName(value);
//			}
//		}
//	}
//}
