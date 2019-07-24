using Ninject;
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

		public IKernel ConfigureServices() =>
            new StandardKernel(
                new MiscModule(),
                new IOModule(),
                new CommandsModule());

		public void Configure()
		{
            if (!File.Exists(Constants.ConfigPath))
                File.Create(Constants.ConfigPath);
		}

		public void RunApp()
        {
            Configure();
            var prefixLine = _serviceProvider.Get<IPrefixLine>();

            while (true)
            {
                var chain = InitMiddlewares();
                prefixLine.PrintPrefix();
                var input = InputHandler.ReadLine();
                
                chain.Begin(input);
            }
		}

        private RequestChain InitMiddlewares() =>
            new RequestChain(
                _serviceProvider.Get<ErrorHanlingMiddleware>(),
                _serviceProvider.Get<ApplicationMiddleware>());
	}
}
