using Ninject;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.IO;

namespace SQLUpdateManager.CLI
{
    public class Startup
    {
        private readonly IKernel _serviceProvider;

        public Startup()
        {
            _serviceProvider = ConfigureServices();
        }

        public void Configure()
        {
            Log.Information("Configuration...");

            if (!File.Exists(Constants.ConfigPath))
                throw new FileNotFoundException(
                    $"{Constants.ConfigPath} is missing. Application cannot be ran without configuration file. Please, re-install program.");

            else
            {
                var dataManager = _serviceProvider.Get<IDataRepository>();
                Session.Current.UpdateSession(dataManager.GetData<AppConfig>(Constants.ConfigPath));
            }

            if (!Directory.Exists(Constants.DataDir))
            {
                Log.Information($"{Constants.DataDir} directory not found.");
                Directory.CreateDirectory(Constants.DataDir);
                Log.Information($"{Constants.DataDir} directory created.");
            }

            if (!File.Exists(Constants.RegisterPath))
            {
                Log.Information($"{Constants.RegisterPath} file not found.");
                using (var file = File.Create(Constants.RegisterPath)) { }
                Log.Information($"{Constants.RegisterPath} file created.");
            }

            Log.Information("Configuration finished.");
        }

        public void RunApp()
        {
            try
            {
                var prefixLine = _serviceProvider.Get<IPrefixLine>();

                ConfigureLogger();

                Log.Information("Starting application...");

                Output.PrintLine(Constants.ASCIIArt);

                Configure();

                Output.PrintEmptyLine();

                while (true)
                {
                    var chain = InitMiddlewares();
                    prefixLine.PrintPrefix();
                    var input = Input.ReadLine();

                    chain.Begin(input);

                    Output.PrintEmptyLine();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Fatal error appeared! {ex.Message}");
                Environment.Exit(1);
            }
        }

        private IKernel ConfigureServices() =>
            new StandardKernel(
                new CLIModule(),
                new CommonModule(),
                new CoreModule(),
                new ApplicationModule());

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Error || logger.Level == LogEventLevel.Fatal)
                        .WriteTo.File(Constants.ErrorLogPath, outputTemplate: "{Message:lj}{NewLine}{Exception}{NewLine}", shared: true, encoding: Session.Current.Encoding)
                        .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == LogEventLevel.Information)
                        .WriteTo.File(Constants.InfoLogPath, shared: true, encoding: Session.Current.Encoding)
                        .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code))
                .CreateLogger();
        }

        private RequestChain InitMiddlewares()
        {
            return new RequestChain(
                _serviceProvider.Get<ErrorHanlingMiddleware>(),
                _serviceProvider.Get<ApplicationMiddleware>());
        }
    }
}
