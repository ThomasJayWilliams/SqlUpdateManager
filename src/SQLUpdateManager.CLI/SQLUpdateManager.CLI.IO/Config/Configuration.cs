using Serilog;
using Serilog.Events;
using SQLUpdateManager.CLI.Common;
using System;

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
                        .WriteTo.File(CLIConstants.ErrorLogPath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", shared: true, encoding: _session.Encoding))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Information)
                        .WriteTo.File(CLIConstants.InfoLogPath, shared: true, encoding: _session.Encoding))
                .CreateLogger();
        }

        public void ConfigureSession(AppConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("Config cannot be null.");

            if (string.IsNullOrEmpty(config.Core.FileEncoding))
                throw new ArgumentException("File encoding cannot be null or empty.");
            _session.Encoding = EncodingHelper.GetEncoding(config.Core.FileEncoding);
        }

        public ConsoleTheme GetDefaultTheme() =>
            new ConsoleTheme
            {
                ThemeName = CLIConstants.DefaultThemeName,
                AppColor = new RGB(37, 105, 62),
                ErrorColor = new RGB(192, 57, 37),
                DatabaseColor = new RGB(243, 156, 18),
                ServerColor = new RGB(142, 68, 128),
                ProcedureColor = new RGB(122, 140, 83),
                TextColor = new RGB(236, 240, 241),
                InfoColor = new RGB(34, 174, 96),
                InputColor = new RGB(236, 240, 241)
            };
    }
}
