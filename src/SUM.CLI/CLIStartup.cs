using System;
using SUM.System;

namespace SUM.CLI
{
	class CLIStartup : IStartup
	{
		public IConfigurator Configurator { get; set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}

		public void RunApp()
		{
			
		}
	}
}
