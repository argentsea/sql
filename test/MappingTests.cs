using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Data.SqlClient;
using FluentAssertions;
using NSubstitute;

namespace ArgentSea.Sql.Test
{
    public class MappingTests
    {
		[Fact]
		public void ValidateInParameterMapper()
		{
			var smv = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = "Test2",
				LatinName = "Test3",
				Iso3166 = "US",
				Iso639 = "en",
				BigCount = 4,
				ValueCount = 5,
				SmallCount = 6,
				ByteCount = 7,
				TrueFalse = true,
				GuidValue = Guid.NewGuid(),
				GuidNull = Guid.NewGuid(),
				Birthday = new DateTime(2008, 8, 8),
				RightNow = new DateTime(2009, 9, 9),
				ExactlyNow = new DateTime(2010, 10, 10),
				NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
				WakeUp = new TimeSpan(12, 12, 12),
				Latitude = 13.13m,
				Longitude = 14.14,
				Altitude = 15.15f,
				Ratio = 16.1,
				Temperature = 32.1f,
				LongStory = "16",
				LatinStory = "17",
				TwoBits = new byte[] { 18, 18 },
				MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 },
				Blob = new byte[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20 },
				Price = 21.0m,
				Cost = 22.0m,
				EnvTarget = EnvironmentVariableTarget.User,
				Color = ConsoleColor.Blue,
				Modifier = ConsoleModifiers.Control,
				DayOfTheWeek = DayOfWeek.Sunday,
				GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
				RecordKey = new ShardKey<byte, int>(new DataOrigin('x'), 2, 1234),
				RecordChild = new ShardChild<byte, int, short>(new DataOrigin('y'), 3, 4567, (short)-23456),
                DataShard2 = new ShardKey<byte, long>('z', (byte)22, 123432L),
                ChildShard2 = new ShardChild<byte, short, string>('!', 255, 255, "testing123")
			};
			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var prms = new QueryParameterCollection();

			prms.CreateInputParameters<SqlMapModel>(smv, dbLogger);

			Assert.True(((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType == System.Data.SqlDbType.Int);
			((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ArgentSeaTestDataId"]).Value.Should().Be(smv.ArgentSeaTestDataId, "that is the assigned value");
			((SqlParameter)prms["@Name"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@Name"]).Value.Should().Be(smv.Name, "that is the assigned value");
			((SqlParameter)prms["@Name"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@LatinName"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinName"]).Value.Should().Be(smv.LatinName, "that is the assigned value");
			((SqlParameter)prms["@LatinName"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@Iso3166"]).SqlDbType.Should().Be(System.Data.SqlDbType.NChar, "that is the correct data type");
			((SqlParameter)prms["@Iso3166"]).Value.Should().Be(smv.Iso3166, "that is the assigned value");
			((SqlParameter)prms["@Iso3166"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@Iso639"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");
			((SqlParameter)prms["@Iso639"]).Value.Should().Be(smv.Iso639, "that is the assigned value");
			((SqlParameter)prms["@Iso639"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@BigCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.BigInt, "that is the correct data type");
			((SqlParameter)prms["@BigCount"]).Value.Should().Be(smv.BigCount, "that is the assigned value");
			((SqlParameter)prms["@ValueCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ValueCount"]).Value.Should().Be(smv.ValueCount, "that is the assigned value");
			((SqlParameter)prms["@SmallCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@SmallCount"]).Value.Should().Be(smv.SmallCount, "that is the assigned value");
			((SqlParameter)prms["@ByteValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@ByteValue"]).Value.Should().Be(smv.ByteCount, "that is the assigned value");
			((SqlParameter)prms["@TrueFalse"]).SqlDbType.Should().Be(System.Data.SqlDbType.Bit, "that is the correct data type");
			((SqlParameter)prms["@TrueFalse"]).Value.Should().Be(smv.TrueFalse, "that is the assigned value");
			((SqlParameter)prms["@GuidValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidValue"]).Value.Should().Be(smv.GuidValue, "that is the assigned value");
			((SqlParameter)prms["@GuidNull"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidNull"]).Value.Should().Be(smv.GuidNull, "that is the assigned value");
			((SqlParameter)prms["@Birthday"]).SqlDbType.Should().Be(System.Data.SqlDbType.Date, "that is the correct data type");
			((SqlParameter)prms["@Birthday"]).Value.Should().Be(smv.Birthday, "that is the assigned value");
			((SqlParameter)prms["@RightNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime, "that is the correct data type");
			((SqlParameter)prms["@RightNow"]).Value.Should().Be(smv.RightNow, "that is the assigned value");
			((SqlParameter)prms["@ExactlyNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime2, "that is the correct data type");
			((SqlParameter)prms["@ExactlyNow"]).Value.Should().Be(smv.ExactlyNow, "that is the assigned value");
			((SqlParameter)prms["@NowElsewhere"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTimeOffset, "that is the correct data type");
			((SqlParameter)prms["@NowElsewhere"]).Value.Should().Be(smv.NowElsewhere, "that is the assigned value");
			((SqlParameter)prms["@WakeUp"]).SqlDbType.Should().Be(System.Data.SqlDbType.Time, "that is the correct data type");
			((SqlParameter)prms["@WakeUp"]).Value.Should().Be(smv.WakeUp, "that is the assigned value");
			((SqlParameter)prms["@Latitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Decimal, "that is the correct data type");
			((SqlParameter)prms["@Latitude"]).Value.Should().Be(smv.Latitude, "that is the assigned value");
			((SqlParameter)prms["@Latitude"]).Precision.Should().Be(9, "that is the specified precision");
			((SqlParameter)prms["@Latitude"]).Scale.Should().Be(6, "that is the specified scale");
			((SqlParameter)prms["@Longitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Longitude"]).Value.Should().Be(smv.Longitude, "that is the assigned value");
			((SqlParameter)prms["@Altitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Altitude"]).Value.Should().Be(smv.Altitude, "that is the assigned value");
			((SqlParameter)prms["@Ratio"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Ratio"]).Value.Should().Be(smv.Ratio, "that is the assigned value");
			((SqlParameter)prms["@Temperature"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Temperature"]).Value.Should().Be(smv.Temperature, "that is the assigned value");
			((SqlParameter)prms["@LongStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@LongStory"]).Value.Should().Be(smv.LongStory, "that is the assigned value");
			((SqlParameter)prms["@LongStory"]).Size.Should().Be(-1, "this is nvarchar(max)");
			((SqlParameter)prms["@LatinStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinStory"]).Value.Should().Be(smv.LatinStory, "that is the assigned value");
			((SqlParameter)prms["@LatinStory"]).Size.Should().Be(-1, "this is varchar(max)");
			((SqlParameter)prms["@TwoBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.Binary, "that is the correct data type");
			((SqlParameter)prms["@TwoBits"]).Value.Should().Be(smv.TwoBits, "that is the assigned value");
			((SqlParameter)prms["@TwoBits"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@MissingBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@MissingBits"]).Value.Should().Be(smv.MissingBits, "that is the assigned value");
			((SqlParameter)prms["@MissingBits"]).Size.Should().Be(12, "that is the max length");
			((SqlParameter)prms["@Blob"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@Blob"]).Value.Should().Be(smv.Blob, "that is the assigned value");
			((SqlParameter)prms["@Blob"]).Size.Should().Be(-1, "this is varbinary(max)");
			((SqlParameter)prms["@Price"]).SqlDbType.Should().Be(System.Data.SqlDbType.Money, "that is the correct data type");
			((SqlParameter)prms["@Price"]).Value.Should().Be(smv.Price, "that is the assigned value");
			((SqlParameter)prms["@Cost"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallMoney, "that is the correct data type");
			((SqlParameter)prms["@Cost"]).Value.Should().Be(smv.Cost, "that is the assigned value");
			((SqlParameter)prms["@EnvironmentTarget"]).Value.Should().Be(smv.EnvTarget.ToString(), "that is the assigned value");
			((SqlParameter)prms["@EnvironmentTarget"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@ConsoleColor"]).Value.Should().Be((short)smv.Color, "that is the assigned value");
			((SqlParameter)prms["@ConsoleColor"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@ConsoleModifiers"]).Value.Should().Be(ConsoleModifiers.Control.ToString(), "that is the assigned value");
			((SqlParameter)prms["@ConsoleModifiers"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@DayOfTheWeek"]).Value.Should().Be((byte)DayOfWeek.Sunday, "that is the assigned value");
			((SqlParameter)prms["@DayOfTheWeek"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@GCNotificationStatus"]).Value.Should().Be(GCNotificationStatus.NotApplicable.ToString(), "that is the assigned value");
			((SqlParameter)prms["@GCNotificationStatus"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");
			((SqlParameter)prms["@DataRecordId"]).Value.Should().Be(smv.RecordKey.Value.RecordId, "that is the assigned value");
			((SqlParameter)prms["@ParentRecordId"]).Value.Should().Be(smv.RecordChild.RecordId, "that is the assigned value");
			((SqlParameter)prms["@ChildRecordId"]).Value.Should().Be(smv.RecordChild.ChildId, "that is the assigned value");
			((SqlParameter)prms["@DataRecordId2"]).Value.Should().Be(smv.DataShard2.RecordId, "that is the assigned value");
			((SqlParameter)prms["@ChildShard2"]).Value.Should().Be(smv.ChildShard2.Value.Key.ShardId, "that is the assigned value");
			((SqlParameter)prms["@ParentRecord2Id"]).Value.Should().Be(smv.ChildShard2.Value.Key.RecordId, "that is the assigned value");
			((SqlParameter)prms["@ChildRecord2Id"]).Value.Should().Be(smv.ChildShard2.Value.ChildId, "that is the assigned value");
		}
		[Fact]
		public void ValidateInParameterNullMapper()
		{
			var smv = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = null,
				LatinName = null,
				Iso3166 = null,
				Iso639 = null,
				BigCount = null,
				ValueCount = null,
				SmallCount = null,
				ByteCount = null,
				TrueFalse = null,
				GuidValue = Guid.Empty,
				GuidNull = null,
				Birthday = null,
				RightNow = null,
				ExactlyNow = null,
				NowElsewhere = null,
				WakeUp = null,
				Latitude = null,
				Longitude = double.NaN,
				Altitude = float.NaN,
				Temperature = null,
				Ratio = null,
				LongStory = null,
				LatinStory = null,
				TwoBits = null,
				MissingBits = null,
				Blob = null,
				Price = null,
				Cost = null,
				EnvTarget = EnvironmentVariableTarget.Machine,
				Color = ConsoleColor.DarkMagenta,
				Modifier = ConsoleModifiers.Shift,
				DayOfTheWeek = DayOfWeek.Saturday,
				GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
                RecordKey = null,
                RecordChild = ShardChild<byte, int, short>.Empty,
                DataShard2 = ShardKey<byte, long>.Empty,
                ChildShard2 = null
            };
			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var prms = new QueryParameterCollection();

			prms.CreateInputParameters<SqlMapModel>(smv, dbLogger);
			Assert.True(((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType == System.Data.SqlDbType.Int);
			((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ArgentSeaTestDataId"]).Value.Should().Be(smv.ArgentSeaTestDataId, "that is the assigned value");
			((SqlParameter)prms["@ArgentSeaTestDataId"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Name"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@Name"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Name"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@Name"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@LatinName"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinName"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@LatinName"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@LatinName"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Iso3166"]).SqlDbType.Should().Be(System.Data.SqlDbType.NChar, "that is the correct data type");
			((SqlParameter)prms["@Iso3166"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Iso3166"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@Iso3166"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Iso639"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");
			((SqlParameter)prms["@Iso639"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Iso639"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@Iso639"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@BigCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.BigInt, "that is the correct data type");
			((SqlParameter)prms["@BigCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@BigCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@ValueCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ValueCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@ValueCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@SmallCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@SmallCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@SmallCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@ByteValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@ByteValue"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@ByteValue"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@TrueFalse"]).SqlDbType.Should().Be(System.Data.SqlDbType.Bit, "that is the correct data type");
			((SqlParameter)prms["@TrueFalse"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@TrueFalse"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@GuidValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidValue"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@GuidValue"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@GuidNull"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidNull"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@GuidNull"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Birthday"]).SqlDbType.Should().Be(System.Data.SqlDbType.Date, "that is the correct data type");
			((SqlParameter)prms["@Birthday"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Birthday"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@RightNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime, "that is the correct data type");
			((SqlParameter)prms["@RightNow"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@RightNow"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@ExactlyNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime2, "that is the correct data type");
			((SqlParameter)prms["@ExactlyNow"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@ExactlyNow"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@NowElsewhere"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTimeOffset, "that is the correct data type");
			((SqlParameter)prms["@NowElsewhere"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@NowElsewhere"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@WakeUp"]).SqlDbType.Should().Be(System.Data.SqlDbType.Time, "that is the correct data type");
			((SqlParameter)prms["@WakeUp"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@WakeUp"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Latitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Decimal, "that is the correct data type");
			((SqlParameter)prms["@Latitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Latitude"]).Precision.Should().Be(9, "that is the specified precision");
			((SqlParameter)prms["@Latitude"]).Scale.Should().Be(6, "that is the specified scale");
			((SqlParameter)prms["@Latitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Longitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Longitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Longitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Altitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Altitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Altitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Ratio"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Ratio"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Ratio"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Temperature"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Temperature"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Temperature"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@LongStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@LongStory"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@LongStory"]).Size.Should().Be(-1, "this is nvarchar(max)");
			((SqlParameter)prms["@LongStory"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@LatinStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinStory"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@LatinStory"]).Size.Should().Be(-1, "this is varchar(max)");
			((SqlParameter)prms["@LatinStory"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@TwoBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.Binary, "that is the correct data type");
			((SqlParameter)prms["@TwoBits"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@TwoBits"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@TwoBits"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@MissingBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@MissingBits"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@MissingBits"]).Size.Should().Be(12, "that is the max length");
			((SqlParameter)prms["@MissingBits"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Blob"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@Blob"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Blob"]).Size.Should().Be(-1, "this is varbinary(max)");
			((SqlParameter)prms["@Blob"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Price"]).SqlDbType.Should().Be(System.Data.SqlDbType.Money, "that is the correct data type");
			((SqlParameter)prms["@Price"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Price"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
			((SqlParameter)prms["@Cost"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallMoney, "that is the correct data type");
			((SqlParameter)prms["@Cost"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
			((SqlParameter)prms["@Cost"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");

			((SqlParameter)prms["@EnvironmentTarget"]).Value.Should().Be(smv.EnvTarget.ToString(), "that is the assigned value");
			((SqlParameter)prms["@EnvironmentTarget"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@ConsoleColor"]).Value.Should().Be((short)smv.Color, "that is the assigned value");
			((SqlParameter)prms["@ConsoleColor"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@ConsoleModifiers"]).Value.Should().Be(ConsoleModifiers.Shift.ToString(), "that is the assigned value");
			((SqlParameter)prms["@ConsoleModifiers"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@DayOfTheWeek"]).Value.Should().Be((byte)DayOfWeek.Saturday, "that is the assigned value");
			((SqlParameter)prms["@DayOfTheWeek"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@GCNotificationStatus"]).Value.Should().Be(GCNotificationStatus.Succeeded.ToString(), "that is the assigned value");
			((SqlParameter)prms["@GCNotificationStatus"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");

            ((SqlParameter)prms["@DataRecordId"]).Value.Should().Be(System.DBNull.Value, "an empty value should create a db null parameter");
            ((SqlParameter)prms["@ParentRecordId"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((SqlParameter)prms["@ChildRecordId"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((SqlParameter)prms["@DataRecordId2"]).Value.Should().Be(System.DBNull.Value, "an empty value should create a db null parameter");
            ((SqlParameter)prms["@ChildShard2"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((SqlParameter)prms["@ParentRecord2Id"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((SqlParameter)prms["@ChildRecord2Id"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");

        }
        [Fact]
		public void ValidateOutParameterCreator()
		{
			var smv = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = null,
				LatinName = null,
				Iso3166 = null,
				Iso639 = null,
				BigCount = null,
				ValueCount = null,
				SmallCount = null,
				ByteCount = null,
				TrueFalse = null,
				GuidValue = Guid.Empty,
				GuidNull = null,
				Birthday = null,
				RightNow = null,
				ExactlyNow = null,
				NowElsewhere = null,
				WakeUp = null,
				Latitude = null,
				Longitude = double.NaN,
				Altitude = float.NaN,
				Ratio = null,
				Temperature = null,
				LongStory = null,
				LatinStory = null,
				TwoBits = null,
				MissingBits = null,
				Blob = null,
				Price = null,
				Cost = null,
				EnvTarget = EnvironmentVariableTarget.Machine,
				Color = ConsoleColor.DarkMagenta,
				Modifier = ConsoleModifiers.Shift,
				DayOfTheWeek = DayOfWeek.Saturday,
				GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
				//RecordKey = new ShardKey<byte, int>(new DataOrigin('9'), 33, int.MinValue),
				//RecordChild = new ShardChild<byte, int, short>(new DataOrigin('A'), 34, 35, -1),
			};
			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var prms = new QueryParameterCollection();

			prms.CreateOutputParameters(typeof(SqlMapModel), dbLogger);

			Assert.True(((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType == System.Data.SqlDbType.Int);
			((SqlParameter)prms["@ArgentSeaTestDataId"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ArgentSeaTestDataId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Name"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@Name"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@Name"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@LatinName"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinName"]).Size.Should().Be(255, "that is the max length");
			((SqlParameter)prms["@LatinName"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Iso3166"]).SqlDbType.Should().Be(System.Data.SqlDbType.NChar, "that is the correct data type");
			((SqlParameter)prms["@Iso3166"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@Iso3166"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Iso639"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");
			((SqlParameter)prms["@Iso639"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@Iso639"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@BigCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.BigInt, "that is the correct data type");
			((SqlParameter)prms["@BigCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@ValueCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
			((SqlParameter)prms["@ValueCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@SmallCount"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@SmallCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@ByteValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@ByteValue"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@TrueFalse"]).SqlDbType.Should().Be(System.Data.SqlDbType.Bit, "that is the correct data type");
			((SqlParameter)prms["@TrueFalse"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@GuidValue"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidValue"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@GuidNull"]).SqlDbType.Should().Be(System.Data.SqlDbType.UniqueIdentifier, "that is the correct data type");
			((SqlParameter)prms["@GuidNull"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Birthday"]).SqlDbType.Should().Be(System.Data.SqlDbType.Date, "that is the correct data type");
			((SqlParameter)prms["@Birthday"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@RightNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime, "that is the correct data type");
			((SqlParameter)prms["@RightNow"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@ExactlyNow"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTime2, "that is the correct data type");
			((SqlParameter)prms["@ExactlyNow"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@NowElsewhere"]).SqlDbType.Should().Be(System.Data.SqlDbType.DateTimeOffset, "that is the correct data type");
			((SqlParameter)prms["@NowElsewhere"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@WakeUp"]).SqlDbType.Should().Be(System.Data.SqlDbType.Time, "that is the correct data type");
			((SqlParameter)prms["@WakeUp"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Latitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Decimal, "that is the correct data type");
			((SqlParameter)prms["@Latitude"]).Precision.Should().Be(9, "that is the specified precision");
			((SqlParameter)prms["@Latitude"]).Scale.Should().Be(6, "that is the specified scale");
			((SqlParameter)prms["@Latitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Longitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Longitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Altitude"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Altitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Ratio"]).SqlDbType.Should().Be(System.Data.SqlDbType.Float, "that is the correct data type");
			((SqlParameter)prms["@Ratio"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Temperature"]).SqlDbType.Should().Be(System.Data.SqlDbType.Real, "that is the correct data type");
			((SqlParameter)prms["@Temperature"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@LongStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@LongStory"]).Size.Should().Be(-1, "this is nvarchar(max)");
			((SqlParameter)prms["@LongStory"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@LatinStory"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@LatinStory"]).Size.Should().Be(-1, "this is varchar(max)");
			((SqlParameter)prms["@LatinStory"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@TwoBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.Binary, "that is the correct data type");
			((SqlParameter)prms["@TwoBits"]).Size.Should().Be(2, "that is the fixed length");
			((SqlParameter)prms["@TwoBits"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@MissingBits"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@MissingBits"]).Size.Should().Be(12, "that is the max length");
			((SqlParameter)prms["@MissingBits"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Blob"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarBinary, "that is the correct data type");
			((SqlParameter)prms["@Blob"]).Size.Should().Be(-1, "this is varbinary(max)");
			((SqlParameter)prms["@Blob"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Price"]).SqlDbType.Should().Be(System.Data.SqlDbType.Money, "that is the correct data type");
			((SqlParameter)prms["@Price"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@Cost"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallMoney, "that is the correct data type");
			((SqlParameter)prms["@Cost"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");

			((SqlParameter)prms["@EnvironmentTarget"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@EnvironmentTarget"]).SqlDbType.Should().Be(System.Data.SqlDbType.VarChar, "that is the correct data type");
			((SqlParameter)prms["@ConsoleColor"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@ConsoleColor"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");
			((SqlParameter)prms["@ConsoleModifiers"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@ConsoleModifiers"]).SqlDbType.Should().Be(System.Data.SqlDbType.NVarChar, "that is the correct data type");
			((SqlParameter)prms["@DayOfTheWeek"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@DayOfTheWeek"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
			((SqlParameter)prms["@GCNotificationStatus"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
			((SqlParameter)prms["@GCNotificationStatus"]).SqlDbType.Should().Be(System.Data.SqlDbType.Char, "that is the correct data type");

            //((SqlParameter)prms["@DataShard"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@DataShard"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
            //((SqlParameter)prms["@DataRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@DataRecordId"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");

            ////((SqlParameter)prms["@ChildShard"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ////((SqlParameter)prms["@ChildShard"]).SqlDbType.Should().Be(System.Data.SqlDbType.TinyInt, "that is the correct data type");
            //((SqlParameter)prms["@ParentRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@ParentRecordId"]).SqlDbType.Should().Be(System.Data.SqlDbType.Int, "that is the correct data type");
            //((SqlParameter)prms["@ChildRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@ChildRecordId"]).SqlDbType.Should().Be(System.Data.SqlDbType.SmallInt, "that is the correct data type");

            //((SqlParameter)prms["@DataRecordId2"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@ChildShard2"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@ParentRecord2Id"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((SqlParameter)prms["@ChildRecord2Id"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
        }
        [Fact]
		public void ValidateOutParameterReader()
		{

			var cmd = new SqlCommand();
			var guid = Guid.NewGuid();

			cmd.Parameters.Add(new SqlParameter("@ArgentSeaTestDataId", System.Data.SqlDbType.Int) { Value = 10, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 255) { Value = "Test2", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@LatinName", System.Data.SqlDbType.VarChar, 255) { Value = "Test3", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Iso3166", System.Data.SqlDbType.NChar, 2) { Value = "US", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Iso639", System.Data.SqlDbType.Char, 2) { Value = "en", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@BigCount", System.Data.SqlDbType.BigInt) { Value = 5L, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@ValueCount", System.Data.SqlDbType.Int) { Value = 6, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@SmallCount", System.Data.SqlDbType.SmallInt) { Value = (short)7, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@ByteValue", System.Data.SqlDbType.TinyInt) { Value = (byte)8, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@TrueFalse", System.Data.SqlDbType.Bit) { Value = false, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@GuidValue", System.Data.SqlDbType.UniqueIdentifier) { Value = guid, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@GuidNull", System.Data.SqlDbType.UniqueIdentifier) { Value = guid, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Birthday", System.Data.SqlDbType.Date) { Value = new DateTime(2018, 1, 1), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@RightNow", System.Data.SqlDbType.DateTime) { Value = new DateTime(2018, 2, 1), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@ExactlyNow", System.Data.SqlDbType.DateTime2) { Value = new DateTime(2018, 3, 1), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@NowElsewhere", System.Data.SqlDbType.DateTimeOffset) { Value = new DateTimeOffset(2018, 1, 1, 1, 1, 1, new TimeSpan()), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@WakeUp", System.Data.SqlDbType.Time) { Value = new TimeSpan(12, 0, 0), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = 12.345m, Precision = 9, Scale = 4, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Longitude", System.Data.SqlDbType.Float) { Value = 123.45, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Altitude", System.Data.SqlDbType.Real) { Value = 1234.5f, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Ratio", System.Data.SqlDbType.Float) { Value = 12345.6, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Temperature", System.Data.SqlDbType.Real) { Value = 123467.8f, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@LongStory", System.Data.SqlDbType.NVarChar) { Value = "Long story....", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@LatinStory", System.Data.SqlDbType.VarChar) { Value = "Long story...", Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@TwoBits", System.Data.SqlDbType.Binary) { Value = new byte[] { 1, 2 }, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@MissingBits", System.Data.SqlDbType.VarBinary) { Value = new byte[] { 1, 2, 4, 8, 16 }, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Blob", System.Data.SqlDbType.VarBinary) { Value = new byte[] { 1, 2, 3, 4, 5 }, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Price", System.Data.SqlDbType.Money) { Value = 1.2345m, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@Cost", System.Data.SqlDbType.SmallMoney) { Value = 12.345m, Direction = System.Data.ParameterDirection.Output });

			cmd.Parameters.Add(new SqlParameter("@EnvironmentTarget", System.Data.SqlDbType.VarChar) { Value = EnvironmentVariableTarget.Machine.ToString(), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@ConsoleColor", System.Data.SqlDbType.SmallInt) { Value = (short)ConsoleColor.Black, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@ConsoleModifiers", System.Data.SqlDbType.NVarChar) { Value = ConsoleModifiers.Control.ToString(), Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@DayOfTheWeek", System.Data.SqlDbType.TinyInt) { Value = (byte)DayOfWeek.Tuesday, Direction = System.Data.ParameterDirection.Output });
			cmd.Parameters.Add(new SqlParameter("@GCNotificationStatus", System.Data.SqlDbType.Char) { Value = GCNotificationStatus.Failed.ToString(), Direction = System.Data.ParameterDirection.Output });

			//cmd.Parameters.Add(new SqlParameter("@DataShard", System.Data.SqlDbType.TinyInt) { Value = (byte)6, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@DataRecordId", System.Data.SqlDbType.Int) { Value = 4, Direction = System.Data.ParameterDirection.Output });

			//cmd.Parameters.Add(new SqlParameter("@ChildShard", System.Data.SqlDbType.TinyInt) { Value = (byte)15, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@ParentRecordId", System.Data.SqlDbType.Int) { Value = 5, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@ChildRecordId", System.Data.SqlDbType.SmallInt) { Value = (short)6, Direction = System.Data.ParameterDirection.Output });

			////cmd.Parameters.Add(new SqlParameter("@DataShard2", System.Data.SqlDbType.TinyInt) { Value = (byte)17, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@DataRecordId2", System.Data.SqlDbType.BigInt) { Value = long.MaxValue, Direction = System.Data.ParameterDirection.Output });

			//cmd.Parameters.Add(new SqlParameter("@ChildShard2", System.Data.SqlDbType.TinyInt) { Value = (byte)255, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@ParentRecord2Id", System.Data.SqlDbType.SmallInt) { Value = (short)12345, Direction = System.Data.ParameterDirection.Output });
			//cmd.Parameters.Add(new SqlParameter("@ChildRecord2Id", System.Data.SqlDbType.NVarChar, 255) { Value = "Test123", Direction = System.Data.ParameterDirection.Output });
			/*
        [MapToShardKey('a', "DataShard", "DataRecordId", "DataTimeStamp")]
        public ShardKey RecordKey { get; set; } = ShardKey.Empty;
             */

			//var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			//var dbLogger = new Microsoft.Extensions.Logging.Console.ConsoleLogger("one";
			var dbLogger2 = new Microsoft.Extensions.Logging.LoggerFactory();
			var dbLogger = dbLogger2.CreateLogger("");
			var result = cmd.Parameters.ToModel<byte, SqlMapModel>((byte)5, dbLogger);
			result.ArgentSeaTestDataId.Should().Be(10, "that was the output parameter value");
			result.Name.Should().Be("Test2", "that was the output parameter value");
			result.LatinName.Should().Be("Test3", "that was the output parameter value");
			result.Iso3166.Should().Be("US", "that was the output parameter value");
			result.Iso639.Should().Be("en", "that was the output parameter value");
			result.BigCount.Should().Be(5L, "that was the output parameter value");
			result.ValueCount.Should().Be(6, "that was the output parameter value");
			result.SmallCount.Should().Be(7, "that was the output parameter value");
			result.ByteCount.Should().Be(8, "that was the output parameter value");
			result.TrueFalse.Should().Be(false, "that was the output parameter value");
			result.GuidValue.Should().Be(guid, "that was the output parameter value");
			result.GuidNull.Should().Be(guid, "that was the output parameter value");
			result.Birthday.Should().Be(new DateTime(2018, 1, 1), "that was the output parameter value");
			result.RightNow.Should().Be(new DateTime(2018, 2, 1), "that was the output parameter value");
			result.ExactlyNow.Should().Be(new DateTime(2018, 3, 1), "that was the output parameter value");
			result.NowElsewhere.Should().Be(new DateTimeOffset(2018, 1, 1, 1, 1, 1, new TimeSpan()), "that was the output parameter value");
			result.WakeUp.Should().Be(new TimeSpan(12, 0, 0), "that was the output parameter value");
			result.Latitude.Should().Be(12.345m, "that was the output parameter value");
			result.Longitude.Should().Be(123.45, "that was the output parameter value");
			result.Altitude.Should().Be(1234.5f, "that was the output parameter value");
			result.Ratio.Should().Be(12345.6, "that was the output parameter value");
			result.Temperature.Should().Be(123467.8f, "that was the output parameter value");
			result.LongStory.Should().Be("Long story....", "that was the output parameter value");
			result.LatinStory.Should().Be("Long story...", "that was the output parameter value");
			result.TwoBits.Should().Equal(new byte[] { 1, 2 }, "that was the output parameter value");
			result.MissingBits.Should().Equal(new byte[] { 1, 2, 4, 8, 16 }, "that was the output parameter value");
			result.Blob.Should().Equal(new byte[] { 1, 2, 3, 4, 5 }, "that was the output parameter value");
			result.Price.Should().Be(1.2345m, "that was the output parameter value");
			result.Cost.Should().Be(12.345m, "that was the output parameter value");

			result.EnvTarget.Should().Be(EnvironmentVariableTarget.Machine, "that was the output parameter value");
			result.Color.Should().Be(ConsoleColor.Black, "that was the output parameter value");
			result.Modifier.Should().Be(ConsoleModifiers.Control, "that was the output parameter value");
			result.DayOfTheWeek.Value.Should().Be(DayOfWeek.Tuesday, "that was the output parameter value");
			result.GarbageCollectorNotificationStatus.Should().Be(GCNotificationStatus.Failed, "that was the output parameter value");

			//result.RecordKey.Value.Origin.SourceIndicator.Should().Be('x', "that is the data origin value");
			//result.RecordKey.Value.ShardId.Should().Be(6, "that was the output parameter value");
			//result.RecordKey.Value.RecordId.Should().Be(4, "that was the output parameter value");

			//result.RecordChild.Key.Origin.SourceIndicator.Should().Be('y', "that is the data origin value");
			//result.RecordChild.Key.ShardId.Should().Be(15, "that was the output parameter value");
			//result.RecordChild.Key.RecordId.Should().Be(5, "that was the output parameter value");
			//result.RecordChild.ChildId.Should().Be(6, "that was the output parameter value");

			//result.DataShard2.Origin.SourceIndicator.Should().Be('A', "that is the data origin value");
			//result.DataShard2.ShardId.Should().Be(5, "that is the value of the current shard");
			//result.DataShard2.RecordId.Should().Be(long.MaxValue, "that is the record id");

			//result.ChildShard2.Value.Origin.SourceIndicator.Should().Be('B', "that is the data origin value");
			//result.ChildShard2.Value.ShardId.Should().Be(255, "that is the value of the current shard");
			//result.ChildShard2.Value.RecordId.Should().Be(12345, "that is the record id");
			//result.ChildShard2.Value.ChildId.Should().Be("Test123", "that is the child id");

		}
		[Fact]
		public void ValidateOutNullParameterReader()
		{

			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var prms = new QueryParameterCollection();
			var guid = Guid.NewGuid();


			prms.Add(new SqlParameter("@ArgentSeaTestDataId", System.Data.SqlDbType.Int) { Value = 11, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 255) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@LatinName", System.Data.SqlDbType.VarChar, 255) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Iso3166", System.Data.SqlDbType.NChar, 2) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Iso639", System.Data.SqlDbType.Char, 2) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@BigCount", System.Data.SqlDbType.BigInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@ValueCount", System.Data.SqlDbType.Int) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@SmallCount", System.Data.SqlDbType.SmallInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@ByteValue", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@TrueFalse", System.Data.SqlDbType.Bit) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@GuidValue", System.Data.SqlDbType.UniqueIdentifier) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@GuidNull", System.Data.SqlDbType.UniqueIdentifier) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Birthday", System.Data.SqlDbType.Date) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@RightNow", System.Data.SqlDbType.DateTime) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@ExactlyNow", System.Data.SqlDbType.DateTime2) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@NowElsewhere", System.Data.SqlDbType.DateTimeOffset) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@WakeUp", System.Data.SqlDbType.Time) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = System.DBNull.Value, Precision = 9, Scale = 4, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Longitude", System.Data.SqlDbType.Float) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Altitude", System.Data.SqlDbType.Real) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Ratio", System.Data.SqlDbType.Float) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Temperature", System.Data.SqlDbType.Real) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@LongStory", System.Data.SqlDbType.NVarChar) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@LatinStory", System.Data.SqlDbType.VarChar) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@TwoBits", System.Data.SqlDbType.Binary) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@MissingBits", System.Data.SqlDbType.VarBinary) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Blob", System.Data.SqlDbType.VarBinary) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Price", System.Data.SqlDbType.Money) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@Cost", System.Data.SqlDbType.SmallMoney) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@EnvironmentTarget", System.Data.SqlDbType.VarChar) { Value = EnvironmentVariableTarget.Process.ToString(), Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@ConsoleColor", System.Data.SqlDbType.SmallInt) { Value = (short)ConsoleColor.DarkBlue, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@ConsoleModifiers", System.Data.SqlDbType.NVarChar) { Value = ConsoleModifiers.Shift.ToString(), Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@DayOfTheWeek", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
			prms.Add(new SqlParameter("@GCNotificationStatus", System.Data.SqlDbType.Char) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@DataShard", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@DataRecordId", System.Data.SqlDbType.Int) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@DataTimeStamp", System.Data.SqlDbType.Binary) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ChildShard", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ParentRecordId", System.Data.SqlDbType.Int) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ChildRecordId", System.Data.SqlDbType.SmallInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@DataShard2", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@DataRecordId2", System.Data.SqlDbType.Int) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ChildShard2", System.Data.SqlDbType.TinyInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ParentRecord2Id", System.Data.SqlDbType.Int) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new SqlParameter("@ChildRecord2Id", System.Data.SqlDbType.SmallInt) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });

            var result = Mapper.ToModel<byte, SqlMapModel>(prms, 16, dbLogger);

			result.ArgentSeaTestDataId.Should().Be(11, "that was the output parameter value");
			result.Name.Should().BeNull("the output parameter was set to DbNull");
			result.LatinName.Should().BeNull("the output parameter was set to DbNull");
			result.Iso3166.Should().BeNull("the output parameter was set to DbNull");
			result.Iso639.Should().BeNull("the output parameter was set to DbNull");
			result.BigCount.Should().BeNull("the output parameter was set to DbNull");
			result.ValueCount.Should().BeNull("the output parameter was set to DbNull");
			result.SmallCount.Should().BeNull("the output parameter was set to DbNull");
			result.ByteCount.Should().BeNull("the output parameter was set to DbNull");
			result.TrueFalse.Should().BeNull("the output parameter was set to DbNull");
			result.GuidValue.Should().Be(Guid.Empty, "the output parameter was set to DbNull");
			result.GuidNull.Should().BeNull("the output parameter was set to DbNull");
			result.Birthday.Should().BeNull("the output parameter was set to DbNull");
			result.RightNow.Should().BeNull("the output parameter was set to DbNull");
			result.ExactlyNow.Should().BeNull("the output parameter was set to DbNull");
			result.NowElsewhere.Should().BeNull("the output parameter was set to DbNull");
			result.WakeUp.Should().BeNull("the output parameter was set to DbNull");
			result.Latitude.Should().BeNull("the output parameter was set to DbNull");
			double.IsNaN(result.Longitude).Should().BeTrue("the output parameter was set to DbNull");
			float.IsNaN(result.Altitude).Should().BeTrue("the output parameter was set to DbNull");
			result.Ratio.Should().BeNull("the output parameter was set to DbNull");
			result.Temperature.Should().BeNull("the output parameter was set to DbNull");
			result.LongStory.Should().BeNull("the output parameter was set to DbNull");
			result.LatinStory.Should().BeNull("the output parameter was set to DbNull");
			result.TwoBits.Should().Equal(null, "the output parameter was set to DbNull");
			result.MissingBits.Should().Equal(null, "the output parameter was set to DbNull");
			result.Blob.Should().Equal(null, "the output parameter was set to DbNull");
			result.Price.Should().BeNull("the output parameter was set to DbNull");
			result.Cost.Should().BeNull("the output parameter was set to DbNull");
			result.EnvTarget.Should().Be(EnvironmentVariableTarget.Process, "that was the output parameter value");
			result.Color.Should().Be(ConsoleColor.DarkBlue, "that was the output parameter value");
			result.Modifier.Should().Be(ConsoleModifiers.Shift, "that was the output parameter value");
			result.DayOfTheWeek.HasValue.Should().BeFalse("the output parameter was set to DbNull");
			result.GarbageCollectorNotificationStatus.HasValue.Should().BeFalse("the output parameter was set to DbNull");
            //result.RecordKey.HasValue.Should().BeFalse("because the input arguments are dbNull");
            //result.RecordChild.Origin.SourceIndicator.Should().Be('0', "that is the empty data origin value");
            //result.RecordChild.Key.ShardId.Should().Be(0, "that is the empty shardchild value");
            //result.RecordChild.Key.RecordId.Should().Be(0, "that is the empty shardchild value");
            //result.RecordChild.ChildId.Should().Be(0, "that is the empty shardchild value");
            //result.DataShard2.Origin.SourceIndicator.Should().Be('0', "that is the empty data origin value");
            //result.DataShard2.ShardId.Should().Be(0, "that is the empty shardKey value");
            //result.DataShard2.RecordId.Should().Be(0, "that is the empty shardKey value");
            //result.ChildShard2.Should().BeNull("because the data values are dbNull");
        }
		[Fact]
		public void ValidateSqlMetadataMapper()
		{
			var smv = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = "Test2",
				LatinName = "Test3",
				Iso3166 = "US",
				Iso639 = "en",
				BigCount = 4,
				ValueCount = 5,
				SmallCount = 6,
				ByteCount = 7,
				TrueFalse = true,
				GuidValue = Guid.NewGuid(),
				GuidNull = Guid.NewGuid(),
				Birthday = new DateTime(2008, 8, 8),
				RightNow = new DateTime(2009, 9, 9),
				ExactlyNow = new DateTime(2010, 10, 10),
				NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
				WakeUp = new TimeSpan(12, 12, 12),
				Latitude = 13.13m,
				Longitude = 14.14,
				Altitude = 15.15f,
				Ratio = 16.5,
				Temperature = 17.6f,
				LongStory = "18",
				LatinStory = "19",
				TwoBits = new byte[] { 20, 20 },
				MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21 },
				Blob = new byte[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22 },
				Price = 23.0m,
				Cost = 24.0m,
				EnvTarget = EnvironmentVariableTarget.User,
				Color = ConsoleColor.Red,
				Modifier = ConsoleModifiers.Shift,
				DayOfTheWeek = DayOfWeek.Wednesday,
				GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
                RecordKey = new ShardKey<byte, int>(new DataOrigin('?'), (byte)254, int.MaxValue),
                //RecordChild = new ShardChild<byte, int, short>(new DataOrigin('!'), (byte)0, 35, short.MinValue),
                //DataShard2 = new ShardKey<byte, long>(new DataOrigin('*'), (byte)0, 123L),
                //ChildShard2 = new Nullable<ShardChild<byte, short, string>>(new ShardChild<byte, short, string>(new DataOrigin('@'), (byte)200, (short)1234, "testing..."))
            };
			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();

			var result = TvpMapper.ToTvpRecord<SqlMapModel>(smv, dbLogger);

			result.GetInt32(0).Should().Be(smv.ArgentSeaTestDataId, "that was the id value provided");
			result.GetString(1).Should().Be(smv.Name, "that was the Name value provided");
			result.GetString(2).Should().Be(smv.LatinName, "that was the Ansi value provided");
			result.GetString(3).Should().Be(smv.Iso3166, "that was the Iso3166 value provided");
			result.GetString(4).Should().Be(smv.Iso639, "that was the Iso639 value provided");
			result.GetInt64(5).Should().Be(smv.BigCount, "that was the value provided");
			result.GetInt32(6).Should().Be(smv.ValueCount, "that was the value provided");
			result.GetInt16(7).Should().Be(smv.SmallCount, "that was the value provided");
			result.GetByte(8).Should().Be(smv.ByteCount, "that was the value provided");
			result.GetBoolean(9).Should().Be(smv.TrueFalse.Value, "that was the value provided");
			result.GetGuid(10).Should().Be(smv.GuidValue, "that was the value provided");
			result.GetGuid(11).Should().Be(smv.GuidNull.Value, "that was the value provided");
			result.GetDateTime(12).Should().Be(smv.Birthday.Value, "that was the value provided");
			result.GetDateTime(13).Should().Be(smv.RightNow.Value, "that was the value provided");
			result.GetDateTime(14).Should().Be(smv.ExactlyNow.Value, "that was the value provided");
			result.GetDateTimeOffset(15).Should().Be(smv.NowElsewhere.Value, "that was the value provided");
			result.GetTimeSpan(16).Should().Be(smv.WakeUp.Value, "that was the value provided");
			result.GetDecimal(17).Should().Be(smv.Latitude, "that was the value provided");
			result.GetDouble(18).Should().Be(smv.Longitude, "that was the value provided");
			result.GetFloat(19).Should().Be(smv.Altitude, "that was the value provided");
			result.GetDouble(20).Should().Be(smv.Ratio, "that was the value provided");
			result.GetFloat(21).Should().Be(smv.Temperature, "that was the value provided");
			result.GetString(22).Should().Be(smv.LongStory, "that was the value provided");
			result.GetString(23).Should().Be(smv.LatinStory, "that was the value provided");
			var twoBits = new byte[smv.TwoBits.Length];
			result.GetBytes(24, 0, twoBits, 0, twoBits.Length);
			for (var i = 0; i < twoBits.Length; i++)
			{
				twoBits[i].Should().Be(smv.TwoBits[i], "that was the value provided");
			}
			var missingBits = new byte[smv.MissingBits.Length];
			result.GetBytes(25, 0, missingBits, 0, missingBits.Length);
			for (var i = 0; i < missingBits.Length; i++)
			{
				missingBits[i].Should().Be(smv.MissingBits[i], "that was the value provided");
			}
			var blob = new byte[smv.Blob.Length];
			result.GetBytes(26, 0, blob, 0, blob.Length);
			for (var i = 0; i < blob.Length; i++)
			{
				blob[i].Should().Be(smv.Blob[i], "that was the value provided");
			}
			result.GetDecimal(27).Should().Be(smv.Price, "that was the value provided");
			result.GetDecimal(28).Should().Be(smv.Cost, "that was the value provided");
			result.GetString(29).Should().Be(EnvironmentVariableTarget.User.ToString(), "that was the value provided");
			result.GetInt16(30).Should().Be((short)ConsoleColor.Red, "that was the value provided");
			result.GetString(31).Should().Be(ConsoleModifiers.Shift.ToString(), "that was the value provided");
			result.GetByte(32).Should().Be((byte)DayOfWeek.Wednesday, "that was the value provided");
			result.GetString(33).Should().Be(GCNotificationStatus.NotApplicable.ToString(), "that was the value provided");
            ////result.GetByte(34).Should().Be((byte)254, "that was the value provided");
            //result.GetInt32(34).Should().Be(int.MaxValue, "that was the value provided");
            ////result.GetByte(36).Should().Be((byte)0, "that was the value provided");
            //result.GetInt32(35).Should().Be(35, "that was the value provided");
            //result.GetInt16(36).Should().Be(short.MinValue, "that was the value provided");
            //result.GetByte(37).Should().Be(0, "that was the value provided");
            //result.GetInt32(38).Should().Be(123, "that was the value provided");
            //result.GetByte(39).Should().Be((byte)200, "that was the value provided");
            //result.GetInt32(40).Should().Be(1234, "that was the value provided");
            //result.GetInt16(41).Should().Be((short)5678, "that was the value provided");
        }
		[Fact]
		public void ValidateSqlMetadataNullMapper()
		{
			var smv = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = null,
				LatinName = null,
				Iso3166 = null,
				Iso639 = null,
				BigCount = null,
				ValueCount = null,
				SmallCount = null,
				ByteCount = null,
				TrueFalse = null,
				GuidValue = Guid.Empty,
				GuidNull = null,
				Birthday = null,
				RightNow = null,
				ExactlyNow = null,
				NowElsewhere = null,
				WakeUp = null,
				Latitude = null,
				Longitude = double.NaN,
				Altitude = float.NaN,
				Ratio = null,
				Temperature = null,
				LongStory = null,
				LatinStory = null,
				TwoBits = null,
				MissingBits = null,
				Blob = null,
				Price = null,
				Cost = null,
				EnvTarget = EnvironmentVariableTarget.User,
				Color = ConsoleColor.Red,
				Modifier = ConsoleModifiers.Shift,
				DayOfTheWeek = null,
				GarbageCollectorNotificationStatus = null
				//RecordKey = ShardKey.Empty,
				//RecordChild = ShardChild.Empty,
				//RecordKeyTwo = null,
				//RecordChild2 = null
			};
			var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var prms = new QueryParameterCollection();

			var result = TvpMapper.ToTvpRecord<SqlMapModel>(smv, dbLogger);

			result.GetInt32(0).Should().Be(smv.ArgentSeaTestDataId, "that was the id value provided");
			result.IsDBNull(1).Should().Be(true, "a null Name property was provided");
			result.IsDBNull(2).Should().Be(true, "a null LatinName property was provided");
			result.IsDBNull(3).Should().Be(true, "a null Iso3166 property was provided");
			result.IsDBNull(4).Should().Be(true, "a null Iso639 property was provided");
			result.IsDBNull(5).Should().Be(true, "a null BigCount property was provided");
			result.IsDBNull(6).Should().Be(true, "a null ValueCount property was provided");
			result.IsDBNull(7).Should().Be(true, "a null SmallCount property was provided");
			result.IsDBNull(8).Should().Be(true, "a null ByteValue property was provided");
			result.IsDBNull(9).Should().Be(true, "a null TrueFalse property was provided");
			result.IsDBNull(10).Should().Be(true, "an empty GuidValue property was provided");
			result.IsDBNull(11).Should().Be(true, "an null GuidNull property was provided");
			result.IsDBNull(12).Should().Be(true, "a null Birthday property was provided");
			result.IsDBNull(13).Should().Be(true, "a null RightNow property was provided");
			result.IsDBNull(14).Should().Be(true, "a null ExactlyNow property was provided");
			result.IsDBNull(15).Should().Be(true, "a null NowElsewhere property was provided");
			result.IsDBNull(16).Should().Be(true, "a null WakeUp property was provided");
			result.IsDBNull(17).Should().Be(true, "a null Latitude property was provided");
			result.IsDBNull(18).Should().Be(true, "a NaN Longitude property was provided");
			result.IsDBNull(19).Should().Be(true, "a NaN Altitude property was provided");
			result.IsDBNull(20).Should().Be(true, "a null Ratio property was provided");
			result.IsDBNull(21).Should().Be(true, "a null Temperature property was provided");
			result.IsDBNull(22).Should().Be(true, "a null LongStory property was provided");
			result.IsDBNull(23).Should().Be(true, "a null LatinStory property was provided");
			result.IsDBNull(24).Should().Be(true, "a null TwoBits property was provided");
			result.IsDBNull(25).Should().Be(true, "a null MissingBits property was provided");
			result.IsDBNull(26).Should().Be(true, "a null Blob property was provided");
			result.IsDBNull(27).Should().Be(true, "a null Price property was provided");
			result.IsDBNull(28).Should().Be(true, "a null Cost property was provided");
			result.IsDBNull(32).Should().Be(true, "a null DayOfTheWeek property was provided");
			result.IsDBNull(33).Should().Be(true, "a null GarbageCollectorNotificationStatus property was provided");
			//result.IsDBNull(34).Should().Be(true, "an empty RecordKey property was provided");
			//result.IsDBNull(35).Should().Be(true, "an empty RecordChild property was provided");
			//result.IsDBNull(36).Should().Be(true, "a empty  RecordChild property was provided");
			//result.IsDBNull(37).Should().Be(true, "a null RecordKeyTwo property was provided");
			//result.IsDBNull(38).Should().Be(true, "a null RecordKeyTwo property was provided");
			//result.IsDBNull(39).Should().Be(true, "a null RecordChild2 property was provided");
			//result.IsDBNull(40).Should().Be(true, "a null RecordChild2 property was provided");
			//result.IsDBNull(41).Should().Be(true, "a null RecordChild2 property was provided");
		}

		[Fact]
		public void ValidateSqlDataReader()
		{
			var modelValues = new SqlMapModel()
			{
				ArgentSeaTestDataId = 1,
				Name = "Test2",
				LatinName = "Test3",
				Iso3166 = "US",
				Iso639 = "en",
				BigCount = 4,
				ValueCount = 5,
				SmallCount = 6,
				ByteCount = 7,
				TrueFalse = true,
				GuidValue = Guid.NewGuid(),
				GuidNull = Guid.NewGuid(),
				Birthday = new DateTime(2008, 8, 8),
				RightNow = new DateTime(2009, 9, 9),
				ExactlyNow = new DateTime(2010, 10, 10),
				NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
				WakeUp = new TimeSpan(12, 12, 12),
				Latitude = 13.13m,
				Longitude = 14.14,
				Altitude = 15.15f,
				Ratio = 16.1,
				Temperature = 32.1f,
				LongStory = "16",
				LatinStory = "17",
				TwoBits = new byte[] { 18, 18 },
				MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 },
				Blob = new byte[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20 },
				Price = 21.0m,
				Cost = 22.0m,
				EnvTarget = EnvironmentVariableTarget.User,
				Color = ConsoleColor.Blue,
				Modifier = ConsoleModifiers.Control,
				DayOfTheWeek = DayOfWeek.Sunday,
				GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
				//RecordKey = new Nullable<ShardKey<byte, int>>(new ShardKey<byte, int>(new DataOrigin('x'), 2, 1234)),
				//RecordChild = new ShardChild<byte, int, short>(new DataOrigin('y'), 3, 4567, (short)-23456),
    //            DataShard2 = new ShardKey<byte, long>(new DataOrigin('z'), 32, -1234),
    //            ChildShard2 = new ShardChild<byte, short, string>(new DataOrigin('y'), 3, -4567, "testing...")
			};

			var rdr = Substitute.For<System.Data.Common.DbDataReader>();
			rdr.NextResult().Returns(false);
			rdr.Read().Returns(true, false);
			rdr.IsClosed.Returns(false);
			rdr.HasRows.Returns(true);

			rdr.FieldCount.Returns(43); //SET THIS IF YOU ADD COLUMNS!

			rdr.GetFieldValue<int>(0).Returns(modelValues.ArgentSeaTestDataId);
			rdr.GetFieldValue<string>(1).Returns(modelValues.Name);
			rdr.GetString(1).Returns(modelValues.Name);
			rdr.GetFieldValue<string>(2).Returns(modelValues.LatinName);
			rdr.GetString(2).Returns(modelValues.LatinName);
			rdr.GetFieldValue<string>(3).Returns(modelValues.Iso3166);
			rdr.GetString(3).Returns(modelValues.Iso3166);
			rdr.GetFieldValue<string>(4).Returns(modelValues.Iso639);
			rdr.GetString(4).Returns(modelValues.Iso639);
			rdr.GetFieldValue<long>(5).Returns(modelValues.BigCount.Value);
			rdr.GetFieldValue<int>(6).Returns(modelValues.ValueCount.Value);
			rdr.GetFieldValue<short>(7).Returns(modelValues.SmallCount.Value);
			rdr.GetFieldValue<byte>(8).Returns(modelValues.ByteCount.Value);
			rdr.GetFieldValue<bool>(9).Returns(modelValues.TrueFalse.Value);
			rdr.GetFieldValue<Guid>(10).Returns(modelValues.GuidValue);
			rdr.GetFieldValue<Guid>(11).Returns(modelValues.GuidNull.Value);
			rdr.GetFieldValue<DateTime>(12).Returns(modelValues.Birthday.Value);
			rdr.GetFieldValue<DateTime>(13).Returns(modelValues.RightNow.Value);
			rdr.GetFieldValue<DateTime>(14).Returns(modelValues.ExactlyNow.Value);
			rdr.GetFieldValue<DateTimeOffset>(15).Returns(modelValues.NowElsewhere.Value);
			rdr.GetFieldValue<TimeSpan>(16).Returns(modelValues.WakeUp.Value);
			rdr.GetFieldValue<decimal>(17).Returns(modelValues.Latitude.Value);
			rdr.GetFieldValue<double>(18).Returns(modelValues.Longitude);
			rdr.GetFieldValue<float>(19).Returns(modelValues.Altitude);
			rdr.GetFieldValue<double>(20).Returns(modelValues.Ratio.Value);
			rdr.GetFieldValue<float>(21).Returns(modelValues.Temperature.Value);
			rdr.GetFieldValue<string>(22).Returns(modelValues.LongStory);
			rdr.GetString(22).Returns(modelValues.LongStory);
			rdr.GetFieldValue<string>(23).Returns(modelValues.LatinStory);
			rdr.GetString(23).Returns(modelValues.LatinStory);
			rdr.GetFieldValue<byte[]>(24).Returns(modelValues.TwoBits);
			rdr.GetFieldValue<byte[]>(25).Returns(modelValues.MissingBits);
			rdr.GetFieldValue<byte[]>(26).Returns(modelValues.Blob);
			rdr.GetFieldValue<decimal>(27).Returns(modelValues.Price.Value);
			rdr.GetFieldValue<decimal>(28).Returns(modelValues.Cost.Value);
			rdr.GetFieldValue<string>(29).Returns(modelValues.EnvTarget.ToString());
			rdr.GetString(29).Returns(modelValues.EnvTarget.ToString());
			rdr.GetFieldValue<short>(30).Returns((short)modelValues.Color);
			rdr.GetFieldValue<string>(31).Returns(modelValues.Modifier.ToString());
			rdr.GetString(31).Returns(modelValues.Modifier.ToString());
			rdr.GetFieldValue<byte>(32).Returns((byte)modelValues.DayOfTheWeek);
			rdr.GetFieldValue<string>(33).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());
			rdr.GetString(33).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());

            //rdr.GetFieldValue<byte>(34).Returns(modelValues.RecordKey.Value.ShardId);
            //rdr.GetFieldValue<int>(35).Returns(modelValues.RecordKey.Value.RecordId);
            //rdr.GetFieldValue<byte>(36).Returns(modelValues.RecordChild.ShardId);
            //rdr.GetFieldValue<int>(37).Returns(modelValues.RecordChild.RecordId);
            //rdr.GetFieldValue<short>(38).Returns(modelValues.RecordChild.ChildId);
            //rdr.GetFieldValue<long>(39).Returns(modelValues.DataShard2.RecordId);
            //rdr.GetFieldValue<byte>(40).Returns(modelValues.ChildShard2.Value.ShardId);
            //rdr.GetFieldValue<short>(41).Returns(modelValues.ChildShard2.Value.RecordId);
            //rdr.GetFieldValue<string>(42).Returns(modelValues.ChildShard2.Value.ChildId);
            //rdr.GetString(42).Returns(modelValues.ChildShard2.Value.ChildId);

            rdr.IsDBNull(0).Returns(false);
			rdr.IsDBNull(1).Returns(false);
			rdr.IsDBNull(2).Returns(false);
			rdr.IsDBNull(3).Returns(false);
			rdr.IsDBNull(4).Returns(false);
			rdr.IsDBNull(5).Returns(false);
			rdr.IsDBNull(6).Returns(false);
			rdr.IsDBNull(7).Returns(false);
			rdr.IsDBNull(8).Returns(false);
			rdr.IsDBNull(9).Returns(false);
			rdr.IsDBNull(10).Returns(false);
			rdr.IsDBNull(11).Returns(false);
			rdr.IsDBNull(12).Returns(false);
			rdr.IsDBNull(13).Returns(false);
			rdr.IsDBNull(14).Returns(false);
			rdr.IsDBNull(15).Returns(false);
			rdr.IsDBNull(16).Returns(false);
			rdr.IsDBNull(17).Returns(false);
			rdr.IsDBNull(18).Returns(false);
			rdr.IsDBNull(19).Returns(false);
			rdr.IsDBNull(20).Returns(false);
			rdr.IsDBNull(21).Returns(false);
			rdr.IsDBNull(22).Returns(false);
			rdr.IsDBNull(23).Returns(false);
			rdr.IsDBNull(24).Returns(false);
			rdr.IsDBNull(25).Returns(false);
			rdr.IsDBNull(26).Returns(false);
			rdr.IsDBNull(27).Returns(false);
			rdr.IsDBNull(28).Returns(false);
			rdr.IsDBNull(29).Returns(false);
			rdr.IsDBNull(30).Returns(false);
			rdr.IsDBNull(31).Returns(false);
			rdr.IsDBNull(32).Returns(false);
			rdr.IsDBNull(33).Returns(false);
            rdr.IsDBNull(34).Returns(false);
            rdr.IsDBNull(35).Returns(false);
            rdr.IsDBNull(36).Returns(false);
            rdr.IsDBNull(37).Returns(false);
            rdr.IsDBNull(38).Returns(false);
            rdr.IsDBNull(39).Returns(false);
            rdr.IsDBNull(40).Returns(false);
            rdr.IsDBNull(41).Returns(false);
            rdr.IsDBNull(42).Returns(false);
            rdr.GetName(0).Returns("ArgentSeaTestDataId");
			rdr.GetName(1).Returns("Name");
			rdr.GetName(2).Returns("LatinName");
			rdr.GetName(3).Returns("Iso3166");
			rdr.GetName(4).Returns("Iso639");
			rdr.GetName(5).Returns("BigCount");
			rdr.GetName(6).Returns("ValueCount");
			rdr.GetName(7).Returns("SmallCount");
			rdr.GetName(8).Returns("ByteValue");
			rdr.GetName(9).Returns("TrueFalse");
			rdr.GetName(10).Returns("GuidValue");
			rdr.GetName(11).Returns("GuidNull");
			rdr.GetName(12).Returns("Birthday");
			rdr.GetName(13).Returns("RightNow");
			rdr.GetName(14).Returns("ExactlyNow");
			rdr.GetName(15).Returns("NowElsewhere");
			rdr.GetName(16).Returns("WakeUp");
			rdr.GetName(17).Returns("Latitude");
			rdr.GetName(18).Returns("Longitude");
			rdr.GetName(19).Returns("Altitude");
			rdr.GetName(20).Returns("Ratio");
			rdr.GetName(21).Returns("Temperature");
			rdr.GetName(22).Returns("LongStory");
			rdr.GetName(23).Returns("LatinStory");
			rdr.GetName(24).Returns("TwoBits");
			rdr.GetName(25).Returns("MissingBits");
			rdr.GetName(26).Returns("Blob");
			rdr.GetName(27).Returns("Price");
			rdr.GetName(28).Returns("Cost");
			rdr.GetName(29).Returns("EnvironmentTarget");
			rdr.GetName(30).Returns("ConsoleColor");
			rdr.GetName(31).Returns("ConsoleModifiers");
			rdr.GetName(32).Returns("DayOfTheWeek");
			rdr.GetName(33).Returns("GCNotificationStatus");
            rdr.GetName(34).Returns("DataShard");
            rdr.GetName(35).Returns("DataRecordId");
            rdr.GetName(36).Returns("ChildShard");
            rdr.GetName(37).Returns("ParentRecordId");
            rdr.GetName(38).Returns("ChildRecordId");
            rdr.GetName(39).Returns("DataRecordId2");
            rdr.GetName(40).Returns("ChildShard2");
            rdr.GetName(41).Returns("ParentRecord2Id");
            rdr.GetName(42).Returns("ChildRecord2Id");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
			var resultList = Mapper.ToList<byte, SqlMapModel>(rdr, 32, dbLogger);
			var result = resultList[0];
			result.ArgentSeaTestDataId.Should().Be(modelValues.ArgentSeaTestDataId, "that is the source value");
			result.Name.Should().Be(modelValues.Name, "that is the source value");
			result.LatinName.Should().Be(modelValues.LatinName, "that is the source value");
			result.Iso3166.Should().Be(modelValues.Iso3166, "that is the source value");
			result.Iso639.Should().Be(modelValues.Iso639, "that is the source value");
			result.BigCount.Should().Be(modelValues.BigCount, "that is the source value");
			result.ValueCount.Should().Be(modelValues.ValueCount, "that is the source value");
			result.SmallCount.Should().Be(modelValues.SmallCount, "that is the source value");
			result.ByteCount.Should().Be(modelValues.ByteCount, "that is the source value");
			result.TrueFalse.Should().Be(modelValues.TrueFalse, "that is the source value");
			result.GuidValue.Should().Be(modelValues.GuidValue, "that is the source value");
			result.GuidNull.Should().Be(modelValues.GuidNull, "that is the source value");
			result.Birthday.Should().Be(modelValues.Birthday, "that is the source value");
			result.RightNow.Should().Be(modelValues.RightNow, "that is the source value");
			result.ExactlyNow.Should().Be(modelValues.ExactlyNow, "that is the source value");
			result.NowElsewhere.Should().Be(modelValues.NowElsewhere, "that is the source value");
			result.WakeUp.Should().Be(modelValues.WakeUp, "that is the source value");
			result.Latitude.Should().Be(modelValues.Latitude, "that is the source value");
			result.Longitude.Should().Be(modelValues.Longitude, "that is the source value");
			result.Altitude.Should().Be(modelValues.Altitude, "that is the source value");
			result.Ratio.Should().Be(modelValues.Ratio, "that is the source value");
			result.Temperature.Should().Be(modelValues.Temperature, "that is the source value");
			result.LongStory.Should().Be(modelValues.LongStory, "that is the source value");
			result.LatinStory.Should().Be(modelValues.LatinStory, "that is the source value");
			result.TwoBits.Length.Should().Be(modelValues.TwoBits.Length, "that is the source value");
			result.MissingBits.Length.Should().Be(modelValues.MissingBits.Length, "that is the source value");
			result.Blob.Length.Should().Be(modelValues.Blob.Length, "that is the source value");
			result.Price.Should().Be(modelValues.Price, "that is the source value");
			result.Cost.Should().Be(modelValues.Cost, "that is the source value");
			result.EnvTarget.Should().Be(modelValues.EnvTarget, "that is the source value");
			result.Color.Should().Be(modelValues.Color, "that is the source value");
			result.Modifier.Should().Be(modelValues.Modifier, "that is the source value");
			result.DayOfTheWeek.Should().Be(modelValues.DayOfTheWeek, "that is the source value");
			result.GarbageCollectorNotificationStatus.Should().Be(modelValues.GarbageCollectorNotificationStatus, "that is the source value");
            //result.RecordKey.HasValue.Should().BeTrue("because the row has values");
            //result.RecordKey.Value.ShardId.Should().Be((byte)2, "because that is the shardId data value");
            //result.RecordKey.Value.RecordId.Should().Be(1234, "because that is the recordId data value");
            //result.RecordChild.ShardId.Should().Be((byte)3, "because that is the shardId data value");
            //result.RecordChild.RecordId.Should().Be(4567, "because that is the recordId data value");
            //result.RecordChild.ChildId.Should().Be((short)-23456, "because that is the childId data value");

            //result.DataShard2.ShardId.Should().Be((byte)32, "because that is the shardId passed into the method call");
            //result.DataShard2.RecordId.Should().Be(-1234, "because that is the recordId data value");
            //result.ChildShard2.Value.ShardId.Should().Be((byte)3, "because that is the shardId data value");
            //result.ChildShard2.Value.RecordId.Should().Be((short)-4567, "because that is the recordId data value");
            //result.ChildShard2.Value.ChildId.Should().Be("testing...", "because that is the childId data value");


        }
        [Fact]
		public void ValidateNullSqlDataReader()
		{
            var modelValues = new SqlMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = null,
                LatinName = null,
                Iso3166 = null,
                Iso639 = null,
                BigCount = null,
                ValueCount = null,
                SmallCount = null,
                ByteCount = null,
                TrueFalse = null,
                GuidValue = Guid.Empty,
                GuidNull = null,
                Birthday = null,
                RightNow = null,
                ExactlyNow = null,
                NowElsewhere = null,
                WakeUp = null,
                Latitude = null,
                Longitude = double.NaN,
                Altitude = float.NaN,
                Temperature = null,
                Ratio = null,
                LongStory = null,
                LatinStory = null,
                TwoBits = null,
                MissingBits = null,
                Blob = null,
                Price = null,
                Cost = null,
                EnvTarget = EnvironmentVariableTarget.Machine,
                Color = ConsoleColor.DarkMagenta,
                Modifier = ConsoleModifiers.Shift,
                DayOfTheWeek = DayOfWeek.Saturday,
                GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
                //RecordKey = ShardKey<byte, int>.Empty,
                //RecordChild = ShardChild<byte, int, short>.Empty,
                //DataShard2 = ShardKey<byte, long>.Empty,
                //ChildShard2 = ShardChild<byte, short, string>.Empty
			};


			var rdr = Substitute.For<System.Data.Common.DbDataReader>();
			rdr.NextResult().Returns(false);
			rdr.Read().Returns(true, false);
			rdr.IsClosed.Returns(false);
			rdr.HasRows.Returns(true);
			rdr.FieldCount.Returns(43);
			rdr.GetFieldValue<int>(0).Returns(modelValues.ArgentSeaTestDataId);
			rdr.GetFieldValue<Guid>(10).Returns(modelValues.GuidValue);
			rdr.GetFieldValue<double>(18).Returns(modelValues.Longitude);
			rdr.GetFieldValue<float>(19).Returns(modelValues.Altitude);
			rdr.GetString(29).Returns(modelValues.EnvTarget.ToString());
			rdr.GetFieldValue<short>(30).Returns((short)modelValues.Color);
			rdr.GetFieldValue<string>(31).Returns(modelValues.Modifier.ToString());
			rdr.GetString(31).Returns(modelValues.Modifier.ToString());
			rdr.GetFieldValue<byte>(32).Returns((byte)modelValues.DayOfTheWeek);
			rdr.GetFieldValue<string>(33).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());
			rdr.GetString(33).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());
            //rdr.GetFieldValue<byte>(34).Returns(modelValues.RecordKey.Value.ShardId);
            //rdr.GetFieldValue<int>(35).Returns(modelValues.RecordKey.Value.RecordId);
            //rdr.GetFieldValue<byte>(36).Returns(modelValues.RecordChild.ShardId);
            //rdr.GetFieldValue<int>(37).Returns(modelValues.RecordChild.RecordId);
            //rdr.GetFieldValue<short>(38).Returns(modelValues.RecordChild.ChildId);
            //rdr.GetFieldValue<long>(39).Returns(modelValues.DataShard2.RecordId);
            //rdr.GetFieldValue<byte>(40).Returns(modelValues.ChildShard2.Value.ShardId);
            //rdr.GetFieldValue<short>(41).Returns(modelValues.ChildShard2.Value.RecordId);
            //rdr.GetFieldValue<string>(42).Returns(modelValues.ChildShard2.Value.ChildId);
            //rdr.GetString(42).Returns(modelValues.ChildShard2.Value.ChildId);

            rdr.IsDBNull(0).Returns(false);
			rdr.IsDBNull(1).Returns(true);
			rdr.IsDBNull(2).Returns(true);
			rdr.IsDBNull(3).Returns(true);
			rdr.IsDBNull(4).Returns(true);
			rdr.IsDBNull(5).Returns(true);
			rdr.IsDBNull(6).Returns(true);
			rdr.IsDBNull(7).Returns(true);
			rdr.IsDBNull(8).Returns(true);
			rdr.IsDBNull(9).Returns(true);
			rdr.IsDBNull(10).Returns(true);
			rdr.IsDBNull(11).Returns(true);
			rdr.IsDBNull(12).Returns(true);
			rdr.IsDBNull(13).Returns(true);
			rdr.IsDBNull(14).Returns(true);
			rdr.IsDBNull(15).Returns(true);
			rdr.IsDBNull(16).Returns(true);
			rdr.IsDBNull(17).Returns(true);
			rdr.IsDBNull(18).Returns(true);
			rdr.IsDBNull(19).Returns(true);
			rdr.IsDBNull(20).Returns(true);
			rdr.IsDBNull(21).Returns(true);
			rdr.IsDBNull(22).Returns(true);
			rdr.IsDBNull(23).Returns(true);
			rdr.IsDBNull(24).Returns(true);
			rdr.IsDBNull(25).Returns(true);
			rdr.IsDBNull(26).Returns(true);
			rdr.IsDBNull(27).Returns(true);
			rdr.IsDBNull(28).Returns(true);
			rdr.IsDBNull(29).Returns(true);
			rdr.IsDBNull(30).Returns(true);
			rdr.IsDBNull(31).Returns(true);
			rdr.IsDBNull(32).Returns(true);
			rdr.IsDBNull(33).Returns(true);
            rdr.IsDBNull(34).Returns(true);
            rdr.IsDBNull(35).Returns(true);
            rdr.IsDBNull(36).Returns(true);
            rdr.IsDBNull(37).Returns(true);
            rdr.IsDBNull(38).Returns(true);
            rdr.IsDBNull(39).Returns(true);
            rdr.IsDBNull(40).Returns(true);
            rdr.IsDBNull(41).Returns(true);
            rdr.IsDBNull(42).Returns(true);
            rdr.GetName(0).Returns("ArgentSeaTestDataId");
			rdr.GetName(1).Returns("Name");
			rdr.GetName(2).Returns("LatinName");
			rdr.GetName(3).Returns("Iso3166");
			rdr.GetName(4).Returns("Iso639");
			rdr.GetName(5).Returns("BigCount");
			rdr.GetName(6).Returns("ValueCount");
			rdr.GetName(7).Returns("SmallCount");
			rdr.GetName(8).Returns("ByteValue");
			rdr.GetName(9).Returns("TrueFalse");
			rdr.GetName(10).Returns("GuidValue");
			rdr.GetName(11).Returns("GuidNull");
			rdr.GetName(12).Returns("Birthday");
			rdr.GetName(13).Returns("RightNow");
			rdr.GetName(14).Returns("ExactlyNow");
			rdr.GetName(15).Returns("NowElsewhere");
			rdr.GetName(16).Returns("WakeUp");
			rdr.GetName(17).Returns("Latitude");
			rdr.GetName(18).Returns("Longitude");
			rdr.GetName(19).Returns("Altitude");
			rdr.GetName(20).Returns("Ratio");
			rdr.GetName(21).Returns("Temperature");
			rdr.GetName(22).Returns("LongStory");
			rdr.GetName(23).Returns("LatinStory");
			rdr.GetName(24).Returns("TwoBits");
			rdr.GetName(25).Returns("MissingBits");
			rdr.GetName(26).Returns("Blob");
			rdr.GetName(27).Returns("Price");
			rdr.GetName(28).Returns("Cost");
			rdr.GetName(29).Returns("EnvironmentTarget");
			rdr.GetName(30).Returns("ConsoleColor");
			rdr.GetName(31).Returns("ConsoleModifiers");
			rdr.GetName(32).Returns("DayOfTheWeek");
			rdr.GetName(33).Returns("GCNotificationStatus");
            rdr.GetName(34).Returns("DataShard");
            rdr.GetName(35).Returns("DataRecordId");
            rdr.GetName(36).Returns("ChildShard");
            rdr.GetName(37).Returns("ParentRecordId");
            rdr.GetName(38).Returns("ChildRecordId");
            rdr.GetName(39).Returns("DataRecordId2");
            rdr.GetName(40).Returns("ChildShard2");
            rdr.GetName(41).Returns("ParentRecord2Id");
            rdr.GetName(42).Returns("ChildRecord2Id");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();

            var resultList = Mapper.ToList<byte, SqlMapModel>(rdr, 200, dbLogger);

            var result = resultList[0];
			result.ArgentSeaTestDataId.Should().Be(modelValues.ArgentSeaTestDataId, "that is the source value");
			result.Name.Should().BeNull("the reader value is DbNull");
			result.LatinName.Should().BeNull("the reader value is DbNull");
			result.Iso3166.Should().BeNull("the reader value is DbNull");
			result.Iso639.Should().BeNull("the reader value is DbNull");
			result.BigCount.Should().BeNull("the reader value is DbNull");
			result.ValueCount.Should().BeNull("the reader value is DbNull");
			result.SmallCount.Should().BeNull("the reader value is DbNull");
			result.ByteCount.Should().BeNull("the reader value is DbNull");
			result.TrueFalse.Should().BeNull("the reader value is DbNull");
			result.GuidValue.Should().Be(Guid.Empty, "the reader value is DbNull");
			result.GuidNull.Should().BeNull("the reader value is DbNull");
			result.Birthday.Should().BeNull("the reader value is DbNull");
			result.RightNow.Should().BeNull("the reader value is DbNull");
			result.ExactlyNow.Should().BeNull("the reader value is DbNull");
			result.NowElsewhere.Should().BeNull("the reader value is DbNull");
			result.WakeUp.Should().BeNull("the reader value is DbNull");
			result.Latitude.Should().BeNull("the reader value is DbNull");
			double.IsNaN(result.Longitude).Should().BeTrue("the reader value is DbNull");
			float.IsNaN(result.Altitude).Should().BeTrue("the reader value is DbNull");
			result.Ratio.Should().BeNull("the reader value is DbNull");
			result.Temperature.Should().BeNull("the reader value is DbNull");
			result.LongStory.Should().BeNull("the reader value is DbNull");
			result.LatinStory.Should().BeNull("the reader value is DbNull");
			result.TwoBits.Should().BeNull("the reader value is DbNull");
			result.MissingBits.Should().BeNull("the reader value is DbNull");
			result.Blob.Should().BeNull("the reader value is DbNull");
			result.Price.Should().BeNull("the reader value is DbNull");
			result.Cost.Should().BeNull("the reader value is DbNull");
			result.EnvTarget.Should().Be(modelValues.EnvTarget, "that is the source value");
			result.Color.Should().Be(modelValues.Color, "that is the source value");
			result.Modifier.Should().Be(modelValues.Modifier, "that is the source value");
			result.DayOfTheWeek.Should().BeNull("the reader value is DbNull");
			result.GarbageCollectorNotificationStatus.Should().BeNull("the reader value is DbNull");
            //result.RecordKey.Should().BeNull("the input values are null");
            //result.RecordChild.Should().Be(ShardChild<byte, int, short>.Empty, "the result should be empty");
            //result.DataShard2.Should().Be(ShardKey<byte, long>.Empty, "the result should be empty");
            //result.ChildShard2.Should().BeNull("the input values are null");
		}
	}
}