using System;
using System.Collections.Generic;
using System.Text;

namespace SqlUpdateManager.Core.Data
{
	public class StorageContext
	{
		public Lazy<StorageCollection<ServerEntity>> Servers { get; }
		public Lazy<StorageCollection<DatabaseEntity>> Databases { get; }
		public Lazy<StorageCollection<ProcedureEntity>> Procedures { get; }

		internal StorageContext(string path)
		{
			Servers = new Lazy<StorageCollection<ServerEntity>>(() =>
				new StorageCollection<ServerEntity>(path));
			Procedures = new Lazy<StorageCollection<ProcedureEntity>>(() =>
				new StorageCollection<ProcedureEntity>(path));
			Databases = new Lazy<StorageCollection<DatabaseEntity>>(() =>
				new StorageCollection<DatabaseEntity>(path));
		}

		public void SaveChanges()
		{
			Servers.Value.SaveChanges();
			Databases.Value.SaveChanges();
			Procedures.Value.SaveChanges();
		}
	}
}
