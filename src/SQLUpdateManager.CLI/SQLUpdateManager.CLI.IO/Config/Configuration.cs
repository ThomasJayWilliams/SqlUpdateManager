using Serilog;
using Serilog.Events;
using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public class Configuration : IConfiguration
    {
        private readonly Session _session;

        public Configuration(Session session)
        {
            _session = session;
        }

        public void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Error || logger.Level == LogEventLevel.Fatal)
                        .WriteTo.File(CLIConstants.ErrorLogPath, outputTemplate: "{Message:lj}{NewLine}{Exception}{NewLine}", shared: true, encoding: _session.Encoding))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Information)
                        .WriteTo.File(CLIConstants.InfoLogPath, shared: true, encoding: _session.Encoding))
                .CreateLogger();
        }

        public ConsoleTheme GetDefaultTheme() =>
            new ConsoleTheme
            {
                ThemeName = CLIConstants.DefaultThemeName,
                AppColor = new RGB(37, 105, 62),
                ErrorColor = new RGB(192, 57, 37),
                DatabaseColor = new RGB(243, 156, 18),
                ServerColor = new RGB(142, 68, 128),
                ProcedureColor = new RGB(122, 140, 83)
            };
    }
}
