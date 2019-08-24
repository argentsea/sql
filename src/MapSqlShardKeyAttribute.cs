using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Sql
{
    public class MapSqlShardKeyAttribute : MapShardKeyAttribute
    {
        public MapSqlShardKeyAttribute(char origin, string recordIdName)
            : base(null, origin, recordIdName, null, null, null) { }

        public MapSqlShardKeyAttribute(string shardIdName, char origin, string recordIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), origin, recordIdName, null, null, null) { }

        public MapSqlShardKeyAttribute(char origin, string recordIdName, string childIdName)
            : base(null, origin, recordIdName, childIdName, null, null) { }

        public MapSqlShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), origin, recordIdName, childIdName, null, null) { }

        public MapSqlShardKeyAttribute(char origin, string recordIdName, string childIdName, string grandChildIdName)
            : base(null, origin, recordIdName, childIdName, grandChildIdName, null) { }

        public MapSqlShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName, string grandChildIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), origin, recordIdName, childIdName, grandChildIdName, null) { }

        public MapSqlShardKeyAttribute(char origin, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName)
            : base(null, origin, recordIdName, childIdName, grandChildIdName, greatGrandChildIdName) { }

        public MapSqlShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName)
            : base(new MapToSqlSmallIntAttribute(shardIdName, false), origin, recordIdName, childIdName, grandChildIdName, greatGrandChildIdName) { }

    }
}
