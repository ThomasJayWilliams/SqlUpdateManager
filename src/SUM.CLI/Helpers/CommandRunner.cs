using SUM.CLI.Commands;

namespace SUM.CLI.Helpers
{
	public class CommandRunner : ICommandRunner
	{
		public void Run(ICommand command) =>
			command.Invoke();
	}
}
