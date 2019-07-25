namespace SQLUpdateManager.CLI.Common
{
    public interface IConfigurationManager
    {
        AppConfig GetConfig(string path);
        void UpdateConfig(AppConfig config, string path);
    }
}
