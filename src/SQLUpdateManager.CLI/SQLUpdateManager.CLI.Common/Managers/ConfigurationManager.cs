using System.IO;

namespace SQLUpdateManager.CLI.Common
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IDataRepository _repository;

        public ConfigurationManager(IDataRepository repository)
        {
            _repository = repository;
        }

        public AppConfig GetConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.");

            return _repository.GetData<AppConfig>(path);
        }

        public void UpdateConfig(AppConfig config, string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.");

            _repository.WriteData(path, config);
        }
    }
}
