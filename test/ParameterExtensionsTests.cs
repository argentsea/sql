using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel.Resolution;
using ArgentSea;
using ArgentSea.Sql;
using FluentAssertions;
using System.Data.SqlClient;
using System.Data;

namespace ArgentSea.Sql.Test
{
    public class ParameterExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void IntegerTests(int value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlIntInputParameter("1", value);
            prms.AddSqlIntInputParameter("2", (int?)value);
            prms["@1"].GetInteger().Should().Be(value, "that was the integer value");
            prms["@2"].GetNullableInteger().Should().Be(value, "that was the integer value");
        }
        [Fact]
        public void NullIntegerTest()
        {
            var prm = new SqlParameter("", SqlDbType.Int) { Value = System.DBNull.Value };
            prm.GetNullableInteger().Should().Be(null, "that was null database integer value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public void ShortTests(short value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlSmallIntInputParameter("1", value);
            prms.AddSqlSmallIntInputParameter("2", (short?)value);
            prms["@1"].GetShort().Should().Be(value, "that was the short value");
            prms["@2"].GetNullableShort().Should().Be(value, "that was the short value");
        }
        [Fact]
        public void NullShortTest()
        {
            var prm = new SqlParameter("", SqlDbType.SmallInt) { Value = System.DBNull.Value };
            prm.GetNullableShort().Should().Be(null, "that was null database short value");
        }
        [Theory]
        [InlineData(0)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void LongTests(long value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlBigIntInputParameter("1", value);
            prms.AddSqlBigIntInputParameter("2", (long?)value);
            prms["@1"].GetLong().Should().Be(value, "that was the long value");
            prms["@2"].GetNullableLong().Should().Be(value, "that was the long value");
        }
        [Fact]
        public void NullLongTest()
        {
            var prm = new SqlParameter("", SqlDbType.BigInt) { Value = System.DBNull.Value };
            prm.GetNullableLong().Should().Be(null, "that was null database long value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(byte.MaxValue)]
        public void ByteTests(byte value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlTinyIntInputParameter("1", value);
            prms.AddSqlTinyIntInputParameter("2", (byte?)value);
            prms["@1"].GetByte().Should().Be(value, "that was the byte value");
            prms["@2"].GetNullableByte().Should().Be(value, "that was the byte value");
        }
        [Fact]
        public void NullByteTest()
        {
            var prm = new SqlParameter("", SqlDbType.TinyInt) { Value = System.DBNull.Value };
            prm.GetNullableByte().Should().Be(null, "that was null database byte value");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BooleanTests(bool value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlBitInputParameter("1", value);
            prms.AddSqlBitInputParameter("2", (bool?)value);
            prms["@1"].GetBoolean().Should().Be(value, "that was the boolean value");
            prms["@2"].GetNullableBoolean().Should().Be(value, "that was the boolean value");
        }
        [Fact]
        public void NullBooleanTest()
        {
            var prm = new SqlParameter("", SqlDbType.Bit) { Value = System.DBNull.Value };
            prm.GetNullableBoolean().Should().Be(null, "that was null database boolean value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void DoubleTests(double value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlFloatInputParameter("1", value);
            prms.AddSqlFloatInputParameter("2", (double?)value);
            prms["@1"].GetDouble().Should().Be(value, "that was the double value");
            prms["@2"].GetNullableDouble().Should().Be(value, "that was the double value");
        }
        [Fact]
        public void NullDoubleTest()
        {
            var prm = new SqlParameter("", SqlDbType.Float) { Value = System.DBNull.Value };
            prm.GetNullableDouble().Should().Be(null, "that was a null database double value");
            double.IsNaN(prm.GetDouble()).Should().Be(true, "that was a null database double value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        public void FloatTests(float value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlRealInputParameter("1", value);
            prms.AddSqlRealInputParameter("2", (float?)value);
            prms["@1"].GetFloat().Should().Be(value, "that was the single value");
            prms["@2"].GetNullableFloat().Should().Be(value, "that was the single value");
        }
        [Fact]
        public void NullFloatTest()
        {
            var prm = new SqlParameter("", SqlDbType.Real) { Value = System.DBNull.Value };
            prm.GetNullableFloat().Should().Be(null, "that was null database single value");
            float.IsNaN(prm.GetFloat()).Should().Be(true, "that was a null database float value");
        }

        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void NVarCharTests(string value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlNVarCharInputParameter("1", value, length);
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            prms["@1"].GetString().Should().Be(value, "that was the string value");
        }
        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void NCharTests(string value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlNCharInputParameter("1", value, length);
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            //prms["@1"].GetString().Should().Be(value?.PadRight(length, ' '), "that was the string value");
            prms["@1"].GetString().Should().Be(value, "that was the string value");
        }
        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void VarCharTests(string value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlVarCharInputParameter("1", value, length, 1033);
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            prms["@1"].GetString().Should().Be(value, "that was the string value");
        }
        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void CharTests(string value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlCharInputParameter("1", value, length, 1033);
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            //prms["@1"].GetString().Should().Be(value?.PadRight(length, ' '), "that was the string value");
            prms["@1"].GetString().Should().Be(value, "that was the string value");
        }

        public static TheoryData<byte[], int> BinaryTestData => new TheoryData<byte[], int>
        {
            { null, 5 },
            { new byte[] { 1, 2, 3 }, 50 },
            { new byte[] { 0 }, 1 }
        };

        [Theory]
        [MemberData(nameof(BinaryTestData))]
        public void BinaryTests(byte[] value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlBinaryInputParameter("1", value, length);
            prms["@1"].Size.Should().Be(length, "that was the specified length");
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
                prms["@1"].GetBytes().Should().BeNull("null was specified");
            }
            else
            {
                var data = prms["@1"].GetBytes();
                for (var i = 0; i < data.Length; i++)
                {
                    data[i].Should().Be(value[i], "that was the array value");
                }
            }
        }

        [Theory]
        [MemberData(nameof(BinaryTestData))]
        public void VarBinaryTests(byte[] value, int length)
        {
            var prms = new ParameterCollection();
            prms.AddSqlVarBinaryInputParameter("1", value, length);
            prms["@1"].Size.Should().Be(length, "that was the specified length");
            if (value is null)
            {
                prms["@1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
                prms["@1"].GetBytes().Should().BeNull("null was specified");
            }
            else
            {
                var data = prms["@1"].GetBytes();
                for (var i = 0; i < data.Length; i++)
                {
                    data[i].Should().Be(value[i], "that was the array value");
                }
            }
        }

        public static TheoryData<DateTime> DateTimeTestData => new TheoryData<DateTime>
        {
            { DateTime.UtcNow },
            { DateTime.Now },
            { DateTime.MinValue },
            { DateTime.MaxValue }
        };

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void DateTimeTests(DateTime value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlDateTimeInputParameter("1", value);
            prms.AddSqlDateTimeInputParameter("2", (DateTime?)value);
            prms["@1"].GetDateTime().Should().Be(value, "that was the datetime value");
            prms["@2"].GetNullableDateTime().Should().Be(value, "that was the datetime value");
        }
        [Fact]
        public void NullDateTimeTest()
        {
            var prm = new SqlParameter("", SqlDbType.DateTime) { Value = System.DBNull.Value };
            prm.GetNullableDateTime().Should().Be(null, "that was null database datetime value");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void DateTime2Tests(DateTime value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlDateTime2InputParameter("1", value);
            prms.AddSqlDateTime2InputParameter("2", (DateTime?)value);
            prms["@1"].GetDateTime().Should().Be(value, "that was the datetime2 value");
            prms["@2"].GetNullableDateTime().Should().Be(value, "that was the datetime2 value");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void DateTests(DateTime value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlDateInputParameter("1", value);
            prms.AddSqlDateInputParameter("2", (DateTime?)value);
            prms["@1"].GetDateTime().Date.Should().Be(value.Date, "that was the date value");
            prms["@2"].GetNullableDateTime().Value.Date.Should().Be(value.Date, "that was the date value");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void TimeTests(DateTime value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlTimeInputParameter("1", value.TimeOfDay);
            prms.AddSqlTimeInputParameter("2", (TimeSpan?)value.TimeOfDay);
            prms["@1"].GetTimeSpan().Should().Be(value.TimeOfDay, "that was the time value");
            prms["@2"].GetNullableTimeSpan().Value.Should().Be(value.TimeOfDay, "that was the time value");
        }

        public static TheoryData<DateTimeOffset> DateTimeOffsetTestData => new TheoryData<DateTimeOffset>
        {
            { DateTimeOffset.Now },
            { DateTimeOffset.MinValue },
            { DateTimeOffset.MaxValue },
            { new DateTimeOffset(2018, 6, 10, 4, 22, 34, new TimeSpan(-4, 0, 0)) },
            { new DateTimeOffset(new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified), new TimeSpan(-4, 0, 0)) }
        };

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        public void DateTimeOffsetTests(DateTimeOffset value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlDateTimeOffsetInputParameter("1", value);
            prms.AddSqlDateTimeOffsetInputParameter("2", (DateTimeOffset?)value);
            prms["@1"].GetDateTimeOffset().Should().Be(value, "that was the time value");
            prms["@2"].GetNullableDateTimeOffset().Value.Should().Be(value, "that was the datetime offset value");
        }

        [Fact]
        public void NullDateTimeOffsetTest()
        {
            var prm = new SqlParameter("", SqlDbType.DateTimeOffset) { Value = System.DBNull.Value };
            prm.GetNullableDateTimeOffset().Should().Be(null, "that was null database datetime offset value");
        }

        public static TheoryData<decimal, byte, byte> DecimalTestData => new TheoryData<decimal, byte, byte>
        {
            { 0M, 9, 2 },
            { decimal.MaxValue, 18, 0 },
            { decimal.MinValue, 22, 4 }
        };

        [Theory]
        [MemberData(nameof(DecimalTestData))]
        public void DecimalTests(decimal value, byte precision, byte scale)
        {
            var prms = new ParameterCollection();
            prms.AddSqlDecimalInputParameter("1", value, precision, scale);
            prms.AddSqlDecimalInputParameter("2", (decimal?)value, precision, scale);
            prms["@1"].GetDecimal().Should().Be(value, "that was the decimal value");
            prms["@2"].GetNullableDecimal().Should().Be(value, "that was the decimal value");
            prms["@1"].Precision.Should().Be(precision, "that was the specified precision");
            prms["@2"].Scale.Should().Be(scale, "that was the specified scale");

            if (value >= -922337203685477.5808M && value >= 922337203685477.5807M)
            {
                prms.AddSqlMoneyInputParameter("3", value);
                prms.AddSqlMoneyInputParameter("4", (decimal?)value);
                prms["@3"].GetDecimal().Should().Be(value, "that was the decimal value");
                prms["@4"].GetNullableDecimal().Should().Be(value, "that was the decimal value");
            }
            if (value >= -214748.3648M && value <= 214748.3647M)
            {
                prms.AddSqlSmallMoneyInputParameter("5", value);
                prms.AddSqlSmallMoneyInputParameter("6", (decimal?)value);
                prms["@5"].GetDecimal().Should().Be(value, "that was the decimal value");
                prms["@6"].GetNullableDecimal().Should().Be(value, "that was the decimal value");
            }
        }
        [Fact]
        public void NullDecimalTest()
        {
            var prm = new SqlParameter("", SqlDbType.Decimal) { Value = System.DBNull.Value };
            prm.GetNullableDecimal().Should().Be(null, "that was null database decimal value");
        }
        public static TheoryData<Guid> GuidTestData => new TheoryData<Guid>
        {
            { Guid.NewGuid() }
        };
        [Theory]
        [MemberData(nameof(GuidTestData))]
        public void UuidTests(Guid value)
        {
            var prms = new ParameterCollection();
            prms.AddSqlUniqueIdentifierInputParameter("1", value);
            prms.AddSqlUniqueIdentifierInputParameter("2", (Guid?)value);
            prms["@1"].GetGuid().Should().Be(value, "that was the guid value");
            prms["@2"].GetNullableGuid().Should().Be(value, "that was the guid value");
        }
        [Fact]
        public void NullGuidTest()
        {
            var prm1 = new SqlParameter("1", SqlDbType.UniqueIdentifier) { Value = System.DBNull.Value };
            prm1.GetNullableGuid().Should().BeNull("that was a null database guid value");
            prm1.GetGuid().Should().Be(Guid.Empty, "that was a null database guid value");
            var prm2 = new SqlParameter("2", SqlDbType.UniqueIdentifier) { Value = Guid.Empty };
            prm2.GetGuid().Should().Be(Guid.Empty, "that was an empty database guid value");
            prm2.GetNullableGuid().Should().Be(Guid.Empty, "that was an empty database guid value");
        }
    }
}
