using Ninject;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
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
            {
                File.Create(Constants.ConfigPath);
                Log.Information("Created configuration file.");
            }

            Log.Information("Configuration finished.");
		}

		public void RunApp()
        {
            ConfigureLogger();

            Log.Information("Starting application...");

            OutputHandler.PrintLine(Constants.ASCIIArt);

            Configure();

            OutputHandler.PrintEmptyLine();
            var prefixLine = _serviceProvider.Get<IPrefixLine>();

            while (true)
            {
                var chain = InitMiddlewares();
                prefixLine.PrintPrefix();
                var input = InputHandler.ReadLine();
                
                chain.Begin(input);

                OutputHandler.PrintEmptyLine();
            }
        }

        private IKernel ConfigureServices() =>
            new StandardKernel(
                new MiscModule(),
                new IOModule(),
                new CoreModule(),
                new CommandsModule());

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information, outputTemplate: "{Message:lj}{NewLine}", theme: SystemConsoleTheme.Colored)
                .WriteTo.File(Constants.ErrorLogPath, Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(Constants.InfoLogPath, Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
        }

        private RequestChain InitMiddlewares() =>
            new RequestChain(
                _serviceProvider.Get<ErrorHanlingMiddleware>(),
                _serviceProvider.Get<ApplicationMiddleware>());
	}
}
