using Ninject;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLUpdateManager.CLI
{
    public class Startup
    {
        private readonly IKernel _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IPrefix _prefixLine;
        private readonly ILogger _logger;

        private readonly Session _session;

        public Startup()
        {
            _serviceProvider = ConfigureServices();

            _session = _serviceProvider.Get<Session>();
            _configuration = _serviceProvider.Get<IConfiguration>();
            _prefixLine = _serviceProvider.Get<IPrefix>();
            _logger = _serviceProvider.Get<ILogger>();
        }

        public void Configure()
        {
            _configuration.ConfigureLogger();

            _logger.LogInfo("Starting application...");

            Output.PrintASCII(CLIConstants.ASCIIArt, CLIConstants.ASCIIColor);

            _logger.LogInfo("Configuration...");

            if (!File.Exists(CLIConstants.ConfigPath))
                throw new FileNotFoundException(
                    $"{CLIConstants.ConfigPath} is missing. Application cannot be ran without configuration file. Please, re-install program.");

            if (!File.Exists(CLIConstants.ConsoleThemesPath))
            {
                _logger.LogInfo("File with console themes is not found. Default theme will be loaded.");
                ConfigureConsole(CLIConstants.DefaultThemeName);
            }

            else
            {
                var dataManager = _serviceProvider.Get<IDataRepository>();
                var config = dataManager.GetData<AppConfig>(CLIConstants.ConfigPath);

                _session.UpdateSession(config);
                ConfigureConsole(config.Core.Theme);
            }

            if (!Directory.Exists(CLIConstants.DataDir))
            {
                _logger.LogInfo($"{CLIConstants.DataDir} directory not found.");
                Directory.CreateDirectory(CLIConstants.DataDir);
                _logger.LogInfo($"{CLIConstants.DataDir} directory created.");
            }

            if (!File.Exists(CLIConstants.RegisterPath))
            {
                _logger.LogInfo($"{CLIConstants.RegisterPath} file not found.");
                using (var file = File.Create(CLIConstants.RegisterPath)) { }
                _logger.LogInfo($"{CLIConstants.RegisterPath} file created.");
            }
            
            Output.PrintEmptyLine();
        }

        public void RunApp()
        {
            try
            {
                Configure();

                while (true)
                {
                    var chain = InitMiddlewares();

                    _prefixLine.PrintPrefix();

                    var input = Input.ReadLine();

                    chain.Begin(input);

                    Output.PrintEmptyLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fatal error appeared! {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void ConfigureConsole(string currentThemeName)
        {
            _logger.LogInfo("Console configuration...");

            if (currentThemeName != CLIConstants.DefaultThemeName)
            {
                var dataRepo = _serviceProvider.Get<IDataRepository>();
                var themes = dataRepo.GetData<IEnumerable<ConsoleTheme>>(CLIConstants.ConsoleThemesPath);

                var currentTheme = themes.FirstOrDefault(t => t.ThemeName == currentThemeName);

                if (currentTheme == null)
                {
                    _logger.LogInfo($"{currentThemeName} theme is not found. Default theme will be loaded.");
                    _session.Theme = _configuration.GetDefaultTheme();
                }

                else
                {
                    _session.Theme = currentTheme;
                    _logger.LogInfo($"{currentThemeName} theme loaded.");
                }
            }

            else
                _session.Theme = _configuration.GetDefaultTheme();
        }

        private IKernel ConfigureServices() =>
            new StandardKernel(
                new CLIModule(),
                new CommonModule(),
                new CoreModule(),
                new ApplicationModule());

        private RequestChain InitMiddlewares()
        {
            return new RequestChain(
                _serviceProvider.Get<ErrorHanlingMiddleware>(),
                _serviceProvider.Get<ApplicationMiddleware>());
        }
    }
}
