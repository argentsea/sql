// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This options class contains the shard dataset configuration information.
    /// </summary>
    /// <typeparam name="TShard"></typeparam>
    public class SqlShardConnectionOptions<TShard> : IShardSetsConfigurationOptions<TShard>
        where TShard : IComparable
    {
        public IShardSetConnectionsConfiguration<TShard>[] ShardSetsInternal { get => SqlShardSets; }
		public SqlShardConnectionsConfiguration[] SqlShardSets { get; set; }

		public class SqlShardConnectionsConfiguration: SqlConnectionPropertiesBase, IShardSetConnectionsConfiguration<TShard>
		{
			public string ShardSetName { get; set; }
			public IShardConnectionConfiguration<TShard>[] ShardsInternal { get => Shards; }
			public SqlShardConnectionConfiguration[] Shards { get; set; }
        }

		public class SqlShardConnectionConfiguration: SqlConnectionPropertiesBase, IShardConnectionConfiguration<TShard>
		{
			public TShard ShardId { get; set; }
			public IDataConnection ReadConnectionInternal { get => ReadConnection; }
			public IDataConnection WriteConnectionInternal { get => WriteConnection; }
            public SqlConnectionConfiguration ReadConnection { get; set; } = new SqlConnectionConfiguration();
            public SqlConnectionConfiguration WriteConnection { get; set; } = new SqlConnectionConfiguration();
        }
	}
}
