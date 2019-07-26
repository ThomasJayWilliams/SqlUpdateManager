using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Common
{
    public interface IConfigurationManager
    {
        AppConfig GetConfig(string path);
        void UpdateConfig(AppConfig config, string path);
        void UpdateConfig(string category, string property, string value, string path);
        IEnumerable<string> GetStringConfig(string path);
    }
}
