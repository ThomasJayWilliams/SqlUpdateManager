using SUM.CLI.Commands;

namespace SUM.CLI.Helpers
{
	public interface ICommandRunner
	{
		void Run(ICommand command);
	}
}
