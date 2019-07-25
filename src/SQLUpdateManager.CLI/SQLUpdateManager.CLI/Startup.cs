﻿using Ninject;
using Serilog;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System.IO;
using System.Text;
using Serilog.Sinks.SystemConsole.Themes;

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

            Output.PrintLine(Constants.ASCIIArt);

            Configure();

            Output.PrintEmptyLine();
            var prefixLine = _serviceProvider.Get<IPrefixLine>();

            while (true)
            {
                var chain = InitMiddlewares();
                prefixLine.PrintPrefix();
                var input = Input.ReadLine();
                
                chain.Begin(input);

                Output.PrintEmptyLine();
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
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == Serilog.Events.LogEventLevel.Error)
                        .WriteTo.File(Constants.ErrorLogPath, shared: true, encoding: Encoding.UTF8)
                        .WriteTo.Console(outputTemplate: "{Level} {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code))
                .WriteTo.Logger(log =>
                    log.Filter.ByIncludingOnly(logger => logger.Level == Serilog.Events.LogEventLevel.Information)
                        .WriteTo.File(Constants.InfoLogPath, shared: true, encoding: Encoding.UTF8)
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
