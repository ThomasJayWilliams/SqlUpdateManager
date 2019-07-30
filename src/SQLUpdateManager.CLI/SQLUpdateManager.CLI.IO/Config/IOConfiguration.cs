using Serilog;
using Serilog.Events;
using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public class IOConfiguration : IIOConfiguration
    {
        private readonly Session _session;

        public IOConfiguration(Session session)
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
    }
}
