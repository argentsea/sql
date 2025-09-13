using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Sql
{
    public class MapSqlShardKeyAttribute : MapShardKeyAttribute
    {
        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey('U', "RecordId")]
        /// [MapToSqlInt("RecordId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        public MapSqlShardKeyAttribute(string recordIdName)
            : base(null, recordIdName, null, null, null, true) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId")]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, null, null, null, true) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey('U', "RecordId")]
        /// [MapToSqlInt("RecordId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="isRecordIdentifier">A optional value which indicates whether the property is the record identifier. Defaults to True if not set.</param>
        public MapSqlShardKeyAttribute(string recordIdName, bool isRecordIdentifier)
            : base(null, recordIdName, null, null, null, isRecordIdentifier) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", false)]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="isRecordIdentifier">A optional value which indicates whether the property is the record identifier. Defaults to True if not set.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, bool isRecordIdentifier)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, null, null, null, isRecordIdentifier) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId")]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, childIdName, childIdName, true) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId", false)]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="isRecordIdentifier">A optional value which indicates whether the property is the record identifier. Defaults to True if not set.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName, bool isRecordIdentifier)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, null, null, isRecordIdentifier) { }

        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId", "GrandChildId")]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// [MapToSqlInt("GrandChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="grandChildIdName">The name of the optional 3rd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName, string grandChildIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, grandChildIdName, null, true) { }


        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId", "GrandChildId", false)]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// [MapToSqlInt("GrandChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="grandChildIdName">The name of the optional 3rd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="isRecordIdentifier">A optional value which indicates whether the property is the record identifier. Defaults to True if not set.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName, string grandChildIdName, bool isRecordIdentifier)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, grandChildIdName, null, isRecordIdentifier) { }


        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId", "GrandChildId", "GreatGrandChildID")]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// [MapToSqlInt("GrandChildId")]
        /// [MapToSqlInt("GreatGrandChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="grandChildIdName">The name of the optional 3rd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="greatGrandChildIdName">The name of the optional 4th data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, grandChildIdName, greatGrandChildIdName, true) { }


        /// <summary>
        /// This property attribute is used to map multiple paramters to a ShardKey object.
        /// This recordIdName attribute name must exactly match the names of the corresponding MapTo attributes which are also on the same property.
        /// </summary>
        /// <example>
        /// For example, you could implement the mapping for a ShardKey property like this:
        /// <code>
        /// [MapShardKey("ShardId", 'U', "RecordId", "ChildId", "GrandChildId", "GreatGrandChildID", false)]
        /// [MapToSqlSmallInt("ShardId")]
        /// [MapToSqlInt("RecordId")]
        /// [MapToSqlInt("ChildId")]
        /// [MapToSqlInt("GrandChildId")]
        /// [MapToSqlInt("GreatGrandChildId")]
        /// public ShardKey&lt;int&gt;? Id { get; set; } = null;
        /// </code>
        /// </example>
        /// <param name="shardIdName">Optional name of the attribute containing a shard data column or parameter.</param>
        /// <param name="recordIdName">The name of the data column, which must exactly match the database attribute.</param>
        /// <param name="childIdName">The name of the optional 2nd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="grandChildIdName">The name of the optional 3rd data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="greatGrandChildIdName">The name of the optional 4th data column, if a compound key, which must exactly match the corresponding database attribute.</param>
        /// <param name="isRecordIdentifier">A optional value which indicates whether the property is the record identifier. Defaults to True if not set.</param>
        public MapSqlShardKeyAttribute(string shardIdName, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName, bool isRecordIdentifier)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), recordIdName, childIdName, grandChildIdName, greatGrandChildIdName, isRecordIdentifier) { }
    }
}
