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

        public Startup()
        {
            _serviceProvider = ConfigureServices();
        }

        public void Configure()
        {
            Configuration.ConfigureLogger();

            SerilogLogger.LogInfo("Starting application...");

            Output.PrintLine(Constants.ASCIIArt);

            SerilogLogger.LogInfo("Configuration...");

            if (!File.Exists(Constants.ConfigPath))
                throw new FileNotFoundException(
                    $"{Constants.ConfigPath} is missing. Application cannot be ran without configuration file. Please, re-install program.");

            if (!File.Exists(Constants.ConsoleThemesPath))
            {
                SerilogLogger.LogInfo("File with console themes is not found. Default theme will be loaded.");
                ConfigureConsole(Constants.DefaultThemeName);
            }

            else
            {
                var dataManager = _serviceProvider.Get<IDataRepository>();
                var config = dataManager.GetData<AppConfig>(Constants.ConfigPath);

                Session.Current.UpdateSession(config);
                ConfigureConsole(config.Theme);
            }

            if (!Directory.Exists(Constants.DataDir))
            {
                SerilogLogger.LogInfo($"{Constants.DataDir} directory not found.");
                Directory.CreateDirectory(Constants.DataDir);
                SerilogLogger.LogInfo($"{Constants.DataDir} directory created.");
            }

            if (!File.Exists(Constants.RegisterPath))
            {
                SerilogLogger.LogInfo($"{Constants.RegisterPath} file not found.");
                using (var file = File.Create(Constants.RegisterPath)) { }
                SerilogLogger.LogInfo($"{Constants.RegisterPath} file created.");
            }

            SerilogLogger.LogInfo("Configuration finished.");
            Output.PrintEmptyLine();
        }

        public void RunApp()
        {
            try
            {
                var prefixLine = _serviceProvider.Get<IPrefixLine>();

                Configure();

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
                SerilogLogger.LogError(ex, $"Fatal error appeared! {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void ConfigureConsole(string currentThemeName)
        {
            if (currentThemeName != Constants.DefaultThemeName)
            {
                var dataRepo = _serviceProvider.Get<IDataRepository>();
                var themes = dataRepo.GetData<IEnumerable<ConsoleTheme>>(Constants.ConsoleThemesPath);

                var currentTheme = themes.FirstOrDefault(t => t.ThemeName == currentThemeName);

                if (currentTheme == null)
                {
                    SerilogLogger.LogError($"{currentThemeName} theme is not found. Default theme will be loaded.");
                    Session.Current.Theme = Configuration.GetDefaultTheme();
                }

                else
                {
                    Session.Current.Theme = currentTheme;
                    SerilogLogger.LogInfo($"{currentThemeName} theme loaded.");
                }
            }

            else
                Session.Current.Theme = Configuration.GetDefaultTheme();
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
