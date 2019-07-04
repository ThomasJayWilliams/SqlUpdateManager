using Ninject;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.IO;

namespace SQLUpdateManager.CLI
{
	public class Startup
	{
		public IKernel ConfigureServices() =>
            new StandardKernel(
                new MiscModule(),
                new IOModule(),
                new CommandsModule());

		public void Configure()
		{
            var serializer = new JsonSerializer();

            if (!File.Exists(Constants.ConfigPath))
                File.Create(Constants.ConfigPath);

            else
            {
                var configFile = File.ReadAllText(Constants.ConfigPath);

                if (!string.IsNullOrEmpty(configFile))
                {
                    var config = serializer.Deserialize<AppConfig>(configFile);
                    InitSession(config);
                }
            }
		}

		public void RunApp()
        {
            Configure();
            var kernel = ConfigureServices();
            var prefixLine = kernel.Get<IPrefixLine>();

            while (true)
            {
                var chain = InitMiddlewares(kernel);

                prefixLine.PrintPrefix();

                var input = InputHandler.ReadLine();
                
                chain.Begin(input);
            }
		}

        private RequestChain InitMiddlewares(IKernel provider) =>
            new RequestChain(
                provider.Get<ErrorHanlingMiddleware>(),
                provider.Get<ApplicationMiddleware>());

        private void InitSession(AppConfig config)
        {
            Session.Current.ApplicationStartTime = DateTime.UtcNow;
            Session.Current.SessionLifeTime = config.SessionLifeTime;
        }
	}
}
