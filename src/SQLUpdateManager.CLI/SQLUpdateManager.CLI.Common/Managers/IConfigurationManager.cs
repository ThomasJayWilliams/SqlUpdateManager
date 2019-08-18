using System.Collections.Generic;

namespace SqlUpdateManager.CLI.Common
{
    public interface IConfigurationManager
    {
        AppConfig GetConfig(string path);
        AppConfig UpdateConfig(AppConfig config, string path);
        AppConfig UpdateConfig(string category, string property, string value, string path);
        IEnumerable<string> GetStringConfig(string path);
    }
}
