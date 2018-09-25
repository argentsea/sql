using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
    /// <summary>
    /// This options class contains the shard dataset configuration information.
    /// </summary>
    /// <typeparam name="TShard"></typeparam>
    public class SqlShardConnectionOptions<TShard> : IShardSetConfigurationOptions<TShard>
        where TShard : IComparable
    {
        public IShardConnectionsConfiguration<TShard>[] ShardSetsInternal { get => SqlShardSets; }
		public SqlShardConnectionsConfiguration[] SqlShardSets { get; set; }

		public class SqlShardConnectionsConfiguration: IShardConnectionsConfiguration<TShard>
		{
			public string ShardSetName { get; set; }
			public IShardConnectionConfiguration<TShard>[] ShardsInternal { get => Shards; }
			public SqlShardConnectionConfiguration[] Shards { get; set; }
		}

		public class SqlShardConnectionConfiguration: IShardConnectionConfiguration<TShard>
		{
			public TShard ShardId { get; set; }
			public IConnectionConfiguration ReadConnectionInternal { get => ReadConnection; }
			public IConnectionConfiguration WriteConnectionInternal { get => WriteConnection; }
			public SqlConnectionConfiguration ReadConnection { get; set; }
			public SqlConnectionConfiguration WriteConnection { get; set; }
		}

	}
}
