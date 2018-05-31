using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea.Sql;

namespace ArgentSea.Sql.Test
{
	class SqlMapModel
	{
		[MapToSqlInt("ArgentSeaTestDataId")]
		public int ArgentSeaTestDataId { get; set; }

		[MapToSqlNVarChar("Name", 255)]
		public string Name { get; set; }

		[MapToSqlVarChar("LatinName", 255, 1033)]
		public string LatinName { get; set; }

		[MapToSqlNChar("Iso3166", 2)]
		public string Iso3166 { get; set; }

		[MapToSqlChar("Iso639", 2, 1033)]
		public string Iso639 { get; set; }

		[MapToSqlBigInt("BigCount")]
		public long? BigCount { get; set; }

		[MapToSqlInt("@ValueCount")]
		public int? ValueCount { get; set; }

		[MapToSqlSmallInt("SmallCount")]
		public short? SmallCount { get; set; }

		[MapToSqlTinyInt("@ByteValue")] //Note that the parameter name and data name don't match.
		public byte? ByteCount { get; set; }

		[MapToSqlBit("TrueFalse")]
		public bool? TrueFalse { get; set; }

		[MapToSqlUniqueIdentifier("GuidValue")]
		public Guid GuidValue { get; set; }

		[MapToSqlUniqueIdentifier("GuidNull")]
		public Guid? GuidNull { get; set; }

		[MapToSqlDate("Birthday")]
		public DateTime? Birthday { get; set; }

		[MapToSqlDateTime("RightNow")]
		public DateTime? RightNow { get; set; }

		[MapToSqlDateTime2("ExactlyNow")]
		public DateTime? ExactlyNow { get; set; }

		[MapToSqlDateTimeOffset("NowElsewhere")]
		public DateTimeOffset? NowElsewhere { get; set; }

		[MapToSqlTime("WakeUp")]
		public TimeSpan? WakeUp { get; set; }

		[MapToSqlDecimal("Latitude", 9, 6)]
		public decimal? Latitude { get; set; }

		[MapToSqlFloat("Longitude")]
		public double Longitude { get; set; }

		[MapToSqlReal("Altitude")]
		public float Altitude { get; set; }

		[MapToSqlFloat("Ratio")]
		public double? Ratio { get; set; }

		[MapToSqlReal("Temperature")]
		public float? Temperature { get; set; }

		[MapToSqlNVarChar("LongStory", -1)]
		public string LongStory { get; set; }

		[MapToSqlVarChar("LatinStory", -1, 1033)]
		public string LatinStory { get; set; }

		[MapToSqlBinary("TwoBits", 2)]
		public byte[] TwoBits { get; set; }

		[MapToSqlVarBinary("MissingBits", 12)]
		public byte[] MissingBits { get; set; }

		[MapToSqlVarBinary("Blob", -1)]
		public byte[] Blob { get; set; }

		[MapToSqlMoney("Price")]
		public decimal? Price { get; set; }

		[MapToSqlSmallMoney("Cost")]
		public decimal? Cost { get; set; }

		[MapToSqlVarChar("EnvironmentTarget", 7, 1033)]
		public System.EnvironmentVariableTarget EnvTarget { get; set; }

		[MapToSqlSmallInt("ConsoleColor")]
		public ConsoleColor Color { get; set; }

		[MapToSqlNVarChar("ConsoleModifiers", 7)]
		public ConsoleModifiers Modifier { get; set; }

		[MapToSqlTinyInt("DayOfTheWeek")]
		public DayOfWeek? DayOfTheWeek { get; set; }

		[MapToSqlChar("GCNotificationStatus", 16, 1033)]
		public GCNotificationStatus? GarbageCollectorNotificationStatus { get; set; }

		[MapShardKey('x', "DataShard", "DataRecordId")]
		[MapToSqlTinyInt("DataShard")]
		[MapToSqlInt("DataRecordId")]
		public ShardKey<byte, int>? RecordKey { get; set; } = ShardKey<byte, int>.Empty;

		[MapShardChild('y', "ChildShard", "ParentRecordId", "ChildRecordId")]
		[MapToSqlTinyInt("ChildShard")]
		[MapToSqlInt("ParentRecordId")]
		[MapToSqlSmallInt("ChildRecordId")]
		public ShardChild<byte, int, short> RecordChild { get; set; } = ShardChild<byte, int, short>.Empty;


		[MapShardKey('A', "@DataRecordId2")]
		[MapToSqlBigInt("@DataRecordId2")]
		public ShardKey<byte, long> DataShard2 { get; set; } = new ShardKey<byte, long>('A', 123, 54321);

		[MapShardChild('B', "ChildShard2", "ParentRecord2Id", "ChildRecord2Id")]
		[MapToSqlTinyInt("ChildShard2")]
		[MapToSqlSmallInt("ParentRecord2Id")]
		[MapToSqlNVarChar("ChildRecord2Id", 255)]
		public ShardChild<byte, short, string>? ChildShard2 { get; set; } = null;



	}
}

