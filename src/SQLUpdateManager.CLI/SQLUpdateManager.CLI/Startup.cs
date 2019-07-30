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
        private readonly IIOConfiguration _ioConfig;
        private readonly ICommonConfiguration _commonConfig;
        private readonly IPrefix _prefixLine;
        private readonly ILogger _logger;
        private readonly IDataRepository _dataRepo;
        private readonly IOutput _output;
        private readonly IInput _input;

        private readonly Session _session;
        private readonly AppConfig _config;

        public Startup()
        {
            _serviceProvider = ConfigureServices();

            _session = _serviceProvider.Get<Session>();

            _ioConfig = _serviceProvider.Get<IIOConfiguration>();
            _prefixLine = _serviceProvider.Get<IPrefix>();
            _logger = _serviceProvider.Get<ILogger>();
            _dataRepo = _serviceProvider.Get<IDataRepository>();
            _output = _serviceProvider.Get<IOutput>();
            _input = _serviceProvider.Get<IInput>();
            _commonConfig = _serviceProvider.Get<ICommonConfiguration>();

            _config = _dataRepo.GetData<AppConfig>(CLIConstants.ConfigPath);
        }

        public void Configure()
        {
            _commonConfig.ConfigureSession(_config);
            _ioConfig.ConfigureLogger();

            _logger.LogInfo("Starting application...");

            _output.PrintASCII(CLIConstants.ASCIIArt, CLIConstants.ASCIIColor);

            _logger.LogInfo("Configuration...");

            if (!File.Exists(CLIConstants.ConfigPath))
                throw new FileNotFoundException(
                    $"{CLIConstants.ConfigPath} is missing. Application cannot be ran without configuration file. Please, re-install program.");

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

            _output.PrintEmptyLine();
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

                    var input = _input.ReadLine();

                    chain.Begin(input);

                    _output.PrintEmptyLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fatal error appeared! {ex.Message}");
                Environment.Exit(1);
            }
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
