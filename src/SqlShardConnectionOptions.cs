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
    public class SqlShardConnectionOptions : IShardSetsConfigurationOptions
    {
        public IShardSetConnectionsConfiguration[] ShardSetsConfigInternal { get => SqlShardSets; }
		public SqlShardConnectionsConfiguration[] SqlShardSets { get; set; }

        public class SqlShardConnectionsConfiguration: SqlConnectionPropertiesBase, IShardSetConnectionsConfiguration
		{
			public string ShardSetName { get; set; }
            public short DefaultShardId { get; set; }
			public IShardConnectionConfiguration[] ShardsConfigInternal { get => Shards; }
			public SqlShardConnectionConfiguration[] Shards { get; set; }
            public IShardConnectionConfiguration ReadConfigInternal => Read;
            public IShardConnectionConfiguration WriteConfigInternal => Write;
            public SqlShardConnectionConfiguration Read { get; set; }
            public SqlShardConnectionConfiguration Write { get; set; }
        }

        public class SqlShardConnectionConfiguration: SqlConnectionPropertiesBase, IShardConnectionConfiguration
		{
			public short ShardId { get; set; }
			public IDataConnection ReadConnectionInternal { get => ReadConnection; }
			public IDataConnection WriteConnectionInternal { get => WriteConnection; }
            public SqlConnectionConfiguration ReadConnection { get; set; } = new SqlConnectionConfiguration();
            public SqlConnectionConfiguration WriteConnection { get; set; } = new SqlConnectionConfiguration();
        }
	}
}
