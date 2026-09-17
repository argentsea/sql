using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace ArgentSea.Sql.Test
{
    internal class CollectionReadChild
    {
        [MapToSqlInt("ChildId", true)]
        public int ChildId { get; set; }

        [MapToSqlNVarChar("ChildName", 100)]
        public string ChildName { get; set; }
    }

    internal class CollectionReadParentImmutableArray
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlTableValuedParameter("@Children", "ChildTableType")]
        public ImmutableArray<CollectionReadChild> Children { get; set; }
    }

    internal class CollectionReadChildA
    {
        [MapToSqlInt("AId", true)]
        public int AId { get; set; }
    }

    internal class CollectionReadChildB
    {
        [MapToSqlInt("BId", true)]
        public int BId { get; set; }
    }

    internal class CollectionReadParent
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }

        [MapToSqlTableValuedParameter("@Children", "ChildTableType")]
        public List<CollectionReadChild> Children { get; set; }
    }

    internal class TwoCollectionsParent
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlTableValuedParameter("@First", "FirstTableType")]
        public List<CollectionReadChildA> FirstChildren { get; set; }

        [MapToSqlTableValuedParameter("@Second", "SecondTableType")]
        public List<CollectionReadChildB> SecondChildren { get; set; }
    }

    internal class PlainNoCollectionModel
    {
        [MapToSqlInt("Id", true)]
        public int Id { get; set; }

        [MapToSqlNVarChar("Name", 255)]
        public string Name { get; set; }
    }

    public class CollectionMapReadTests
    {
        [Fact]
        public void ModelFromReaderWithCollectionsHandler_OneCollection_HydratesParentAndChildren()
        {
            // Arrange: result set 0 is the parent's single row, result set 1 is the collection's rows.
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);
            rdr.NextResult().Returns(true);
            rdr.Read().Returns(true, false, //parent
                true, true, false // children
                );
            rdr.FieldCount.Returns(2);
            rdr.GetName(0).Returns("Id", "ChildId");
            rdr.GetName(1).Returns("Name", "ChildName");
            rdr.GetFieldValue<int>(0).Returns(1, 10, 20);
            rdr.GetFieldValue<string>(1).Returns("Parent", "Ten", "Twenty");
            rdr.GetString(1).Returns("Parent", "Ten", "Twenty");
            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<CollectionReadParent>(new CollectionReadParent(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1, "that is the parent scalar value");
            result.Name.Should().Be("Parent");
            result.Children.Should().NotBeNull();
            result.Children.Count.Should().Be(2, "two child rows were returned by the second result set");
            result.Children[0].ChildId.Should().Be(10);
            result.Children[0].ChildName.Should().Be("Ten");
            result.Children[1].ChildId.Should().Be(20);
            result.Children[1].ChildName.Should().Be("Twenty");
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_TwoCollections_HydrateInDeclarationOrder()
        {
            // Arrange: result set 0 is the parent row, result set 1 is FirstChildren (declared first),
            // result set 2 is SecondChildren (declared second).
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);
            rdr.NextResult().Returns(true, true);
            rdr.Read().Returns(true, false, //parent
                true, true, false, //FirstChildren
                true, false // SecondChildren
                );
            rdr.FieldCount.Returns(1);
            rdr.GetName(0).Returns("Id", "AId", "BId");
            rdr.GetFieldValue<int>(0).Returns(1, 100, 200, 300);
            rdr.IsDBNull(0).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<TwoCollectionsParent>(new TwoCollectionsParent(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().NotBeNull();
            result.FirstChildren.Should().NotBeNull();
            result.FirstChildren.Count.Should().Be(2, "FirstChildren is declared before SecondChildren and reads the second result set");
            result.FirstChildren[0].AId.Should().Be(100);
            result.FirstChildren[1].AId.Should().Be(200);
            result.SecondChildren.Should().NotBeNull();
            result.SecondChildren.Count.Should().Be(1, "SecondChildren is declared last and reads the third result set");
            result.SecondChildren[0].BId.Should().Be(300);
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_EmptyChildResultSet_ProducesEmptyListNotNull()
        {
            // Arrange: the second result set exists (NextResult returns true) but has zero rows.
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.NextResult().Returns(true);
            rdr.HasRows.Returns(true, false); // parent has rows; the child result set does not
            rdr.Read().Returns(true, false); //parent only
            rdr.FieldCount.Returns(2);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetFieldValue<int>(0).Returns(1);
            rdr.GetFieldValue<string>(1).Returns("Parent");
            rdr.GetString(1).Returns("Parent");
            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<CollectionReadParent>(new CollectionReadParent(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().NotBeNull();
            result.Children.Should().NotBeNull("a present-but-empty child result set must yield an empty collection, not null");
            result.Children.Should().BeEmpty();
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_MissingChildResultSet_Throws()
        {
            // Arrange: the query returns only the parent's own result set - the second result set the model's
            // Children property requires never arrives at all (NextResult returns false, not true-with-zero-rows).
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.NextResult().Returns(false);
            rdr.HasRows.Returns(true);
            rdr.Read().Returns(true, false); //parent only
            rdr.FieldCount.Returns(2);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetFieldValue<int>(0).Returns(1);
            rdr.GetFieldValue<string>(1).Returns("Parent");
            rdr.GetString(1).Returns("Parent");
            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            Action act = () => Mapper.ModelFromReaderWithCollectionsHandler<CollectionReadParent>(new CollectionReadParent(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert: a missing result set is a contract violation between the model and its query, so this must
            // throw rather than silently produce an empty collection - which would look identical to "no children"
            // and could cause a subsequent save to diff away real child rows.
            act.Should().Throw<UnexpectedSqlResultException>()
                .WithMessage("*CollectionReadParent*")
                .Which.Message.Should().Contain("Children");
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_MissingRecord_ReturnsNullWithoutThrowing()
        {
            // Arrange: the first (parent) result set has no rows at all - the grain/record does not exist.
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<CollectionReadParent>(new CollectionReadParent(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().BeNull("an empty first result set means the record does not exist");
            rdr.DidNotReceive().NextResult(); // must not attempt to hydrate collections against a missing record
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_ModelWithoutCollections_BehavesLikeScalarOnlyRead()
        {
            // Arrange: a model with no CollectionMap-attributed properties should read exactly one result set.
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);
            rdr.Read().Returns(true, false);
            rdr.FieldCount.Returns(2);
            rdr.GetName(0).Returns("Id");
            rdr.GetName(1).Returns("Name");
            rdr.GetFieldValue<int>(0).Returns(7);
            rdr.GetFieldValue<string>(1).Returns("Plain");
            rdr.GetString(1).Returns("Plain");
            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<PlainNoCollectionModel>(new PlainNoCollectionModel(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(7);
            result.Name.Should().Be("Plain");
            rdr.DidNotReceive().NextResult(); // zero collection properties means zero additional result sets are read
        }

        [Fact]
        public void ModelFromReaderWithCollectionsHandler_ImmutableArrayCollectionProperty_HydratesParentAndChildren()
        {
            // Arrange: result set 0 is the parent's single row, result set 1 is the collection's rows. The
            // collection property is ImmutableArray<T> rather than List<T>, exercising the same conversion the
            // read side already applies via Mapper.ConvertListToCollectionPropertyType.
            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);
            rdr.NextResult().Returns(true);
            rdr.Read().Returns(true, false, //parent
                true, true, false // children
                );
            rdr.FieldCount.Returns(2);
            rdr.GetName(0).Returns("Id", "ChildId");
            rdr.GetName(1).Returns("Name", "ChildName");
            rdr.GetFieldValue<int>(0).Returns(1, 10, 20);
            rdr.GetFieldValue<string>(1).Returns("Parent", "Ten", "Twenty");
            rdr.GetString(1).Returns("Parent", "Ten", "Twenty");
            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);

            var dbLogger = new DebugLogger();

            // Act
            var result = Mapper.ModelFromReaderWithCollectionsHandler<CollectionReadParentImmutableArray>(new CollectionReadParentImmutableArray(), 0, "testSproc", null, rdr, null, "test connection", dbLogger);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1, "that is the parent scalar value");
            result.Name.Should().Be("Parent");
            result.Children.IsDefault.Should().BeFalse("a hydrated collection must never be the default ImmutableArray<T>");
            result.Children.Length.Should().Be(2, "two child rows were returned by the second result set");
            result.Children[0].ChildId.Should().Be(10);
            result.Children[0].ChildName.Should().Be("Ten");
            result.Children[1].ChildId.Should().Be(20);
            result.Children[1].ChildName.Should().Be("Twenty");
        }

        [Fact]
        public void HasCollectionMapProperties_ReportsPresenceCorrectly()
        {
            Mapper.HasCollectionMapProperties(typeof(CollectionReadParent)).Should().BeTrue();
            Mapper.HasCollectionMapProperties(typeof(PlainNoCollectionModel)).Should().BeFalse();
        }
    }
}
