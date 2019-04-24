using SUM.CLI.Helpers;
using SUM.CLI.UI;
using SUM.Core;

namespace SUM.CLI
{
	class CLIStartup : IStartup
	{
		private readonly ICommandReader reader;
		private readonly ICommandParser parser;
		private readonly ICommandRunner runner;

		public IConfigurator Configurator { get; set; }

		public CLIStartup(ICommandParser parser, ICommandReader reader, ICommandRunner runner)
		{
			this.reader = reader;
			this.runner = runner;
			this.parser = parser;
		}

		public static void Main(string[] args)
		{
			// Replace on Ninject injection
			var start = new CLIStartup(new CommandParser(), new CommandReader(new InputHandler()), new CommandRunner());
			start.RunApp();
		}

		public void RunApp()
		{
			while (true)
			{
				var command = parser.Parse(reader.Read());

				runner.Run(command);
			}
		}
	}
}
