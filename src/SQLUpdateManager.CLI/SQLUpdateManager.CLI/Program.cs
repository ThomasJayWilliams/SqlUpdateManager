using Ninject;

namespace SqlUpdateManager.CLI
{
    public class Program
	{
		public static void Main(string[] args)
		{
            var serviceProvider = new StandardKernel(
                new CLIModule(),
                new CommonModule(),
                new CoreModule(),
                new ApplicationModule());

            var app = serviceProvider.Get<Startup>();
			app.RunApp();
        }
    }
}
