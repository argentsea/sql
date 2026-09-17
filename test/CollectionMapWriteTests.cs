using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using FluentAssertions;

namespace ArgentSea.Sql.Test
{
    internal class CollectionWriteChild
    {
        [MapToSqlInt("ChildId", true)]
        public int ChildId { get; set; }

        [MapToSqlNVarChar("ChildName", 100)]
        public string ChildName { get; set; }
    }

    internal class CollectionWriteParent
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlTableValuedParameter("@Children", "ChildTableType")]
        public List<CollectionWriteChild> Children { get; set; }
    }

    internal class CollectionWriteParentImmutableArray
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlTableValuedParameter("@Children", "ChildTableType")]
        public ImmutableArray<CollectionWriteChild> Children { get; set; }
    }

    public class CollectionMapWriteTests
    {
        [Fact]
        public void AddSqlTableValuedParameter_NonEmptyCollection_ProducesRecordList()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var values = new List<CollectionWriteChild>
            {
                new CollectionWriteChild { ChildId = 1, ChildName = "One" },
                new CollectionWriteChild { ChildId = 2, ChildName = "Two" },
            };

            // Act
            prms.AddSqlTableValuedParameter<CollectionWriteChild>("@Children", values, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.SqlDbType.Should().Be(System.Data.SqlDbType.Structured, "table-valued parameters use the Structured data type");
            prm.Value.Should().BeAssignableTo<IEnumerable<SqlDataRecord>>("a populated collection is sent as a list of records");
            ((IReadOnlyCollection<SqlDataRecord>)prm.Value).Count.Should().Be(2, "two child rows were provided");
        }

        [Fact]
        public void AddSqlTableValuedParameter_EmptyCollection_ProducesNullValueNotAnEmptyEnumeration()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var values = new List<CollectionWriteChild>();

            // Act
            prms.AddSqlTableValuedParameter<CollectionWriteChild>("@Children", values, dbLogger);

            // Assert
            // SQL Server cannot infer TVP row metadata from an IEnumerable<SqlDataRecord> with zero elements, and the
            // driver rejects DBNull for table-valued parameters outright. An empty collection must therefore be
            // represented as a null Value, which the driver sends as DEFAULT (an empty table for a READONLY TVP),
            // never as a non-null empty enumeration and never as DBNull.
            var prm = (SqlParameter)prms["@Children"];
            prm.SqlDbType.Should().Be(System.Data.SqlDbType.Structured);
            prm.Value.Should().BeNull("a zero-row table-valued parameter is sent as DEFAULT via a null Value; DBNull is rejected by the driver");
        }

        [Fact]
        public void AddSqlTableValuedParameter_WithColumnList_EmptyCollection_ProducesNullValue()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var values = new List<CollectionWriteChild>();

            // Act
            prms.AddSqlTableValuedParameter<CollectionWriteChild>("@Children", values, new List<string> { "ChildId", "ChildName" }, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.Value.Should().BeNull("the column-list overload must apply the same empty-collection rule as the default overload");
        }

        [Fact]
        public void CreateInputParameters_NonEmptyCollectionProperty_SetsTypeNameAndRows()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var model = new CollectionWriteParent
            {
                Id = 1,
                Name = "Parent",
                Children = new List<CollectionWriteChild>
                {
                    new CollectionWriteChild { ChildId = 10, ChildName = "Ten" },
                    new CollectionWriteChild { ChildId = 20, ChildName = "Twenty" },
                    new CollectionWriteChild { ChildId = 30, ChildName = "Thirty" },
                }
            };

            // Act
            prms.CreateInputParameters<CollectionWriteParent>(model, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.TypeName.Should().Be("ChildTableType", "the attribute declares this as the table type name");
            prm.Value.Should().BeAssignableTo<IEnumerable<SqlDataRecord>>();
            ((IReadOnlyCollection<SqlDataRecord>)prm.Value).Count.Should().Be(3, "three child rows were provided");
        }

        [Fact]
        public void CreateInputParameters_EmptyCollectionProperty_SendsDefaultTvp()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var model = new CollectionWriteParent
            {
                Id = 1,
                Name = "Parent",
                Children = new List<CollectionWriteChild>()
            };

            // Act
            prms.CreateInputParameters<CollectionWriteParent>(model, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.TypeName.Should().Be("ChildTableType");
            prm.Value.Should().BeNull("an empty collection must still produce a valid (empty) table-valued parameter");
        }

        [Fact]
        public void CreateInputParameters_NonEmptyImmutableArrayCollectionProperty_SetsTypeNameAndRows()
        {
            // Arrange
            // ImmutableArray<T> is a value type, so the collection property's static expression type is not
            // reference-assignable to the IEnumerable<TElement> parameter of AddSqlTableValuedParameter without
            // an explicit conversion in the expression tree.
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var model = new CollectionWriteParentImmutableArray
            {
                Id = 1,
                Name = "Parent",
                Children = ImmutableArray.Create(
                    new CollectionWriteChild { ChildId = 10, ChildName = "Ten" },
                    new CollectionWriteChild { ChildId = 20, ChildName = "Twenty" },
                    new CollectionWriteChild { ChildId = 30, ChildName = "Thirty" })
            };

            // Act
            prms.CreateInputParameters<CollectionWriteParentImmutableArray>(model, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.TypeName.Should().Be("ChildTableType", "the attribute declares this as the table type name");
            prm.Value.Should().BeAssignableTo<IEnumerable<SqlDataRecord>>();
            ((IReadOnlyCollection<SqlDataRecord>)prm.Value).Count.Should().Be(3, "three child rows were provided");
        }

        [Fact]
        public void CreateInputParameters_EmptyImmutableArrayCollectionProperty_SendsDefaultTvp()
        {
            // Arrange
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var model = new CollectionWriteParentImmutableArray
            {
                Id = 1,
                Name = "Parent",
                Children = ImmutableArray<CollectionWriteChild>.Empty
            };

            // Act
            prms.CreateInputParameters<CollectionWriteParentImmutableArray>(model, dbLogger);

            // Assert
            var prm = (SqlParameter)prms["@Children"];
            prm.TypeName.Should().Be("ChildTableType");
            prm.Value.Should().BeNull("an empty collection must still produce a valid (empty) table-valued parameter");
        }

        [Fact]
        public void CreateInputParameters_DefaultImmutableArrayCollectionProperty_DoesNotThrowAndSendsDefaultTvp()
        {
            // Arrange
            // A default (uninitialized) ImmutableArray<T> - as opposed to ImmutableArray<T>.Empty - throws
            // InvalidOperationException when enumerated. Since the model never distinguishes "never assigned"
            // from "assigned as empty", it must be treated the same as an empty collection rather than throwing.
            var dbLogger = new DebugLogger();
            var prms = new ParameterCollection();
            var model = new CollectionWriteParentImmutableArray
            {
                Id = 1,
                Name = "Parent",
                Children = default
            };
            model.Children.IsDefault.Should().BeTrue("the test must exercise the uninitialized struct, not ImmutableArray<T>.Empty");

            // Act
            Action act = () => prms.CreateInputParameters<CollectionWriteParentImmutableArray>(model, dbLogger);

            // Assert
            act.Should().NotThrow("a default ImmutableArray<T> must be treated as an empty collection, not enumerated directly");
            var prm = (SqlParameter)prms["@Children"];
            prm.TypeName.Should().Be("ChildTableType");
            prm.Value.Should().BeNull("a default collection must still produce a valid (empty) table-valued parameter");
        }
    }
}
