using Ninject;
using SQLUpdateManager.CLI.Common;
using System;
using System.IO;

namespace SQLUpdateManager.CLI
{
	public class Startup
	{
		public IKernel ConfigureServices() =>
            new StandardKernel(new MiscModule());

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
            var kernel = ConfigureServices();
            Configure();
            var chain = InitMiddlewares(kernel);

            while (true)
                chain.Begin();
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
