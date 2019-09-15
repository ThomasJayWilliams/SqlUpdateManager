using System;

namespace SqlUpdateManager.Core.Data
{
	public class StorageContext
	{
		private readonly Lazy<StorageCollection<ServerEntity>> _servers;
		private readonly Lazy<StorageCollection<DatabaseEntity>> _databases;
		private readonly Lazy<StorageCollection<ProcedureEntity>> _procedures;

		public StorageCollection<ServerEntity> Servers => _servers.Value;
		public StorageCollection<DatabaseEntity> Databases => _databases.Value;
		public StorageCollection<ProcedureEntity> Procedures => _procedures.Value;

		internal StorageContext(string path)
		{
			_servers = new Lazy<StorageCollection<ServerEntity>>(() =>
				new StorageCollection<ServerEntity>(path));
			_databases = new Lazy<StorageCollection<DatabaseEntity>>(() =>
				new StorageCollection<DatabaseEntity>(path));
			_procedures = new Lazy<StorageCollection<ProcedureEntity>>(() =>
				new StorageCollection<ProcedureEntity>(path));
		}
	}
}
