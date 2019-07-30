using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public interface IConfiguration
    {
        void ConfigureLogger();
        void ConfigureSession(AppConfig config);
        ConsoleTheme GetDefaultTheme();
    }
}
