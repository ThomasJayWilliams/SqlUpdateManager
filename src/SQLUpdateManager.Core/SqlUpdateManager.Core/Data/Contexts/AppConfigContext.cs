using System;

namespace SqlUpdateManager.Core.Data
{
	public class AppConfigContext
	{
		private readonly Lazy<StorageCollection<AppConfigEntity>> _appConfig;

		public StorageCollection<AppConfigEntity> AppConfig => _appConfig.Value;

		internal AppConfigContext(string path)
		{
			_appConfig = new Lazy<StorageCollection<AppConfigEntity>>(() =>
				new StorageCollection<AppConfigEntity>(path));
		}
	}
}
