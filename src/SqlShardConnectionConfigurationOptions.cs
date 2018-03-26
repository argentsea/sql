using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql
{
    public class SqlShardConnectionConfigurationOptions<TShard> : IShardDataConfigurationOptions<TShard>
	{
		public IShardConnectionsConfiguration<TShard>[] ShardSetsInternal { get => ShardSets; }
		public SqlShardConnectionsConfiguration[] ShardSets { get; set; }

		public class SqlShardConnectionsConfiguration: IShardConnectionsConfiguration<TShard>
		{
			public string ShardSetKey { get; set; }
			public string SecurityKey { get; set; }
			public string DataResilienceKey { get; set; }
			public IShardConnectionConfiguration<TShard>[] ShardsInternal { get => Shards; }
			public SqlShardConnectionConfiguration[] Shards { get; set; }
		}

		public class SqlShardConnectionConfiguration: IShardConnectionConfiguration<TShard>
		{
			public TShard ShardNumber { get; set; }
			public IConnectionConfiguration ReadConnectionInternal { get => ReadConnection; }
			public IConnectionConfiguration WriteConnectionInternal { get => WriteConnection; }
			public SqlConnectionConfiguration ReadConnection { get; set; }
			public SqlConnectionConfiguration WriteConnection { get; set; }
		}

	}
}
