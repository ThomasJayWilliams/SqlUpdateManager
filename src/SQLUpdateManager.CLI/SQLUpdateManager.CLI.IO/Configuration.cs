using Serilog;
using Serilog.Events;
using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public static class Configuration
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Error || logger.Level == LogEventLevel.Fatal)
                        .WriteTo.File(Constants.ErrorLogPath, outputTemplate: "{Message:lj}{NewLine}{Exception}{NewLine}", shared: true, encoding: Session.Current.Encoding))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Information)
                        .WriteTo.File(Constants.InfoLogPath, shared: true, encoding: Session.Current.Encoding))
                .CreateLogger();
        }

        public static ConsoleTheme GetDefaultTheme() =>
            new ConsoleTheme
            {
                ThemeName = Constants.DefaultThemeName,
                AppColor = new RGB(37, 105, 62),
                ErrorColor = new RGB(192, 57, 37),
                DatabaseColor = new RGB(243, 156, 18),
                ServerColor = new RGB(142, 68, 128),
                ProcedureColor = new RGB(122, 140, 83)
            };
    }
}
