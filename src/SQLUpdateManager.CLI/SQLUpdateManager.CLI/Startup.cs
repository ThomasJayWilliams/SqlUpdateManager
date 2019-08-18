using Ninject;
using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using System;
using System.IO;

namespace SqlUpdateManager.CLI
{
    public class Startup
    {
        private readonly IKernel _serviceProvider;
        private readonly IIOConfiguration _ioConfig;
        private readonly ICommonConfiguration _commonConfig;
        private readonly ILogger _logger;
        private readonly IDataRepository _dataRepo;
        private readonly IOutput _output;
        private readonly IInput _input;

        private readonly Session _session;

        public Startup(
            Session session,
            IIOConfiguration ioConfig,
            ICommonConfiguration commonConfig,
            IInput input,
            IOutput output,
            IDataRepository dataRepo,
            ILogger logger,
            IKernel serviceProvider)
        {
            _session = session;

            _ioConfig = ioConfig;
            _logger = logger;
            _dataRepo = dataRepo;
            _output = output;
            _input = input;
            _commonConfig = commonConfig;
            _serviceProvider = serviceProvider;
        }

        public void Configure()
        {
            var config = _dataRepo.GetData<AppConfig>(CLIConstants.ConfigPath);
            var storage = _dataRepo.GetData<Storage>(CLIConstants.StoragePath);

            _commonConfig.ConfigureSession(config, storage);
            _ioConfig.ConfigureLogger();

            _logger.LogInfo("Starting application...");

            _output.PrintASCII(CLIConstants.ASCIIArt);

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

                    var input = _input.ReadLine();

                    chain.Begin(input);

                    _output.PrintEmptyLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UNHANDLED EXCEPTION! {ex.Message}");
                Environment.Exit(1);
            }
        }

        private RequestChain InitMiddlewares()
        {
            return new RequestChain(
                _serviceProvider.Get<ErrorHanlingMiddleware>(),
                _serviceProvider.Get<ApplicationMiddleware>());
        }
    }
}
