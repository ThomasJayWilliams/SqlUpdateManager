using System;

namespace SqlUpdateManager.CLI.Common
{
	public class CommonConfiguration : ICommonConfiguration
    {
        private readonly Session _session;
        private readonly IDataRepository _dataRepo;

        public CommonConfiguration(Session session, IDataRepository dataRepo)
        {
            _session = session;
            _dataRepo = dataRepo;
        }

        public void ConfigureSession(AppConfig config, Storage storage)
        {
            if (config == null)
                throw new ArgumentNullException("Config cannot be null.");

            if (string.IsNullOrEmpty(config.Core.FileEncoding))
                throw new ArgumentException("File encoding cannot be null or empty.");

            _session.Encoding = EncodingHelper.GetEncoding(config.Core.FileEncoding);

            _session.Storage = storage;
        }
    }
}
