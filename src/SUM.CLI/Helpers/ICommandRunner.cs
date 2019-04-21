using SUM.CLI.Core.Commands;

namespace SUM.CLI.Helpers
{
	public interface ICommandRunner
	{
		void Run(ICommand command);
	}
}
