using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
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
                        .WriteTo.File(Constants.ErrorLogPath, outputTemplate: "{Message:lj}{NewLine}{Exception}{NewLine}", shared: true, encoding: Session.Current.Encoding)
                        .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}", theme: AnsiConsoleTheme.Code))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Information)
                        .WriteTo.File(Constants.InfoLogPath, shared: true, encoding: Session.Current.Encoding)
                        .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code))
                .CreateLogger();
        }

        public static void ConfigureConsole()
        {

        }
    }
}
