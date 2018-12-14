using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel.Resolution;
using ArgentSea;
using ArgentSea.Sql;
using FluentAssertions;
using System.Data.SqlClient;
using System.Data;
using NSubstitute;

namespace ArgentSea.Sql.Test
{
    internal class MasterModel
    {

        public IList<ModelTwo> PropertyTwo { get; set; }
        public IList<ModelOne> PropertyOne { get; set; }
        public IList<ModelFour> PropertyFour { get; set; }
        public IList<ModelZero> PropertyZero { get; set; }
        public IList<ModelSix> PropertySix { get; set; }
        public IList<ModelSeven> PropertySeven { get; set; }
        public IList<ModelFive> PropertyFive { get; set; }
        public IList<ModelThree> PropertyThree { get; set; }

        [MapToSqlNVarChar("Description", 255)]
        public string MasterDescription { get; set; }
    }
    internal class ModelZero
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlBigInt("Status")]
        public long Status { get; set; }

        [MapToSqlDecimal("Amount", 9, 2)]
        public decimal Amount { get; set; }
    }
    internal class ModelOne
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }
    }
    internal class ModelTwo
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlBigInt("Status")]
        public long State { get; set; }

        [MapToSqlDecimal("Amount", 9, 2)]
        public decimal Value { get; set; }
    }
    internal class ModelThree
    {
        [MapToSqlInt("Id", true)]
        public int Count { get; set; }

        public decimal Value { get; set; }
    }
    internal class ModelFour
    {
        [MapToSqlInt("Id", true)]
        public int FourID { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string FourDescription { get; set; }
    }
    internal class ModelFive
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }
    }
    internal class ModelSix
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlBigInt("Status")]
        public long Count { get; set; }
    }
    internal class ModelSeven
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlBigInt("Status")]
        public long Status { get; set; }

        [MapToSqlDecimal("Amount", 18, 5)]
        public decimal Amount { get; set; }
    }

    public class ModelFromReaderResultsTests
    {

        [Fact]
        private void OutPrmWithOneDataset()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(false);
            rdr.Read().Returns(true, true, false);
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4);
            rdr.GetFieldValue<int>(0).Returns(4, 1);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithTwoDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false  //1
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithThreeDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false  //2
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithFourDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false,  //2
                true, true, false  //3
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4, 1);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(3, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithFiveDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false,  //2
                true, true, false,  //3
                true, true, true, false //4
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4, 1, 2);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(2, "three rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(3, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(2, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(123, "that is the recordset id value");
            result.PropertyFour[1].FourID.Should().Be(456, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithSixDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false,  //2
                true, true, false,  //3
                true, true, true, false, //4
                true, false //5
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4, 1, 2, 1);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(2, "three rows were returned by the recordset");
            result.PropertyFive.Count.Should().Be(1, "one row was returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(3, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(2, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(123, "that is the recordset id value");
            result.PropertyFour[1].FourID.Should().Be(456, "that is the recordset id value");
            result.PropertyFive[0].Id.Should().Be(3, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithSevenDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false,  //2
                true, true, false,  //3
                true, true, true, false, //4
                true, false, //5
                true, false //6
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo", "Six");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo", "Six");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(2, "three rows were returned by the recordset");
            result.PropertyFive.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertySix.Count.Should().Be(1, "one row was returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(3, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(2, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(123, "that is the recordset id value");
            result.PropertyFour[1].FourID.Should().Be(456, "that is the recordset id value");
            result.PropertyFive[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertySix[0].Id.Should().Be(76, "that is the recordset id value");
        }
        [Fact]
        private void OutPrmWithEightDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, true, false, //0
                true, false,  //1
                true, true, true, false,  //2
                true, true, false,  //3
                true, true, true, false, //4
                true, false, //5
                true, false, //6
                true, true, false //7
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(4, 2, 4, 1, 2, 1, 3, 4);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroOne", "One", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new QueryParameterCollection();
            var prm = new SqlParameter("@Description", ParameterDirection.Output);
            prm.Value = "Root Model";
            prms.Add(prm);
            var result = Mapper.ModelFromOutResultsHandler<int, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix, ModelSeven>(0, "testSproc", null, rdr, prms, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyTwo.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(2, "three rows were returned by the recordset");
            result.PropertyFive.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertySix.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertySeven.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyTwo[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(3, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(2, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(123, "that is the recordset id value");
            result.PropertyFour[1].FourID.Should().Be(456, "that is the recordset id value");
            result.PropertyFive[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertySix[0].Id.Should().Be(76, "that is the recordset id value");
            result.PropertySeven[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertySeven[1].Id.Should().Be(5, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithOneOtherDataset()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false  //0
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithTwoOtherDatasets()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false  //1
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithThreeOtherDatasets()
        {
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false,  //1
                true, true, false  //2
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyTwo.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(2, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithFourOtherDatasets()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false,  //1
                true, true, false,  //2
                true, true, true, false //3
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyTwo.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(123, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(456, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithFiveOtherDatasets()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false,  //1
                true, true, false,  //2
                true, true, true, false, //3
                true, false //4
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyTwo.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(1, "one row was returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(123, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(456, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(3, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithSixOtherDatasets()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false,  //1
                true, true, false,  //2
                true, true, true, false, //3
                true, false, //4
                true, false //5
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyTwo.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyFive.Count.Should().Be(1, "one row was returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(123, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(456, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(3, "that is the recordset id value");
            result.PropertyFive[0].Id.Should().Be(76, "that is the recordset id value");
        }
        [Fact]
        private void RecordWithSevenOtherDatasets()
        {

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(true, true, true, true, true, true, true, false);
            rdr.Read().Returns(true, false, //root
                true, true, true, false,  //0
                true, true, true, false,  //1
                true, true, false,  //2
                true, true, true, false, //3
                true, false, //4
                true, false, //5
                true, true, false //6
                );
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(1, 4, 2, 4, 1, 2, 1, 3);
            rdr.GetFieldValue<int>(0).Returns(4, 1, 2, 3, 1, 2, 3, 2, 123, 456, 3, 76, 2, 5);
            rdr.GetFieldValue<string>(0).Returns("Root Model");
            rdr.GetString(0).Returns("Root Model");
            rdr.GetFieldValue<string>(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetString(1).Returns("ZeroZero", "ZeroZero", "ZeroOne", "ZeroZero", "ZeroOne", "ZeroTwo", "One", "TwoZero", "TwoZero", "TwoOne", "TwoZero", "TwoOne", "TwoTwo", "FourZero", "FourZero", "FourOne", "FourZero", "FourOne", "FourTwo", "Six", "SevenZero", "SevenZero", "SevenOne");
            rdr.GetFieldValue<long>(2).Returns(11L, 12L, 31L, 32L, 33L, 71, 81L, 82L);
            rdr.GetFieldValue<decimal>(3).Returns(1.01M, 1.02M, 3.01M, 3.02M, 3.03M, 3.03M, 8.01M, 8.02M);

            rdr.IsDBNull(0).Returns(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.GetName(0).Returns("Description", "Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Status");
            rdr.GetName(3).Returns("Amount");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var result = Mapper.ModelFromReaderResultsHandler<int, MasterModel, MasterModel, ModelZero, ModelOne, ModelTwo, ModelThree, ModelFour, ModelFive, ModelSix>(0, "testSproc", null, rdr, null, "test connection", dbLogger);
            result.PropertyZero.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyOne.Count.Should().Be(3, "three rows were returned by the recordset");
            result.PropertyTwo.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyThree.Count.Should().Be(2, "two rows were returned by the recordset");
            result.PropertyFour.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertyFive.Count.Should().Be(1, "one row was returned by the recordset");
            result.PropertySix.Count.Should().Be(2, "two rows were returned by the recordset");
            result.MasterDescription.Should().Be("Root Model", "that is the parameter value");
            result.PropertyZero[0].Id.Should().Be(4, "that is the recordset id value");
            result.PropertyZero[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyZero[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyOne[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyOne[1].Id.Should().Be(1, "that is the recordset id value");
            result.PropertyOne[2].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyTwo[0].Id.Should().Be(3, "that is the recordset id value");
            result.PropertyTwo[1].Id.Should().Be(2, "that is the recordset id value");
            result.PropertyThree[0].Count.Should().Be(123, "that is the recordset id value");
            result.PropertyThree[1].Count.Should().Be(456, "that is the recordset id value");
            result.PropertyFour[0].FourID.Should().Be(3, "that is the recordset id value");
            result.PropertyFive[0].Id.Should().Be(76, "that is the recordset id value");
            result.PropertySix[0].Id.Should().Be(2, "that is the recordset id value");
            result.PropertySix[1].Id.Should().Be(5, "that is the recordset id value");
        }
    }
}
