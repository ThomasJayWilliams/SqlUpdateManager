using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public interface IConfiguration
    {
        void ConfigureLogger();
        ConsoleTheme GetDefaultTheme();
    }
}
